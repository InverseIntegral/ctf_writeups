# show me your private key

Category: Crypto<br/>
Author: ykm11

I’ve exposed my private key… police is coming…

[problem.sage](problem.sage)<br/>
[result.txt](result.txt)

## Solution

This was a really interesting challenge. The flag is used to get the initial coordinates of point `G` on the elliptic curve `y^2 =
x^3 + b` over the ring of integers modulo `n`. Then a multiplication of `e` with this point `G` is performed:

```python
Gx = bytes_to_long(flag[len(flag)//2:])
Gy = bytes_to_long(flag[:len(flag)//2])

def getC2Prime(kbits):
    while True:
        p = getPrime(kbits)
        if p % 3 == 2:
            break
    return p


def gen_key(kbits):
    p = getC2Prime(kbits//2)
    q = getC2Prime(kbits//2)
    return 65537, p, q


e, p, q = gen_key(512)
d = inverse_mod(e, (p-1)*(q-1))
n = p*q
print "[+] (n, e, d) :", (n, e, d)

b = (pow(Gy, 2, n) - pow(Gx, 3, n)) % n
EC = EllipticCurve(Zmod(n), [0, b])

G = EC(Gx, Gy)
Cx, Cy = (e*G).xy()
print "[+] Cx:", Cx
print "[+] Cy:", Cy
```

So far we know `Cx`, `Cy`, `n`, `e` and `d`. We can calculcate `b` because we know the coordinates of a point on the
elliptic curve (`Cx`, `Cy`): 

```python
b = (pow(Cy, 2, n) - pow(Cx, 3, n)) % n
```

Now we have to find the inverse for `e` in `E_n(0, b)` to get (`Gx`, `Gy`). To do so we can use [this
property](https://math.stackexchange.com/questions/348262/inverse-scalar-multiplication-of-a-point-over-elliptic-curve#answer-348269).
We just need to find the cardinality of `E_n(0, b)`.

The cardinality of `E_n(0, b)` can be calculated by multiplying the cardinality of 
`Ep(0, b)` with `Eq(0, b)` where `n = p * q` and `p` and `q` are prime.
[[1]](https://link.springer.com/content/pdf/10.1007/3-540-46766-1_20.pdf)

Furthermore the cardinality of `Ep(0, b)` with `p = 3 mod 4` is simply `p + 1`.
[[1]](https://link.springer.com/content/pdf/10.1007/3-540-46766-1_20.pdf)

So the cardinality of `E_n(0, b)` is just `(p + 1) * (q + 1)`.
With that the inverse for `e` can be calculated and (`Cx`, `Cy`) calculated.

`p` and `q` can be calculated from the private key exponent `d` using [this Java
program](https://stackoverflow.com/questions/2921406/calculate-primes-p-and-q-from-private-exponent-d-public-exponent-e-and-the#answer-28299742).

```python
Cx = 4143446088312921816758362264853048120154280049677909632349103364802575463576509561464947871773793787896063253331418475283720886100034333135184249344102365
Cy = 8384037709829308179633895299138296616530497125381624381678499818112417287445046103971322133573513084823937517071462947639275474462359445732327289575301489

n = 9799080661501467884467225188078342742766492539290954649052326288545249523485259554498055327101620585612049935019772095457875188392850174807669467113561703
e = 65537

p = 92612344360820268580364307306616213661094478510448287251372441577708991857811
q = 105807500383793084625630519283985041680512853206910763583384270571251151476573

b = (pow(Cy, 2, n) - pow(Cx, 3, n)) % n
EC = EllipticCurve(Zmod(n), [0, b])

cardinality = (p - 1) * (q - 1)

RP = EC(Cx, Cy)
Cx, Cy = (inverse_mod(e, cardinality) * RP).xy()

print (Cx)
print (Cy)
```

Converting `Cx` and `Cy` to bytes and the printing them:

```python
from Crypto.Util.number import long_to_bytes

x = long_to_bytes(293287502352283012030139917140690694691558092157)
y = long_to_bytes(1614142473028995026146621731223303794610497908)

print(y + x)
```

The flag was `HarekazeCTF{dynamit3_with_a_las3r_b3am}`.
