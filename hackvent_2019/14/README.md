# 14 - Achtung das Flag

## Description

Level: Medium<br/>
Author: M.

Let's play another little game this year. Once again, I promise it is hardly obfuscated.

```perl
use Tk;use MIME::Base64;chomp(($a,$a,$b,$c,$f,$u,$z,$y,$r,$r,$u)=<DATA>);sub M{$M=shift;##
@m=keys %::;(grep{(unpack("%32W*",$_).length($_))eq$M}@m)[0]};$zvYPxUpXMSsw=0x1337C0DE;###
/_help_me_/;$PMMtQJOcHm8eFQfdsdNAS20=sub{$zvYPxUpXMSsw=($zvYPxUpXMSsw*16807)&0xFFFFFFFF;};
($a1Ivn0ECw49I5I0oE0='07&3-"11*/(')=~y$!-=$`-~$;($Sk61A7pO='K&:P3&44')=~y$!-=$`-~$;m/Mm/g;
($sk6i47pO='K&:R&-&"4&')=~y$!-=$`-~$;;;;$d28Vt03MEbdY0=sub{pack('n',$fff[$S9cXJIGB0BWce++]
^($PMMtQJOcHm8eFQfdsdNAS20->()&0xDEAD));};'42';($vgOjwRk4wIo7_=MainWindow->new)->title($r)
;($vMnyQdAkfgIIik=$vgOjwRk4wIo7_->Canvas("-$a"=>640,"-$b"=>480,"-$u"=>$f))->pack;@p=(42,42
);$cqI=$vMnyQdAkfgIIik->createLine(@p,@p,"-$y"=>$c,"-$a"=>3);;;$S9cXJIGB0BWce=0;$_2kY10=0;
$_8NZQooI5K4b=0;$Sk6lA7p0=0;$MMM__;$_=M(120812).'/'.M(191323).M(133418).M(98813).M(121913)
.M(134214).M(101213).'/'.M(97312).M(6328).M(2853).'+'.M(4386);s|_||gi;@fff=map{unpack('n',
$::{M(122413)}->($_))}m:...:g;($T=sub{$vMnyQdAkfgIIik->delete($t);$t=$vMnyQdAkfgIIik->#FOO
createText($PMMtQJOcHm8eFQfdsdNAS20->()%600+20,$PMMtQJOcHm8eFQfdsdNAS20->()%440+20,#Perl!!
"-text"=>$d28Vt03MEbdY0->(),"-$y"=>$z);})->();$HACK;$i=$vMnyQdAkfgIIik->repeat(25,sub{$_=(
$_8NZQooI5K4b+=0.1*$Sk6lA7p0);;$p[0]+=3.0*cos;$p[1]-=3*sin;;($p[0]>1&&$p[1]>1&&$p[0]<639&&
$p[1]<479)||$i->cancel();00;$q=($vMnyQdAkfgIIik->find($a1Ivn0ECw49I5I0oE0,$p[0]-1,$p[1]-1,
$p[0]+1,$p[1]+1)||[])->[0];$q==$t&&$T->();$vMnyQdAkfgIIik->insert($cqI,'end',\@p);($q==###
$cqI||$S9cXJIGB0BWce>44)&&$i->cancel();});$KE=5;$vgOjwRk4wIo7_->bind("<$Sk61A7pO-n>"=>sub{
$Sk6lA7p0=1;});$vgOjwRk4wIo7_->bind("<$Sk61A7pO-m>"=>sub{$Sk6lA7p0=-1;});$vgOjwRk4wIo7_#%"
->bind("<$sk6i47pO-n>"=>sub{$Sk6lA7p0=0 if$Sk6lA7p0>0;});$vgOjwRk4wIo7_->bind("<$sk6i47pO"
."-m>"=>sub{$Sk6lA7p0=0 if $Sk6lA7p0<0;});$::{M(7998)}->();$M_decrypt=sub{'HACKVENT2019'};
__DATA__
The cake is a lie!
width
height
orange
black
green
cyan
fill
Only perl can parse Perl!
Achtung das Flag! --> Use N and M
background
M'); DROP TABLE flags; -- 
Run me in Perl!
__DATA__
```

## Solution

For this challenge we get an obfuscated perl program. Running it (after installing the tk dependency) shows a GUI where
we have to "hit" parts of the flag without drawing over the previous paths taken. Obviously, this is impossible to solve
by just playing and therefore I tried to deobfuscate the code by hand. After several failed attempts I realized that the
program uses the variable names as well. Renaming variables would lead to different outputs.

In the end I was able to remove the complete GUI interaction. It was very important to not remove any variables tough.
The deobfuscated code looks like this and prints the flag
`HV19{s@@jSfx4gPcvtiwxPCagrtQ@,y^p-za-oPQ^a-z\x20\n^&&s[(.)(..)][\2\1]g;s%4(...)%"p$1t"%ee}`.

```perl
use MIME::Base64;
sub M {
    $M = shift;
    @m = keys %::;
    ( grep { ( unpack( "%32W*", $_ ) . length($_) ) eq $M } @m )[0];
}
$zvYPxUpXMSsw = 0x1337C0DE;
$PMMtQJOcHm8eFQfdsdNAS20 = sub { 
    $zvYPxUpXMSsw = ($zvYPxUpXMSsw * 16807) & 0xFFFFFFFF; 
};
$a1Ivn0ECw49I5I0oE0 = $Sk61A7pO = $sk6i47pO = 0;
$d28Vt03MEbdY0 = sub {
    pack('n', $fff[$S9cXJIGB0BWce++] ^ ($PMMtQJOcHm8eFQfdsdNAS20->() & 0xDEAD));
};
$vgOjwRk4wIo7_ = $vMnyQdAkfgIIik = $cqI = $S9cXJIGB0BWce = $_2kY10 = $_8NZQooI5K4b = $Sk6lA7p0 = 0;
$_ = M(120812).'/'.M(191323).M(133418).M(98813).M(121913).M(134214).M(101213).'/'.M(97312).M(6328).M(2853).'+'.M(4386);
s|_||gi;
@fff = map { unpack( 'n', $::{ M(122413) }->($_) ) } m:...:g;

for ($q = 0; $q < 45; $q++) {
        $PMMtQJOcHm8eFQfdsdNAS20->();
        $PMMtQJOcHm8eFQfdsdNAS20->();
        print($d28Vt03MEbdY0->());
}
```
