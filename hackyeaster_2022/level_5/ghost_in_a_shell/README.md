# Ghost in a Shell 3

## Description
Level: easy<br/>
Author: PS

Connect to the server, snoop around, and find the flag!

- ssh 46.101.107.117 -p 2203 -l pinky
- password is: !speedy!

## Solution

Connecting to the server and looking for interesting files, we can see:

```
-rw-r--r--    1 root     root            64 Apr 12 18:25 flag.enc
-r--------    1 root     root            32 Mar 13 18:22 flag.txt
```

Unfortunately, we can only read the encrypted flag. But maybe we can find the key that was used to encrypt the
flag.Checking the file `/etc/crontabs/root` leads us to the script that encrypts the flag:

```
* * * * * /opt/bannerkoder/cipher.sh > /dev/null 2>&1
```

The script looks like this:

```bash
#!/bin/bash
date +%s | md5sum | base64 | head -c 32 > /tmp/7367111C2875730D00686C13B98E7F36
openssl enc -aes-256-cbc -e -in /home/pinky/flag.txt -out /home/pinky/flag.enc -kfile /tmp/7367111C2875730D00686C13B98E7F36bf21a174387a
```

We can see that `flag.txt` gets encrypted with a different key every time the script is called. All we have to do now is
to get `flag.enc` and the key file and decrypt the flag.

