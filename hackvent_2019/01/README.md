# 01 - censored

## Description

Level: Easy<br/>
Author: M.

I got this little image, but it looks like the best part got censored on the way. Even the tiny preview icon looks
clearer than this! Maybe they missed something that would let you restore the original content?

![Censored QR code](f182d5f0-1d10-4f0f-a0c1-7cba0981b6da.jpg)

## Solution

From the challenge description it was clear that the flag would be visible in the thumbnail. I extracted it with `exiv2
-et f182d5f0-1d10-4f0f-a0c1-7cba0981b6da.jpg`.

![QR code](f182d5f0-1d10-4f0f-a0c1-7cba0981b6da-thumb.jpg)

Scanning this QR code gave the flag `HV19{just-4-PREview!}`.
