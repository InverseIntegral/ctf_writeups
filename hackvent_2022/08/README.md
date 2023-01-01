# 08 - Santa's Virus

## Description

Level: Medium<br/>
Author: yuva

A user by the name of HACKventSanta may be spreading viruses. But Santa would never do that! The elves want you to find
more information about this filthy impersonator.

![image.jpg](image.jpg)

## Solution

For this challenge, we are given a profile picture as well as a username of a hacker spreading a virus. Initially,
I tried to search for profiles with that exact name but did not find anything. Using google reverse search in
combination with the username, however, leads us to a [a linkedin profile](https://ch.linkedin.com/in/hackventsanta/)
where we can find [a link to a GitHub profile](https://github.com/HackerSanta). 

There's [exactly one repository](https://github.com/HackerSanta/FILES) on said profile. The "releases" tab of GitHub
immediately drew my attention as it contained a binary:

```shell
file Undetected
Undetected: ELF 64-bit LSB pie executable, x86-64, version 1 (SYSV), dynamically linked, interpreter /lib64/ld-linux-x86-64.so.2, BuildID[sha1]=ed87578ddf875b9911abf41472ed1b68ccc21cf4, for GNU/Linux 3.2.0, not stripped
```

I checked the strings of the bianry:
```shell
strings Undetected

I can only give you key which you might need:
 ThisIsTheKeyToReceiveTheGiftFromSanta
But Go ahead and check my md5, I swear I am undetected!
```

And since we are dealing with a virus,
[VirusTotal](https://www.virustotal.com/gui/file/4d0e17d872f1d5050ad71e0182073b55009c56e9177e6f84a039b25b402c0aef/community)
came to mind. Performing a scan and checking the "Community" tab leads us to a [Twitter
profile](https://twitter.com/SwissSanta2022). The twitter pofile contains multiple QR codes, some of which get you rick
rolled (duh).

One QR Code leads us to [a password protected
PDF](https://drive.google.com/file/d/11pKYrcwr7Hf1eSUq8twtN5aMK-oziPE4/view?usp=sharing). Earlier, we received the key
`ThisIsTheKeyToReceiveTheGiftFromSanta` which we can now enter to get the base64 encoded flag:
`SFYyMntIT0hPK1NBTlRBK0dJVkVTK0ZMQUdTK05PVCtWSVJVU30=`. Which finally gives us `HV22{HOHO+SANTA+GIVES+FLAGS+NOT+VIRUS}`
when decoding properly.
