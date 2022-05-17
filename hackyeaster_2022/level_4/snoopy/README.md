# Snoopy

## Description
Level: easy<br/>
Author: PS

Snoopy dog found something interesting.
Can you get something interesting out of the 256 bytes he found?

```
IKIANJKDPKKAPJIDNKKAPNBHELCBHMGGDLOBLIPCKNAHFOEEBNFHALLBOMPGKJADFKDAGMNGIIGCDPEFBINCIPNFIMKGPPLFOMLGOKFAAIECBPJFM</Password><Domain type="NT">CORP</Domain></Credentials><ClientName>THUMPERSDESK7</ClientName><ClientType>ica30</ClientType><ClientAddress>10.1
```

## Solution

After some googling I found out that we are given an encoded Citrix password. Using Citrix CTX1 Decode of CyberChef
should decode the password but the length seems to be incorrect. Removing the first character of the encoded version
yields the flag: `he2022{ctx1_41nt_3nKryp710n!}`.

