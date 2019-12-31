# 04 - password policy circumvention

## Description

Level: Easy<br/>
Author: DanMcFly

Santa released a new password policy (more than 40 characters, upper, lower, digit, special). The elves can't remember
such long passwords, so they found a way to continue to use their old (bad) password:

```
merry chistmas geeks
```

[HV19-PPC.zip](6473254e-1cb3-444e-9dac-5baeaaaf6d11.zip)

## Solution

The zip file contains an AutoHotkey script. Running the script and typing `merry christmas geeks` in a text file
replaces the text with the flag `HV19{R3memb3r, rem3mber - the 24th 0f December}`.
