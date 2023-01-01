# 03 - gh0st

## Description

Level: Easy<br/>
Author: 0xdf

The elves found this Python script that Rudolph wrote for Santa, but it's behaving very strangely. It shouldn't even run
at all, and yet it does! It's like there's some kind of ghost in the script! Can you figure out what's going on and
recover the flag?

## Solution

We are given the file [gh0st.py](gh0st.py) which contains some code that checks for the correct flag input.
Reversing the flag check as such

```python
correct = [17, 55, 18, 92, 91, 10, 38, 8, 76, 127, 17, 12, 17, 2, 20, 49, 3, 4, 16, 8, 3, 58, 67, 60, 10, 66, 31, 95, 1, 93]

for i,c in enumerate(correct):
    print(chr(ord(song[i*10 % len(song)]) ^ c), end='')
```

prints the flag `HV22{nUll_bytes_st0mp_cPy7h0n}`.
