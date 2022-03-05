# pwn/babyssid

## Description

What are these signals we are recieving ?
nc chall.nitdgplug.org 30092

## Solution

This challenge was super easy. I connected to the service via netcat and tried some inputs i.e. a long string and some
string formats to check for common vulnerabilities. Entering `%s %s %s %s` gave back the output which contained th flag:

```
(null) /bit.ly/3vPXAuD}
 %s %s %s %s
 GLUG{https://bit.ly/3vPXAuD}
```

