# 16 - B0rked Calculator

## Description

Level: Hard<br/>
Author: hardlock

Santa has coded a simple project for you, but sadly he removed all the operations.
But when you restore them it will print the flag!

[HV19.16-b0rked.zip](9b90c573-d530-401b-b3f8-24454bbf015e.zip)

## Solution

For this challenge we got a PE32 executable (GUI) which I directly opened in Ghidra. Whilst going through the functions
I realized that Ghidra wasn't able to define one function and so I did this manually. The important part of this
function was:

```c
if (DAT_00402138 == '+') {
  uValue = FUN_004015b6((char)uValue,extraout_DL_02,extraout_CL_02,DAT_00402120);
} else {
  if (DAT_00402138 == '-') {
    uValue = FUN_004015c4((char)uValue,extraout_DL_02,extraout_CL_02,(char)DAT_00402120,
                          uValue);
  } else {
    if (DAT_00402138 == '*') {
      uValue = FUN_004015d4();
    } else {
      if (DAT_00402138 == '/') {
        uValue = FUN_004015e4();
      }
    }
  }
}
```

This part handled the different operators of the calculator. I renamed the variables and functions to:

```c
 if (operator == '+') {
  left = add(left,right);
} else {
  if (operator == '-') {
    left = substract(left,right);
  } else {
    if (operator == '*') {
      left = times(left,right);
    } else {
      if (operator == '/') {
        left = divide(left,right);
      }
    }
  }
}
```

The function `add`, `subtract`, `times` and `divide` where all empty. The idea of the challenge was to patch them. I
simply searched for other locations where the functions were used and found:

```c
byte* flag = ...;
flag[0] = add(0x21ceb5d8, 0x1762a070);
flag[4] = substract(0xaae5b913, 0x38b57698);
flag[8] = divide(0xbec8cad6, 0x2);
flag[12] = times(0x33b0b623, 0x2);
flag[16] = add(0x18a3cd45, 0x53bd761a);
flag[20]  = substract(0xa8359657, 0x46c920f4);
flag[24] = times(0x1f5c8c1d, 0x4);
SetDlgItemTextA(param_4, 1000, flag);
```

With this I was able to directly calculate the flag `HV19{B0rked_Flag_Calculat0r}` and print it:

```python
from binascii import unhexlify
import struct

def unpack(value):
   print(struct.pack('>I', value).decode("utf-8")[::-1], end = '')

unpack(0x21ceb5d8 + 0x1762a070)
unpack(0xaae5b913 - 0x38b57698)
unpack(int(0xbec8cad6 / 0x2))
unpack(0x33b0b623 * 0x2)
unpack(0x18a3cd45 + 0x53bd761a)
unpack(0xa8359657 - 0x46c920f4)
unpack(0x1f5c8c1d * 0x4)
```
