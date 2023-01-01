# 02 - Santa's song

## Description

Level: Easy<br/>
Author: kuyaya

Santa has always wanted to compose a song for his elves to cherish their hard work. Additionally, he set up a vault with a secret access code only he knows!
The elves say that Santa has always liked to hide secret messages in his work and they think that the vaults combination number may be hidden in the magnum opus of his.
What are you waiting for? Go on, help the elves!

## Solution

For this challenge we are given a website that checks a code and contains [a PDF File](song.pdf).
We simply convert the notes into their A B/H C D E F G notation `baefacedabaddeed`. This can then be converted into
a number `13470175147275968237` which gives the flag when entered on the website `HV22{13..s0me_numb3rs..37}`.
