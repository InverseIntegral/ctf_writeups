# 11 - Memeory 2.0

## Description

Level: medium<br/>
Author: otaku

We improved Memeory 1.0 and added an insane serverside component. So, no more CSS-tricks. **Muahahaha.**<br/>
Flagbounty for everyone who can solve 10 successive rounds. Time per round is 30 seconds and only 3 missclicks are allowed.<br/>
Good game.

## Solution

To solve this challenge I wrote [a Java program](Memeory.java) that downloads all images of the current round, hashes them and compares
the hashes to find pairs. After ten rounds the flag is returned:

```
ok, here is your flag: 1-m3m3-4-d4y-k33p5-7h3-d0c70r-4w4y
```
