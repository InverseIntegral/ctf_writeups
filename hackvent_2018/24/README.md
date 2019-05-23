# Day 24: Take the red pill, take the blue pill

This challenge was awesome! We get two zip files, one represents the red pill and one the blue pill. The red pill
contains an encrypted flag and a jar file. The blue pill contains an encrypted flag as well and an exe file.

The RedPill class contained the following code:

```java
if (args.length != 1) {
    System.out.println("java -jar redpill.jar <red pill serial number> \n");
    System.exit(0);
}

if (!args[0].matches("[0-9]{2}-[0-9]{3}-[0-9]{3}")) {
    System.out.println("That's not a red pill");
    System.exit(0);
}

byte[] iv = args[0].replace("-", "").getBytes();
byte[] k = new byte[16];

System.arraycopy(iv, 0, k, 0, 8);
System.arraycopy(iv, 0, k, 8, 8);

byte[] b = Files.readAllBytes(new File("flag").toPath());
byte[] f = new byte[(b.length + 1) / 2];

int i = 0;
while (i < b.length) {
    if (i % 2 == 0) {
        f[i / 2] = (byte) (f[i / 2] | b[i] << 4); // lower 4 bits
    } else {
        f[i / 2] = (byte) (f[i / 2] | b[i] & 15); // lower 4 bits
    }

    ++i;
}

Cipher3 c = new Cipher3();
c.setupKey(k);
c.setupIV(iv);

byte[] fc = c.crypt(f);

Files.write(new File("flag_encrypted").toPath(), fc);
```

