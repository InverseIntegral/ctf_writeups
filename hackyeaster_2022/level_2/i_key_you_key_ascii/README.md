# I Key, You Key, ASCII

## Description
Level: noob<br/>
Author: PS

Look what I was drawing in my text editor!

```
.. .. .. 68 65 32 30 .. .. ..  
.. .. 32 ██ ██ ██ ██ 32 .. ..  
.. 7b ██ ██ ██ ██ ██ ██ 74 ..  
.. 68 ██ ██ ██ ██ ██ ██ 31 ..  
73 ██ ██ ██ ██ ██ ██ ██ ██ 5f  
30 ██ ██ ██ ██ ██ ██ ██ ██ 6e  
33 ██ ██ ██ ██ ██ ██ ██ ██ 5f  
31 ██ ██ ██ ██ ██ ██ ██ ██ 73  
5f ██ ██ ██ ██ ██ ██ ██ ██ 72  
33 ██ ██ ██ ██ ██ ██ ██ ██ 33  
33 ██ ██ ██ ██ ██ ██ ██ ██ 33  
.. 6c ██ ██ ██ ██ ██ ██ 79 ..  
.. 5f ██ ██ ██ ██ ██ ██ 73 ..  
.. .. 31 ██ ██ ██ ██ 6d .. ..  
.. .. .. 70 6c 33 7d .. .. ..  
```

## Solution

The challenge title says it all, simply remove the extra symbols and convert from hex to ascii.
This can be done in [CyberChef](https://gchq.github.io/CyberChef/) by using two Find/Replace recipes as well as a From
Hex one: [Cyber Chef with
recipes](https://gchq.github.io/CyberChef/#recipe=Find_/_Replace(%7B'option':'Regex','string':'%E2%96%88%E2%96%88'%7D,'',true,false,true,false)Find_/_Replace(%7B'option':'Simple%20string','string':'..'%7D,'',true,false,true,false)From_Hex('Auto')&input=Li4gLi4gLi4gNjggNjUgMzIgMzAgLi4gLi4gLi4gIAouLiAuLiAzMiDilojilogg4paI4paIIOKWiOKWiCDilojiloggMzIgLi4gLi4gIAouLiA3YiDilojilogg4paI4paIIOKWiOKWiCDilojilogg4paI4paIIOKWiOKWiCA3NCAuLiAgCi4uIDY4IOKWiOKWiCDilojilogg4paI4paIIOKWiOKWiCDilojilogg4paI4paIIDMxIC4uICAKNzMg4paI4paIIOKWiOKWiCDilojilogg4paI4paIIOKWiOKWiCDilojilogg4paI4paIIOKWiOKWiCA1ZiAgCjMwIOKWiOKWiCDilojilogg4paI4paIIOKWiOKWiCDilojilogg4paI4paIIOKWiOKWiCDilojiloggNmUgIAozMyDilojilogg4paI4paIIOKWiOKWiCDilojilogg4paI4paIIOKWiOKWiCDilojilogg4paI4paIIDVmICAKMzEg4paI4paIIOKWiOKWiCDilojilogg4paI4paIIOKWiOKWiCDilojilogg4paI4paIIOKWiOKWiCA3MyAgCjVmIOKWiOKWiCDilojilogg4paI4paIIOKWiOKWiCDilojilogg4paI4paIIOKWiOKWiCDilojiloggNzIgIAozMyDilojilogg4paI4paIIOKWiOKWiCDilojilogg4paI4paIIOKWiOKWiCDilojilogg4paI4paIIDMzICAKMzMg4paI4paIIOKWiOKWiCDilojilogg4paI4paIIOKWiOKWiCDilojilogg4paI4paIIOKWiOKWiCAzMyAgCi4uIDZjIOKWiOKWiCDilojilogg4paI4paIIOKWiOKWiCDilojilogg4paI4paIIDc5IC4uICAKLi4gNWYg4paI4paIIOKWiOKWiCDilojilogg4paI4paIIOKWiOKWiCDilojiloggNzMgLi4gIAouLiAuLiAzMSDilojilogg4paI4paIIOKWiOKWiCDilojiloggNmQgLi4gLi4gIAouLiAuLiAuLiA3MCA2YyAzMyA3ZCAuLiAuLiAuLiAg).
This directly gives the flag `he2022{th1s_0n3_1s_r3333ly_s1mpl3}`.

