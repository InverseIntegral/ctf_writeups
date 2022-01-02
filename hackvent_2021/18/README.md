# 18 - Lost Password

## Description

Santa is getting a bit cross with Snowball the elf...

**SANTA**: Let me get this straight. I asked you to encrypt all our PDF files to prevent our lists of names from getting into the wrong hands, right?
**SNOWBALL**: Right.
**SANTA**: And then I asked you to send me the password to these files so I can access them, yes?
**SNOWBALL**: Yes.
**SANTA**: So you sent me the password in a PDF file.
**SNOWBALL**: A PDF, yes.
**SANTA**: Which was encrypted.
**SNOWBALL**: Yes. As per your instructions. We don't want the password to get into the wrong hands, do we?
**SANTA**: But I can't open this password file without knowing what the password is, can I? Could you please just write it down for me? You do remember it, don't you?
**SNOWBALL**: Umm....
**SANTA**: Snowball! Don't tell me you forgot the password!

Uh-oh. It's starting to look like Christmas is ruined. Is there anything you can do to retrieve the password from this file?

Format: `HV{__________}`

## Solution

This was a really interesting challenge! We are given a password protected pdf file. From the challenge description we
know that the password of the pdf is part of the pdf itself. We know that the flag consists of `HV{}` and 10 characters.
This seems a bit much to bruteforce, so we have to restrict the character set.

### Restricting the character set

Using [a pdf-parser tool](https://blog.didierstevens.com/programs/pdf-tools/) I took a look at the various objects of
the password protected pdf file. The font object was interesting:

```
obj 9 0
 Type: /Font
 Referencing: 15 0 R

  <<
    /Type /Font
    /Subtype /Type1
    /BaseFont /AIIWIA+CairoFont-0-0
    /FontDescriptor 15 0 R
    /Encoding /MacRomanEncoding
    /FirstChar 35
    /LastChar 125
    /Widths '[ 720\n0 0 0 0 0 0 0 0 0 0 0 720 0 0 0 0 0 0 720 0 0 0 0 0 0 0 0 0 0 0 0 0 0 720\n0 0 720 0 0 0 0 0 0 0 0 720 720 0 0 0 720 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0\n720 0 0 720 0 0 0 0 0 0 0 0 0 0 0 0 720 720 0 0 720 0 720 ]'
  >>
```

We can see which characters of this particular font have a width unequal to zero. Since the password itself is part of
the pdf file, we can restrict the character set we need to bruteforce. The following script gets us the characters we
need:

```python
lis = [720,0,0,0,0,0,0,0,0,0,0,0,720,0,0,0,0,0,0,720,0,0,0,0,0,0,0,0,0,0,0,0,0,0,720,0,0,720,0,0,0,0,0,0,0,0,720,720,0,0,0,720,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,720,0,0,720,0,0,0,0,0,0,0,0,0,0,0,0,720,720,0,0,720,0,720]

i = 0
s = list(range(35, 126))

for e in lis:
    if (e!=0):
        print(chr(s[i]), end='')
    i += 1

print()
```

Now we know that the characters of the pdf file are in `#/6EHQRVgjwx{}`.

### Bruteforcing

First, I extracted the hash of the pdf password using `pdf2john`. With the hash we can now bruteforce the password using
`hashcat`. Note that I assumed that HV{} do not appear in the 10 characters of the flag.

```
hashcat -m 10500 hash -a 3 -1 "#/6EQRgjwx" "HV{?1?1?1?1?1?1?1?1?1?1}"
```

After a few minutes we get the flag `HV{E6wRx#jQ/g}`.

