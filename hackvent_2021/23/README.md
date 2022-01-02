# 23 - Pixel Perfect

## Description

Finally, Santa has decided to reply to the Easter Bunny. He created a message scrolling across the screen and asked one
of the elves to send it, but it seems they've sent it via a very low resolution channel.

Because it's almost Christmas, neither Santa nor the elves have time now, so... Would you mind restoring the message and
forwarding to the Easter Bunny? I mean, if that's even possible!

## Solution

For this challenge we are provided [a website](index.html) that shows a pixelated flag passing by. The important part of
the JS code is the following:

```js
let move = -320;
let moving = 0;

function printPixels() {
    if (moving++ % 2 === 0) move++;
    if (move>300) move =-320;

    for (let i = 0; i < 40; i++) {
        for (let j = 0; j < 9; j++) {
            let x = (i*8+move);
            let y = ((j-2)*8+(i-15));
            let color = x>0&&y>0&&x<300&&y<36?digits[x*36+y]:0;

            context.fillStyle = "#"+("000000"+((color << 16) | (color << 8) | color).toString(16)).slice(-6);
            context.fillRect(i * 40, j * 40, 40, 40);
        }
    }

    requestAnimationFrame(printPixels);
}

printPixels();
```

After some experimenting, I converted the above code into the following:

```js
function printPixels() {
    for (let x = 0; x < 300; x++) {
        for (let y = 0; y < 100; y++) {
            let color = digits[x * 36 + y];

            context.fillStyle = "#"+("000000"+((color << 16) | (color << 8) | color).toString(16)).slice(-6);
            context.fillRect(i * 40, j * 40, 40, 40);
        }
    }
}
```

This simply converts the digits one-by-one into pixels. With this we get a stable image:

![Blurred flag](blurred.png)

At this point I got stuck for quite a while. In the end I solved it using a tool to reverse the motion blur:

![Recovered flag](recovered.png)

From there I had to guess a bit but in the end I got the flag `HV21{P1xeliz4t10n_N07_54v3}`.

