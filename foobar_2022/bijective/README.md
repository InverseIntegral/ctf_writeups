# rev/bijective

## Description

Encodings are supposed to be one to one functions. But can it always be acheived...

Downloads:
[chall.pyc](chall.pyc) [output.txt](output.txt)

## Solution

For this challenge I first decompiled the pyc file. The python code simply encodes the flag. To reverse the encoding, I
generated the alphabet as follows:

```python
s = 'aabacadaeafagaha'
characters = 'abcdefghijklmnopqrstuvwxyz1234567890{}_'

def encode(ch):
    i = characters.index(ch)
    return s[i // 3:i // 3 + i % 3 + 2]


for c in characters:
    print(c, encode(c))
```

```
a aa
b aab
c aaba
d ab
e aba
f abac
g ba
h bac
i baca
j ac
k aca
l acad
m ca
n cad
o cada
p ad
q ada
r adae
s da
t dae
u daea
v ae
w aea
x aeaf
y ea
z eaf
1 eafa
2 af
3 afa
4 afag
5 fa
6 fag
7 faga
8 ag
9 aga
0 agah
{ ga
} gah
_ gaha
```

The problem here is that the function values are of different lengths. And some have common prefixes which makes the
decoding not unique. One approach to solve this would be to consider all the possible decompositions and decode them
s.t. the output is readable ascii text. I took a different approach and simply solved it by hand.

First, I removed the prefix and suffix that contained the usual flag format i.e. `glug{` and `}`. At this point I'm left
with the encoded text

```
caafagcadeagahadaeagahgahaagahcadaba
``` 

Then I realized that the letter `h` only appears in the encoding of `}`, `0` and `_`.
I assumed that `}` doesn't appear in the flag itself. This only left the possibilies `0` and `_`.

```
caafagcadeagahadaeagah
gaha -> _
agahcadaba
``` 

Following this logic, the encoded part `gaha` must be an `_` in the flag. I assumed that the characters before and after
that must be `0` since it would be quite unlikely that we have multiple `_` next to each other.

```
caafagcadeagahadae
agah -> 0
gaha -> _
agah -> 0
cadaba
``` 

Similarly, we can deduce that `agahadae` must be `_t`:

```
caafagcade
agaha -> _
dae   -> t
agah  -> 0
gaha  -> _
agah  -> 0
cadaba
``` 

The `ca` at the very start of the flag must be an encoded `m` since no other encoding has the same prefix:

```
ca    -> m
afagcade
agaha -> _
dae   -> t
agah  -> 0
gaha  -> _
agah  -> 0
cadaba
``` 

`cadaba` can be either `ne` or `og`. The former makes more sense becaus we get the word `0ne`.

```
ca    -> m
afagcade
agaha -> _
dae   -> t
agah  -> 0
gaha  -> _
agah  -> 0
cad   -> n
aba   -> e
``` 

At this point I guessed the flag to be `many_t0_0ne` or something similar using leet speech.
And indeed `glug{m4ny_t0_0ne}` was the correct flag.

