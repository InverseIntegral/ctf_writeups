# 22 - Santa's Gift Encryptor

## Description

Santa takes security very seriously and encrypts all his gifts. However, his elves were very busy this year and could
not finish the work on the new encryption tool, and therefore it is not possible to decrypt the gifts now. As Christmas
is coming closer and closer, we urgently need someone who can finish their work so that the children won't be left
without presents. And if that wasn't already bad enough, Santa has also lost his license key, but that's probably the
least of our problems.

## Solution

For this challenge we are given a binary as well as an encrypted gift (the flag). Running the binary prints the
following output:

``` 
usage: ./sge <file> <license>
```

Entering a random string as a license prints `invalid license key`. From this I guessed that there is some kind of
license check as well as an encryption going on. In a first step I analyzed the binary in ghidra and then I decrypted
the flag.

### The Main

The main function looked like this:

```c
int main(int argc,char **argv) {
  if (argc != 3) {
    puts("usage: ./sge <file> <license>");
    exit(0);
  }

  check_license(argv[2]);

  char key [24];
  hash(argv[2], 29, key);
  encrypt(argv[1], key);

  return 0;
}
```

We can see that the license is checked first. Then the license itself is hashed and used as the key in an encryption
algorithm.

### The License Check

The license check is quite straight-forward and looks like this:

```c
void check_license(char *license) {
  if (strlen(license) != 29) {
    error();
  }

  for (int i = 0; i < 29; i++) {
    if ((i + 1) % 6 == 0) {
      if (license[i] != '-') {
        error();
      }
    } else if (license[i] < '0' || '9' < license[i]) &&
              (license[i] < 'A' || 'Z' < license[i]) &&
              (license[i] < 'a' || 'z' < license[i]) {
      error();
    }
  }

  for (int j = 0; j < 29; j += 6) {
    char hashed_license_part[24];
    hash(license + j, 5, hashed_license_part);

    for (int k = 0; k < 3; k++) {
      if (hashed_license_part[k] != "S4nT4s3NcrYpt0r"[k + (j / 6) * 3]) {
        error();
      }
    }
  }
}
```

The license has to consist of 29 characters and every sixth character is a `-`. The characters of the license itself are
in `[a-zA-Z0-9]`. We can imagine the license to look like this: `XXXXX-XXXXX-XXXXX-XXXXX-XXXXX`. In the last loop the
individual groups of five characters between the `-` are hashed and compared to the string `S4n`, `T4s`, `3Nc`, `rYp`,
`t0r` respectively. At this point it's a good idea to find out which has function was used.

### The Hash Function

The hash function starts like this:

```c
void hash(char *input,int length,char *output) {
  long local_70 = 0;
  long local_68 = 0xffffffffffffffff;
  long local_60 = 0xf1b5e0d34a192c68;

  undefined local_58[56];
  memset(local_58, 0, 64);
  memcpy(local_58, input, length);
  local_58[length] = 1;

  FUN_001025de(&local_70, &local_68, &local_60, local_58);
  memcpy(output, &local_70, 0x18);

  return;
}
```

Googling those constants didn't lead to much at first. But if we take a look at the function `FUN_001025de` we can see
the following:

```c
void FUN_001025de(long *param_1, long *param_2, long *param_3, undefined8 param_4) {
  ulong uVar1;
  ulong uVar2;
  ulong uVar3;
  
  *param_1 = *param_1 ^ 0x123456789abcdef;
  *param_2 = *param_2 ^ *param_1;
  *param_3 = *param_3 ^ *param_1;

  uVar1 = *param_1;
  uVar2 = *param_2;
  uVar3 = *param_3;
  ...
``` 

We can see that the constants get manipulated. If we do the same and print them as such:

```python
param_1 = 0
param_2 = 0xffffffffffffffff
param_3 = 0xf1b5e0d34a192c68

param_1 = param_1 ^ 0x123456789abcdef;
param_2 = param_2 ^ param_1;
param_3 = param_3 ^ param_1;

print(hex(param_1))
print(hex(param_2))
print(hex(param_3))
```

