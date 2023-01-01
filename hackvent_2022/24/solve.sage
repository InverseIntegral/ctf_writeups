#!/usr/bin/env python3
# -*- coding: utf-8 -*-

from Crypto.Util.number import long_to_bytes

from pwn import *
import re

def decode(message):
    res = []

    # split into groups of 8 bytes
    for i in range(0, len(message), 16):
        block = message[i:i + 16]

        # little endian
        block = p64(int(block, 16)).hex()

        # revert to us
        s2 = block[:8]
        s1 = block[8:]
        
        u0 = int(s2, 16) - int(s1, 16)
        u1 = int(s2, 16) - 2 * u0

        u0 = u0 & ((1 << 32) - 1)
        u1 = u1 & ((1 << 32) - 1)

        first = p64(u0, endian="little").hex()
        second = p64(u1, endian="little").hex()
        res.append(first[:8] + second[:8])

    decoded = ''
    for i in range(8):
        for part in res:
            decoded += part[i * 2:(i+1) * 2]

    return decoded

def coppersmith(p_high):
    p_high //= 2
    print("Found potential p: " + str(hex(p_high)))

    P.<x> = PolynomialRing(Zmod(n))
    f = p_high + x

    sol = f.small_roots(beta=0.5)

    if sol != []:
        io.recvline()
        io.sendline(b"n")
        flag = io.recvline()

        flag = re.search('And here is your flag: (.*)\n', flag.decode('utf-8'))
        ct = int(flag.group(1), 16)
        
        p = p_high + sol[0]
        q = int(n) // int(p)
        
        assert(p.is_prime())
        assert(ZZ(q).is_prime())
        assert(p * q == n)
        
        e = 0x10001
        d = pow(e, -1, (p - 1) * (q - 1))
        m = pow(ct, d, n)
        print(long_to_bytes(m))
        exit()

cache = {}

def T(num):
    global current_energy
    num = hex(num)[2:].zfill(72 * 2)

    global cache
    if num in cache:
        return cache[num]

    to_send = decode(num)

    io.sendline(b'y')
    io.recvline()

    io.sendline(to_send)
    io.recvline()

    io.recvline()
    energy = io.recvline()
    
    remaining_energy = re.search('Santa has (.*) energy remaining. Don\'t over-tax santa\n', energy.decode('utf-8'))
    remaining_energy = int(remaining_energy.group(1))
    diff = current_energy - remaining_energy
    cache[num] = diff
    current_energy = remaining_energy
    return diff

offsets = {}

def phase2(left, right):
    global offsets
    print(hex(left)[2:], hex(right)[2:])

    # Runs until we have at least 192 top bits
    if (right - left <= 2**(192)):
        coppersmith(left)
 
    while True:
        mid = (right + left) // 2

        if is_in(left, mid):
           (good, res) = phase2(left, mid)
           if good:
               return (True, res)
        elif is_in(mid, right):
           (good, res) = phase2(mid, right)
           if good:
               return (True, res)
        else:
           # Backtrack case
           return (False, None)

# Returns false if in case A
def is_in(left, right):
    global offsets
    orig_left = left
    orig_right = right
    key = (left, right)

    if key in offsets:
        left += offsets[key]
        right += offsets[key]

    N = 7
    counter = 0
    while (True):
        diff = T(right) - T(left)
        if diff < -0.25 * c_ER * (768 / (b * 2 ** (b + 1)) + 2**b - 3):
            counter = counter - 1
        else:
            counter = counter + 1

        if (counter  >= N):
            offsets[(orig_left, orig_right)] = left - orig_left
            return False
        elif (counter <= -1 * N):
            offsets[(orig_left, orig_right)] = left - orig_left
            return True
        left += 1
        right += 1

### Interaction stuff ###
url = "XXXX.rdocker.vuln.land"
io = remote(url, 5825)

io.recvline()
io.recvline()
modulus = io.recvline()

m = re.search('modulus: n = (.+)\n', modulus.decode('utf-8'))
n = int(m.group(1)[2:], 16)
e = 0x10001
io.recvline()

prime = io.recvline()
m = re.search('prime: p = 0x(.+)...\n', prime.decode('utf-8'))
p = int(m.group(1), 16)

n_bits = 768
p_bits = 384
p_known_bits = 160

io.recvline()
io.recvline()

current_energy = 75000000000
c_ER = 400
b = 4

# lower and upper bound
shift = p_bits - p_known_bits
left = p << shift
right = (p + 1) << shift

phase2(2 * left, 2 * right)

