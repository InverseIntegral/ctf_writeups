# STOP GAN (bof)

## Description

Success, you've gotten the picture of your lost love, not knowing that pictures and the things you take pictures of are
generally two seperate things, you think you've rescue them and their brethren by downloading them all to your ships
hard drive. They're still being eaten, but this is a fact that has escaped you entirely. Your thoughts swiftly shift to
revenge. It's important now to stop this program from destroying these "Cauliflowers" as they're referred to, ever
again.

```
buffer-overflow.ctfcompetition.com 1337
```

[Attachment](4a8becb637ed2b45e247d482ea9df123eb01115fc33583c2fa0e4a69b760af4a)

## Solution

Connecting to `buffer-overflow.ctfcompetition.com` on port `1337` prints:

```
Your goal: try to crash the Cauliflower system by providing input to the program which is launched by using 'run'
command. Bonus flag for controlling the crash.

Console commands: 
run
quit
``` 

Choosing `run` and then entering 400 `A`s gives the flag:

```
CTF{Why_does_cauliflower_threaten_us}
Cauliflower systems never crash >>
segfault detected! ***CRASH***
```
