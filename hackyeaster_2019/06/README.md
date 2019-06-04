# 06 - Dots

## Description

Level: easy<br/>
Author: J03ll3

Uncover the dots' secret!

![Dots 1](dots1.png)

![Dots 2](dots2.png)

## Solution

I solved this challenge by chance without realizing that there was a pattern.

First of all I figured that all possible positions in a 3x3 block have to be used in each "iteration".
For the bottom right corner this leaves only the `B` and `K`. I read the text from top to bottom and from left to right and
got `HELLOBUCK`.

From here on the dots are rotated counterclockwise to get the dots of the next iteration.
The following images show the dots after each iteration.

![Iteration 1](iterations/first_iteration.png)

![Iteration 2](iterations/second_iteration.png)

![Iteration 3](iterations/third_iteration.png)

![Iteration 4](iterations/fourth_iteration.png)

We get the following sentence:

`HELLO BUCK THE PASSWORD IS WHITE CHOCOLATE.`
