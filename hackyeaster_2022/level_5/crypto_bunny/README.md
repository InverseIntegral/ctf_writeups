# Crypto Bunny

## Description
Level: easy<br/>
Author: PS

View my verified achievement from (HOP)².

[crypto_bunny.png](crypto_bunny.png)

## Solution

Opening the image in a text editor shows some interesting metadata:

```
{
  "@context": "https://w3id.org/openbadges/v2",
  "type": "Assertion",
  "id": "https://api.eu.badgr.io/public/assertions/aeT2h9EWTHyiqHk7Yx4X4Q",
  "badge": "https://api.eu.badgr.io/public/badges/LaGEPKu1R2W5mg221vdV4g",
  "image": "https://api.eu.badgr.io/public/assertions/aeT2h9EWTHyiqHk7Yx4X4Q/image",
  "verification": {
    "type": "HostedBadge"
  },
  "issuedOn": "2021-07-14T22:00:00+00:00",
  "recipient": {
    "hashed": true,
    "type": "email",
    "identity": "sha256$821158dcab489c45156fd110707bd2ec51d4365b1f34ed42ddde612383717338",
    "salt": "9529d9c5e91b4475a52b46fbe37cb55d"
  },
  "extensions:recipientProfile": {
    "@context": "https://openbadgespec.org/extensions/recipientProfile/context.json",
    "type": [
      "Extension",
      "extensions:RecipientProfile"
    ],
    "name": "Hacky Easter"
  }
}
```

It seems to be a badge that can be earned so I visited [the badgr
website](https://eu.badgr.com/public/badges/LaGEPKu1R2W5mg221vdV4g). There we can see the following description as well
as the earning criteria of the badge:

> Crypto Bunny Award, for the rabbit cipher masters only.

```
U2FsdGVkX1/G2uIf1R3WmIzrCnm3Hz6UQ9Dmm94/0/TtatYB5MDZZRgn/tjzQs5uzuxxPutLznGQlXOTMlcWjg==
```

The earning criteria seems to be base64 encoded and the description of the badge hints at the rabbit cipher. Decoding
the criteria returns a string that starts with `Salted__` so I was quite sure that this must be some kind of ciphertext.
However, to decrypt the ciphertext I needed a password. This step took me a while to figure out. It turns out that the
tag of badge (carrot) was the key of the rabbit cipher. With the help of [this
tool](https://www.browserling.com/tools/rabbit-decrypt) I was able to decrypt the criteria and get the flag:
`he2022{b4dg3_4w4rd3d}`.

