# 26 - Hidden Egg 2

## Description

Level: hidden<br/>
Author: PS

A stylish blue egg is hidden somewhere here on the web server. Go catch it!

## Solution

From the challenge description it was quite obvious that the egg had something to do with the styling of the web page.
In the Firefox developer tools I used the style editor to explore the CSS. In one of them I found the following
interesting piece:

```css
@font-face {
    font-family: 'Egg26';
    font-weight: 400;
    font-style: normal;
    font-stretch: normal;
    src: local('Egg26'),
    local('Egg26'),
    url('../fonts/TTF/Egg26.ttf') format('truetype');
}
```

So I downloaded [the font](Egg26.ttf). This wasn't a ttf file but much rather a PNG image! I opened it in Gimp and got
the flag.
