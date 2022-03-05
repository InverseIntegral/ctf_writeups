# crypto/daredevil

## Description

Matt-Murdock is one my favourite character from marvel universe.
I found his msg signing server can you help me to retrive his secret.

nc chall.nitdgplug.org 30093

## Solution

For this challenge we are given the following source code of the server:

```python
TOKEN = b'd4r3d3v!l'
def chall():
    s = Sign()
    while True:
        choice = input("> ").rstrip()
        if choice == 'P':
            print("\nN : {}".format(hex(s.n)))
            print("\ne : {}".format(hex(s.e)))
        elif choice == 'S':
            try:
                msg = bytes.fromhex(input('msg to sign : '))
                if TOKEN in msg:
                    print('[!] NOT ALLOWED')
                else:
                    m = bytes_to_long(msg)
                    print("\nsignature : {}".format(hex(s.sign(m))))      #pow(msg,d,n)
                    print('\n')
            except:
                print('\n[!] ERROR (invalid input)')

        elif choice == 'V':
            try:

                msg = bytes.fromhex(input("msg : "))
                m = bytes_to_long(msg)
                signature = int(input("signature : "),16)
                if m < 0 or m > s.n:
                    print('[!] ERROR')

                if s.verify(m, signature):                           #pow(sign, e, n) == msg
                    if long_to_bytes(m) == TOKEN:
                        print(SECRET)

                    else:
                        print('\n[+] Valid signature')

                else:
                    print('\n[!]Invalid signature')

            except:
                print('\n[!] ERROR(invalid input)')


        elif choice == 'Q':
            print('OK BYE :)')
            exit(0)
        else:
            print('\n[*] SEE OPTIONS')
```

The server uses RSA encryption and decryption to sign messages. Our goal is to sign the message `d4r3d3v!l` in order to
get the flag. The server allows us to sign arbitrary messages except the target message itself. Signing a messages
corresponds to RSA decryption. With this decryption oracle we now want to forge a valid signature. This is actually
quite simple. Instead of letting the server sign the token `m` directly, we sign the message `m * r^e`. The signature is
then `m^d * (r^e)^d = m^d * r`. If we multiply the signature with the inverse of `r` we can recover `m^d` which is the
signature that we want. All we have to do is to choose `r` such that it has an inverse in `n`. It turns out that `2` is
a suitable candidate for this.

With all this knowledge, we can get the valid signature:

```sage
from Crypto.Util.number import bytes_to_long, long_to_bytes

n = 0x8b50f9379dded6b6c7a98aa2fe5b092b0d23831349e4772aefca7dc200dff8e8bbf524b24b9cbf5fb3dc6bec9579b94628879b98804a0828b8e164109e7dd99e1178b620da8f8bd9405767126b9a51c6cb8b8191371db67ff2a25ec1382f790006ba6734e9a3c137796f32db11bc57188d1c5c741ddc65fa902dfcf8381ef745
e = 0x10001
r = 2

assert gcd(r, n) == 1

i = inverse_mod(r, n)

TOKEN = b'd4r3d3v!l'

m = bytes_to_long(TOKEN)
sig = mod(m * r^e, n)
print(long_to_bytes(int(sig)).hex())
```

With the response from the server we can then recover the valid signature:

```sage
received = 0x44f78cccfad638a7ea2b7ee0a2c518c3d235dfb54dde3b48087879d8c28cf1aac57e7c0a95958379475ca55a666a4fea90ff696abad6e7e7c44e8e7071ebe588905eb35551876009aebb2da1f28438aea1ea697bf63bb57bf2b1b301045af698251e71af747757e4ce9fb6319b76888015930a4c60e9890123788f31691f7566

res = mod(received * i, n)

print(TOKEN.hex())
print(hex(int(res))[2:])
```

