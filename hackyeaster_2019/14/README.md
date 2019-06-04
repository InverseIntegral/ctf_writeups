# 14 - White Box

## Description
Level: medium<br/>
Author: darkstar

Do you know the mighty **WhiteBox** encryption tool? Decrypt the following cipher text!

```
9771a6a9aea773a93edc1b9e82b745030b770f8f992d0e45d7404f1d6533f9df348dbccd71034aff88afd188007df4a5c844969584b5ffd6ed2eb92aa419914e
```

[WhiteBox](WhiteBox)

## Solution
From the title of the challenge we can guess that this challenge is about [white-box
cryptography](https://whiteboxcrypto.com/). This means that even if we have access to the binary we won't be able to
read the key easily.

The main of the decompiled binary looks something like this:

```c
int main(int argc, char **argv) {
  puts("WhiteBox Test");
  printf("Enter Message to encrypt: ");

  char s[524];
  fgets(s, 512, stdin);

  int sLength = strlen(s) - 1;
  int blocks = sLength / 16 + 1;
  int paddingLength = 16 - sLength % 16;

  for (int i = 0; i < paddingLength; i++) {
    s[i + v7] = paddingLength;
  }

  for (int j = 0; j < blocks; j++) {
    encrypt(&s[16 * j]);
  }

  printEncrypted(s, 16 * blocks);
  putchar(10);
  return 0;
}
```

The plaintext is encrypted in blocks of length 16 and PKCS#7 padding is used. Based on this I guessed that it must be a
whitebox AES 128 encryption algorithm.

After some research I came across [the Deadpool repository](https://github.com/SideChannelMarvels/Deadpool) and
[phoenixAES](https://github.com/SideChannelMarvels/JeanGrey/tree/master/phoenixAES). With those tools I can perform a
[Differential fault analysis](https://en.wikipedia.org/wiki/Differential_fault_analysis) to extract the key.

I wrote [a script](dfa.py) to perform the DFA. Running this gave me the following output:

```
Press Ctrl+C to interrupt
Send SIGUSR1 to dump intermediate results file: $ kill -SIGUSR1 7746
Lvl 008 [0x0001F144-0x0001F145[ xor 0x91 74657374746573747465737474657374 -> A9DDDF1521A954306161E8212FDE37D4 GoodEncFault Column:0 Logged
Lvl 008 [0x0001F144-0x0001F145[ xor 0xA7 74657374746573747465737474657374 -> A6DDDF1521A9548A61619D212F1437D4 GoodEncFault Column:0 Logged
Lvl 008 [0x0001F144-0x0001F145[ xor 0x10 74657374746573747465737474657374 -> 94DDDF1521A9548D6161C0212F6137D4 GoodEncFault Column:0 Logged
Lvl 008 [0x0001F144-0x0001F145[ xor 0xA1 74657374746573747465737474657374 -> 32DDDF1521A954806161F7212FAA37D4 GoodEncFault Column:0 Logged
Lvl 008 [0x0001F145-0x0001F146[ xor 0x40 74657374746573747465737474657374 -> 8DDDDFA221A9F9556150CA2112D237D4 GoodEncFault Column:3 Logged
Lvl 008 [0x0001F145-0x0001F146[ xor 0x15 74657374746573747465737474657374 -> 8DDDDF4521A9395561D5CA2153D237D4 GoodEncFault Column:3 Logged
Lvl 008 [0x0001F145-0x0001F146[ xor 0x59 74657374746573747465737474657374 -> 8DDDDF0321A9EC55610CCA21A1D237D4 GoodEncFault Column:3 Logged
Lvl 008 [0x0001F145-0x0001F146[ xor 0x71 74657374746573747465737474657374 -> 8DDDDF4921A90755617ACA2181D237D4 GoodEncFault Column:3 Logged
Lvl 008 [0x0001F146-0x0001F147[ xor 0x4E 74657374746573747465737474657374 -> 8DDD4F15212E54557761CA212FD2377D GoodEncFault Column:2 Logged
Lvl 008 [0x0001F146-0x0001F147[ xor 0xFA 74657374746573747465737474657374 -> 8DDD341521715455B661CA212FD23721 GoodEncFault Column:2 Logged
Lvl 008 [0x0001F146-0x0001F147[ xor 0x6A 74657374746573747465737474657374 -> 8DDD5715217454557061CA212FD2377E GoodEncFault Column:2 Logged
Lvl 008 [0x0001F146-0x0001F147[ xor 0xE7 74657374746573747465737474657374 -> 8DDDF615211B54553361CA212FD237BD GoodEncFault Column:2 Logged
Lvl 008 [0x0001F147-0x0001F148[ xor 0x85 74657374746573747465737474657374 -> 8D4DDF150AA954556161CA742FD2DAD4 GoodEncFault Column:1 Logged
Lvl 008 [0x0001F147-0x0001F148[ xor 0xF5 74657374746573747465737474657374 -> 8D29DF151FA954556161CA882FD27ED4 GoodEncFault Column:1 Logged
Lvl 008 [0x0001F147-0x0001F148[ xor 0x1A 74657374746573747465737474657374 -> 8D65DF152AA954556161CA0D2FD255D4 GoodEncFault Column:1 Logged
Lvl 008 [0x0001F147-0x0001F148[ xor 0xF9 74657374746573747465737474657374 -> 8D53DF1597A954556161CA392FD234D4 GoodEncFault Column:1 Logged
Saving 17 traces in dfa_enc_20190503_092826-092845_17.txt
Last round key #N found:
FD83DB41AC158393CC291088B76F201A
```

According to [this writeup](https://github.com/ResultsMayVary/ctf/tree/master/RHME3/whitebox) we have to undo the
expansion of the AES key. [This script](https://github.com/ResultsMayVary/ctf/blob/master/RHME3/whitebox/inverse_aes.py)
is able to do just that.

Running `python2 inverse_aes.py FD83DB41AC158393CC291088B76F201A` prints:

```
Inverse expanded keys = [
	fd83db41ac158393cc291088b76f201a
	91879460519658d2603c931b7b463092
	508d33cfc011ccb231aacbc91b7aa389
	a0c83a2a909cff7df1bb077b2ad06840
	9f60d8933054c5576127f806db6b6f3b
	96e8ff67af341dc451733d51ba4c973d
	f344af8e39dce2a3fe472095eb3faa6c
	473a36d7ca984d2dc79bc23615788af9
	5268bc628da27bfa0d038f1bd2e348cf
	b1aef4fcdfcac79880a1f4e1dfe0c7d4
	336d62336e6433645f6b33795f413335
]
Cipher key: 336d62336e6433645f6b33795f413335
As string: '3mb3nd3d_k3y_A35'
```

Now I can decrypt the ciphertext:

```bash
echo 9771a6a9aea773a93edc1b9e82b745030b770f8f992d0e45d7404f1d6533f9df348dbccd71034aff88afd188007df4a5c844969584b5ffd6ed2eb92aa419914e | xxd -r -p | openssl enc -d -aes-128-ecb -nopad -K 336d62336e6433645f6b33795f413335
```

and get the following plaintext:

```
Congrats! Enter whiteboxblackhat into the Egg-o-Matic!
```
