#!/usr/bin/env python3

from Crypto.Cipher import AES
from os import urandom

# pad block size to 16, zfill() fills on left. Invert the string to fill on right, then invert back.
def pad(msg):
    if len(msg) % 16 != 0:
        msg = msg[::-1].zfill(len(msg) - len(msg) % 16 + 16)[::-1]
    return msg

flag = open('flag.txt').read().strip()

while True:
    aes = AES.new(urandom(16), AES.MODE_ECB)
    msg = input("Enter access code:\n")
    enc = pad(msg) + pad(flag)
    enc = aes.encrypt(pad(enc.encode()))
    print(enc.hex())

    retry = input("Do you want to try again [y/n]:\n")
    if retry != "y":
        break
