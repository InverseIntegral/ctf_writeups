# misc/firecracker

## Description

Zip me up and crack me down

Downloads:
[CRACKME.jpg](CRACKME.jpg) [ENC.zip](ENC.zip)

## Solution

For this challenge we are given two files. An encrypted ZIP file that contains the flag as well as JPG file and the JPG
that is contained within the zip file. Since we know one file that is contained within the encrypted ZIP archive we can
perform a known plaintext attack. [PkCrack](https://www.unix-ag.uni-kl.de/~conrad/krypto/pkcrack.html) is a tool that
does exactly that:

``` 
./pkcrack -C ENC.zip -c CRACKME.jpg -P REF.zip -p CRACKME.jpg -d decrypted -a
```

This directly gives the flag `GLUG{CRAckERs_5hInE_BrI6h7_1n_THE_ni6ht_5Ky}`.

