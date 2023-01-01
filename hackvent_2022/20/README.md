# 20 - § 1337: Use Padding 📝

## Description

Level: Hard<br/>
Author: kuyaya

Santa has written an application to encrypt some secrets he wants to hide from the outside world. Only he and his elves,
who have access too all the keys used, can decrypt the messages 🔐.

Santa's friends Alice and Bob have suggested that the application has a padding vulnerability❗, so Santa fixed it 🎅.
This means it's not vulnerable anymore, right❗❓

## Solution

For this challenge we are given a short python script as well as a remote service.
We can call the service and let it prepend our input to the flag which is then encrypted:

```python
def pad(msg):
    if len(msg) % 16 != 0:
        msg = msg[::-1].zfill(len(msg) - len(msg) % 16 + 16)[::-1]
    return msg

while True:
    aes = AES.new(urandom(16), AES.MODE_ECB)
    msg = input("Enter access code:\n")
    enc = pad(msg) + pad(flag)
    enc = aes.encrypt(pad(enc.encode()))
    print(enc.hex())
```

The custom pad function immediately jumps out. Why would the author of the challenge not use the one provided by the
pycryptodome library. There must be something wrong with the implementation. The challenge description hints at special
unicode points. I remembered a challenge that I had previously solved where we had to abuse special unicode quirks to
pass longer input than expected and my suspicion was that we could do the same here.

My intuition was right:
```python
flag = "HV22{FLAGFLAGFLAGFLAGFLAG}"

def split(msg, delim=16):
    l = len(msg) // delim
    for i in range(l):
        lower = i * delim
        upper = (i+1) * delim
        print(lower, upper, msg[lower:upper])

aes = AES.new(urandom(16), AES.MODE_ECB)
msg = '0' * 14 + 'HV' + 'A' * 4 + 14 * "®"
enc = pad(msg) + pad(flag)
split(pad(enc.encode()))
enc = aes.encrypt(pad(enc.encode()))
split(enc.hex(), 32)
```

would print:

```
0 16 b'00000000000000HV'
16 32 b'AAAA\xc2\xae\xc2\xae\xc2\xae\xc2\xae\xc2\xae\xc2\xae'
32 48 b'\xc2\xae\xc2\xae\xc2\xae\xc2\xae\xc2\xae\xc2\xae\xc2\xae\xc2\xae'
48 64 b'00000000000000HV'
64 80 b'22{FLAGFLAGFLAGF'
80 96 b'LAGFLAG}00000000'

0 32 1cf05805ffe2a3ac2c146804a4ba0fff
32 64 e9ffa4f247e318e1f5ca3dbbce5d3ede
64 96 bd8790dbeaee1534a9fbb7370c84537f
96 128 1cf05805ffe2a3ac2c146804a4ba0fff
128 160 35c7ad70248e0cef9c3ca791b045d818
160 192 3266beadea0f5adeadfdc616d6010db9
```

And with this, we are able to leak the first 2 characters of the flag. We simply compare block 0 and block 3 with each
other and go through all possible ASCII characters to find a match. If we do, we can move on to the next two characters.
The pattern for the next two characters is:

```python
msg = '0' * 12 + 'HV22' + 'A' * 8 + 12 * "®"
```

We can repeat this pattern (increase the amount of `A`s by 2 and decrease the amount of `®`s by 2) until we have a full
block decrypted. From there I simply guessed the rest of the flag which was quite short and obtained `HV22{len()!=len()}`.

This challenge shows that it's a bad idea to write your own padding function. And that the length of a string changes
when it is reversed (and the length function is naively implemented).

