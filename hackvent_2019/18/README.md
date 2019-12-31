# 18 - Dance with me

## Description

Level: Hard<br/>
Author: hardlock

Santa had some fun and created todays present with a special dance. this is what he made up for you:

```
096CD446EBC8E04D2FDE299BE44F322863F7A37C18763554EEE4C99C3FAD15
```
Dance with him to recover the flag. 

[HV19-dance.zip](93d0df60-3579-4672-8efc-f32327d3643f.zip)

## Solution

The zip file for this challenge contained a debian binary package. To unpack it I ran `ar -xv dance` and then `tar -xvf
data.tar.lzma`. From there I got a MACH-O binary. These are commonly used on iOS systems. From there I opened the binary
in Ghidra. From the challenge name I already guessed that the cipher could be
[ChaCha](https://en.wikipedia.org/wiki/Salsa20#ChaCha_variant) or [Salsa20](https://en.wikipedia.org/wiki/Salsa20) and
therefore I was search for specific constants in the decompiled code. I found the constant `0x61707865` which is part of
a Salsa20 implementation. All I had to do now is to find key and nonce in the binary.

The nonce of Salsa20 is 64-bit long so I looked for 16 hex characters and found `0xb132d0a8e78f4511`. Because this was a
little endian binary I swapped the endianness and got `0x11458fe7a8d032b1`. In the call to `dance` I also found
the key as the third argument. The key was previously constructed as:

```c
key[8] = 0x9bb500ea7ec276aa;
key[0] = 0xaf3cb66146632003;
key[24] = 0x46eeef0429ac57b2;
key[16] = 0x4cd04f2197702ffb;
```

By swapping the endianness I got the key `0320634661b63cafaa76c27eea00b59bfb2f7097214fd04cb257ac2904efee46`. The
following scrip then calculated the flag `HV19{Danc1ng_Salsa_in_ass3mbly}` and printed it:

```python
from Crypto.Cipher import Salsa20
import binascii

secret = binascii.unhexlify('0320634661b63cafaa76c27eea00b59bfb2f7097214fd04cb257ac2904efee46')
msg_nonce = binascii.unhexlify('11458fe7a8d032b1')
ciphertext = binascii.unhexlify('096CD446EBC8E04D2FDE299BE44F322863F7A37C18763554EEE4C99C3FAD15')
cipher = Salsa20.new(key=secret, nonce=msg_nonce)
plaintext = cipher.decrypt(ciphertext)

print(plaintext)
```
