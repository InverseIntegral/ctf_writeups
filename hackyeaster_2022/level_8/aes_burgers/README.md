# AES Burgers

## Description
Level: hard<br/>
Author: PS

Welcome to AES Burgers - where the patty is tatty™ !
Connect to our server, and place your order!

```
nc 46.101.107.117 2207
```

[aesburgers.py](aesburgers.py)

## Solution

This was probably my favourite challenge this year. We are given a python script as well as a remote service. The
service lets us create "burgers" thar are then encrypted using AES_ECB:

```python
def encrypt(plain):
    cipher = AES.new(key.encode(), AES.MODE_ECB)
    plain += b' ' * ((16 - (len(plain) % 16)) % 16)
    enc = cipher.encrypt(plain)
    return enc.hex()

def makeBurger(bun, patties):
    burger = b''
    burger += bun[::-1]
    burger += patties * flag
    burger += bun
    return burger

patties = int(input("How many patties: "))
if (patties < 1 or patties > 24):
    print('That won''t work  ¯\_ツ_/¯')
    exit()

bun = input("Which bun? ").strip().encode()
if len(bun) != 16:
    print('We don''t have that, sorry.  ¯\_ツ_/¯')
    exit()

burger = makeBurger(bun, patties)
print(encrypt(burger))
```

Notice that we get an encryption oracle for free here, since the first block (16 bytes long) is just `bun[::-1]` which we
control directly. One important property of AES_ECB is that the same block encrypts to the same ciphertext block
independent of its location. We will have to use this later one so keep it in mind.

### Finding the flag length

First I tried to figure out the flag length. To do this I simply went through different values of patties (The patty is
multiplied by the flag and then encrypted). If the first and last block are the same, we know that `len(flag) * patties
% 16 == 0`. Or in other words, we can take the ciphertext length minus the first and last block and divide it by our
% current patty value to obtain the flag length:

```python
import socket
from textwrap import wrap

hostname = '46.101.107.117'
port = 2207

def send(patties, bun):
    s.sendall(str(patties).encode("utf-8") + b'\n')
    s.recv(1024)
    s.sendall(bun + b'\n')
    ct = s.recv(6000).split(b'\n')
    return wrap(ct[1].decode("utf-8"), 32)

for i in range(1, 25):
    s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    s.connect((hostname, port))
    s.recv(1024)
    ct = send(i, b'A' * 16)
    first_block = ct[0]

    if first_block in ct[1:]:
        print((len(ct) - 2) * 16 // i)
        break
```

The flag has a length of 35 so it uses a bit more than two full blocks.

### Decrypting the last block of the burger

Now that we know the length of the flag we can start using the encryption oracle. The idea is as follows:

- Find a patty value (multiplier) such that only one unknown character gets placed into the last block
- Use the encryption oracle of the first block (bun) to bruteforce the character
- Remember the known suffix and repeat this process

In python code the above looks like this:

```python
import socket
from textwrap import wrap
import string

hostname = '46.101.107.117'
port = 2207

def send(patties, bun, should_lookup):
    global saved
    s.sendall(str(patties).encode("utf-8") + b'\n')
    s.recv(1024)
    s.sendall(bun + b'\n')
    ct = s.recv(6000).split(b'\n')
    blocks = wrap(ct[1].decode("utf-8"), 32)

    if should_lookup:
        if saved == blocks[0]:
            return True
    else:
        saved = blocks[-2]

    return False

suffix = ''
pre_suffix = 'A' * 16

while True:
    saved = ''
    s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    s.connect((hostname, port))
    s.recv(1024)
    
    for i in range(1, 25):
        if 35 * i % 16 == (len(suffix) + 1) % 16:
            send(i, b'A' * 16, False)
            break
    
    s.close()
    
    for c in string.printable:
        if (c == ' '): break
    
        s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        s.connect((hostname, port))
        s.recv(1024)
    
        if send(1, (pre_suffix[len(suffix)+1:] + suffix + c).encode("utf-8"), True):
            suffix += c
            print(suffix[::-1])
            break
    
        s.close()

    if len(suffix) == 16:
        break
```

And prints the last block `f00d_s0m3t1m3s!}`.

### Decrypting the second last block of the burger

The idea for the second last block is almost the same as for the last one. However, we have to change a few indices and
we have to be careful since the second to last block isn't directly adjacent to the bun. That is why I introduced the
`pre_prefix` which stores the plaintext of the last block. It is later used to query the encryption oracle (Keep in mind
that the bun is reversed!).

```python
import socket
from textwrap import wrap
import string

hostname = '46.101.107.117'
port = 2207

def send(patties, bun, should_lookup):
    global saved
    s.sendall(str(patties).encode("utf-8") + b'\n')
    s.recv(1024)
    s.sendall(bun + b'\n')
    ct = s.recv(6000).split(b'\n')
    blocks = wrap(ct[1].decode("utf-8"), 32)

    if should_lookup:
        if saved == blocks[0]:
            return True
    else:
        saved = blocks[-3]

    return False

suffix = ''
pre_suffix = '}!s3m1t3m0s_d00f'

while True:
    saved = ''
    s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    s.connect((hostname, port))
    s.recv(1024)
    
    for i in range(1, 25):
        if 35 * i % 16 == (len(suffix) + 1) % 16:
            send(i, b'A' * 16, False)
            break
    
    s.close()
    
    for c in string.printable:
        if (c == ' '): break
    
        s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        s.connect((hostname, port))
        s.recv(1024)
    
        if send(1, (pre_suffix[len(suffix)+1:] + suffix + c).encode("utf-8"), True):
            suffix += c
            print(suffix[::-1])
            break
    
        s.close()

    if len(suffix) == 16:
        break
```

This prints the second to last block: `022{w3_luv_junk_`. Concatenated with the last block and adding the missing
characters at the start gives the flag: `he2022{w3_luv_junk_f00d_s0m3t1m3s!}`.

