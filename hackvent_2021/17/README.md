# 17 - Forging Santa's Signature

## Description

Santa is out of town and the elves have to urgently sign for an order. What to do, what to do? Well, need to save
Christmas, so forge Santa's signature they shall!

The message to be signed is hashed as follows: `int(sha512(content.encode('utf-8')).hexdigest(), 16)`

## Solution

This was the second challenge that I provided. I will present the intended solution here.
Connection to the service shows us the following text:

```
==========================================
| Welcome to the P-384 signing programme |
==========================================

==========================================
|       What would you like to do?       |
|       [S]ign a sample message          |
|       [E]xecute a signed command       |
==========================================
```

From the name of the programme, we can guess that the challenge is about the ECDSA. We can choose to sign a sample text
and when I signed two different message (`Sample 1` and `Sample 2`) I got the following outputs:

```
(15071107002814951020552418888607518785289507295855966671902420272906115905500366617712699293466192226054215355138817, 17282678428260699875262213684657331671291521629337685973329262864761176078912977181982049910743541572976977440877688)
(15071107002814951020552418888607518785289507295855966671902420272906115905500366617712699293466192226054215355138817, 37122713374886649545615408297104032155888273765851718137529325647579006757197352821947439980935413424534881743509816)
``` 

The first coordinate seems to be the same for both signatures. This should give us a hint in the right direction. The
`k` of [the
ECDSA](https://en.wikipedia.org/wiki/Elliptic_Curve_Digital_Signature_Algorithm#Signature_verification_algorithm) seems
to be the same for each signature. [This can be abused to recover the private
key](https://crypto.stackexchange.com/questions/46621/ecdsa-security-using-same-per-message-secrets-k-but-different-signing-keys).
I have implemented this in sage:

```sage
from hashlib import sha512

# P-384 curve
p = 39402006196394479212279040100143613805079739270465446667948293404245721771496870329047266088258938001861606973112319
b = 0xb3312fa7e23ee7e4988e056be3f82d19181d9c6efe8141120314088f5013875ac656398d8a2ed19d2a85c8edd3ec2aef

G_x = 0xaa87ca22be8b05378eb1c71ef320ad746e1d3b628ba79b9859f741e082542a385502f25dbf55296c3a545e3872760ab7
G_y = 0x3617de4a96262c6f5d9e98bf9292dc29f8f41dbd289a147ce9da3113b5f0b8c00a60b1ce1d7e819d7a431d7c90ea0e5f

F = FiniteField(p)
E = EllipticCurve(F, [-3, b])
G = E([G_x, G_y])
n = 39402006196394479212279040100143613805079739270465446667946905279627659399113263569398956308152294913554433653942643
L_n = int(n).bit_length()

def hash(content):
    return int(sha512(content.encode('utf-8')).hexdigest(), 16)

def high_bits(num):
    return num >> (512 - L_n)

e_1 = hash("Sample 1")
z_1 = high_bits(e_1)
s_1 = 4236413337427581354656035013952787010462233198203390781270198909336892530338488069185901948060447092648218593788236
r_1 = 24938502030180107216386469766852065246288224125868426655511703381743753302017557998605079980211072384160200110639748

e_2 = hash("Sample 2")
z_2 = high_bits(e_2)
s_2 = 13352141313849707030095182263181788632003460487449251167996626997839369450150338861546490897344246651964596395933127

k = int(mod(z_1 - z_2, n) / mod(s_1 - s_2, n))
f = mod(s_1 * k, n)
q = mod(f - z_1, n)
private_key = int(mod(q / r_1, n))
```

With that, we are now able to sign our own messages. 

```sage
e = hash('cat flag.txt')
z = high_bits(e)

(x, y, _) = k * G
r = mod(x, n)
s = mod(inverse_mod(k, n) * (z + r * private_key), n)

print((int(r), int(s)))
```

