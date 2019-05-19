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
