# 13 - Symphony in HEX

## Description

Level: medium<br/>
Author: readmyusername

A lost symphony of the genius has reappeared.

![Symphony in hex](symphonyinhex.png)

Hint: count quavers, read semibreves

## Solution

This challenge was quite straight-forward. In the following image I marked the relevant chunks of the first row:

![First row of the symphony](first_row.png)

In the first chunk I counted the quavers and got 4. I continued until I got to the eight chunk. There I read the
semibreve as G. I continued doing this until I got to the end. The text we get from that is the following:

```
4841434B5F
4D455F414D4
144455553
```

Decoding this form HEX to ASCII gives the password `HACK_ME_AMADEUS`.
