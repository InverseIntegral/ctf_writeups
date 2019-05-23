# Day 15: Watch Me

In this challenge we get an ipa file. When we unzip it we get an Mach-O executable.
When looking at the code we can see calls to `AES128DecryptedDataWithKey`. Now we have to find the ciphertext and the
key that is used in the decryption.

To do this I first searched through all the strings. In radare2 this is done by using `iZ`. Now we get the following
list of strings:

```
0x1000080a0 0x1000080a0  44  45 () ascii cstr.xQ34V+MHmhC8V88KyU66q0DE4QeOxAbp1EGy9tlpkLw=
0x1000080c0 0x1000080c0   6   7 () ascii cstr.%@\n\n%@
0x1000080e0 0x1000080e0  37  38 () ascii cstr.HACKvent 2018 - now on your Apple TV!
```

The first one looks like base64. Now we can search the instructions that use this string `/c 0x1000080a0`:

```
0x100006410   # 4: adr x1, str.cstr.xQ34V_MHmhC8V88KyU66q0DE4QeOxAbp1EGy9tlpkLw
```

So the instruction at `0x100006410` uses the base64 encoded data. We can go to this position using `s 0x100006410` and
print the instructions using `pdf`.

```
ldrsw x8, sym.field_int_ViewController::flag
add x0, x19, x8
adr x1, str.cstr.xQ34V_MHmhC8V88KyU66q0DE4QeOxAbp1EGy9tlpkLw
bl sym.imp.objc_storeStrong
```

This stores the base64 encoded data at x19 + x8, a field named flag. Then these instructions get executed:

```
ldr x1, str.decryptFlag
mov x0, x19
bl sym.imp.objc_msgSend 
```

This calls the decrypt function where x19 is passed as the first parameter. In this function the key gets constructed.
The relevant instructions for this are:

```
0x10000657c      adr x8, str.uQA___nM__1wl  ; 0x100007efa
0x100006584      ldr q0, [x8] 
0x100006588      str q0, [sp]
0x10000658c      mov x0, sp
0x100006590      bl sym.imp.strlen
0x100006594      cbz x0, 0x1000065d8
0x100006598      orr w8, wzr, 0x78
0x10000659c      strb w8, [sp]
0x1000065a0      mov x20, sp
0x1000065a4      mov x0, sp
0x1000065a8      bl sym.imp.strlen
0x1000065ac      cmp x0, 2
0x1000065b0      b.lo 0x1000065d8
0x1000065b4      orr w21, wzr, 1
0x1000065b8      ldrb w8, [x20, x21]
0x1000065bc      add w8, w8, 3
0x1000065c0      strb w8, [x20, x21]
0x1000065c4      add x21, x21, 1
0x1000065c8      mov x0, sp
0x1000065cc      bl sym.imp.strlen
0x1000065d0      cmp x0, x21
0x1000065d4      b.hi 0x1000065b8
```

`sp` points to the encrypted string. We can find the encrypted data by going to the address `s 0x100007efa` and printing a hex dump `x`:

```
0x100007efa  7551 415c 2d6e 4d40 3d31 776c 1e62 4e21  uQA\-nM@=1wl.bN! 
```

Reversing the key generation we get the following Java code:

```java
byte[] key = new byte[] { 0x75, 0x51, 0x41, 0x5C, 0x2D, 0x6E, 0x4d, 0x40, 0x3D, 0x31, 0x77, 0x6C, 0x1E, 0x62, 0x4E, 0x21 };

key[0] = 120;
int v3 = 1;

do {
    key[v3++] += 3;
} while (key.length > v3);
```

And now we can simply decrypt the ciphertext:

```java
Cipher cipher = Cipher.getInstance("AES/ECB/NOPADDING");
cipher.init(Cipher.DECRYPT_MODE, new SecretKeySpec(key, "AES"));

byte[] cipherText = Base64.getDecoder().decode("xQ34V+MHmhC8V88KyU66q0DE4QeOxAbp1EGy9tlpkLw=");
byte[] result = cipher.doFinal(cipherText);

System.out.println(new String(result));
```
