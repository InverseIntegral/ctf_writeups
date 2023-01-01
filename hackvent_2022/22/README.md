# 22 - Santa's UNO flag decrypt0r

## Description

Level: Leet<br/>
Author: explo1t

The elves made Santa a fancy present for this Christmas season. He received a fancy new Arduino where his elves encoded
a little secret for him. However, Santa is super stressed out at the moment, as the children's presents have to be sent
out soon. Hence, he forgot the login, the elves told him earlier. Can you help Santa recover the login and retrieve the
secret the elves sent him?

## Solution

For this challenge we are given an AVR binary [unoflagdecryptor.elf](unoflagdecryptor.elf) and from the challenge name
we can guess that it's made for an [Arduino Uno](https://en.wikipedia.org/wiki/Arduino_Uno). I opened the binary in
Ghidra to identify the interesting parts:

```c
correct = 0;

do {
  // Some black magic
  ...

  R20 = R20 ^ R25R24;

  if (R20 == R5R4) {
    correct++;
  }

} while (...);

if (correct == 0x21) {
  // Successful login
  ...
}
```

We can see that the password is checked character by character in a loop. If all `0x21` characters are correct, then the
flag gets decrypted and printed. All we have to do now, is to open the binary in a debugger and check the comparison of
`R20` and `R5R4`.

To debug I use `qemu` and `avr-gdb` which I run with these two commands:
```shell
qemu-system-avr -S -s -nographic -machine arduino-uno -bios main.elf
avr-gdb -ex 'target remote :1234' main.elf
```

I set a breakpoint at the relevant instructioins:

```
 0x00000648 <+428>:   eor     r20, r24
 0x0000064a <+430>:   cpse    r20, r5
 0x0000064c <+432>:   rjmp    .+2             ;  0x650 <main+436>
```

The first instruction is the XOR and after that the comparison happens. I set a breakpoint at `0x00000648` and check the
content of `r20` and `r24`:

```
(gdb) info registers r20
r20            0x73                115
(gdb) info registers r24
r24            0xf3                243
```

The first one contains our input (the ascii character 's'). If we step one instruction further and inspect `r5`, we see
the value that it's compared to:

```
(gdb) info registers r5
r5             0x80                128
```

It turns out that the content of `r5` can be found directly in the `.data` segment:
`80d7463c677b95516e676690359bd75a2c6571986b665773875997cc0969277bd5`. At this point, I simply stepped through the loop
and extracted the values of `r24` during the xor to obtain
`f3b628480641fc0e020810f56af3b628480641fc0e020810f56af3b628480641fc`. Then I simply xored the two together to get the
password `santa:i_love_hardc0ded_cr3dz!!!:)`. Entering this gave the flag `HV22{n1c3_r3v3r51n6_5k1llz_u_g07}`.
