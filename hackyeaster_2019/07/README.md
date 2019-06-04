# 07 - Shell we Argument

## Description
Level: easy<br/>
Author: inik

Let's see if you have the right arguments to get the egg.
[eggi.sh](eggi.sh)

## Solution

To solve this challenge I used `set -x` to display commands and their arguments as they are executed.
Running the script showed:

```bash
if [ $# -lt 1 ]; then
echo "Give me some arguments to discuss with you"
exit -1
fi
if [ $# -ne 10 ]; then
echo "I only discuss with you when you give the correct number of arguments. Btw: only arguments in the form /-[a-zA-Z] .../ are accepted"
exit -1
fi
if [ "$1" != "-R" ]; then
echo "Sorry, but I don'\''t understand your argument. $1 is rather an esoteric statement, isn'\''t it?"
exit -1
fi
if [ "$3" != "-a" ]; then
echo "Oh no, not that again. $3 really a very boring type of argument"
exit -1
fi
if [ "$5" != "-b" ]; then
echo "I'\''m clueless why you bring such a strange argument as $5?. I know you can do better"
exit -1
fi
if [ "$7" != "-I" ]; then
echo "$7 always makes me mad. If you wanna discuss with be, then you should bring the right type of arguments, really!"
exit -1
fi
if [ "$9" != "-t" ]; then
echo "No, no, you don'\''t get away with this $9 one! I know it'\''s difficult to meet my requirements. I doubt you will"
exit -1
fi
echo "Ahhhh, finally! Let'\''s discuss your arguments"
function isNr() {
[[ ${1} =~ ^[0-9]{1,3}$ ]]
}
if isNr $2 && isNr $4 && isNr $6 && isNr $8 && isNr ${10} ; then
echo "..."
else
echo "Nice arguments, but could you formulate them as numbers between 0 and 999, please?"
exit -1
fi
low=0
match=0
high=0
function e() {
if [[ $1 -lt $2 ]]; then
low=$((low + 1))
elif [[ $1 -gt $2 ]]; then
high=$((high + 1))
else
match=$((match + 1))
fi
}
e $2 465
e $4 333
e $6 911
e $8 112
e ${10} 007
function b () {
type "$1" &> /dev/null ;
}
if [[ $match -eq 5 ]]; then
t="$Az$Bz$Cz$Dz$Ez$Fz$Gz$Hz$Iz$Jz$Ez$Fz$Kz$Lz$Mz$Nz$Oz$Pz$Ez$Fz$Gz$Hz$Iz$Qz$Rz$Sz$Tz$Uz$Vz$Wz$Xz$Yz$Zz$az$bz$cz$dz$ez$fz$gz$hz$iz$jz$kz$lz$mz$nz$oz$Zz$pz$qz$rz"
echo "Great, that are the perfect arguments. It took some time, but I'\''m glad, you see it now, too!"
sleep 2
if b x-www-browser ; then
x-www-browser $t
else
echo "Find your egg at $t"
fi
else
echo "I'\''m not really happy with your arguments. I'\''m still not convinced that those are reasonable statements..."
echo "low: $low, matched $match, high: $high"
```

From this it was clear that 10 arguments had to be supplied.
The odd arguments had to be -R -a -b -I -t and the even arguments had to be numbers between 0 and 999.
The expected numbers can be found in the output:

```bash
e $2 465
e $4 333
e $6 911
e $8 112
e ${10} 007
```

So the arguments were `-R 465 -a 333 -b 911 -I 112 -t 007` and this gave the following output:

```
Ahhhh, finally! Let's discuss your arguments
...
Great, that are the perfect arguments. It took some time, but I'm glad, you see it now, too!
Find your egg at https://hackyeaster.hacking-lab.com/hackyeaster/images/eggs/a61ef3e975acb7d88a127ecd6e156242c74af38c.png
```
