# 07 - Santa Rider

## Description

Level: Easy<br/>
Author: inik

Santa is prototyping a new gadget for his sledge. Unfortunately it still has some glitches, but look for yourself.

[HV19-SantaRider.zip](3dbe0c12-d794-4f79-ae67-09ac27bd099d.zip)

## Solution

For this challenge we get an mp4 file which shows eight blinking LEDs. After a few iterations of a simple blinking
pattern the LEDs start blinking in a new pattern. I went through the video frame by frame and wrote down the state of
each LED (1 means on and 0 represents off). Converting the binary sequence to a string gives the flag
`HV19{1m_als0_w0rk1ng_0n_a_r3m0t3_c0ntr0l}`.
