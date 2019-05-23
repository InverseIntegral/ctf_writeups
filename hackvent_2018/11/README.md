### Day 11: Crypt-o-Math 3.0

We get the following formulas and values:

```
c = (a * b) % p
c = 0x7E65D68F84862CEA3FCC15B966767CCAED530B87FC4061517A1497A03D2
p = 0xDD8E05FF296C792D2855DB6B5331AF9D112876B41D43F73CEF3AC7425F9
b = 0x7BBE3A50F28B2BA511A860A0A32AD71D4B5B93A8AE295E83350E68B57E5
```

We have to find a. I remembered reading about this in last years writeup.
To solve the equation we can rewrite it:

`c * b^-1 mod p = a`

The following Java code solves the equation:

```java
BigInteger c = new BigInteger("7E65D68F84862CEA3FCC15B966767CCAED530B87FC4061517A1497A03D2", 16);
BigInteger p = new BigInteger("DD8E05FF296C792D2855DB6B5331AF9D112876B41D43F73CEF3AC7425F9", 16);
BigInteger b = new BigInteger("7BBE3A50F28B2BA511A860A0A32AD71D4B5B93A8AE295E83350E68B57E5",16);

BigInteger bInverse = b.modInverse(p);
BigInteger a = c.multiply(bInverse).mod(p);
```

This a doesn't give a valid flag when decoding it to ASCII. So I assumed that we have to take a different solution for
a. I wrote a program that gets me other valid solutions for a and checked if they start with HV

```java
BigInteger c = new BigInteger("7E65D68F84862CEA3FCC15B966767CCAED530B87FC4061517A1497A03D2", 16);
BigInteger p = new BigInteger("DD8E05FF296C792D2855DB6B5331AF9D112876B41D43F73CEF3AC7425F9", 16);
BigInteger b = new BigInteger("7BBE3A50F28B2BA511A860A0A32AD71D4B5B93A8AE295E83350E68B57E5",16);

BigInteger bInverse = b.modInverse(p);
BigInteger a = c.multiply(bInverse).mod(p);

int i = 1;
while (true) {
    BigInteger current = new BigInteger(a.toString());

    for (int j = 0; j < i; j++)  {
        current = current.add(p);
    }

    if (current.toString(16).startsWith("4856")) {
        System.out.println(current.toString(16));
        return;
    }

    i++;
}
```

After 1337 iterations I found a new solution which was the correct flag.