We get the values `0x123456789abcdef, 0xfedcba9876543210, 0xf096a5b4c3b2e187`. Googling those leads us directly to [the
tiger hash function](https://cryptopp.com/docs/ref521/tiger_8cpp-source.html). Now that we know the hash function used.
We can take a look at the encryption algorithm.

### The Encryption

For this part I used IDA because the output of ghidra was a bit obsure.

```c
void encrypt(char *file_name, char *key) {
  fh = fopen(file_name, "rb");
  fseek(fh,  0, 2);
  size = ftell(fh]);
  rewind(fh);

  content = calloc(1, size + 32);
  encrypted_content = calloc(1, size + 32);

  fread(content, size, 1, fh);
  fclose(fh);

  size = (size >> 4) + 1;

  sub_130F(key, 24LL, v14);
  sub_201E(conent, encrypted_content, HIDWORD(size), v14);

  hash(key, 24, &key);

  char *dest = (char *) calloc(1, strlen(filename) + 4);
  strcpy(dest, filename);
  strcat(dest, ".enc");

  fh  = fopen(dest, "wb");
  fwrite(key, 24, 1, fh);
  fwrite(encrypted_content, 0x10, size, fh);
  fclose(fh);
```

We can see that the key is hashed again (using the same hash function) and it is then prepended to the encrypted file.
This means we can bruteforce the key and decrypt the gift. We just need to find out which encryption algorithm is
used. Inside the encryption algorithm I found the constant `0x9e3779b9` which is used in different encryption
algorithms. At first I tried TEA which was wrong. Later on I found [the serpent
cipher](https://en.wikipedia.org/wiki/Serpent_(cipher)) which is written by the same author as the tiger hash function.

### Getting the license key

Now that we reversed everything, we can bruteforce the license key parts as well as the complete license key. To do this
I used the [Crypto++ library](https://cryptopp.com/). To get the different license parts I wrote the following code:

```c++ 
std::string charset = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

for (int a = 0; a < charset.length(); a++) {
    for (int b = 0; b < charset.length(); b++) {
        for (int c = 0; c < charset.length(); c++) {
            for (int d = 0; d < charset.length(); d++) {
                for (int e = 0; e < charset.length(); e++) {
                    std::string current{charset[a], charset[b], charset[c], charset[d], charset[e]};
                    std::string digest;
                    std::string output;

                    Tiger hash;
                    StringSource s(current, true, new HashFilter(hash, new HexEncoder(new StringSink(digest))));

                    if (digest.rfind("53346E", 0) == 0) {
                        std::cout << 1 << " " << current << std::endl;
                    }

                    if (digest.rfind("543473", 0) == 0) {
                        std::cout << 2 << " " << current << std::endl;
                    }

                    if (digest.rfind("334E63", 0) == 0) {
                        std::cout << 3 << " " << current << std::endl;
                    }

                    if (digest.rfind("725970", 0) == 0) {
                        std::cout << 4 << " " << current << std::endl;
                    }

                    if (digest.rfind("743072", 0) == 0) {
                        std::cout << 5 << " " << current << std::endl;
                    }

                }
            }
        }
    }
}

```

This prints possible parts of the license. Now we have to try all the different combinations and find out which one is
the correct one. We know the (double-hashed) reference that we can compare to:


```c++
std::string group0[] = {"2Aqll", "2gLoy", "3AQfq", "7Fy7d", "8pasV", "adlsB", "BXb55", "coFWX", "dAVjd", "eqvsO",
                        "Fumzi", "G007k", "gDaZS", "Gi2vt", "gIT3S", "Gr16b", "gyfiM", "hsotk", "itmd8", "juIhA",
                        "JxrBh", "kVDgs", "MDgmm", "MKHsL", "NtkxJ", "o3rKT", "o4YUM", "ot5ke", "P3J7T", "SNmHu",
                        "T7yjy", "TuA2q", "tYHqG", "U0rOF", "UlwxL", "uPfTb", "UreDP", "uUxaL", "uVJTg", "V3obI",
                        "VbVZM", "vohtO", "vYE7S", "WFego", "x8U0P", "yUokP", "zdvgw", "ZSWc9"};

std::string group1[] = {"1RKc2", "2c2Kt", "2kwdU", "2wA68", "2x11q", "42a9V", "4uHIW", "63zRg", "6rLr9", "6stx2",
                        "8jIpC", "A3PM0", "A4ifz", "A7368", "APM7L", "bnXWw", "cfr8M", "CIost", "ctl8u", "D8yjm",
                        "ddmFi", "DvIZk", "eAA82", "EbkvT", "eUeDp", "EwNHN", "eXn2h", "fgaRU", "FMwBO", "fO9Rt",
                        "IGm3M", "iXpFk", "J9eRW", "JGk2f", "kECUl", "l5Bve", "lVkn8", "m2iX2", "ma5a6", "mjhEz",
                        "nbMi8", "oqOJe", "OUSxb", "QFhcz", "RAGLI", "Riazr", "rx6C3", "Sd5gV", "SulIF", "t9ilq",
                        "TI3CF", "TP5Zo", "tSSW5", "tt4w2", "TwEDx", "u51ZZ", "UKiVs", "uRZXH", "vL7It", "VR4Hu",
                        "VwoN6", "XhF9Y", "xMtqG", "yJsK2", "ytB9l"
};

std::string group2[] = {"13XB7", "31SHQ", "3cOM8", "41Itd", "5RXrU", "5tnCy", "8BBSg", "9CIrU", "9nzmk", "a0Pc4",
                        "azD5T", "BAYEj", "csWhR", "DtC4I", "DvE2a", "DVk9a", "ElnXm", "fiixd", "fOKC5", "fp79w",
                        "FTpcE", "G0p1a", "gdrFr", "GKhdq", "GVanO", "HQboL", "ieUt4", "iOAMl", "kfOy5", "n9yiN",
                        "NGdwR", "Nvv07", "OAM1e", "Otk5A", "P66qO", "PeN1g", "qDVZ5", "qKB1V", "QmupY", "QN4lW",
                        "r5xAG", "sivTf", "SjqYr", "Skiv5", "tLFTx", "TYDLC", "uDM8s", "UE7A2", "vOIzV", "WK8eQ",
                        "WssIl", "XDh6A", "xmpWp", "XwgNE", "ZdOP9", "ZE1gc", "ZfSdH", "ZhScA", "zIx0G"};

std::string group3[] = {"2fwBV", "2sMn6", "34PVW", "4lZzZ", "4nhP3", "6jraV", "82qlS", "bPDoZ", "CXcQj", "D0t0J",
                        "DhdFn", "fthoz", "Fts7Y", "GbzeZ", "jqEML", "KMG5Y", "Mm6xs", "MzKLg", "nlUcI", "OCD3u",
                        "PkwYk", "pT3NQ", "PXF7S", "pZFRf", "Q5Ozd", "quvHH", "s0B4F", "tIxoq", "uRWYx", "vRnkK",
                        "Xkl6J", "XUnff", "Y6KPZ", "ySLHn", "Zrlrl"
};

std::string group4[] = {"23WOp", "2FM38", "3jLb2", "3r8UN", "51y3N", "5KM4W", "74CDh", "8LWcr", "9Hb8Z", "9uBTE",
                        "af79P", "ayyIr", "BM2Xl", "boeRp", "bUNP2", "BWAhp", "CtBSX", "CwyDD", "d6yQz", "DYDeW",
                        "elYSP", "EQrOx", "gKshj", "gU7R8", "HhyG6", "idzju", "iVa5n", "jMUal", "jtuG0", "KIRjh",
                        "KkarB", "LEizD", "MwTiP", "nHq5j", "o0Q24", "O17sm", "oQfpM", "oVLfh", "puNbJ", "PyYyE",
                        "QpxUh", "TLtVK", "u59v1", "UXhjd", "VVEMT", "VWjdf", "wtSLl", "xLgEy", "Xob7u", "xoZ7j",
                        "yFSk6", "yk4fu", "yKMxT", "yXUyL", "zJbW3", "zMFUD", "ZUIz2"
};

std::string ref_hash = "6385EF2616C572906C5363A990D41AA9D36AFAA02E1485A7";

for (auto & a :   group0) {
    for (auto & b : group1) {
        for (auto & c : group2) {
            for (auto & d : group3) {
                for (auto & e : group4) {
                    std::string current = a + "-" + b + "-" + c + "-" + d + "-" + e;
                    std::string digest;

                    Tiger hash;
                    StringSource s(current, true, new HashFilter(hash, new HexEncoder(new StringSink(digest))));

                    std::string digest2;

                    Tiger hash2;
                    StringSource s2(digest, true, new HashFilter(hash2, new HexEncoder(new StringSink(digest2))));

                    if (digest2 == ref_hash) {
                        std::cout << current << std::endl;
                    }
                }
            }
        }
    }
}
```

After a while this prints `TuA2q-kECUl-n9yiN-82qlS-af79P`! We are almost done, we can now decrypt the gift using the
serpent algorithm and we get the gift:

```
HV21{all_children_thank_you_for_saving_their_gifts}
```

