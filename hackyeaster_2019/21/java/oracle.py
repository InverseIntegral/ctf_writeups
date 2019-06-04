import random
import sys

initial = int(sys.argv[1])
current = initial

for i in range(1337):
  random.seed(current)
  current = random.randint(-(1337**42), 1337**42)

print (current)
