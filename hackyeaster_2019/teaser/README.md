# Teaser

## Description

[he2019_teaser.zip](he2019_teaser.zip)

## Solution

In this challenge we get a video with a lot of frames. Each frame consists of a single colour.
First of all I extracted the frames with ffmpeg:

```
ffmpeg -i he2019_teaser.mp4 -f image2 frames/frame%06d.jpg
```

Then I wrote [a Java program](Teaser.java) to go over all frames and take their colour. After that I put all the colours
together into one image. This gave me the QR Code:

![QR Code](solved.png)
