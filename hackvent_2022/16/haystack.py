from PIL import Image
import glob
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
