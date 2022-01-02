# 20 - Trolling Crypto Elves

## Description

The elves have been back to school and now they're trolling Santa. They've encrypted a message and are challenging him
to decrypt it. Of course Santa doesn't want to look stupid, so he's asking you for help. Would you mind decrypting the
message and letting Santa know what's in it? You'll get a cookie in return (well, a flag-shaped one, but aren't those
the best?).

## Solution

This was the last challenge that I submitted this year. I will present the intended solution here. Because I changed the
flag (the challenge was originally planned for Hackyeaster) the challenge got slightly b0rked. You couldn't recover the
flag reliably anymore. For this reason I published a new version of the challenge which worked.

We are given two files. One is the encrypted flag and the other one contains a public key. From the public key we get
the parameters `n` and `e` that are used in RSA:

```python
with open("../problem/out/broken.key", "rb") as key_file:
    rsa = RSA.import_key(key_file.read())

public_key = rsa.publickey()
n = public_key.n
e = public_key.e
```

In this case `n` is
`21841229176641676811074222222429036686010157493819478906104756224784026782720095285562077658175994506814988868039420867464335127876647084137268626334223518969271953762934538192829593027351087506564856372252764608983654907443877304590598039308085484230387424168108589912364744981715047690934796624671825840761147121835935311518027061488285356813614197767377798508227633097728683793773240697883725855674919223272992158621864652379194787743287739980453395882286994644048503221607873267720518809023683802183778543479285200786684047572628386266548042179603137730815862594760654189158575192864779821225861303652435417736529`
and `e` is `4242`. At this point, we would usually try to factorize `n` into two prime numbers `p` and `q`. However, `n`
is a square number i.e. `n = p^2`. So now we can just calculate `phi(n)` and the modular inverse of e in `phi(n)` right?
Well, that only works if `gcd(e, phi(n)) = 1` which is not true in our case. `gcd(e, phi(n))` is 6 and RSA was used
incorrectly here.

If we take another look at the equations of textbook RSA, we can simplify them:

```
c = m^4242 mod n
c^(707^-1) = m^(4242 * 707^-1) mod n
c^(707^-1) = m^(6 * 707 * 707^-1) mod n
c^(707^-1) = m^6 mod n
```

Here I used [fermat's little theorem](https://en.wikipedia.org/wiki/Fermat%27s_little_theorem#Generalizations). Now we
can take the 6-th root on both sides and hopefully recover `m`. This only works if `m < n` (which was the problem when I
first published the challenge). The complete solution looks like this:

```python
from Crypto.Util.number import bytes_to_long, long_to_bytes
from Crypto.PublicKey import RSA

with open("broken.key", "rb") as key_file:
    rsa = RSA.import_key(key_file.read())

public_key = rsa.publickey()
n = public_key.n
e = public_key.e

assert is_square(n)

p = int(sqrt(n))
phi = euler_phi(n)

common = gcd(phi, e)
e1 = e // common
d = inverse_mod(int(e1), phi)

with open("broken.flag", "rb") as flag_file:
    c = bytes_to_long(flag_file.read())

c1 = power_mod(c, d, n)
print(long_to_bytes(int(int(c1)^(1/common))))
```

This gives the flag `HV{F3M4TS L1TTL3 TH30R3M}`.

