# Day 20: I want to play a game

In this challenge we get an nro file. It turns out that it's a game of the Switch console. When loading the game in the
[yuzu emulator](https://yuzu-emu.org/) we get the instructions to encrypt the following text:
`f42df92b389fffca59598465c7a51d36082ecfea567a900e5eac9e5e9fb1`. Whilst searching through the binary I found the
following string `shuffle*whip$crush%squeeze`. Those are names for the basic functions of the [Spritz
cipher](https://hackage.haskell.org/package/spritz-0.1.0.0/docs/src/Crypto-Cipher-Spritz.html).

Thw following python program encrypts our text and prints the flag:

```python
from spritz import Spritz
import binascii

spritz = Spritz()

K = bytearray('shuffle*whip$crush%squeeze\0')
M = bytearray.fromhex('f42df92b389fffca59598465c7a51d36082ecfea567a900e5eac9e5e9fb1')

C = spritz.encrypt(K, M)

print C
```
