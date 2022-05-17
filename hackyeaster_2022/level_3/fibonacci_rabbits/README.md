# Fibonacci Rabbits

## Description
Level: easy<br/>
Author: PS

Everyone loves rabbits!

http://46.101.107.117:2201

## Solution

For this challenge we are given a website that contains several images of rabbits:

![](rabbits.png)

Upon closer inspection, we can see that the images contain fibonacci numbers as a suffix:

```html
        <div><img src="images/rabbit-17711.jpg" /><a href="#">Petal</a></div>
        <div><img src="images/rabbit-75025.jpg" /><a href="#">Harley</a></div>
        <div><img src="images/rabbit-34.jpg" /><a href="#">Rosie</a></div>
        <div><img src="images/rabbit-987.jpg" /><a href="#">Petunia</a></div>
        <div><img src="images/rabbit-8.jpg" /><a href="#">Mortimer</a></div>
        <div><img src="images/rabbit-1.jpg" /><a href="#">Henry</a></div>
        <div><img src="images/rabbit-144.jpg" /><a href="#">Miffy</a></div>
        <div><img src="images/rabbit-2584.jpg" /><a href="#">E.B.</a></div>
        <div><img src="images/rabbit-89.jpg" /><a href="#">Baxter</a></div>
        <div><img src="images/rabbit-55.jpg" /><a href="#">Archie</a></div>
        <div><img src="images/rabbit-5.jpg" /><a href="#">Murphy</a></div>
        <div><img src="images/rabbit-317811.jpg" /><a href="#">Doc</a></div>
        <div><img src="images/rabbit-2.jpg" /><a href="#">Hopper</a></div>
        <div><img src="images/rabbit-6765.jpg" /><a href="#">Fluffy</a></div>
        <div><img src="images/rabbit-46368.jpg" /><a href="#">Daffodil</a></div>
        <div><img src="images/rabbit-28657.jpg" /><a href="#">Buttons</a></div>
        <div><img src="images/rabbit-233.jpg" /><a href="#">Freddie</a></div>
        <div><img src="images/rabbit-1597.jpg" /><a href="#">Roger</a></div>
        <div><img src="images/rabbit-514229.jpg" /><a href="#">Bucky</a></div>
        <div><img src="images/rabbit-4181.jpg" /><a href="#">Oliver</a></div>
        <div><img src="images/rabbit-13.jpg" /><a href="#">Olive</a></div>
        <div><img src="images/rabbit-3.jpg" /><a href="#">Bugs</a></div>
        <div><img src="images/rabbit-377.jpg" /><a href="#">Flower</a></div>
        <div><img src="images/rabbit-10946.jpg" /><a href="#">Chester</a></div>
        <div><img src="images/rabbit-610.jpg" /><a href="#">Bubbles</a></div>
        <div><img src="images/rabbit-121393.jpg" /><a href="#">Coco</a></div>
        <div><img src="images/rabbit-21.jpg" /><a href="#">Clover</a></div>
```

Stripping the unnecessary HTML stuff and ordering them by number we get:

```
1 
2 
3 
5 
8 
13 
21 
34 
55 
89 
144 
233 
377 
610 
987 
1597 
2584 
4181 
6765 
10946 
17711 
28657 
46368 
75025 
121393
317811
514229
```

Note that the 27th fibonacci number is missing, so that rabbit must be hiding something.
We can simply visit `images/rabbit-196418.jpg` to get the flag:

![](cat.jpg)

