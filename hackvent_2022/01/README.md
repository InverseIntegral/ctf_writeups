# 01 - QR means quick reactions, right?

## Description

Level: Easy<br/>
Author: Deaths Pirate

Santa's brother Father Musk just bought out a new decoration factory. He sacked all the developers and tried making his
own QR code generator but something seems off with it. Can you try and see what he's done wrong?

![QR code GIF](hackvent2022_01.gif)

## Solution

We are given a gif that cycles through multiple QR codes. We simply extract the frames by running:

```shell
convert -coalesce hackvent2022_01.gif out%05d.png
```

And then we scan all the images:

```shell
zbarimg out*
```

which outputs

```
QR-Code:H
QR-Code:V
QR-Code:2
QR-Code:2
QR-Code:{
QR-Code:I
QR-Code:_
QR-Code:C
QR-Code:a
QR-Code:N
QR-Code:_
QR-Code:H
QR-Code:a
QR-Code:Z
QR-Code:_
QR-Code:A
QR-Code:l
QR-Code:_
QR-Code:T
QR-Code:3
QR-Code:h
QR-Code:_
QR-Code:Q
QR-Code:R
QR-Code:s
QR-Code:_
QR-Code:P
QR-Code:l
QR-Code:z
QR-Code:}
```

which we can concat to get the flag `HV22{I_CaN_HaZ_Al_T3h_QRs_Plz}`.
