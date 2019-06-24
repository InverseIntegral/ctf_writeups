# Home Computer

## Description

Blunderbussing your way through the decision making process, you figure that one is as good as the other and that
further research into the importance of Work Life balance is of little interest to you. You're the decider after all.
You confidently use the credentials to access the "Home Computer. "

Something called "desktop" presents itself, displaying a fascinating round and bumpy creature (much like yourself)
labeled "cauliflower 4 work - GAN post." Your 40 hearts skip a beat. It looks somewhat like your neighbors on XiXaX3.
..Ah XiXaX3... You'd spend summers there at the beach, an awkward kid from ObarPool on a family vacation, yearning, but
without nerve, to talk to those cool sophisticated locals.

So are these "Cauliflowers" earthlings? Not at all the unrelatable bipeds you imagined them to be. Will they be at the
party? Hopefully SarahH has left some other work data on her home computer for you to learn more.

[Attachment](86863db246859897dda6ba3a4f5801de9109d63c9b6b69810ec4182bf44c9b75)

## Solution

Extracting the zip file gives two files `family.ntfs` and `note.txt`. The file system can be mounted with `mount
family.ntfs /mnt/family`. Under `Users/Family/Documents` there is a `credentials.txt` which contains the following
text:

```
I keep pictures of my credentials in extended attributes.
```

`attr -l credentials.txt` prints the list of attributes:

```
Attribute "FILE0" has a 38202 byte value for credentials.txt
```

Now to print the `FILE0` attribute: `attr -g FILE0 credentials.txt >> flag.png`

The image contains the flag `CTF{congratsyoufoundmycreds}`.

