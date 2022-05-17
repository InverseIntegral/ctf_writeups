# Knäck låset

## Description
Level: easy<br/>
Author: PS

Knäck the cØde!

koda   ✅ 🔀 ❌   
2-9-7  1  0  2   
2-3-0  0  1  2   
7-8-2  0  2  1   
5-1-9  0  0  3   
5-9-8  0  1  2   

⚑ format: he2022{999}

## Solution

This challenge was pretty simple. We are given a table consisting of guesses and results similar to
[Mastermind](https://en.wikipedia.org/wiki/Mastermind_(board_game)). The first column indicates if a guess was at the
correct position and at the correct place. The second column indicates a correct number at a wrong place. And the last
column indicates that a number isn't part of the result.

With these rules in mind, we can solve the code by applying simple logic:

``` 
5-1-9  0  0  3   
```

This implies that 5, 1, 9 aren't present in the code. This only leaves us with 0, 2, 3, 4, 6, 7, 8.

``` 
5-9-8  0  1  2 
```

Implies that 8 is present in the code and doesn't appear at the last position.

```
7-8-2  0  2  1  
```

Implies that 8 doesn't appear at the middle position. Therefore, the code must start with 8.

```
2-9-7  1  0  2  
```

Implies that the code ends in a 7. This is because it starts with an 8 and a 9 isn't present in the code. Now, we just
have to find the middle number of 8X7. Where X can only be in 0, 3, 4, 5, 8.

```
2-3-0  0  1  2   
```

The last hint implies that the middle number must be a 0. If it wasn't a zero, we would have the 3 at the correct
position. Therefore, the code is 807.

