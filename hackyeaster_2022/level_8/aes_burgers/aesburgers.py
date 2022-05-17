#!/usr/bin/python3
import sys
import os
from Crypto.Cipher import AES
from base64 import b64encode

flag = b'--REDACTED--'
key = '--REDACTED--'

def encrypt(plain):
    cipher = AES.new(key.encode(), AES.MODE_ECB)
    plain += b' ' * ((16 - (len(plain) % 16)) % 16)
    enc = cipher.encrypt(plain)
    return ''.join('%02x' % b for b in enc)

def makeBurger(bun, patties):
    burger = b''
    burger += bun[::-1] # the bottom bun, flipped
    burger += patties * flag # the patties
    burger += bun # the top bun
    return burger

def main():
    print('-- Welcome to AES Burgers!!! --')
    print(' - where the patty is tatty™ - ')
    print('  ---------------------------  ')
    while(1):
        try:
            patties = int(input("How many patties: "))
            if (patties < 1 or patties > 24):
                print('That won''t work  ¯\_ツ_/¯')
                exit()
            bun = input("Which bun? ").strip().encode()
            if len(bun) != 16:
                print('We don''t have that, sorry.  ¯\_ツ_/¯')
                exit()
            burger = makeBurger(bun, patties)
            print('Here''s your order, enjoy!')
            print(encrypt(burger))
        except ValueError:
            print('Whaaaaat?? ¯\_ツ_/¯')
            exit()

if __name__ == '__main__':
    main()
