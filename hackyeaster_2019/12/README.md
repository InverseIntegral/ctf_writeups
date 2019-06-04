# 12 - Decrypt0r

## Description

Level: medium</br>
Author: darkstar

Crack the might Decryt0r and make it write a text with a flag.

[decryptor.zip](decryptor.zip)

## Solution

This challenge was quite though. We get a binary that asks for a password. Decompiling the binary gives us the following
main:

```c
const char *v3;
unsigned int s[4];

printf("Enter Password: ", argv, envp, argv);
fgets((char *)s, 16, stdin);
v3 = hash(s);
printf(v3, 16LL);
return 0;
```

The interesting part is the hash function. It looks something like this:

```c
v3[4 * i] = -403835911
                      - ((271733878
                        - ((-271733879 - (v3[4 * i] & data[i]) + 271733878) & data[i])
                        - 271733879) & (-1732584194
                                      - ((1732584193 - (v3[4 * i] & data[i]) - 1732584194) & v3[4 * i])
                                      + 1732584193))
                      + 403835910;
```

This can be simplified significantly. To do this we have to realize that the subtractions result in -1:

```c
int d = data[i];
unsigned long v = *(unsigned long *) &v3[4 * i];

v = -1 - ((-1 - ((-1 - (v & d)) & d)) & (-1 - ((-1 - (v & d)) & v)));
```

`-1 - x` is the same as `~x`. This can be used to further simplify the code:

```c
v = ~((~((~(v & d)) & d)) & (~((~(v & d)) & v)));
```

We could now use De Morgan's laws to simplify the term on our own. Instead of this I used [a boolean expression
simplificator](https://www.dcode.fr/boolean-expressions-calculator) to simplify the expressio as far as possible.
Now we get:

```c
v = (d & ~v) || (~d & v)
```

This is exactly the same as an [XOR operation](https://en.wikipedia.org/wiki/Exclusive_or). So the whole thing is just:

```c
v = d ^ v;
```

Now that we know that it's an XOR cipher, we can get the key by using [this tool](https://wiremask.eu/tools/xor-cracker/).
The key is `x0r_w1th_n4nd`. Finally we're able to decrypt the cipher text and get the flag:

```
Hello, 
congrats you found the hidden flag: he19-Ehvs-yuyJ-3dyS-bN8U. 

'The XOR operator is extremely common as a component in more complex ciphers. By itself, using a constant repeating key, a simple XOR cipher can trivially be broken using frequency analysis. If the content of any message can be guessed or otherwise known then the key can be revealed.'
(https://en.wikipedia.org/wiki/XOR_cipher)

'An XOR gate circuit can be made from four NAND gates. In fact, both NAND and NOR gates are so-called "universal gates" and any logical function can be constructed from either NAND logic or NOR logic alone. If the four NAND gates are replaced by NOR gates, this results in an XNOR gate, which can be converted to an XOR gate by inverting the output or one of the inputs (e.g. with a fifth NOR gate).' 
(https://en.wikipedia.org/wiki/XOR_gate)
```

