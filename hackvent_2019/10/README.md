# 10 - Santas Quick Response 3.0

## Description

Level: Medium<br/>
Author: inik

The flag is right, of course

![HV19.10-guess3.zip](d658ab66-6859-416d-8554-9a4ee0105794.zip)

## Solution

The zip file contained a stripped linux binary. Running it asks for an input and it then checks if the supplied input is equal
to the flag. `strace ./guess3` shows that there are multiple `execve` calls happening during the execution of the
binary. I assumed that it was unpacking the actual code. Running `ltrace ./guess3` showed an interesting memcpy:

```
memcpy(0x55a77993b2c0, "#!/bin/bash\n\nread -p "Your input: " input\n\nif [ $input = "HV19{Sh3ll_0bfuscat10n_1s_fut1l3}" ] \nthen\n  echo "success"\nelse \n  echo "nooooh. try harder!"\nfi\n\n\0", 158) = 0x55a77993b2c0
```

The input is directly compared to the flag `HV19{Sh3ll_0bfuscat10n_1s_fut1l3}`.
