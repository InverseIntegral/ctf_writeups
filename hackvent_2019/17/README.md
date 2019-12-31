# 17 - Unicode Portal

## Description

Level: Hard<br/>
Author: scryh

Buy your special gifts online, but for the ultimative gift you have to become admin.

[http://whale.hacking-lab.com:8881/](http://whale.hacking-lab.com:8881/)

## Solution

Before I could get the source of the web application I had to register a new user. The following pieces are the
important part of the registration code:

```php
function isAdmin($username) {
  return ($username === 'santa');
}

function isUsernameAvailable($conn, $username) {
  $usr = $conn->real_escape_string($username);
  $res = $conn->query("SELECT COUNT(*) AS cnt FROM users WHERE LOWER(username) = BINARY LOWER('".$usr."')");
  $row = $res->fetch_assoc();
  return (int)$row['cnt'] === 0;
}

function registerUser($conn, $username, $password) {
  $usr = $conn->real_escape_string($username);
  $pwd = password_hash($password, PASSWORD_DEFAULT);
  $conn->query("INSERT INTO users (username, password) VALUES (UPPER('".$usr."'),'".$pwd."') ON DUPLICATE KEY UPDATE password='".$pwd."'");
}
```

I had to find a username which satisifies `upper(username) == 'SANTA'` and `'santa != lower(username)` and the same
time. From the name of the challenge it was clear that this had to be something unicode specific. After a bit of
research I found [this blog entry](https://eng.getwisdom.io/hacking-github-with-unicode-dotless-i/) which described that
`upper('ſ') == 'S'`. This was perfect for the challenge because with `ſanta` as username both conditions were satisifed.

I simply registered `ſanta` with a random password and then used that password to log in `santa`. The flag was
`HV19{h4v1ng_fun_w1th_un1c0d3}`.

Apparently it was also possible to register the username `santa ` (with additional whitespace) to satisfy both
conditions.