This was the code that was used to encrypt the flag. We can see that the key is just the IV repeated. Furthermore we
realize that only the lower 4 bits of each byte get encrypted. So we must still be missing something. The Cipher3 class
uses some constants and a quick search shows that it's an implementation of the [Rabbit
cipher](https://en.wikipedia.org/wiki/Rabbit_(cipher)).

When looking at the blue pill we can see that it also encrypts a flag. We assume that it must be the same flag as the
one of the red pill. From the blue pill we also learn that the flag is a PNG image. The first 32bit integer of the file
that gets encrypted is compared to `1196314761` which is also the first value of the PNG header.

The blue pill encrypts only the higher 4 bits of each byte:

```c
v8 = 0;
while (v8 != numberOfBytesToWrite) {
    *(v7 + v8) = (v9[2 * v8 + 1] >> 4) | 16 * (v9[2 * v8] >> 4);
    v8++;
}
```

The key is a constant value but the IV changes based on the current time:

```c
RegCreateKeyExA(HKEY_CURRENT_USER, "SOFWARE\HACKvent2018", 0, 0, 0, 0x4001F, 0, &hkey, 0);
RegQueryInfoKeyA(hkey, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, &FileTime);
RegCloseKey(hkey);
FileTimeToSystemTime(&FileTime, &SystemTime);
SystemTimeToFileTime(&SystemTime, &FileTime);

char key[16] = { 0x87, 0x05, 0x89, 0xCD, 0xA8, 0x75, 0x62, 0xEF, 0x38, 0x45, 0xFF, 0xD1, 0x41, 0x37, 0x54, 0xD5 };

setKey(&state, &key);
setIV(&state, &FileTime);
```

This takes the time when the register got created and saves it as a
[FILETIME](https://msdn.microsoft.com/en-us/library/windows/desktop/ms724284(v=vs.85).aspx). This struct gets
converted into a [SYSTEMTIME](https://msdn.microsoft.com/en-us/library/windows/desktop/ms724950(v=vs.85).aspx) and then
back to a FILETIME. In this process the lowest four digits get set to zero because the SYSTEMTIME only saves
milliseconds whereas the FILETIME saves the time in 100 nanosecond steps. This fact allows us to bruteforce the IV of
the blue pill much faster. We can get the FILETIME of the encrypted flag with the following PowerShell script:

```powershell

(Get-ChildItem .\flag_encrypted).CreationTime.ToFileTime()
```

We now know how the IVs get created, what the keys are and that the flag must be a PNG image. Now we're able to
bruteforce the correct IVs by comparing the decrypted content to the PNG headers. For the red pill we take the lower
bits of the PNG header bytes and for the blue pill we take the higher bits.

To bruteforce the red pill I first wrote a Java program. It took extremly long to find the correct IV so I wrote a C
program that solves the problem much faster:

```c
FILE *fp;
long size;
unsigned char *buffer;

fp = fopen("flag_encrypted" , "rb");

fseek(fp , 0L , SEEK_END);
size = ftell(fp);
rewind(fp);

buffer = calloc(1, size + 1);
fread(buffer , size, 1 , fp);

char png[8] = {0x9, 0x0, 0xe, 0x7, 0xd, 0xa, 0xa, 0xa};

cc_byte key[16];
cc_byte destination[16];

for (char a = '0'; a <= '9'; a++) {
    for (char b = '0'; b <= '9'; b++) {
        for (char c = '0'; c <= '9'; c++) {
            for (char d = '0'; d <= '9'; d++) {
                for (char e = '0'; e <= '9'; e++) {
                    for (char f = '0'; f <= '9'; f++) {
                        for (char g = '0'; g <= '9'; g++) {
                            for (char h = '0'; h <= '9'; h++) {
                                cc_byte iv[8] = { a, b, c, d, e, f, g, h };

                                memcpy(key, iv, 8);
                                memcpy(key + 8, iv, 8);

                                rabbit_instance r_master_inst, r_inst;

                                rabbit_key_setup(&r_master_inst, key, 16);
                                rabbit_iv_setup(&r_master_inst, &r_inst, iv, 8);
                                rabbit_cipher(&r_inst, buffer, destination, 16);

                                bool wrong = false;

                                for (int i = 0; i < 8; i++) {
                                    char current = png[i];

                                    if (i % 2 == 0) {
                                        if (current != (destination[i / 2] >> 4) & 15) {
                                            wrong = true;
                                            break;
                                        }
                                    } else {
                                        if (current != (destination[i / 2] & 15)) {
                                            wrong = true;
                                            break;
                                        }
                                    }
                                }

                                if (!wrong) {
                                    printf("Valid solution found\n");

                                    for (int i = 0; i < 8; i++){
                                        printf("%c", *(iv+i));
                                    }

				    printf("\n");

                                    fclose(fp);
                                    free(buffer);
                                    return 0;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

fclose(fp);
free(buffer);

return 0;
```

To bruteforce the blue IV I wrote a small Java program:
```java
byte[] blueKey = new byte[]{(byte) 0x87, 0x05, (byte) 0x89, (byte) 0xCD, (byte) 0xA8, 0x75, 0x62, (byte) 0xEF, 0x38, 0x45, (byte) 0xFF, (byte) 0xD1, 0x41, 0x37, 0x54, (byte) 0xD5};
byte[] pngHigherHeader = new byte[]{0x8, 0x5, 0x4, 0x4, 0x0, 0x0, 0x1, 0x0};

byte[] cipherText = Files.readAllBytes(new File("flag_encrypted").toPath());
BigInteger initial = new BigInteger("131852077180000000");

outer:
while (true) {
    initial = initial.subtract(new BigInteger("10000"));

    byte[] iv = ByteBuffer.allocate(8)
            .order(ByteOrder.LITTLE_ENDIAN)
            .putLong(initial.longValue())
            .array();

    Cipher3 cipher = new Cipher3();
    cipher.setupKey(blueKey);
    cipher.setupIV(iv);

    byte[] clearText = cipher.crypt(Arrays.copyOf(cipherText, cipherText.length));
    byte[] reversed = new byte[clearText.length * 2];

    for (int i = 0; i < reversed.length; i += 2) {
        reversed[i] = (byte) ((clearText[i / 2] >>> 4) & 15);
        reversed[i + 1] = (byte) (clearText[i / 2] & 15);
    }

    for (int i = 0; i < pngHigherHeader.length; i++) {
        if (reversed[i] != pngHigherHeader[i]) {
            continue outer;
        }
    }

    List<String> bytes = new ArrayList<>();

    for (byte b : iv) {
        bytes.add(String.format("%02X", b));
    }

    System.out.println(bytes);

    return;
}
```

Now we have all the parts that we need to reassemble the flag:

```java
byte[] blueIV = new byte[]{ 0x10, 0x71, (byte) 0xEF, (byte) 0xFE, (byte) 0xC3, 0x6E, (byte) 0xD4, 0x01 };
byte[] blueKey = new byte[]{ (byte) 0x87, 0x05, (byte) 0x89, (byte) 0xCD, (byte) 0xA8, 0x75, 0x62, (byte) 0xEF, 0x38, 0x45, (byte) 0xFF, (byte) 0xD1, 0x41, 0x37, 0x54, (byte) 0xD5 };

byte[] redIV = "45288109".getBytes();
byte[] redKey = new byte[16];

System.arraycopy(redIV, 0, redKey, 0, 8);
System.arraycopy(redIV, 0, redKey, 8, 8);

byte[] redCipher = Files.readAllBytes(new File("flag_encrypted_red").toPath());
byte[] blueCipher = Files.readAllBytes(new File("flag_encrypted_blue").toPath());

Cipher3 c = new Cipher3();
c.setupKey(redKey);
c.setupIV(redIV);

byte[] redClear = c.crypt(redCipher);

Cipher3 c2 = new Cipher3();
c2.setupKey(blueKey);
c2.setupIV(blueIV);

byte[] blueClear = c2.crypt(blueCipher);

byte[] b = new byte[redClear.length * 2];
int i = 0;
int dest = 0;

while (i < b.length / 2) {
    byte lowerBitsFirst = (byte) ((redClear[i] >> 4) & 0b1111);
    byte lowerBitsSecond = (byte) (redClear[i] & 0b1111);

    byte higherBitsFirst = (byte) ((blueClear[i] >> 4) & 0b1111);
    byte higherBitsSecond = (byte) (blueClear[i] & 0b1111);

    b[dest++] = (byte) ((higherBitsFirst << 4) | lowerBitsFirst);
    b[dest++] = (byte) ((higherBitsSecond << 4) | lowerBitsSecond);

    i++;
}

Files.write(new File("final.png").toPath(), b);
```

