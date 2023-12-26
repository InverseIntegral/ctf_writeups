# 12 - unsanta

## Description

Level: Medium<br/>
Author: kuyaya

To train his skills in cybersecurity, Grinch has played this year's SHC qualifiers. He was inspired by the cryptography
challenge `unm0unt41n` (can be found [here](https://library.m0unt41n.ch/)) and thought he might play a funny prank on Santa. Grinch is a script kiddie
and stole the malware idea and almost the whole code. Instead of using the original encryption malware from the challenge
though, he improved it a bit so that no one can recover his secret!

Luckily, Santa had a backup of one of the images. Maybe this can help you find the secret and recover all of Santa's
lost data...?

## Solution

For this challenge we are given [a ZIP file](unsanta.zip) that contains a challenge similar to a previous CTF challenge.
[We can find a writeup of the original challenge online](https://blog.gk.wtf/ctf/unm0unt41n/). In the original challenge
it was enough to:

- XOR an encrypted file and its original to obtain the cypher stream
- Use [RandCrack](https://github.com/tna0y/Python-random-module-cracker) to continue the random stream
- With the random stream decrypt the other files

This won't work on our challenge, unfortunately. In our case the malware looks like this:

```python
flag = b"REDACTED"
random.seed(bytes_to_long(flag))

path = Path("memes/")
for p in sorted(path.rglob("*")):
    if os.path.isfile(p):
        with open(p, "rb") as fp:
            c = fp.read()

        s = b""
        for _ in range((len(c) + 3) // 4):
            s += random.getrandbits(32).to_bytes(4, "big")
        cenc = b"".join([bytes([s[i] ^ c[i]]) for i in range(len(c))])

        with open(p, "wb+") as fp:
            fp.write(cenc)
```

The flag is used as the seed of the random stream. Then the images in `memes` are encrypted via an XOR cypher. After
some googling [I came across a tool](https://github.com/deut-erium/RNGeesus/tree/main) that is able to recover the seed
of the PRNG based on just 3 random bytes.

I simply XORed the encrypted meme `a.jpg` with its original to obtain enough bytes to recover the seed:

```python
from Crypto.Util.number import long_to_bytes
from mersenne import *

def array_to_int(arr):
    arr_bytes = b"".join([int.to_bytes(i, 4, 'little') for i in arr])
    return int.from_bytes(arr_bytes, 'little')

with open('./memes/a.jpg', 'rb') as f:
    encrypted = f.read()

with open('./backup/a.jpg', 'rb') as f:
    decrypted = f.read()

key = bytes([b ^ a for a, b in zip(encrypted, decrypted)])
key = [int.from_bytes(key[i:i + 4], "big") for i in range(0, len(key), 4)]
outputs = key[:624]
b = BreakerPy()

seed_arr = b.get_seeds_python_fast(outputs)
print(long_to_bytes(array_to_int(seed_arr)))
```

This prints the flag `HV23{s33d_r3c0very_1s_34sy}` and shows how easy it really is. 
