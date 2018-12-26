# HACKvent 2018

## Teaser

### -10

The first image looks like Braille.

![](images/teaser_lines.png)

When decoding it we get a bit.ly link.
The QR code on the website says "rushed by ...". So we must have missed something.
When going back to the bit.ly link there are two redirects happening. The first Location contains
`https://hackvent.hacking-lab.com/T34s3r_MMXVIII/index.php?flag=UI18-GAUa-lXhq-htyV-w2Wr-0yiV`
ROT13 decoding the flag parameters gives the first flag.

### -9

When entering the flag -10 as parameter in the previous link we get a PDF.
Using `pdftotext`we get a morse code

```
.... ...- .---- ---.. -....- --. --- .-. .. -....- --.. .-. ... -... -....- ..- ..-. .- . -....- - ... -.... -.-. -....- -.-. ...- - -
```

Decoding the morse code reveals the next flag.

### -8

The next flag was hidden inside one of the images of the PDF. The images can be extracted with: `pdfimages -png teaser.pdf images/`
One of the images looks like an autostereogram. [This site](http://magiceye.ecksdee.co.uk/) revealed the correct QR code.

### -7

When using `binwalk -e teaser.pdf` we get several files. One file was named `Q3RC.png`. After some searching I came
across a [writeup](https://github.com/shiltemann/CTF-writeups-public/blob/master/Hackvent_2015/writeup.md#dec-17-santas-quick-response)
of the 17th day of the HACKvent 2015.

So the QR code uses three bits as an encoding isntead of the normal two bits. With the help of the writeup I decoded the message by hand.

### -6

The PDF contained a file named `Santa.txt`. It states that Santa now uses encrypted communications and that the key is
reindeer name. It was clear that it must be a transposition cipher because the ciphertext already contained the flag "structure".
Using cryptool I tried a simple transposition with every name but that didn't work. Two names stood out "Donder" and
"Blitzen". Those two were written in the german spelling. When trying a double permutation with those names we get the
correct text and the flag.

![](images/cryptool.png)

### -5

The PDF contained a file named `teaser.pls`. The file contains wrapped PL/SQL code. The wrapping is there to protect the
source code but it can easily be reverted.  Using this [tool](https://codecrete.net/UnwrapIt/)reveals the unwrapped
source code. The function takes a flag as a parameter and checks if it's valid.

```
FUNCTION checkHV18teaser(FLAG VARCHAR2) RETURN NUMBER IS
	A VARCHAR2(4);
	B NUMBER(10);
	C NUMBER(10);
	H VARCHAR(40);
BEGIN
	A := SUBSTR(FLAG,1,4);
	IF NOT (A = 'HV18') THEN
		RETURN 0;
	END IF;
	
	B := TO_NUMBER(SUBSTR(FLAG,6,2));
	C := TO_NUMBER(SUBSTR(FLAG,8,2));
	IF NOT (((B * C) = 6497) AND (B < C)) THEN
		RETURN 0;
	END IF;
	
	A := SUBSTR(FLAG,11,4);
	SELECT STANDARD_HASH(A, 'MD5') INTO H FROM DUAL;
	IF NOT (H = 'CF945B5A36D1D3E68FFF78829CC8DBF6') THEN	
	RETURN 0;
	END IF;
	
	IF NOT ((UTL_RAW.CAST_TO_VARCHAR2(UTL_RAW.BIT_XOR (UTL_RAW.CAST_TO_RAW(SUBSTR(FLAG,16,4)), UTL_RAW.CAST_TO_RAW(SUBSTR(FLAG,21,4)))) = 'zvru') AND (TO_NUMBER(SUBSTR(FLAG,21,4)) = SQRT(8814961))) THEN
		RETURN 0;
	END IF;
	
	IF NOT ( UTL_RAW.CAST_TO_VARCHAR2(UTL_ENCODE.BASE64_ENCODE(UTL_RAW.CAST_TO_RAW(SUBSTR(FLAG,26,4)))) = 'RjBtMA==') THEN
		RETURN 0;
	END IF;
	
	DBMS_OUTPUT.PUT_LINE(A);
	RETURN 1;
END;
```

The first part of the flag consists of two number that, when multiplied result in 6497.
Using an online number factorizer we get B = 73and C = 89.

The second part of the flag is md5 hashed and has to be equal to `CF945B5A36D1D3E68FFF78829CC8DBF6`. A reverse lookup
shows that this part has to be `H0b0`.

The fourth part of the flag is the square root of 8814961. So it has to be 2969

The third part of the flag is XORed with the fourth one to result in 'zvru'.
Now we can XOR 'zvru' with '2969' to get the third part.

The fifth part of the flag is base64 encoded. When decoding it we get `F0m0`.

Putting it all together we get the flag:
HV18-7389-H0b0-H0DL-2969-F0m0

### -4

The PDF contained a file named `old_school.jpg`. It showed a punchcard and had a small hint "IBM-029" on it.  With the help of this
[image](https://en.wikipedia.org/wiki/Keypunch#/media/File:Blue-punch-card-front-horiz_top-char-contrast-stretched.png)
the decoding was quite easy.

### -3

This one was quite difficult to find. The command `unrar vta 39A25.rar`showed that one of the RAR files hidden inside
the PDF contained an NTFS alternate data stream. 

```
       Name: STM
       Type: NTFS alternate data stream
     Target: :quickresponse.txt
       Size: 625
Packed size: 142
      Ratio: 22%
      mtime: 2018-12-03 02:15:08,000000000
 Attributes: .B
      CRC32: 993E3536
    Host OS: Windows
Compression: RAR 3.0(v29) -m3 -md=64K
```

Unpacking the rar on Windows and opening the data stream gives us a sequence of 0 and 1. The sequence has a length of
625 characters so it's probably a 25x25 QR code. With the help of [this site](https://www.dcode.fr/binary-image) the QR code can be
restored.

### -2

Inside the PDF was a password protected ZIP file named `z.zip`. After some trial and error I realized that the password
was only one character. The ZIP file contains another password protected ZIP file.

I wrote a small Java program that bruteforces the one character passwords. Inside the password was a flag and the final
zip file contained a file named `xenon.elf`.

```
private static final List<String> alphabet = Arrays.asList(" .abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ01234567890-,".split(""));

public static void main(String[] args) throws ZipException {
    String source = "z.zip";
    String destination = "out";

    while (true) {
        for (String current : alphabet) {
            ZipFile zipFile = new ZipFile(source);
            zipFile.setPassword(current.toCharArray());

            try {
                zipFile.extractFile(source, destination);
            } catch (ZipException e) {
                continue;
            }

            System.out.print(current);

            File theOne = new File(destination, source);
            theOne.renameTo(new File(source));
        }
    }
}
```

### -1

This challenge was a though one. The last ZIP file from the challenge -2 contained a file named `xenon.elf`. Using
`file` it was clear that this file was a PowerPC binary and the name suggested that it's for a XCPU used by the Xbox
360.

Radare2 revealed calls to a RC4 encryption function. So now it's all about finding the ciphertext and the corresponding key.
Whilst trying to find the ciphertext I came across the following strings, they will be important later on:

![](images/strings.png)

The ciphertext is first read from .rodata and then written to memory. The order of the bytes remains unchanged and the
ciphertext can be found at address `8001EA24`:

```
5e93 c8d4 c4e9 5e36 b155 144a be83 c90a dc2b c5f0 8fab bbac 49dd 0f01 97f6 668b 07a0 b443 0a48
```

At the beginning of the main function those instructions can be found:

```
li r3, 0
bl sym.xenon_secotp_read_line
mr r8, r4

li r3, 1
bl sym.xenon_secotp_read_line
```

They load the values of the fuseset `0` and `1` into `r8` and `r4` respectively. Here it's important to use the devkit values
for fuseset `1` which can be found [here](https://github.com/Free60Project/wiki/blob/master/Fusesets.md). That's what the
string above tried to tell us. So the registers now look like this:

```
r8 = C0FFFFFFFFFFFFFF
r4 = 0F0F0F0F0F0F0F0F
```

The following instructions create the key:

```
srawi r7, r9, 1
addze r7, r7
addi r10, r1, 0x150
subfic r7, r7, 7
slwi r7, r7, 3
srd r6, r8, r7
srd r7, r4, r7
stbux r6, r10, r9
addi r9, r9, 2
stb r7, 1(r10)
```

I translated them to pseudocode:

```
for (int i = 0; i < 16; i += 2) {
    r7 = 8 * (7 - (i / 2));

    *(key + i) = r8 >> r7;
    *(key + i + 1)= r4 >> r7;
}
```

This just intertwines the two fusekit values. The final key is therefore: `C00FFF0FFF0FFF0FFF0FFF0FFF0FFF0F`
Now the ciphertext can be decripted.
