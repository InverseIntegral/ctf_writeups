# 22 - The command ... is lost

## Description

Level: leet<br/>
Author: inik

Santa bought this gadget when it was released in 2010. He did his own DYI project to control his sledge by serial
communication over IR. Unfortunately Santa lost the source code for it and doesn't remember the command needed to send
to the sledge. The only thing left is this file: [thecommand7.data](thecommand7.data)

Santa likes to start a new DYI project with more commands in January, but first he needs to know the old command. So,
now it's on you to help out Santa. 

## Solution

After some research I found out that the file was in [Intel HEX](https://en.wikipedia.org/wiki/Intel_HEX) format. My
first idea was to simply simulate the instructions and for that I found [simavr](https://github.com/buserror/simavr).
Now I only had to find the correct AVR microcontroller. I also read that Arduinos use AVR microcontrollers and so I
checked their releases of 2009 and found that they used an ATmega168 and ATmega328P processor in those versions. With
`simavr -f 1 -m atmega168 thecommand7.hex` I got the flag `HV19{H3y_Sl3dg3_m33t_m3_at_th3_n3xt_c0rn3r}`.
