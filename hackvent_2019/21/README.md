# 21 - Happy Christmas 256

## Description

Level: Hard<br/>
Author: hardlock

Santa has improved since the last Cryptmas and now he uses harder algorithms to secure the flag.
This is his public key:

```
X: 0xc58966d17da18c7f019c881e187c608fcb5010ef36fba4a199e7b382a088072f
Y: 0xd91b949eaf992c464d3e0d09c45b173b121d53097a9d47c25220c0b4beb943c
```

To make sure this is safe, he used the NIST P-256 standard. But we are lucky and an Elve is our friend. We were able to
gather some details from our whistleblower:

- Santa used a password and SHA256 for the private key (d)
- His password was leaked 10 years ago
- The password is length is the square root of 256
- The flag is encrypted with AES256
- The key for AES is derived with pbkdf2_hmac, salt: "TwoHundredFiftySix", iterations: 256 * 256 * 256

Phew - Santa seems to know his business - or can you still recover this flag?

```
Hy97Xwv97vpwGn21finVvZj5pK/BvBjscf6vffm1po0=
```

## Solution

From the description of the challenge it was clear that we could not simply bruteforce the password because
`pbkdf2_hmac` with `256^3` iterations made this infeasable. Since the password was leaked 10 years ago, I assumed that
it must be in `rockyou.txt`. I simply went through all passwords of length 16 and checked if their SHA256 hash was a
valid private key for the given public key. 

The following script prints the flag after about a minute `HV19{sry_n0_crypt0mat_th1s_year}`.

```python
from Crypto.PublicKey import ECC
from Crypto.Hash import SHA256
from Crypto.Cipher import AES 
import base64
import hashlib

x = 0xc58966d17da18c7f019c881e187c608fcb5010ef36fba4a199e7b382a088072f
y = 0xd91b949eaf992c464d3e0d09c45b173b121d53097a9d47c25220c0b4beb943c

r = ECC.EccPoint(x, y)

Gx = 0x6b17d1f2e12c4247f8bce6e563a440f277037d812deb33a0f4a13945d898c296
Gy = 0x4fe342e2fe1a7f9b8ee7eb4a7c0f9e162bce33576b315ececbb6406837bf51f5

g = ECC.EccPoint(Gx, Gy)

with open("rockyou.txt", "r") as ins:
   for line in ins:
      line = line.strip()
      if len(line) == 16:
         h = SHA256.new()
         h.update(line)
         digest = int(h.hexdigest(), 16)
         if (g * digest == r):
            key = hashlib.pbkdf2_hmac('sha256', line, 'TwoHundredFiftySix', 256 * 256 * 256)
            cipher = AES.new(key, AES.MODE_ECB)
            plaintext = cipher.decrypt(base64.b64decode('Hy97Xwv97vpwGn21finVvZj5pK/BvBjscf6vffm1po0='))
            print(plaintext)
            break
```
