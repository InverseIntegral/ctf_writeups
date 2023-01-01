# 05 - Missing gift

## Description

Level: Easy<br/>
Author: wangibangi

Like every year the elves were busy all year long making the best toys in Santas workshop. This year they tried some new
fabrication technology. They had fun using their new machine, but it turns out that the last gift is missing.

Unfortunately, Alabaster who was in charge of making this gift is not around, because he had to go and fulfill his scout
elf duty as an elf on the shelf.

But due to some very lucky circumstances the IT-guy elf was capturing the network traffic during this exact same time.

## Solution

For this challenge we are given [a pcap file](tcpdump.pcap). Opening it and looking at the traffic, we can see a large
file being transferred. Using File > Export Objects > HTTP we obtain the file [v22.gcode](hv22.gcode). GCODE files
describe how a 3D printer should print a job. We can simulate this online:

![image.png](image.png)
