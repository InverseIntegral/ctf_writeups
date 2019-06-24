# Enter Space-Time Coordinates

## Description

Ok well done. The console is on. It's asking for coordinates. Beating heavily on the console yields little results, but
the only time anything changes on your display is when you put in numbers.. So what numbers are you going to go for? You
see the starship's logs, but is there a manual? Or should you just keep beating the console?

[Attachment](00c2a73eec8abb4afb9c3ef3a161b64b451446910535bfc0cc81c2b04aa132ed)

## Solution

Unpacking the zip file gives two files `log.txt` and `rand2`. Running `strings rand2 | grep ctf` gives the flag
`CTF{welcome_to_googlectf}`.
