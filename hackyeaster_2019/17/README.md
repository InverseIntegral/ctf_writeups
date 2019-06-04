# 17 - New Egg Design

## Description
Level: medium<br/>
Author: darkice

Thumper is looking for a new design for his eggs.
He tried several **filters** with his graphics program, but unfortunately the QR codes got unreadable.
Can you help his?! 

![eggdesign](eggdesign.png)

## Solution

The hint from the challenge description lead me to [a writeup of a similar
challenge](https://github.com/ctfs/write-ups-2015/tree/master/confidence-ctf-teaser-2015/stegano/a-png-tale-200). PNG
images contain information about which filter was used before the image was compressed. [The filter types](https://www.w3.org/TR/PNG-Filters.html) can be used to
hide information inside an image.

With this in mind I wrote my own program that extracts the PNG filter information. To make this task a bit easier I used
[PNGJ](https://github.com/leonbloy/pngj/), a Java library for PNG encoding.

```java
public static void main(String[] args) throws Exception {
    PngReader pngReader = new PngReaderByte(new File("eggdesign.png"));
    StringBuilder binary = new StringBuilder();

    for (int row = 0; row < pngReader.imgInfo.rows; row++) {
        IImageLine line = pngReader.readRow();
        FilterType filterType = ((ImageLineByte) line).getFilterType();

        if (filterType == FilterType.FILTER_NONE) {
            binary.append("0");
        } else if (filterType == FilterType.FILTER_SUB) {
            binary.append("1");
        } else {
            throw new Exception("Unknown filter type");
        }

    }

    pngReader.end();

    StringBuilder flag = new StringBuilder();

    Arrays.stream(binary.toString().split("(?<=\\G.{8})"))
            .forEach(chunk -> flag.append((char) Integer.parseInt(chunk, 2)));

    System.out.println(flag.toString());
}
```

The text hidden inside the image was:

```
Congratulation, here is your flag: he19-TKii-2aVa-cKJo-9QCj
```
