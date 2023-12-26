# 11 - Santa's Pie

## Description

Level: Medium<br/>
Author: coderion

Santa baked you a pie with hidden ingredients!

## Solution

The category of this challenge was forensics and we were given a png image of a green cake. It's clear that this
challenge would have to do with some form of steganography. I immediately loaded the file in a Python script and looked
at the RGB values. The R values were simply the digits of the constant Pi. The B values were also interesting as they
all were in the range 32 - 127 (printable ASCII). Printing the B values didn't lead to the flag yet. But if we XOR the R
and B values, we obtain a readable text:

```python
from PIL import Image

img = Image.open('cake.png')
pixels = img.load()

flag = ''
for x in range(img.width):
    for y in range(img.height):
        flag += chr((pixels[x, y][2] ^ pixels[x, y][0]))

print(flag)
```

The script prints a long text of "Never gonna give you up" as well as the flag `HV23{pi_1s_n0t_r4nd0m}`. I have to admit
that I almost missed the flag there.
