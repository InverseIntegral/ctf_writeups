# ONCE UPON A TIME

Category: Crypto<br/>
Author: ykm11

Now!! Let the games begin!!

[problem.py](problem.py)<br/>
[result.txt](result.txt)

## Solution

From the python script we can see that the flag gets split into 5x5 matrices:

```python
mat = []
for i in range(0, len(text), 25):
    mat.append([
        [text[i], text[i+1], text[i+2], text[i+3], text[i+4]],
        [text[i+5], text[i+6], text[i+7], text[i+8], text[i+9]],
        [text[i+10], text[i+11], text[i+12], text[i+13], text[i+14]],
        [text[i+15], text[i+16], text[i+17], text[i+18], text[i+19]],
        [text[i+20], text[i+21], text[i+22], text[i+23], text[i+24]],
        ])
return mat
```

The function `takenoko` performs a matrix multiplication:

```python
def takenoko(X, Y):
    W = [[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0]]
    for i in range(5):
        for j in range(5):
            for k in range(5):
                W[i][j] = (W[i][j] + X[i][k] * Y[k][j]) % 251
    return W
```

Here we can see that either `m2` is multiplied with `mat` or vice-versa.
Note that matrix multiplication is not commutative.

```python
g = random.randint(0,1)
if g == 0:
    mk = takenoko(m2, mat)
else:
    mk = takenoko(mat, m2)
```

The encrypted flag is 100 characters long so we know that there must be two matrix multiplications happening. To undo
the multiplication we simply have to solve `M2 * Mat = R` for `Mat` in the finite field `GF(251)`.

```python
from sage.all import *

encrypted = 'ea5929e97ef77806bb43ec303f304673de19f7e68eddc347f3373ee4c0b662bc37764f74cbb8bb9219e7b5dbc59ca4a42018'
nums = []

# Split encrypted into groups of two characters and convert them to ints
for i in range(0, len(encrypted), 2):
  nums.append(int(encrypted[i:i+2], 16))

R = []
C = []

# Create 5x5 matrices
for i in range(0, len(nums)):
  if (i % 5 == 0 and i != 0):
    R.append(C)
    C = [nums[i]]
  else:
    C.append(nums[i])

R.append(C)

m2 = [[1,3,2,9,4], [0,2,7,8,4], [3,4,1,9,4], [6,5,3,-1,4], [1,4,5,3,5]]

GF251 = Zmod(251)
RM = Matrix(GF251, R[0:5])
RM2 = Matrix(GF251, R[5:])
M2 = Matrix(GF251, m2).inverse()

flag = ''
for row in RM * M2:
  for element in row:
    flag += chr(element)

for row in RM2 * M2:
  for element in row:
    flag += chr(element)

print flag
```

This gives the flag: `HarekazeCTF{Op3n_y0ur_3y3s_1ook_up_t0_th3_ski3s_4nd_s33}`
