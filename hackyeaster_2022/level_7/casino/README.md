# Casino

## Description
Level: hard<br/>
Author: ice

Wanna try your luck in our new casino?
To prove we're not cheating, we are publishing our source code.
Connect to the server and start gamblin'!

## Solution

We are given the following sage script for this challenge:

```sage
from random import randint
from secrets import flag
from Crypto.Cipher import AES
from Crypto.Hash import SHA256
from Crypto.Util.Padding import pad

class RNG:
    def __init__(self):
        p = 115792089210356248762697446949407573530086143415290314195533631308867097853951
        b = 0x5ac635d8aa3a93e7b3ebbd55769886bc651d06b0cc53b0f63bce3c3e27d2604b
        self.curve = EllipticCurve(GF(p), [-3,b])

        self.P = self.curve.lift_x(15957832354939571418537618117378383777560216674381177964707415375932803624163)
        self.Q = self.curve.lift_x(66579344068745538488594410918533596972988648549966873409328261501470196728491)
        
        self.state = randint(1, 2**256)
        
    def next(self):
        r = (self.state * self.P)[0].lift()
        self.state = (r * self.P)[0].lift()
        return (r * self.Q)[0].lift() >> 8

class Casino:
    def __init__(self, rng):
        self.rng = rng
        self.balance = 10

    def play(self):
        print("Your bet: ", end='')
        bet = input()
        if (bet in ["0", "1"]):
            bet = Integer(bet)
            if (self.rng.next() % 2 == bet):
                self.balance += 1
            else:
                self.balance -= 1
                if (self.balance == 0):
                    print("You are broke... play again")
                    exit()
            print(f"Your current balance: {self.balance}")
        else:
            print("Invalid bet option, use either 0 or 1")
            
    def buy_flag(self):
        if (self.balance >= 1337):
            key = SHA256.new(str(self.rng.next()).encode('ascii')).digest()
            cipher = AES.new(key, AES.MODE_ECB)
            print(cipher.encrypt(pad(flag.encode('ascii'), 16)).hex())
        else:
            print("No flag for the poor. Gamble more")

def main():
    rng = RNG()
    casino = Casino(rng)

    print("Welcome to the Casino")
    print(f"Your id is {rng.next()}")
    print("What would you like to do?")
    print("(p)lay and win some money")
    print("(b)uy the flag")

    while (True):
        print("> ", end='')
        option = input()

        if (not option in ["b", "p"]):
            print("Unknown option, use 'b' or 'p'")
        elif (option == "b"):
            casino.buy_flag()
        elif (option == "p"):
            casino.play()
```

