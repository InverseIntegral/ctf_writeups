# 25 - Santa's Prophesy

## Description

Level: Hard<br/>
Author: ShegaHL

Based on an old fairytale from Kobeliaky, Santa can provide more than presents. He can show you the future!

## Solution

This was a bonus challenge given to us on day 25. I didn't immediately solve it because I was still busy solving 24.

We were given a website for this challenge and based on the following hint, I started with running dirbuster on it:

> You may need to do some directory enumeration to find the path to salvation 😉.

Through this I found the path `/upload` which allowed us to upload a pytorch model. The goal of the challenge was to
train a model well enough with some hidden data found in an image. However, I found a way that did not need this input
data at all:

```python
import torch

class MyModule(torch.nn.Module):
    def forward(self, x):
        return torch.sign(x)

m = torch.jit.script(MyModule())
m.save("scriptmodule.pt")
```

It turns out that the `sign` function was good enough to pass the remote test and get the flag: `HV22{AA21B6AB-4520-4AD2-8016-4A9F2C371E6E}`.

