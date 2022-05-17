# C2 Traffic

## Description
Level: medium<br/>
Author: otaku

We have detected C2 payloads on one of our servers! The blue team have extracted its communications from the traffic logs, and Operations have dumped the payload code from the running process.
Find out what the actors have exfiltrated!

[c2traffic.zip](c2traffix.zip)

## Solution

For this challenge we are given a python file as well as a JSON file that logged some traffic. The relevant python part
is:

```python
def decrypt(cipher, e):
    return unpad(cipher.decrypt(standard_b64decode(e)), 16)

def handle(j):
    if cipher is None:
        p = base64_to_long(j['p'])
        g = base64_to_long(j['g'])
        A = base64_to_long(j['A'])
        b = randint(1, p)
        shared = pow(A, b, p)
        shared = sha256(long_to_bytes(shared)).digest()
        cipher = AES.new(shared, AES.MODE_ECB)
        return {
            'B': long_to_base64(pow(g, b, p))
        }
    
    cmd = decrypt(cipher, j['rpc'])

    return {
        'return': encrypt(cipher, subprocess.check_output(cmd))
    }
```

We can see that a shared secret is first calculated and using said secret an AES decryption is performed.
`shared` is calculated as `A^b mod p` and `B` is equal to `g^b mod p`. Since we want to find `shared` and we know `g`,
`B` and `p`, we can simply solve for `b` and then use that to calculate `shared`:

```sage
def decrypt(cipher, e):
    return unpad(cipher.decrypt(standard_b64decode(e)), 16)

p = bytes_to_long(base64.b64decode("h3rl/Q=="))
g = bytes_to_long(base64.b64decode("Ag=="))
A = bytes_to_long(base64.b64decode("QpFOyA=="))
B = bytes_to_long(base64.b64decode("Ph6IeA=="))

F = GF(p)
b = int(F(B).log(F(g)))

shared = pow(A, b, p)
shared = sha256(long_to_bytes(shared)).digest()
cipher = AES.new(shared, AES.MODE_ECB)

print(decrypt(cipher, "3eWXhpQagWGMlfc71Qxd2QMvy4EVIyLfP54Jm6lpyHot6Qz+U7t3q2DdKnOxZBQf"))
```

This prints the flag `he2022{wh4dy4_m3an_32_b1t5_1s_1n53cur3}`.

