# 10 - Christmas Trophy

## Description

The elves thought Santa should relax a bit, so they're inviting him to a round of golf. But the organizers must have
understood, when they get there, what they get is keyboards instead of clubs!

## Solution

This was a challenge that I provided. We are given a service with the following JS code:

```js
let output = '';
const code = req.query.code;

if (code && code.length < 400 && /^[^a-zA-Z\\\:\_]*$/.test(code)) {
    try {
        const result = new vm.Script(code).runInNewContext(undefined, {timeout: 500});

        if (result === 'Hackvent') {
            output = flag;
        } else {
            output = "Bad result: " + result;
        }
    } catch (e) {
        console.log(e);
        output = 'Exception :(';
    }
} else {
    output = "Bad code";
}
```

The goal is to print `Hackvent` in JS without using characters from `a-z`, `A-Z`, `\`, `:` or `_`. Moreover, we are only
allowed to supply 400 characters of JS code. There are many solutions here and some of them are much shorter than what I
present here.

### Constructing functions from strings

In order to solve this challenge I would build a string that contains the code that I want to execute. But how can we
run JS based on a string? One way to do this is getting a hold of a function object and then using the `[]`- operator to
get the constructor. With that it's then possible to construct any code from a string:

```
(() => {})["constructor"]("return 10")(); // returns 10
```

For this we would need quite a few characters though.


### The basic blocks

We can get quite a few characters from the following already:

```js
{} + "" 	// [object Object]
{}[1]  + ""     // undefined
""==""		// true
"."=="" 	// false
"."-1 		// NaN
```

With this we have access to strings like `constructor` or `console`. But we still can't print `Hackvent`. At this point
I decided to go for `String.fromCharCode`. This would allow me to print arbitrary text. For this to work I would need
the characters `SgCemkh`. To get them I decided to use the following tricks:

```js
var res = '';

for (x in console) {
    res += x;
}

console.log(res); // logwarndirtimetimeEndtimeLogtraceassertclearcountcountResetgroupgroupEndtabledebuginfodirxmlerrorgroupCollapsedConsoleprofileprofileEndtimeStampcontext
```

Similary we get:

```js
var res = '';

for (x in Error) {
    res += x;
}

console.log(res); // stackTraceLimit
```

Now we have everything except the character `h`. To get that one I used the function `.link()` on a string. This gave me
the string `href="..."`.

### Putting everything together

```js
let c = ({} + "")[5];
let o = ({} + "")[1];
let n = ({}[1] + "")[1];
let s = (("."=="")+"")[3];
let t = ((""=="")+"")[0];
let r = ((""=="")+"")[1];
let u = ((""=="")+"")[2];
let e = ((""=="")+"")[3];
let l = (("."=="")+"")[2];
let f = ({}[1] + "")[4];
let i = ({}[1] + "")[5];
let a = (("." - 1)+"")[1];
let d = ({}[1] + "")[2];

let _constructor = c + o + n + s + t + r + u + c + t + o + r;
let _return = r + e + t + u + r + n;
let _console = c + o + n + s + o + l + e;

let _ = () => {};

let _for = f + o + r;
let _in = i + n;

let lots = _[_constructor]("__ = ''; " + _for + "(_ " + _in + " " + _console + ") { __ += _ } " + _return + " __")();
let S = lots[139];
let g = lots[2];
let C = lots[102];
let E = lots[18];
let m = lots[12];

let _String = S + t + r + i + n + g;
let _Error = E + r + r + o + r;

let lots2 = _[_constructor]("__ = ''; " + _for + "(_ " + _in + " " + _Error + ") { __ += _ } " + _return + " __")();
let k = lots2[4];

let link = l + i + n + k;
let h = _[_constructor](_return + " ''." + link + "()")()[3];

let fromCharCode = f + r + o + m + C + h + a + r + C + o + d + e;
let leFunc = _[_constructor]( _return + " " + _String + "." + fromCharCode)();

let res = leFunc(72) + leFunc(97) + leFunc(99) + leFunc(107) + leFunc(118) + leFunc(101) + leFunc(110) + leFunc(116);
console.log(res);
```

This now prints `Hackvent` and we are done, right? Well not quite, the server might use a different version of NodeJS in
which the order of the strings is different. We have to be exta careful here. We can print the strings of the server
through the form and adjust the code as necessary. The final payload I cam up with has a length of 393 characters:

```js
ϴ="";П=" ";Ϗ={}[1];[,ᱻ,,,,ʹ]={}+ϴ;[,,ᴶ,ᴱ]=(0==1)+ϴ;[ᴲ,ᴳ,ᴴ,ᴵ]=(1==1)+ϴ;[,ᱽ,,,ᴷ,ᴸ]=Ϗ+ϴ;$=Ϙ=>"Ͼ=ϴ;"+ᴷ+ᱻ+ᴳ+"(ω "+ᴸ+ᱽ+П+Ϙ+")Ͼ+=ω;"+ᴺ+" Ͼ";Ҩ=Ҳ=>$[ʹ+ᱻ+ᱽ+ᴱ+ᴲ+ᴳ+ᴴ+ʹ+ᴲ+ᱻ+ᴳ](ϴ,Ҳ)();ᴺ=ᴳ+ᴵ+ᴲ+ᴴ+ᴳ+ᱽ;ᴾ=Ҩ($(ʹ+ᱻ+ᱽ+ᴱ+ᱻ+ᴶ+ᴵ));ᴿ=ᴾ[50];Յ=Ҩ(ᴺ+П+ᴾ[156]+ᴲ+ᴳ+ᴸ+ᱽ+ᴾ[4]+"."+ᴷ+ᴳ+ᱻ+ᴾ[28]+ᴿ+Ҩ(ᴺ+"''."+ᴶ+ᴸ+ᱽ+Ҩ($(ᴾ[64]+ᴳ+ᴳ+ᱻ+ᴳ))[4]+"()")[3]+(("."-1)+ϴ)[1]+ᴳ+ᴿ+ᱻ+(Ϗ+ϴ)[2]+ᴵ);Յ(72)+Յ(97)+Յ(99)+Յ(107)+Յ(118)+Յ(101)+Յ(110)+Յ(116)
```

This gives us the flag `HV{W4NN4 G0 G0LFING T0M0RR0W?}`.

