# \[a-z().\]

Category: Misc<br/>
Author: st98

`if (eval(your_code) === 1337) console.log(flag);`

## Solution

The relevant source looks like this:

```js
const express = require('express');
const path = require('path');
const vm = require('vm');

const app = express();

app.get('/', function (req, res, next) {
  let output = '';
  const code = req.query.code + '';

  if (code && code.length < 200 && !/[^a-z().]/.test(code)) {
    try {
      const result = vm.runInNewContext(code, {}, { timeout: 500 });
      if (result === 1337) {
        output = process.env.FLAG;
      } else {
        output = 'nope';
      }
    } catch (e) {
      output = 'nope';
    }
  } else {
    output = 'nope';
  }

  res.render('index', { title: '[a-z().]', output });
});
```

We have to return the number `1337` and we can only use `a-z`, `()` and `.` in our code.
I made a string of length `1337` to get the number. `1337` is `7 * 191`. So we need a string of length `7` and one of
length `191`.

To get `7` I used `console.profile.name`.

And to get `191` I used:

```js
console.profile.name.anchor().anchor().anchor().anchor().blink().anchor().anchor().blink() // 181
   .concat(console.log.name) // + 3
   .concat(console.profile.name) // + 10
```

Now I was able to repeat the first string of length `7` and `191` times. This results in a string of length `1337`:

```js
console.profile.name
    .repeat(console.profile.name
        .anchor().anchor().anchor().anchor().blink().anchor().anchor().blink()
        .concat(console.log.name)
        .concat(console.profile.name)
        .length)
    .length;
```

The flag was `HarekazeCTF{sorry_about_last_year's_js_challenge...}`.
