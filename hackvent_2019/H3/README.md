# H3 - Hidden Three

## Description

Author: M. / inik

Not each quote is compl

## Solution

From the challenge description it was clear that this had to do with the quote API from challenge [11](../11). Since the
challenge category was Penetration Testing I did a quick port scan and got the following result:

```
PORT     STATE  SERVICE
17/tcp   open   qotd
22/tcp   open   ssh
25/tcp   open   smtp
80/tcp   closed http
443/tcp  closed https
2222/tcp closed EtherNetIP-1
4444/tcp closed krb524
5555/tcp closed freeciv
8888/tcp open   sun-answerbook
9001/tcp open   tor-orport
```

From this I knew that there was a quote of the day service running. I promptly queried it with `nc whale.hacking-lab.com
17` and got a single character back. Repeating this after a few minutes gave me a different character and I realized
that it changed every hour. With `while sleep 3600; do nc.whale.hacking-lab.com 17 >> out.log; done` and some character
guessing I got the flag `HV19{an0ther_DAILY_fl4g}`.
