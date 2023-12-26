# 23 - Roll your own RSA

## Description

Level: Leet<br/>
Author: cryze

Santa wrote his own script to encrypt his secrets with RSA. He got inspired from the windows login where you can specify
a hint for your password, so he added a hint for his own software. This won't break the encryption, will it?

## Solution

This was the second challenge that I got first blood on. To be fair, it was easier than one would expect from a leet
challenge. The challenge was developed within a few hours since the actual challenge didn't work on the
infrastructure provided for the CTF.

For this challenge we are given [roll-rsa.zip](roll-rsa.zip) that contains an output text file and the following Python
script:

```python
import random

from Crypto.Util.number import *
from sage.all import *
from secret import FLAG, x, y


# D = {x∈ℕ | 0 ≤ x ≤ 1000}
# D = {y∈ℕ | 0 ≤ y ≤ 1000}

def enc(flag, polynomial_function):
    p = getStrongPrime(512)
    q = getStrongPrime(512)
    N = p * q
    e = 65537
    hint = p ** 3 - q ** 8 + polynomial_function(x=x)
    encrypted = pow(bytes_to_long(flag), e, N)
    print(f"{N=}")
    print(f"{e=}")
    print(f"{hint=}")
    print(f"{encrypted=}")


def generate_polynomial_function(seed):
    x = SR.var("x")
    random.seed(seed)
    grade = random.choice([2, 3])
    a = random.randint(9999, 999999)
    b = random.randint(8888, 888888)
    c = random.randint(7777, 777777)

    if grade == 2:
        y_x = a * x ** 2 + b * x + c
    if grade == 3:
        d = random.randint(6666, 666666)
        y_x = a * x ** 3 + b * x ** 2 + c * x + d

    print(a + b + c)
    return y_x


y_x = generate_polynomial_function(y)
enc(FLAG.encode(), y_x)
```

We can see that the `FLAG` gets passed into an encryption function that uses RSA. We also see a secret `x` and `y` being
loaded. `y` is used as a seed for the PRNG, then a few values are generated and the sum of `a`, `b` and `c` is printed.
The polynomial `p(x)` is then returned and used with the other secret input `x`. Finally, we are
given `N, e, hint, encryped`. The hint is `p ** 3 - q ** 8 + p(x)`. We also know that `x, y` are integers
between 0 and 1000.

First things first, we try to bruteforce the values of `x` and `y` since they are so small, we could easily try all
possible combinations. Luckily, we can easily find a valid `y`:

```python
for y in range(1000):
    x = SR.var("x")
    random.seed(y)
    grade = random.choice([2, 3])
    a = random.randint(9999, 999999)
    b = random.randint(8888, 888888)
    c = random.randint(7777, 777777)

    if grade == 2:
        y_x = a * x ** 2 + b * x + c
    if grade == 3:
        d = random.randint(6666, 666666)
        y_x = a * x ** 3 + b * x ** 2 + c * x + d

    if (a + b + c == 1709262):
        print(y)
```

This prints only `787` so we already know half of the secret. All we need to do now, is to bruteforce `x` by
calculating `p, q` using the `hint`.

From the equality `hint == p ** 3 - q ** 8 + p(x)` we already know `hint` and `p(x)`. If we add the
equation `n == p * q` we have two equations as well as two unknowns. This is solvable using Sage!.
All we need to do is to re-arrange the formula into a polynomial `p'` that has a root at `q`. We
obtain `p'(q) = hint - p(x) - (N/q) ** 3 + q ** 8`. With this we can look for roots of the polynomial `p'` and
obtain `q`:

```sage
N=143306145185651132108707685748692789834391223254420921250753369412889732941905250889012412570851623535344424483564532684976892052830348014035303355261052741504390590825455129003262581199117432362073303998908141781601553213103109295898711066542593102305069363965164592663322089299134520383469241654273153506653
e=65537
hint=-367367861727692900288480576510727681065028599304486950529865504611346573250755811691725216308460956865709134086848666413510519469962840879406666853346027105744846872125225171429488388383598931153062856414870036460329519241754646669265989077569377130467115317299086371406081342249967666782962173513369856861858058676451390037278311316937161756731165929187543148639994660265783994439168583858109082136915810219786390452412584110468513829455001689531028969430907046738225668834761412112885772525079903072777443223873041260072918891696459905352737195384116938142788776947705026132197185926344278041831047013477983297898344933372775972141179163010102537733004410775357501267841845321271140399200044741656474378808452920297777911527159803159582800816951547394087190043792625664885536154225227819735800442814065528155407746556297892931242208688533313054308779657788077807340045465701247210553988059519291363634253248268722975827616752514688291723712069675405995149499947239454505797412122124933836396842943540518521648803348207619354854290787969076059265170474203200482079680136404766877617679652611682327535174212016390608658107555103054183393719700027186913354158961245998591486268846852581402900857595817303811471853325463202817521164757
encrypted=72792762778232160989381071629769766489971170790967414271032682193723004039685063639675377805724567838635943988752706743932748347933013530918279285456553768626331874756049006544553546268049053833014940495217179504587162478219970564159885619559723778613379425375733026859684952880028997538045791748027936366062
y = 787

for x in range(1000):
    try:
        q = SR.var('q')
        val = hint - (509736*x^3 + 671618*x^2 + 527908*x + 165945)
        poly = val - (N/q)**3 + q**8
        q = poly.roots()[0][0]
        p = N // int(q)
        phi = (p - 1) * (q - 1)
        d = inverse_mod(e, phi)
        flag = long_to_bytes(power_mod(encrypted, d, N))
        print(flag)
        break
    except:
        continue
```

This prints the flag `HV23{1t_w4s_4b0ut_t1m3_f0r_s0me_RSA_4g41n!}'`. The full solution script can be found
in [solve.sage](solve.sage). Luckily, this year's RSA challenges were much simpler 😌.
