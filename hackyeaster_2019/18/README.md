# 18 - Egg Storage

## Description

Level: medium<br/>
Author: 0xI

Last year someone stole some eggs from Thumper. This year he decided to use cutting edge technology to protect his eggs.

## Solution

As this was one of my challenges I will discuss the intended solution for this challenge. The source code of the
challenge can be found [here](egg_storage/). 

First of all we decompile the WebAssembly or reverse engineer the code by hand. There are three functions in total:
validateRange, validatePassword and decrypt

The first one checks if all characters of the password (starting at the fifth) are in `[48, 49, 51, 52, 53, 72, 76, 88, 99, 100, 102, 114]`.

ValidatePassword contains the first four characters hard-coded: `Th3P` and checks multiple conditions:

```javascript
m[19]               == m[13]
m[8]                == m[12]
m[18]               == m[11]
m[1] - m[3]         == 14
m[10] + 1           == m[11]
m[5] % m[4]         == 40
m[1] - m[5] + m[15] == 79
m[3] - m[10]        == m[16]
m[5] % m[0] * 2     == m[9]
m[9] % m[2]         == 20
m[7] % m[9]         == m[17] - 46
m[3] % m[2]         == m[6]
m[19] % m[18]       == 2
```

Furthermore, the sum of all ASCII values has to be 1352 and all ASCII values XORed should be 44.

With all those conditions we can write a Z3 solver, that prints the flag:

```python
def check():
    s =  Solver()

    def is_valid(x):
        return Or(
                (x == ord('0')),
                (x == ord('1')),
                (x == ord('3')),
                (x == ord('4')),
                (x == ord('5')),
                (x == ord('H')),
                (x == ord('L')),
                (x == ord('X')),
                (x == ord('c')),
                (x == ord('d')),
                (x == ord('f')),
                (x == ord('r')))

    key=""
    for i in range(0, 24):
        key += "key[{}] ".format(i)

    m = BitVecs(key, 8)

    for i in range(4, 24):
        s.add(is_valid(m[i]))

    s.add(m[0] == ord('T'))
    s.add(m[1] == ord('h'))
    s.add(m[2] == ord('3'))
    s.add(m[3] == ord('P'))
    s.add(m[4] == ord('4'))

    s.add(m[23] == m[17])
    s.add(m[12] == m[16])
    s.add(m[22] == m[15])

    s.add(m[5] - m[7] == 14)
    s.add(m[14] + 1 == m[15])
    s.add(m[9] % m[8] == 40)
    s.add(m[5] - m[9] + m[19] == 79)
    s.add(m[7] - m[14] == m[20])
    s.add(m[9] % m[4] * 2 == m[13])
    s.add(m[13] % m[6] == 20)
    s.add(m[11] % m[13] == m[21] - 46)
    s.add(m[7] % m[6]  == m[10])
    s.add(m[23] % m[22] == 2)

    # Those conditions can be found by running the z3 solver
    s.add(m[5] == ord('r'))
    s.add(m[6] == ord('4'))
    s.add(m[7] == ord('d'))
    s.add(m[8] == ord('0'))
    s.add(m[9] == ord('X'))

    while s.check() == z3.sat:
      model = s.model()

      out = ""
      nope = []

      for i in m:
          if str(i):
              out += chr(model[i].as_long() & 0xff)
          nope.append(i != model[i])

      # Ignore solutions that are equal to this one
      s.add(Or(nope[:-1]))

      sum = 0
      xor = 0

      for i in range(4, 24):
        sum += ord(out[i])
        xor ^= ord(out[i])

      print(out)

      if (sum == 1352 and xor == 44):
        print("Solution found: %s" % out)
        return

check()
```

The flag is `Th3P4r4d0X0fcH01c3154L13`.
