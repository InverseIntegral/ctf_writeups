# Signal

Categories: misc pwned! <br/>
Description: There is no signal, everything is silent.Nothing is impossible. <br/>
File: [signal.pdf](signal.pdf)

## Solution

The PDF displayed a keyboard:

```
                ________________________________________________
             _-'... - .-. .- -. --. . .-.-.- -.- . -.-- -... ---`-_
          _-' .- .-. -.. .-.-.- ... . -.-. ..-. .-.-.- -- .---- .`-_
       _-'-.. .---- - ....- .-. -.-- ..--.- --. .-. ....- -.. ...-- -_
    _-'..--.- ...-- -. -.-. .-. -.-- .--. - .---- ----- -. .-.-.- .-- -_
 _-'.- .. - .-.-.- -.-. --- ..- .-.. -.. .-.-.- .-. .- -. -.. --- --   `-_
:-------------------------------------------------------------------------:
`---._.-------------------------------------------------------------._.---'

```

The keyboard contains morse code, the extracted code looks like this:

```
... - .-. .- -. --. . .-.-.- -.- . -.-- -... ---
 .- .-. -.. .-.-.- ... . -.-. ..-. .-.-.- -- .---- .
-.. .---- - ....- .-. -.-- ..--.- --. .-. ....- -.. ...-- 
..--.- ...-- -. -.-. .-. -.-- .--. - .---- ----- -. .-.-.- .-- 
.- .. - .-.-.- -.-. --- ..- .-.. -.. .-.-.- .-. .- -. -.. --- --
```

Decoding it with [an online tool](https://morsecode.scphillips.com/translator.html) gave some errors. The sequence
`..--.-` represents an underscore but this is not recognised by the translator. The decoded text is:

```
STRANGE.KEYBOARD.SECF.M1ED1T4RY_GR4D3_3NCRYPT10N.WAIT.COULD.RANDOM
```

The decoder probably got something wrong in the first part of the flag. Furthermore a flag should begin with `sctf`.
Considering this, the flag was: `sctf{m1l1t4ry_gr4d3_3ncrypt10n}`
