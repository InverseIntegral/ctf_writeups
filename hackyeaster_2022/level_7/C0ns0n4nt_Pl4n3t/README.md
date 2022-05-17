# C0ns0n4nt Pl4n3t

## Description
Level: medium<br/>
Author: PS

**Apollo** wants his name printed on that fancy new site. He's constantly failing as vowels and some special characters
are blocked when entered.

Can you help him?

http://46.101.107.117:2205

## Solution

We are given a webservice that takes a name as input. The goal is to print `Apollo` but certain characters are filtered
out, namely vowels and semicolons. If we enter `"` we get a PHP error in an `eval` call. It seems like the supplied name
is actually evaled on the server. After some googling I found a filter bypass technique using the xor operation on
strings. In this challenge, this allows us to xor two strings to get the string `Apollo` without using vowels.
The input be `##<??<"^"bSSSSS` becomes `"Apollo"` when evaluated and prints the flag `he2022{v0w3ls_4r3_f0r_n3rd5!}`.

