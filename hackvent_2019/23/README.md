# 23 - Internet Data Archive

## Description

Level: leet<br/>
Author: M.

Today's flag is available in the Internet Data Archive (IDA).

[http://whale.hacking-lab.com:23023/](http://whale.hacking-lab.com:23023/)

## Solution

For this challenge we get a web application where we can download old challenges as well as todays flag. Unfortunately
we can't select the flag as data to download. After some futher investigation, I realized that the folder of the
generate ZIP files was enumerable and found the list of all previous ZIPs under
[http://whale.hacking-lab.com:23023/tmp/](http://whale.hacking-lab.com:23023/tmp/). Sorting by last modified I found
`Santa-data.zip`. `unzip -l Santa-data.zip` showed that the `flag.txt` was part of the archive. Unfortunately, the ZIP
file had a password. At first I thought that the challenge was to find the initial seed used to generate the OTPs. This
turned out to be wrong. First, I checked for patterns in the OTPs and realized that they all had the same charset and
length i.e. length of 12 and characters from `abcdefghijkmpqrstuvwxyzABCDEFGHJKLMPQRSTUVWXYZ23456789`. With that I came
across [a blog post about hacking the IDA Pro
installer](https://devco.re/blog/2019/06/21/operation-crack-hacking-IDA-Pro-installer-PRNG-from-an-unusual-way-en/). At
that point the name of the challenge made much more sense. Instead of bruteforcing the ZIP password directly I only had
to bruteforce the seed used for the PRNG. Since the exact alphabet order was not known I used the same as in the blog
post and wrote the following script:

```php
#!/usr/bin/php
<?php
$alphabet = "abcdefghijkmpqrstuvwxyzABCDEFGHJKLMPQRSTUVWXYZ23456789";

for ($j = 0; $j < 2^32; $j++) {
    mt_srand($j);

    for ($i = 0; $i < 12; $i++) {
        $r = mt_rand(0, strlen($alphabet) - 1);
        $pass[$i] = $alphabet[$r];
    }

    echo(implode($pass)."\n");
}
?>
```

To bruteforce the ZIP I then used `zip2john -o flag.txt Santa-data.zip > santa.hash` and `php generator.php | john
--stdin santa.hash` to find the password `Kwmq3Sqmc5sA`. Finally, I used `7z -pKwmq3Sqmc5sA e Santa-data.zip && cat
flag.txt` to get the flag `HV19{Cr4ckin_Passw0rdz_like_IDA_Pr0}`.
