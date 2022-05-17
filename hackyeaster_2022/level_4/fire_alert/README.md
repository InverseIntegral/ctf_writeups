# Fire Alert

## Description
Level: easy<br/>
Author: PS

In case of fire, break the glass and press the button.

http://46.101.107.117:2204

## Solution

For this challenge we are given a website, looking at the JS source, we can see a Base64 encoded string to be logged to
the console:

```js
console.log(atob("JWMgZmxhZzogaGUyMDIye3RoMXNfZmw0Z18xc19ibDRja19uMHR9"))
```

Running the above code prints the flag `he2022{th1s_fl4g_1s_bl4ck_n0t}`.

