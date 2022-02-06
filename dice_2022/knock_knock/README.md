# web/knock-knock

## Description

Knock knock? Who's there? Another pastebin!!

Downloads:
[index.js](index.js) [Dockerfile](Dockerfile)

## Solution

For this challenge we are given a web service that creates notes and stores them assoicated with a token (hmac of the
id). The note with id `0` contains the flag. Initially, I assumed that it might be possible to circumvent some checks by
passing arrays or objects via `/note?id[]=123&token[]=1337`. The bug, however, was much simpler and can be seen here:

```js
const crypto = require('crypto');

class Database {
  constructor() {
    this.notes = [];
    this.secret = `secret-${crypto.randomUUID}`;
  }
...
```

This does not use a random uuid as the secret but the function itself i.e. the secret value is known. To make sure I
would get the correct results, I installed the same nodeJS version which gave me the following secret:

```
secret-function randomUUID(options) {
  if (options !== undefined)
    validateObject(options, 'options');
  const {
    disableEntropyCache = false,
  } = options || {};

  validateBoolean(disableEntropyCache, 'options.disableEntropyCache');

  return disableEntropyCache ? getUnbufferedUUID() : getBufferedUUID();
}
```

Now, I could simply call `generateToken(0)` and use the token
`7bd881fe5b4dcc6cdafc3e86b4a70e07cfd12b821e09a81b976d451282f6e264` to fetch the flag
`dice{1_d00r_y0u_d00r_w3_a11_d00r_f0r_1_d00r}`.

