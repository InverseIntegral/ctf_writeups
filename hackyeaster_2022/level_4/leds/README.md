# Snoopy

## Description
Level: medium<br/>
Author: PS

I got this hex dump, but I don't know what it is.
Any idea?

[leds.hex](leds.hex)

## Solution

For this challenge we are given a hex file that start with

```
:020000040000FA
:0400000A9901C0DEBA
``` 

Some googling lead me to [micro:bit](https://makecode.microbit.org/). There I uploaded the hex file and got the
following translated JS code:

```js
input.onButtonPressed(Button.A, function () {
    j += 0 - 1
})
input.onButtonPressed(Button.AB, function () {
    basic.showString("" + (scribble(c)))
})
input.onButtonPressed(Button.B, function () {
    j += 5
})
function scribble (s: string) {
    for (let i = 0; i <= s.length - 1; i++) {
        r = "" + r + String.fromCharCode(s.charCodeAt(i) + j)
    }
    return r
}
let r = ""
let j = 0
let c = ""
c = "ZW$\"$$m`%&fQ^#ff^%QV%h#U%o"
```

This seems to perform a ceasar cipher on `c`. I quickly tried different shifts:

```js
let c = "ZW$\"$$m`%&fQ^#ff^%QV%h#U%o"

for (let j = -20; j <= 20; j++) {
    let r = "";
    for (let i = 0; i <= c.length - 1; i++) {
        r = "" + r + String.fromCharCode(c.charCodeAt(i) + j)
    }
    console.log(r);
}
```

And one of them gave the flag `he2022{n34t_l1ttl3_d3v1c3}`.

