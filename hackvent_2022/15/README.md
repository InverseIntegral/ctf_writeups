# 15 - Message from Space

## Description

Level: Hard<br/>
Author: cbrunsch

One of Santa's elves is a bit of a drunkard and he is incredibly annoyed by the banning of beer from soccer stadiums. He
is therefore involved in the "No Return to ZIro beer" community that pledges for unrestricted access to hop brew during
soccer games. The topic is sensitive and thus communication needs to be top secret, so the community members use
a special quantum military-grade encryption radio system.

Santa's wish intel team is not only dedicated to analyzing terrestrial hand-written wishes but aims to continuously
picking up signals and wishes from outer space too. By chance the team got notice of some secret radio communication.
They notice that the protocol starts with a preamble. However, the intel team is keen to learn if the message is some
sort of wish they should follow-up. Can you lend a hand?

## Solution

For this challenge we are given a file that recorded some transmission. The description hints at the NRZI encoding. One
very useful program to solve this challenge is [Universal Radio Hacker](https://github.com/jopohl/urh). We can import
the file there, set the correct encoding and read out the data as ASCII to obtain `SFYyMnt2LXdpc2gtdi1nMHQtYjMzcn0=`.
Now we just have to Base64 decode it to get the flag `HV22{v-wish-v-g0t-b33r}`.
