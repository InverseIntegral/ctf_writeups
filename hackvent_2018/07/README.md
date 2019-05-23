# Day 07: flappy.pl

We get the following Perl code:

```perl
use Term::ReadKey; sub k {ReadKey(-1)}; ReadMode 3;
sub rk {$Q='';$Q.=$QQ while($QQ=k());$Q}; $|=1;
print "\ec\e[0;0r\e[4242;1H\e[6n\e[1;1H";
($p .= $c) until (($c=k()) eq 'R'); $x=75;$dx=3;
(($yy) = ($p =~ /(\d+);/))&&($yy-=10);
print (("\r\n\e[40m\e[37m#".(' 'x78)."#")x100);
$r=(sub {$M=shift; sub {$M=(($M*0x41C64E6D)+12345)&0x7FFFFFFF;$M%shift;}})
->(42);$s=(sub {select($HV18, $faLL, $D33p, shift);});$INT0?$H3ll:$PERL;
@HASH=unpack("C*",pack("H*",'73740c12387652487105575346620e6c55655e1b4b6b6f541a6b2d7275'));
for $i(0..666){$s->(0.1);print("\e[40;91m\e[${yy};${x}H.");
$dx += int(rk() =~ / /g)*2-1; $dx = ($dx>3?3:($dx<-3?-3:$dx));
$x += $dx; ($x>1&&$x<80)||last;
(($i%23)&&print ("\e[4242;1H\n\e[40m\e[37m#".(' 'x78)."#"))||
(($h=20+$r->(42))&&(print ("\e[4242;1H\n\e[40m\e[37m#".
((chr($HASH[$i/23]^$h))x($h-5)).(" "x10).((chr($HASH[$i/23]^$h))x(73-$h))."#")));
(($i+13)%23)?42:((abs($x-$h)<6)||last);
print ("\e[${yy};${x}H\e[41m\e[37m@");
}; ReadMode 1;###################-EOF-flappy.pl###############
```

The code seems to use the variable $x for the height and $dx for the change in height (delta x). So this part seems
important: `(abs($x-$h)<6)`. It checks if x is in the interval [h - 6; h + 6]. This is the collision detection of the
game. If we change the 6 to a higher number we can fly through the walls.
