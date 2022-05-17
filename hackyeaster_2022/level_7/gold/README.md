# Go For Gold!

## Description
Level: hard<br/>
Author: PS

Go for Gold!

[gold.zip](gold.zip)

## Solution

For this challenge we are given an ELF file. Running it prints the following text:

```
    ________         ___________              ________       .__       .___._.
   /  _____/  ____   \_   _____/__________   /  _____/  ____ |  |    __| _/| |
  /   \  ___ /  _ \   |    __)/  _ \_  __ \ /   \  ___ /  _ \|  |   / __ | | |
  \    \_\  (  <_> )  |     \(  <_> )  | \/ \    \_\  (  <_> )  |__/ /_/ |  \|
   \______  /\____/   \___  / \____/|__|     \______  /\____/|____/\____ |  __
          \/              \/                        \/                  \/  \/

Enter the passphrase to unlock the gold.
```

Clearly, the goal is to find the correct passphrase. Looking at the binary in ghidra we can see that it is in fact a Go
binary. The `main.main` function is interesting:

```c
fmt.Fscanln(1,1,&DAT_00493980,auStack152);
if (puVar1[1] != 28) {
  auStack168 = CONCAT88(0x4c37c0,0x495400);
  fmt.Fprintln(1,1,&DAT_00495400,auStack168);
  os.Exit();
auVar2 = runtime.convTstring();
auStack184 = CONCAT88(SUB168(auVar2,0),0x495400);
fmt.Fprintln(1,1,SUB168(auVar2 >> 0x40,0),auStack184);
main.maschadar();
main.remplazzar();
main.verifitgar(puVar1[1]);
```

It reads a string and checks if its length is 28. If not it prints `Length mismatch`. Otherwise three functions are
called. The last one seems to be interesting:

```c
local_38 + 8 != *(undefined **)(ulong *)(unaff_R14 + 0x10)) {
local_48 = CONCAT88(4,0x4a48e9);
local_38 = CONCAT88(8,0x4a5048);
local_28 = CONCAT88(10,0x4a5626);
local_18 = CONCAT88(3,0x4a483c);
strings.Join(&DAT_004a475a,1,local_48,4);
if ((unaff_RBX == 4) && (cVar1 = runtime.memequal(), cVar1 != '\0')) {
  lVar2 = 0;
  uVar3 = extraout_RDX;
}
else {
  auVar4 = runtime.cmpstring(4);
  uVar3 = SUB168(auVar4 >> 0x40,0);
  if (SUB168(auVar4,0) < 0) {
    lVar2 = -1;
  }
  else {
    lVar2 = 1;
  }
}
if (lVar2 == 0) {
  runtime.concatstring3
            (uStack0000000000000018,uStack0000000000000020,uVar3,0x1e,&DAT_004a475e,1);
  uVar3 = runtime.convTstring();
  local_58 = CONCAT88(uVar3,0x495400);
  fmt.Fprintln(1,1,&DAT_00495400,local_58);
}
else {
  local_68 = CONCAT88(0x4c37d0,0x495400);
  fmt.Fprintln(1,1,&PTR_DAT_004c37d0,local_68);
}
return;
```

We can see that it seems to compare our input to some other string using `cmpstring`. At this point I switched to
dynamic analysis with pwndbg. I set breakpoints before the call to `maschadar` and before the string comparison. The
input is first modified by the two functions `maschadar` and `remplazzar` and then compared to
`aug{lmepdpeuvlisvohxhqjxlfhr`. The first function seems to permute (mix) the input string and the second function seems
to then replace the characters by a fixed offset.

I reimplemented the functions in python:

```python
def mix(input):
    first_c = input[0]
    first = input[1:3]
    second = input[3:6]
    third = input[6:8]
    fourth = input[8:13]
    fifth = input[13:]
    return third + first + first_c + fifth + second + fourth

def replace(input):
    shift = [0, 2, 2, 4, 4, 1, 1, 3, 3, 5, 0, 2, 2, 4, 4, 1, 1, 3, 3, 5, 0, 2, 2, 4, 4, 1, 1, 3]
    res = []
    
    for i in range(len(input)):
        res += chr(ord(input[i]) + shift[i])
    
    return ''.join(res)
```

All that's left to do now is find the input such that `replace(mix(input)) == 'aug{lmepdpeuvlisvohxhqjxlfhr'`. The
second function can be reversed by replacing the shift with a negative one. The inverse of `mix` is simply:

```python
def unmix(input):
    first_c = input[4]
    third = input[0:2]
    first = input[2:4]
    fifth = input[5:20]
    second = input[20:23]
    fourth = input[23:]
    return first_c + first + second + third + fourth + fifth

def unreplace(input):
    shift = [0, 2, 2, 4, 4, 1, 1, 3, 3, 5, 0, 2, 2, 4, 4, 1, 1, 3, 3, 5, 0, 2, 2, 4, 4, 1, 1, 3]
    res = []
    
    for i in range(len(input)):
        res += chr(ord(input[i]) - shift[i])
    
    return ''.join(res)
```

Now we can use `unreplace(unmix('aug{lmepdpeuvlisvohxhqjxlfhr'))` to obtain
`hewhohasthegoldmakestherules`. If we use this as the input, we get the flag:

```
Congrats, the flag is: he2022{hewhohasthegoldmakestherules}
```

