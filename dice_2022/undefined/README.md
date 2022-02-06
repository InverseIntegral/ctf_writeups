# misc/undefined

## Description

I was writing some Javascript when everything became undefined...
Can you create something out of nothing and read the flag at `/flag.txt`? Tested for Node version 17.

Downloads:
[index.js](index.js)

## Solution

For this challenge we are given a js script that reads our input and then calls `eval` on it. Before doing so, it sets
almost all global functions to `undefined`. After some experimenting, I realized that `import` could still be used. All
I had to do at this point is to import `fs` and read the flag:

```js
import('fs').then(fs => fs.readFile('/flag.txt', 'utf8', (err, data) => {console.log(data, err)}));
```

I'm quite sure that this solution is not the intended one but it still gives the flag: `dice{who_needs_builtins_when_you_have_arguments}`.