We can see that the goal is to gamble and get a balance of `1337` to buy the flag. The casino uses a PRNG based on
elliptic curve cryptography using the P-256 curve. The PRNG is an implementation of
[Dual_EC_DRBG](https://en.wikipedia.org/wiki/Dual_EC_DRBG). The hint of the challenge states that the casino is run by
the NSA and they made sure that they can always win. This should point us into the right direction. There is actually a
backdoor that can be used to recover the internal state of the PRNG. [Other
writeups](https://born2scan.run/writeups/2021/03/15/UTCTF.html#sleeves) describe how to do that.

First we have to recover `d`. Using sage that can be done quite fast:

```sage
p = 115792089210356248762697446949407573530086143415290314195533631308867097853951
b = 0x5ac635d8aa3a93e7b3ebbd55769886bc651d06b0cc53b0f63bce3c3e27d2604b

curve = EllipticCurve(GF(p), [-3,b])
P = curve.lift_x(15957832354939571418537618117378383777560216674381177964707415375932803624163)
Q = curve.lift_x(66579344068745538488594410918533596972988648549966873409328261501470196728491)

d = Q.discrete_log(P)
```

From the server we get one complete output of the PRNG at the start:
```sage
print(f"Your id is {rng.next()}")
``` 

We can now use this and recover the internal state of the PRNG. Note that there are multiple candidates (256 in total).
To find the correct one, we simply gamble a few times and compare the actual output (which is taken mod 2) to our
guessed output. With high probability we will find the correct internal state. Once we know which state is the correct
one, we always win and are able to recover the flag.

This is the complete exploit:

```sage
import socket
from Crypto.Cipher import AES
from Crypto.Hash import SHA256
from Crypto.Util.Padding import pad
from Crypto.Util.number import long_to_bytes

hostname = 'localhost'
port = 1337

s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
s.connect((hostname, port))

# Get the inital leak
msg = s.recv(1024)
r1 = int(msg.split(b"Your id is ")[1].split(b"\n")[0])

reference_stream = []
balance = 10

# Generate a random stream based on the PRNG numbers modulo 2
for _ in range(8):
    s.sendall(b'p\n')
    s.recv(1024)
    s.sendall(b'1\n')

    response = s.recv(1024)
    new_balance = int(response.split(b"balance: ")[1].split(b"\n")[0])

    if new_balance > balance:
       reference_stream.append(1)
    else:
       reference_stream.append(0)

    balance = new_balance

print(f"Reference stream: {reference_stream}")

# Copy & Paste from the challenge
class RNG:
    def __init__(self, state):
        p = 115792089210356248762697446949407573530086143415290314195533631308867097853951
        b = 0x5ac635d8aa3a93e7b3ebbd55769886bc651d06b0cc53b0f63bce3c3e27d2604b
        self.curve = EllipticCurve(GF(p), [-3,b])

        self.P = self.curve.lift_x(15957832354939571418537618117378383777560216674381177964707415375932803624163)
        self.Q = self.curve.lift_x(66579344068745538488594410918533596972988648549966873409328261501470196728491)
        
        self.state = state
        
    def next(self):
        r = (self.state * self.P)[0].lift()
        self.state = (r * self.P)[0].lift()
        return (r * self.Q)[0].lift() >> 8

p = 115792089210356248762697446949407573530086143415290314195533631308867097853951
b = 0x5ac635d8aa3a93e7b3ebbd55769886bc651d06b0cc53b0f63bce3c3e27d2604b

curve = EllipticCurve(GF(p), [-3,b])
P = curve.lift_x(15957832354939571418537618117378383777560216674381177964707415375932803624163)
Q = curve.lift_x(66579344068745538488594410918533596972988648549966873409328261501470196728491)

# This only needs to be done once, takes a few seconds
d = Q.discrete_log(P)

# Get a stream based on a state
def get_stream(state):
    r = RNG(state)
    l = []
    
    for _ in range(8):
        l.append(r.next() % 2)
        
    return l

# Go through all possible states (256 possibilities)
r_x_guess = r1 << 8

for i in range(0, 2**8):
    try:
        R = curve.lift_x(r_x_guess)
        state = Integer((d * R)[0])
        print(f"Possible state {state}")

        # If the two streams match we likely found the correct one
        if (get_stream(state) == reference_stream):
            print(f"found state {i}")

            rng = RNG(state)

            # Skip over the first 8 states since they have been used already
            for _ in range(8):
                rng.next()

            # Gamble enough to be able to buy the flag
            for _ in range(1337 + 8):
                s.sendall(b'p\n')
                s.recv(1024)

                if (rng.next() % 2 == 0):
                    s.sendall(b'0\n')
                else:
                    s.sendall(b'1\n')
            
                response = s.recv(1024)
                new_balance = int(response.split(b"balance: ")[1].split(b"\n")[0])
                print(new_balance)

            # Buy the flag
            s.sendall(b'b\n')
            ct = int(s.recv(1024).split(b'\n')[0], 16)
            print(f"Bought the flag and got the ciphertext: {ct}")

            key = SHA256.new(str(rng.next()).encode('ascii')).digest()
            cipher = AES.new(key, AES.MODE_ECB)
            print(f"Flag: {cipher.decrypt(long_to_bytes(ct))}")
            exit()
    except:
        pass
    r_x_guess += 1

s.shutdown(socket.SHUT_WR)
s.close()
```

