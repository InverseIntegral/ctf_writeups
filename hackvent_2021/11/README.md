# 11 - Oversized Gifts

## Description

To ensure that Santa does not have to carry such a heavy load, our elves are always trying to shrink the gifts as much
as possible. New technologies are constantly being developed in our laboratories. Unfortunately, an incident occurred
during a test, when restoring the original size, an error occurred and now we are no longer able to achieve the original
size.

## Solution

For this challenge we are given a huge PNG file. Opening it isn't simply possible since it wouldn't even fit into
RAM. We have to scale it down somehow. I found two different solutions to this challenge.

### First Solution using VIPs

One solution uses [the image processing library vips](https://www.libvips.org/). We can simply call the following
command to resize the image and then read the QR code:

```
vips resize 72d85b7f-4325-432e-93ff-cfdc019306c6.png out.png 0.0078125 --vips-progress
```

### Second Solution using Java

Alternatively, we can just take the pixel data using a library. In my case, I took [PNGJ a java library for PNG
decoding](https://github.com/leonbloy/pngj). From a hint that was published during the competition, we know that the
original QR code (before scaling) was generated using `qrencode`. This means that the original image was of size
`111x111`. The scaled version was of size `909312x909312` and the scaling factor was `8192`. 

With this in mind, we can extract every 8192-th pixel in a grid-like fashion:

```java
PngReader pngReader = new PngReaderByte(new File("72d85b7f-4325-432e-93ff-cfdc019306c6.png"));
BufferedImage image = new BufferedImage(111, 111, BufferedImage.TYPE_INT_RGB);
int outputRow = 0;

for (int row = 0; row < pngReader.imgInfo.rows; row += 8192) {
    IImageLine line = pngReader.readRow(row);
    int outputCol = 0;

    for (int i = 0; i < ((ImageLineByte) line).getScanline().length; i += (4 * 8192)) {
        byte r = ((ImageLineByte) line).getScanline()[i];
        byte g = ((ImageLineByte) line).getScanline()[i + 1];
        byte b = ((ImageLineByte) line).getScanline()[i + 2];
        byte a = ((ImageLineByte) line).getScanline()[i + 3];
        int rgb = (r << 16 | g << 8 | b);

        image.setRGB(outputRow, outputCol, rgb);
        outputCol++;
    }

    outputRow++;
    ImageIO.write(image, "PNG", new File("done.png"));
}

pngReader.end();
```

