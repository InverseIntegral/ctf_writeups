# 18 - Santa's Nice List

## Description

Level: Hard<br/>
Author: keep3r

Santa stored this years "Nice List" in an encrypted zip archive. His mind occupied with christmas madness made him
forget the password. Luckily one of the elves wrote down the SHA-1 hash of the password Santa used.

```
xxxxxx69792b677e3e4c7a6d78545c205c4e5e26
```

Can you help Santa access the list and make those kids happy?

## Solution

For this challenge we are given a password protected zip [nice-list.zip](nice-list.zip). At first, I tried a few
password lists to crack the password. This, of course, didn't work. At this point I inspected the zip file details:

```
zipdetails nice-list.zip

0000 LOCAL HEADER #1       04034B50
0004 Extract Zip Spec      33 '5.1'
0005 Extract OS            03 'Unix'
0006 General Purpose Flag  0001
     [Bit  0]              1 'Encryption'
0008 Compression Method    0063 'AES Encryption'
000A Last Mod Time         557D12F4 'Tue Nov 29 03:23:40 2022'
000E CRC                   00000000
0012 Compressed Length     0000005D
0016 Uncompressed Length   00000041
001A Filename Length       0008
001C Extra Length          000B
001E Filename              'flag.txt'
0026 Extra ID #0001        9901 'AES Encryption'
0028   Length              0007
002A   Vendor Version      0002 'AE-2'
002C   Vendor ID           4541 'AE'
002E   Encryption Strength 03 '256-bit encryption key'
002F   Compression Method  0000 'Stored'
0031 AES Salt              E0 7F 14 DE 6A 21 90 6D 63 53 FD 5F 65
                           BC B3 39
0041 AES Pwd Ver           56 64
0043 PAYLOAD               ..C{..k.F....0Q....f..+3..C.-
                           sb..gM.&...s.b..9./d..............i]
0084 AES Auth              40 66 34 73 53 92 04 E3 CE FD
```

Unfortunately, the encryption used isn't suspectible to the [PkCrack
attack](https://www.unix-ag.uni-kl.de/~conrad/krypto/pkcrack.html). But it turns out that we can use the unusual
behaviour of the KDF ([PBKDF2](https://en.wikipedia.org/wiki/PBKDF2)) which is used in combination with HMAC-SHA1: If
the password length exceeds 64 bytes then the HMAC first hashes the password with SHA-1. And since we know the hash
(kindly given to us by santa), we are able to crack the password fairly easily. We simply generate all possible hashes,
interpret them as ASCII and then try to unpack the ZIP file.

`69792b677e3e4c7a6d78545c205c4e5e26` in ASCII is `iy+g~>LzmxT\ \N^&` which will be our known suffix of the password. We
then extract the hash of our zip file:

```shell
zip2john nice-list.zip

nice-list.zip/flag.txt:$zip2$*0*3*0*e07f14de6a21906d6353fd5f65bcb339*5664*41*e6f2437b18cd6bf346bab9beaa3051feba189a66c8d12b33e6d643c52d7362c9bb674d8626c119cb73146299db399b2f64e3edcfdaab8bc290fcfb9bcaccef695d*40663473539204e3cefd*$/zip2$:flag.txt:nice-list.zip:nice-list.zip
```

We generate all possible passwords as such:

```python
import string

hash_base = '69792b677e3e4c7a6d78545c205c4e5e26'
suffix = bytes.fromhex(hash_base).decode("ascii")

for a in string.printable:
    for b in string.printable:
        for c in string.printable:
            print(a + b + c + suffix)
```

And finally use john to crack the password:

```shell
john hash.txt --wordlist=word-list.txt
```

And just like that, we get the password `4Ltiy+g~>LzmxT\ \N^&` and we can extract the files. With this we get the flag
`HV22{HAVING_FUN_WITH_CHOSEN_PREFIX_PBKDF2_HMAC_COLLISIONS_nzvwuj}` as well as the nice list:

```
darkice
explo1t
darkstar
drschottky
smartsmurf
keep3r
0xi
mcia
jokker
logicaloverflow
engycz
daubsi
```

Looks like I made it on the list this year ;)

In retrospect it would have been easier to use hashcat direcly to solve the challenge:
`hashcat -m 13600 -a 3 hash.txt "?b?b?biy+g~>LzmxT\ \N^&`
