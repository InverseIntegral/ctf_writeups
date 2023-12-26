# 17 - Lost Key

## Description

Level: Hard<br/>
Author: darkstar

After losing another important key, the administrator sent me a picture of a key as a replacement. But what should I do
with it?

![Key picture distributed as part of the challenge](key.png)

## Solution

This was the first challenge I got first blood on this year. I really enjoyed this challenge, so let's get into it. We
are given a [PNG file](key.png) as well as an [encrypted flag](flag.enc) for this challenge, checking the strings
using `strings` gives us this hint `Key Info: 0x10001`. From this we know that we are dealing with RSA since `0x10001`
is a common constant used for the public exponent `e`. All we need to solve the challenge are `p` or `q`.

After digging for a while, I realized that there were two pixels in the image that seemed off. One was placed exactly in
the middle on the right side. The second weird pixel was at the bottom row in the last column (right corner). This
confirmed my suspicion, `p` and `q` were part of the image and the last pixel was used to make the bytes a valid prime
number.

To confirm this, we can estimate the size of `n` by looking at `flag.enc`:

```
du -b flag.enc
6930    flag.enc
```

`flag.enc` contains 6930 bytes and `n` should be at least that size since otherwise the decryption using RSA is no
longer unique. If we assume that `p` and `q` are roughly the same size, they have to be of size at
least `6930 * 8 / 2 = 27720` bits. The `key.png` contains exactly `1155` pixels which means that every pixel should
contain `27720 / 1155 = 24` bits of the key. Coincidentally, this is exactly the amount of bits we have in a PNG per
pixel.

Enough maths, all we have to do is extract `p` and `q` and decrypt the flag:

```python
from Crypto.Util.number import bytes_to_long, long_to_bytes
from PIL import Image

im = Image.open('key.png')
rgb_im = im.convert('RGB')
width, height = im.size
sys.set_int_max_str_digits(999999999)
pq = im.tobytes()
size = len(pq) // 2

p = bytes_to_long(pq[:size])
q = bytes_to_long(pq[size:])

in_file = open("flag.enc", "rb")
data = in_file.read()

phi = (p - 1) * (q - 1)
n = p * q
ct = mod(bytes_to_long(data), n)

e = 65537
d = inverse_mod(e, phi)

c = pow(int(ct), int(d), int(n))
flag = long_to_bytes(c)

with open('flag.png', 'wb') as f:
    f.write(flag)
```

It takes a while to run the script since `pow` takes a few minutes (not sure why exactly, I always thought RSA
decryption was faster). Scanning the QR code found in [flag.png](flag.png) gives us the
flag `HV23{Thanks_for_finding_my_key}`. The idea behind this challenge is fascinating and I was surprised that is
feasible to find a valid prime being only able to modify the last 24 bits.
