from z3 import *
from timeit import default_timer as timer

def check():
    start = timer()
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

      # Th3P4r4d0X0fcH01c3154L13
      if (sum == 1352 and xor == 44):
        end = timer()
        print("Solution found: %s" % out)
        print(end - start)
        return

check()
