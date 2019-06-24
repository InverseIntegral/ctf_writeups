# Work Computer

## Description

With the confidence of conviction and decision making skills that made you a contender for Xenon's Universal takeover
council, now disbanded, you forge ahead to the work computer. This machine announces itself to you, surprisingly with a
detailed description of all its hardware and peripherals. Your first thought is "Why does the display stand need to
announce its price? And exactly how much does 999 dollars convert to in Xenonivian Bucklets?" You always were one for
the trivialities of things.

Also presented is an image of a fascinating round and bumpy creature, labeled "Cauliflower for cWo" - are "Cauliflowers"
earthlings? Your 40 hearts skip a beat - these are not the strange unrelatable bipeds you imagined earthings to be..
this looks like your neighbors back home. Such curdley lobes. Will it be at the party?

SarahH, who appears to be a programmer with several clients, has left open a terminal. Oops. Sorry clients! Aliens will
be poking around attempting to access your networks.. looking for Cauliflower. That is, *if* they can learn to navigate
such things.

`readme.ctfcompetition.com 1337`

## Solution

Running `nc readme.ctfcompetition.com 1337` gives a remote shell with restricted binaries. Running `ls -la` returns:

```
total 12
drwxrwxrwt    2 0        0               80 Jun 23 20:59 .
drwxr-xr-x   20 0        0             4096 Jun 13 14:28 ..
----------    1 1338     1338            33 Jun 23 20:59 ORME.flag
-r--------    1 1338     1338            28 Jun 23 20:59 README.flag
```

Unfortunately `cat` is not available to print `README.flag`. I found two solutions for this challenge. `tar c
README.flag` and `fold README.flag` both print the flag `CTF{4ll_D474_5h4ll_B3_Fr33}`.

