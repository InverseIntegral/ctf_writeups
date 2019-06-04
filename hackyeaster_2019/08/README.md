# 08 - Modern Art

## Description

Level: easy<br/>
Author: readmyusername

Do you like modern art?

![Modern Art](modernart.jpg)

## Solution

To solve this challenge I obviously tried to turn the image into a valid QR code but this didn't lead to anything.
After this I looked at the image in HEX for that I used vim and the command `:%!xxd`.

This revealed a key: `(KEY=1857304593749584)` and a ciphertext `(E7EF085CEBFCE8ED93410ACF169B226A)`. The key was 16
characters long so I thought that it could be AES. I used [this online
tool](https://www.devglan.com/online-tools/aes-encryption-decryption) to decrypt the ciphertext. The plaintext was:
`Ju5t_An_1mag3`.
