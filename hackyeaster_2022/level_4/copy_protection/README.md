# Copy Protection Pioneers 

## Description
Level: easy<br/>
Author: otaku

The copy protection pioneers were really creative and lived the jet set life.

http://46.101.107.117:2209

## Solution

For this challenge we are given a website that asks us to enter 5 codes from different "grid locations". Googling for
"copy protection grid location" led me to [this website](https://intarch.ac.uk/journal/issue45/2/1.html). Here I found
the grid:

![](https://intarch.ac.uk/journal/issue45/2/images/figure9.jpg)

As well as how to interpret it:

![](https://intarch.ac.uk/journal/issue45/2/images/figure10.jpg)

For instance `A4` translates to `Red Blue Green Red` and this is then translated to `2142`.
Doing this for all the codes gives us the flag: `he2022{J3t-53t-W1llY-f0r3v3R}`.

