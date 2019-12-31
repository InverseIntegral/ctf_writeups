# 08 - SmileNcryptor 4.0

## Description

Level: Medium<br/>
Author: otaku

You hacked into the system of very-secure-shopping.com and you found a SQL-Dump with $$-creditcards numbers. As a good
hacker you inform the company from which you got the dump. The managers tell you that they don't worry, because the data
is encrypted.

Dump-File: [dump.zip](c635204a-6347-45d7-91f8-bd7b94b111f1.zip)

## Solution

The dump contains an SQL script with teh following interesting lines:

```sql
CREATE TABLE `creditcards` (
  `cc_id` int(11) NOT NULL AUTO_INCREMENT,
  `cc_owner` varchar(64) DEFAULT NULL,
  `cc_number` varchar(32) DEFAULT NULL,
  `cc_expires` varchar(7) DEFAULT NULL,
  PRIMARY KEY (`cc_id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;

INSERT INTO `creditcards` VALUES 
(1,'Sirius Black',':)QVXSZUVY\ZYYZ[a','12/2020'),
(2,'Hermione Granger',':)QOUW[VT^VY]bZ_','04/2021'),
(3,'Draco Malfoy',':)SPPVSSYVV\YY_\\]','05/2020'),
(4,'Severus Snape',':)RPQRSTUVWXYZ[\]^','10/2020'),
(5,'Ron Weasley',':)QTVWRSVUXW[_Z`\b','11/2020');

CREATE TABLE `flags` (
  `flag_id` int(11) NOT NULL AUTO_INCREMENT,
  `flag_prefix` varchar(5) NOT NULL,
  `flag_content` varchar(29) NOT NULL,
  `flag_suffix` varchar(1) NOT NULL,
  PRIMARY KEY (`flag_id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

INSERT INTO `flags` VALUES (1,'HV19{',':)SlQRUPXWVo\Vuv_n_\ajjce','}');
```

Since there are more ciphertext characters than possible digits I assumed that the cipher used here is polyalphabetic. I
wrote a program that checks possible shifts for each position of the ciphertexts. A shift is only valid if all
ciphertext characters are mapped to a digit.

```java
public class Smile {

    private static String[] cipher = {
            "QVXSZUVY\\ZYYZ[a",
            "QOUW[VT^VY]bZ_",
            "SPPVSSYVVYY_\\\\]",
            "RPQRSTUVWXYZ[]^",
            "QTVWRSVUXW[_Z`\\b"
    };

    public static void main(String[] args) {
        for (int i = 0; i < 7; i++) {
            System.out.println("Index: " + i);
            printValidShifts(i);
            System.out.println();
        }
    }

    private static void printValidShifts(int position) {
        boolean good = true;

        for (int i = -100; i < 100; i++) {
            for (int j = 0; j < 4; j++) {
                char res = shift(cipher[j], position, i);

                if (!Character.isDigit(res)) {
                    good = false;
                }
            }

            if (good) {
                System.out.println(i);
            }

            good = true;
        }
    }

    private static char shift(String in, int digit, int shift)  {
        char c1 = in.charAt(digit);
        return (char) (c1 + shift);
    }

}
```

From the output it was clear that the shifts decrease by one at each position and start at -30. The following function
decrypts the flag to `HV19{5M113-420H4-KK3A1-19801}`.

```java
private static void decrypt(String in)  {
    int shift = -30;

    for (String c : in.split("")) {
        char c1 = c.charAt(0);
        char c2 = (char) (c1 + shift);

        System.out.print(c2);
        shift--;
    }
    
    System.out.println();
}
```
