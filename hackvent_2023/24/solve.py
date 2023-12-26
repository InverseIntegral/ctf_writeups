import struct

from PIL import Image, ImageOps


def plot(qr, location):
    pixels = []

    for current in qr:
        mask = 128

        while mask != 0:
            output = 1
            output2 = 1

            # the mask here goes 128, 32, 8, 2
            if (mask & current) != 0:
                output = 0

            # the mask here goes 64, 16, 4, 1
            if ((mask // 2) & current) != 0:
                output2 = 0

            mask = mask // 4
            pixels.append(output * 255)
            pixels.append(output2 * 255)

    data = struct.pack('B' * len(pixels), *pixels)
    img = Image.frombuffer('L', (32, 29), data)
    ImageOps.expand(img, border=10, fill='white').save(location)  # add a white border just in case the scanner doesn't recognize it


# D6, D0, D4, D5
# row, col, n, direction
def rotate_inner(qr, row, col, n, direction):
    for _ in range(n):
        if direction == 0:
            # rotate column D0 upward starting at the end of the column
            if col != -1:
                index = (28 * 4) + col
                a = qr[index]

                for _ in range(28):
                    qr[index] = qr[index - 4]
                    index -= 4

                qr[index] = a

            # rotate row D6 to the right starting at the beginning of the row
            if row != -1:
                index = row * 4
                a = qr[index]

                for _ in range(3):
                    qr[index] = qr[index + 1]
                    index += 1

                qr[index] = a

        else:
            # rotate row D6 to the left starting at the end of the row
            if row != -1:
                index = 3 + row * 4
                a = qr[index]

                for _ in range(3):
                    qr[index] = qr[index - 1]
                    index -= 1

                qr[index] = a

            # rotate column D0 downward starting at the start of the column
            if col != -1:
                index = col
                a = qr[index]

                for _ in range(28):
                    qr[index] = qr[index + 4]
                    index += 4

                qr[index] = a
    return qr


# from 0x0021f186
lookup_table = bytes.fromhex(
    "ff0301000aff01000bff0100ff0101010dff01000eff0100ff010101ff0201011102010012ff010013ff0100ff0102010eff01000aff0100ff010100ff0301000d020100ff0101000f01010114010101ff020100ff02010113ff0100ff010101")


def rotate(qr):
    current = 0
    for _ in range(24):
        D6 = lookup_table[current]
        D0 = lookup_table[current + 1]
        D4 = lookup_table[current + 2]
        D5 = lookup_table[current + 3]

        # signed byte
        if D6 == 0xff:
            D6 = -1

        # signed byte
        if D0 == 0xff:
            D0 = -1

        qr = rotate_inner(qr, D6, D0, D4, D5)
        current += 4

    return qr


current_qr = bytearray.fromhex(
    "fea7f57082773b40bac86a90bae31a08bac2ca7082ac72e8fe0b2ac000d0ab08fbaa2070e588cd004faad2cdcb58b6d98f10dbf62dd59be945698bd89cc5683f1322e70be9d9a8d800a071849eac83f8a4f008a00050efe8fe9e08f882ad9ae8ba43b880baf9cff8bab8acf082b09b50fe2b29d0")

for i in range(10_000):
    current_qr = rotate(current_qr)
    plot(current_qr, f"./out/{str(i)}.png")
