# Coney Island Hackers

## Description
Level: medium<br/>
Author: PS

Coney Island Hackers have a secret web portal.

Using advanced social engineering techniques, you found out their secret passphrase: **eat,sleep,hack,repeat**. However, it
seems to take more than just entering the passphrase as-is. Can you find out what?

http://46.101.107.117:2202

## Solution

For this challenge we are given a website that accepts our input. From the task description we know that the input is
supposed to be `eat,sleep,hack,repeat`. However, entering commas into the field gives an error message. A quick check of
the headers of the website confirmed that it was using JS in the backend. Now it was clear to me what to do. Instead of
supplying a string we have to supply an array that is then converted into the comma separated string (One of many JS
quirks when comparing with `==`). All we have to do now is to supply the array `[eat, sleep, hack, repeat]`:

```
http://46.101.107.117:2202/?passphrase[]=eat&passphrase[]=sleep&passphrase[]=hack&passphrase[]=repeat
```

And this gave the flag: `he2022{el_dorado_arkade}`.

