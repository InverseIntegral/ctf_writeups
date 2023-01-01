# 16 - Needle in a qrstack

## Description

Level: Hard<br/>
Author: dr_nick

Santa has lost his flag in a qrstack - it is really like finding a needle in a haystack.
Can you help him find it?

## Solution

This was an awesome challenge. We are given a large image that contains a lot of QR-codes within itself:

![haystack.png](haystack.png)

The goal is clear, we just have to scan all the codes and find the one that contains the flag. To do this, I measured
the size of the biggest blocks. They are arranged in a 25x25 grid. After that we can simply divide the cropped block
into four sub blocks again. We do this until we reach the smallest possible block size. At each step we scan the QR code
and print its content:

```python
from PIL import Image
from pyzbar.pyzbar import decode

Image.MAX_IMAGE_PIXELS = 615040000

# 400, 200, 100, 25
def cutIn4(image):
    delta = image.width // 2

    if (delta < 20):
        return

    for i in range(2):
        for j in range(2):
            x = i * delta
            y = j * delta
            box = image.crop((x, y, x + delta, y + delta))
            print(decode(box))
            cutIn4(box)


im = Image.open("haystack.png")

for i in range(25):
    for j in range(25):
        x = 2400 + i * 800
        y = 2400 + j * 800
        box = im.crop((x, y, x + 800, y + 800))
        print(decode(box))
        cutIn4(box)
```

And after a few seconds, we find the flag `HV22{1'm_y0ur_need13.}`.
