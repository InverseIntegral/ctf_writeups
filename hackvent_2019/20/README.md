# 20 - i want to play a game

## Description

Level: Hard<br/>
Author: hardlock

Santa was spying you on Discord and saw that you want something weird and obscure to reverse?

your wish is my command.

[HV19-game.zip](e22163c8-e0a4-475b-aef5-6a8aba51fd93.zip)

## Solution

For this challenge we are given an executable in COFF format. Running strings on it reveals two interesting strings
`/mnt/usb0/PS4UPDATE.PUP` and `f86d4f9d2c049547bd61f942151ffb55`. The latter one looked like a hash and so I did a quick
google search. The second string was indeed the hash of the PS4 system software 5.05. At this point I opened the binary
in Ghidra and found the following intersting parts:

```c
char socketName[] = "sendflag";
struct sockaddr_in server;

server.sin_len = sizeof(server);
server.sin_family = AF_INET;
server.sin_addr.s_addr = IP(127, 0, 0, 1);
server.sin_port = sceNetHtons(1337);
memset(server.sin_zero, 0, sizeof(server.sin_zero));

int sock = sceNetSocket(socketName, AF_INET, SOCK_STREAM, 0);
sceNetConnect(sock, (struct sockaddr *)&server, sizeof(server));
sceNetSend(sock, flag, size, 0);
sceNetSocketClose(sock);
```

Here the flag is sent over a socket and before that flag is calculated as:

```c
#include <stdio.h>

void main() {
  char local_4e0[] = { 0xce, 0x55, 0x95, 0x4e, 0x38, 0xc5, 0x89, 0xa5, 0x1b, 0x6f, 0x5e, 0x25, 0xd2, 0x1d, 0x2a, 0x2b, 0x5e, 0x7b, 0x39, 0x14, 0x8e, 0xd0, 0xf0, 0xf8, 0xf8, 0xa5, 0x00 };
  char local_4c0[27] = { 0 };

  int counter = 0x1337;
  FILE* uVar4 = fopen("/mnt/usb0/PS4UPDATE.PUP", "r");

  do {
    fseek(uVar4, counter, 0);
    fread(local_4c0, 0x1a, 1, uVar4);
    int counter2 = 0;
    do {
      local_4e0[counter2] = local_4e0[counter2] ^ local_4c0[counter2];
      counter2 = counter2 + 1;
    } while (counter2 != 0x1a);
    counter = counter + 0x1337;
  } while (counter != 0x1714908);

  local_4e0[26] = 0;
  printf("%s", local_4e0);
}
```

First the program checked if `/mnt/usb0/PS4UPDATE.PUP` had the correct hash (not included above). If so then the flag
was calculated i.e. first reading some data and then XOring them multiple times with the data of the PS4 software
update. I got the PS4 software from [the psdev wiki](https://www.psdevwiki.com/ps4/05.050.000) and ran the above program
with the correct file path to get the flag `HV19{C0nsole_H0mebr3w_FTW}`.
