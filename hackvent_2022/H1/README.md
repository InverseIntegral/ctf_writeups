# H1 - Santa's Secret

## Description

Level: Easy<br/>

## Solution

The first hidden flag could be found in [challenge 05](../05/) in the `hv22.gcode` file, we find the following commented
section:

```
;G1 X34.st3r E36 ;)
;G1 X72.86 Y50.50 E123.104
;G1 X49.100 Y100.51 E110.45
;G1 X102.108 Y52.103 E33,125
```

Taking the values on their own:

```
72 86 50 50 123 104
49 100 100 51 110 45
102 108 52 103 33 125
```

And interpreting them as ASCII gives the flag: `HV22{h1dd3n-fl4g!}`
