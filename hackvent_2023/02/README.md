# 02 - Who am I?

## Description

Level: Easy<br/>
Author: explo1t

Have you ever wished for an efficient dating profile for geeks? Here's a great example:

```
G d--? s+: a+++ C+++$ UL++++$ P--->$ L++++$ !E--- W+++$ N* !o K--? w O+ M-- V PS PE Y PGP++++ t+ 5 X R tv-- b DI- D++ G+++ e+++ h r+++ y+++
```

Find the flag using the code block provided in the introduction.

Flag format: `HV23{<Firstname Lastname>}`

## Solution

After some googling I stumbled across [Geek Code](https://en.wikipedia.org/wiki/Geek_Code), a way to describe geeks
using a series of letters and symbols. If we enter the above sequence [on here](https://www.dcode.fr/geek-code), we get,
amongst other attributes, a description of the PGP usage:

```
PGP:	I am Philip Zimmerman.
```

Therefore, the solution was `HV23{PhilipZimmerman}`. Neat challenge and an interesting way to describe geeks.
