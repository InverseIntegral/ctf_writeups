# rev/babyrev

## Description

nothing to say just solve it ....

Downloads:
[chall](chall)

## Solution

For this challenge we are given an ELF file. From the decompiled code in ghidra we can see that its a straight-forward
keygen challenge:


```c
s = *(char **)(param_2 + 8);
ar2 = strlen(__s);
if ((int)sVar2 == 0x2a) {
    bVar3 = (int)__s[8] + (int)__s[0xd] + (int)__s[7] == 0x10d;
    bVar4 = (int)*__s + ((int)*__s - (int)__s[1]) + (int)__s[0xe] == 0xa5;
    bVar5 = (int)__s[0x26] + (int)__s[0x15] * (int)__s[0x10] + (int)__s[0x22] == 0x250a;
    bVar6 = (int)__s[0x17] + (int)__s[0x29] + (int)__s[6] * (int)__s[8] == 0x157c;
    bVar7 = -(int)__s[0x15] - (int)__s[0x29] == -0xdf;
    bVar8 = (int)__s[0x13] + (int)__s[0x12] * (int)__s[4] * (int)__s[0xb] == 0x9c2de;
    bVar9 = (int)__s[0x22] * (int)__s[0x21] + (int)__s[0x17] == 0x1903;
    bVar10 = (int)__s[0xe] * (int)__s[0x12] - (int)__s[0x21] == 0x13d0;
    bVar11 = (((int)__s[0x18] - (int)__s[0x27]) - (int)__s[0x1e]) - (int)__s[0x16] == -0x6e;
    bVar12 = (int)__s[1] + (((int)__s[0x1e] + (int)__s[10]) - (int)__s[0x13]) == 0x6e;
    bVar13 = ((int)__s[0xf] - (int)__s[0x14]) - (int)__s[0x29] == -0xa9;
    bVar14 = (int)__s[0x23] * (int)__s[0xf] - (int)__s[8] * (int)__s[0x29] == -0x27f7;
    bVar15 = ((int)__s[0xb] * (int)__s[0x1f] + (int)__s[0x24]) - (int)__s[0x20] == 0x20ec;
    bVar16 = (int)__s[0x28] + (int)__s[0x19] + (int)__s[0x1d] == 0x121;
    bVar17 = (int)__s[0x18] + ((int)__s[7] - (int)__s[0xc]) == 100;
    bVar18 = (int)__s[0x1e] * (int)__s[0x15] - (int)__s[6] == 0x242e;
    bVar19 = (int)__s[3] * (int)__s[0x21] * (int)__s[0x26] == 0x753f4;
    bVar20 = ((int)__s[0x14] - (int)*__s * (int)__s[0x1f]) - (int)__s[2] == -0x1742;
    bVar21 = (int)__s[0x15] * (int)__s[0xc] + (int)__s[0x1b] == 0x13e7;
    bVar22 = ((int)__s[6] + (int)__s[8] * (int)__s[0xb]) - (int)__s[8] == 0x2aba;
    bVar23 = (int)__s[0x18] * (int)__s[7] + ((int)__s[0x22] - (int)__s[5]) == 0x1396;
    bVar24 = ((int)__s[0x28] - (int)__s[0x12]) - (int)__s[2] == -0x53;
    bVar25 = (int)__s[0x18] * (int)__s[9] + ((int)__s[0xb] - (int)__s[0x1f]) == 0x2782;
    bVar26 = ((int)__s[0x1c] + (int)__s[0x1e]) - (int)__s[0x10] * (int)__s[3] == -0x198f;
    bVar27 = (int)__s[0x19] * (int)__s[0x12] - (int)__s[0xb] == 0x16c4;
    bVar28 = (int)__s[0xb] * (int)__s[9] * (int)__s[8] == 0x109de8;
    bVar29 = (int)__s[0x19] * (int)__s[3] - (int)__s[6] * (int)__s[0x1d] == 0x8ee;
    bVar30 = (int)__s[0x24] - (int)__s[0x21] * (int)__s[7] == -0xe3a;
    bVar31 = (int)__s[0x14] + ((int)__s[0x20] - (int)__s[1]) == 0x49;
    bVar32 = (int)__s[4] * (int)__s[5] + (int)__s[0x27] == 0x2073;
    bVar33 = (int)__s[8] * (int)__s[0x27] * (int)*__s == 0x7dd84;
    bVar34 = (int)__s[0x1f] + ((int)__s[0xc] - (int)__s[0xd]) == 0x19;
    bVar35 = (int)__s[0x29] + (int)__s[0x29] + (int)__s[10] + (int)__s[0x12] == 0x15f;
    bVar36 = (int)__s[0x16] + (int)__s[1] * (int)__s[0xe] + (int)__s[7] == 0x1dc8;
    bVar37 = (int)__s[0xe] + (int)__s[0x12] * (int)__s[0x18] + (int)__s[0x1b] == 0x157c;
    bVar38 = (int)__s[0x12] + ((int)__s[0x14] - (int)__s[6] * (int)__s[0x29]) == -0x16dd;
    bVar39 = ((int)__s[0x21] - (int)__s[2]) - (int)__s[0x1f] * (int)__s[0x19] == -0x2571;
    bVar40 = (int)__s[0x25] * (int)__s[0xb] * (int)__s[0x12] == 0x56540;
    bVar41 = ((int)__s[7] + (int)__s[8] + (int)__s[0x11]) - (int)__s[0x27] == 0xc0;
    bVar42 = ((int)__s[0xb] - (int)__s[0x23]) - (int)__s[0x1f] * (int)__s[9] == -0x205d;
    bVar43 = (int)__s[0x27] + ((int)__s[0x17] - (int)__s[0x1d]) == 0x28;
    bVar44 = (int)__s[0x14] * (int)__s[0x19] * (int)__s[10] + (int)__s[0x1c] == 0x81959;
    bVar45 = (int)__s[3] * (int)__s[0x1d] * (int)__s[0x20] == 0x7142a;
    bVar46 = (int)__s[0x1e] + ((int)__s[0x20] - (int)__s[0x16]) == 0x62;
    bVar47 = (((int)*__s - (int)__s[0xd]) + (int)__s[0x28]) - (int)__s[0x26] == -0x4a;
    bVar48 = ((int)__s[0x15] + (int)__s[0x11]) - (int)__s[0x26] == 0x6c;
    bVar49 = (int)*__s - (int)__s[0x17] * (int)__s[0x29] == -0x2e1c;
    bVar50 = (int)__s[0x1b] * (int)__s[0x1d] * (int)__s[2] == 0xf390d;
    bVar51 = (int)__s[0x19] - (int)__s[0x23] * (int)__s[0x13] == -0x1d34;
    bVar52 = (int)__s[0x10] - (int)__s[7] * (int)__s[0x13] == -0x14af;
    bVar53 = (int)__s[0x16] + (int)__s[0x21] + (int)__s[0x1a] * (int)__s[0xc] == 0xaa8;
    bVar54 = (int)__s[0x20] + (int)__s[0x18] + (int)__s[0x29] == 0x119;
    bVar55 = (int)__s[0xe] * (int)__s[0x1f] * (int)__s[0x17] == 0xc0e04;
    bVar56 = ((int)__s[0x23] - (int)__s[6] * (int)__s[0x23]) - (int)__s[0xe] == -0xd0e;
    bVar57 = ((int)__s[0x1f] + (int)__s[0x28]) - (int)__s[0x19] * (int)__s[0x11] == -0x2b8c;
    bVar58 = (int)__s[0x13] * (int)__s[0xd] + (int)__s[0x12] * (int)__s[0x24] == 0x3fec;
    bVar59 = (int)__s[0x12] * (int)__s[2] + ((int)__s[0x28] - (int)__s[5]) == 0x1137;
    bVar60 = (int)__s[3] + ((int)__s[0x15] - (int)__s[0x19]) == 0x37;
    bVar61 = ((int)__s[0xd] + (int)__s[0xe] + (int)__s[0xe]) - (int)__s[2] == 0xdf;
    bVar62 = (int)__s[0x23] * (int)__s[0x24] - (int)__s[0x1d] * (int)__s[5] == -0x991;
    bVar63 = (int)__s[1] + ((int)__s[0x29] - (int)__s[0x27]) == 0x87;
    bVar64 = (int)*__s + ((int)__s[0x23] - (int)__s[0x23] * (int)*__s) == -0x1297;
    bVar65 = ((int)__s[8] - (int)__s[0x15] * (int)__s[10]) - (int)__s[0x1f] == -0x12a8;
    bVar66 = (int)__s[0x1c] + ((int)__s[0x1d] - (int)__s[0x18]) == 0x7e;
    bVar67 = ((int)__s[10] * (int)*__s - (int)__s[0x20]) - (int)__s[8] == 0xcf3;
    bVar68 = (int)__s[0x29] + (int)__s[0x20] * (int)__s[0x1c] == 0x170f;
    bVar69 = (int)__s[0x20] + ((int)__s[0x25] - (int)__s[0x18]) == 0x14;
    bVar70 = (int)__s[0x1f] + ((int)__s[10] * (int)__s[0x14] - (int)__s[0xf]) == 0x1250;
    bVar71 = ((int)__s[0x24] - (int)__s[9]) - (int)__s[0x12] * (int)__s[0x12] == -0xaa1;
    bVar72 = (int)__s[0x1e] * (int)__s[0x10] + (int)__s[7] * (int)__s[9] == 0x3634;
    bVar73 = ((int)__s[0x18] + (int)__s[0x22] + (int)__s[0x12]) - (int)__s[7] == 0xbc;
    bVar74 = (int)__s[0x14] + (int)__s[0x1b] * (int)__s[0x10] == 0x245e;
    bVar75 = (((int)__s[0x16] - (int)__s[0x1e]) - (int)__s[0x25]) - (int)__s[9] == -0xd3;
    bVar76 = (int)__s[0x1b] * (int)__s[0x29] * (int)__s[4] - (int)__s[0x26] == 0x16c156;
    bVar77 = (int)__s[0xd] + ((int)__s[0x23] - (int)__s[8] * (int)__s[0x1d]) == -0x334b;
    bVar78 = (((int)__s[0x17] - (int)__s[7]) - (int)__s[0x18]) - (int)__s[0x16] == -0x6b;
    bVar79 = (int)__s[5] * (int)__s[4] * (int)__s[0x25] == 0x88d04;
    bVar80 = (int)__s[0x20] * (int)__s[0x11] - (int)__s[0xf] == 0x14af;
    bVar81 = ((int)__s[0x20] + (int)__s[0x12] * (int)__s[0x17]) - (int)__s[5] == 0x133f;
    bVar82 = (int)__s[0x27] + (int)__s[3] + (int)__s[0x27] * (int)__s[8] == 0x1ce5;
    bVar83 = (int)__s[0x24] + ((int)__s[0x19] * (int)__s[7] - (int)__s[3]) == 0x15dd;
    bVar84 = ((int)__s[9] - (int)__s[0x18]) - (int)__s[0x21] == -0x4f;
    bVar85 = (int)__s[0x24] * (int)__s[0xe] + (int)__s[0x1e] == 0x2015;

    if (bVar85 && (bVar84 && (bVar83 && (bVar82 && (bVar81 && (bVar80 && (bVar79 && (bVar78 && (bVar77 && (bVar76 && (bVar75 && (bVar74 && (bVar73 && (bVar72 && (bVar71 && (bVar70 && (bVar69 && (bVar68 && (bVar67 && (bVar66 && (bVar65 && (bVar64 && (bVar63 && (bVar62 && (bVar61 && (bVar60 && (bVar59 && (bVar58 && (bVar57 && (bVar56 && (bVar55 && (bVar54 && (bVar53 && (bVar52 && (bVar51 && (bVar50 && (bVar49 && (bVar48 && (bVar47 && (bVar46 && (bVar45 && (bVar44 && (bVar43 && (bVar42 && (bVar41 && (bVar40 && (bVar39 && (bVar38 && (bVar37 && (bVar36 && (bVar35 && (bVar34 && (bVar33 && (bVar32 && (bVar31 && (bVar30 && (bVar29 && (bVar28 && (bVar27 && (bVar26 && (__s[0x29] == '}' && (bVar25 && (bVar24 && (bVar23 && (bVar22 && (bVar21 && (bVar20 && (bVar19 && (bVar18 && (bVar17 && (bVar16 && (bVar15 && (bVar14 && (bVar13 && (bVar12 && (bVar11 && (bVar10 && (bVar9 && (bVar8 && (bVar7 && (bVar6 && (bVar5 && (bVar4 && bVar3))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))) {
        puts(":) CORRECT!");
}
```

I first tried to solve the constraints using angr but that did not work. Instead, I used z3 to solve it. I saw that the
last character had to be a `}` which implied that the correct input must follow the usual flag format. Furthermore, I
restricted the search space to printable ascii characters.

```python
#!/usr/bin/env python3

from z3 import *

# List of BitVec
s = [BitVec(f'{i}', 8) for i in range(42)]
solver = Solver()

# Constraints
solver.add(s[8] + s[0xd] + s[7] == 0x10d)
solver.add(s[0] + (s[0] - s[1]) + s[0xe] == 0xa5)
solver.add(s[0x26] + s[0x15] * s[0x10] + s[0x22] == 0x250a)
solver.add(s[0x17] + s[0x29] + s[6] * s[8] == 0x157c)
solver.add(-s[0x15] - s[0x29] == -0xdf)
solver.add(s[0x13] + s[0x12] * s[4] * s[0xb] == 0x9c2de)
solver.add(s[0x22] * s[0x21] + s[0x17] == 0x1903)
solver.add(s[0xe] * s[0x12] - s[0x21] == 0x13d0)
solver.add(((s[0x18] - s[0x27]) - s[0x1e]) - s[0x16] == -0x6e)
solver.add(s[1] + ((s[0x1e] + s[10]) - s[0x13]) == 0x6e)
solver.add((s[0xf] - s[0x14]) - s[0x29] == -0xa9)
solver.add(s[0x23] * s[0xf] - s[8] * s[0x29] == -0x27f7)
solver.add((s[0xb] * s[0x1f] + s[0x24]) - s[0x20] == 0x20ec)
solver.add(s[0x28] + s[0x19] + s[0x1d] == 0x121)
solver.add(s[0x18] + (s[7] - s[0xc]) == 100)
solver.add(s[0x1e] * s[0x15] - s[6] == 0x242e)
solver.add(s[3] * s[0x21] * s[0x26] == 0x753f4)
solver.add((s[0x14] - s[0] * s[0x1f]) - s[2] == -0x1742)
solver.add(s[0x15] * s[0xc] + s[0x1b] == 0x13e7)
solver.add((s[6] + s[8] * s[0xb]) - s[8] == 0x2aba)
solver.add(s[0x18] * s[7] + (s[0x22] - s[5]) == 0x1396)
solver.add((s[0x28] - s[0x12]) - s[2] == -0x53)
solver.add(s[0x18] * s[9] + (s[0xb] - s[0x1f]) == 0x2782)
solver.add((s[0x1c] + s[0x1e]) - s[0x10] * s[3] == -0x198f)
solver.add(s[0x19] * s[0x12] - s[0xb] == 0x16c4)
solver.add(s[0xb] * s[9] * s[8] == 0x109de8)
solver.add(s[0x19] * s[3] - s[6] * s[0x1d] == 0x8ee)
solver.add(s[0x24] - s[0x21] * s[7] == -0xe3a)
solver.add(s[0x14] + (s[0x20] - s[1]) == 0x49)
solver.add(s[4] * s[5] + s[0x27] == 0x2073)
solver.add(s[8] * s[0x27] * s[0] == 0x7dd84)
solver.add(s[0x1f] + (s[0xc] - s[0xd]) == 0x19)
solver.add(s[0x29] + s[0x29] + s[10] + s[0x12] == 0x15f)
solver.add(s[0x16] + s[1] * s[0xe] + s[7] == 0x1dc8)
solver.add(s[0xe] + s[0x12] * s[0x18] + s[0x1b] == 0x157c)
solver.add(s[0x12] + (s[0x14] - s[6] * s[0x29]) == -0x16dd)
solver.add((s[0x21] - s[2]) - s[0x1f] * s[0x19] == -0x2571)
solver.add(s[0x25] * s[0xb] * s[0x12] == 0x56540)
solver.add((s[7] + s[8] + s[0x11]) - s[0x27] == 0xc0)
solver.add((s[0xb] - s[0x23]) - s[0x1f] * s[9] == -0x205d)
solver.add(s[0x27] + (s[0x17] - s[0x1d]) == 0x28)
solver.add(s[0x14] * s[0x19] * s[10] + s[0x1c] == 0x81959)
solver.add(s[3] * s[0x1d] * s[0x20] == 0x7142a)
solver.add(s[0x1e] + (s[0x20] - s[0x16]) == 0x62)
solver.add(((s[0] - s[0xd]) + s[0x28]) - s[0x26] == -0x4a)
solver.add((s[0x15] + s[0x11]) - s[0x26] == 0x6c)
solver.add(s[0] - s[0x17] * s[0x29] == -0x2e1c)
solver.add(s[0x1b] * s[0x1d] * s[2] == 0xf390d)
solver.add(s[0x19] - s[0x23] * s[0x13] == -0x1d34)
solver.add(s[0x10] - s[7] * s[0x13] == -0x14af)
solver.add(s[0x16] + s[0x21] + s[0x1a] * s[0xc] == 0xaa8)
solver.add(s[0x20] + s[0x18] + s[0x29] == 0x119)
solver.add(s[0xe] * s[0x1f] * s[0x17] == 0xc0e04)
solver.add((s[0x23] - s[6] * s[0x23]) - s[0xe] == -0xd0e)
solver.add((s[0x1f] + s[0x28]) - s[0x19] * s[0x11] == -0x2b8c)
solver.add(s[0x13] * s[0xd] + s[0x12] * s[0x24] == 0x3fec)
solver.add(s[0x12] * s[2] + (s[0x28] - s[5]) == 0x1137)
solver.add(s[3] + (s[0x15] - s[0x19]) == 0x37)
solver.add((s[0xd] + s[0xe] + s[0xe]) - s[2] == 0xdf)
solver.add(s[0x23] * s[0x24] - s[0x1d] * s[5] == -0x991)
solver.add(s[1] + (s[0x29] - s[0x27]) == 0x87)
solver.add(s[0] + (s[0x23] - s[0x23] * s[0]) == -0x1297)
solver.add((s[8] - s[0x15] * s[10]) - s[0x1f] == -0x12a8)
solver.add(s[0x1c] + (s[0x1d] - s[0x18]) == 0x7e)
solver.add((s[10] * s[0] - s[0x20]) - s[8] == 0xcf3)
solver.add(s[0x29] + s[0x20] * s[0x1c] == 0x170f)
solver.add(s[0x20] + (s[0x25] - s[0x18]) == 0x14)
solver.add(s[0x1f] + (s[10] * s[0x14] - s[0xf]) == 0x1250)
solver.add((s[0x24] - s[9]) - s[0x12] * s[0x12] == -0xaa1)
solver.add(s[0x1e] * s[0x10] + s[7] * s[9] == 0x3634)
solver.add((s[0x18] + s[0x22] + s[0x12]) - s[7] == 0xbc)
solver.add(s[0x14] + s[0x1b] * s[0x10] == 0x245e)
solver.add(((s[0x16] - s[0x1e]) - s[0x25]) - s[9] == -0xd3)
solver.add(s[0x1b] * s[0x29] * s[4] - s[0x26] == 0x16c156)
solver.add(s[0xd] + (s[0x23] - s[8] * s[0x1d]) == -0x334b)
solver.add(((s[0x17] - s[7]) - s[0x18]) - s[0x16] == -0x6b)
solver.add(s[5] * s[4] * s[0x25] == 0x88d04)
solver.add(s[0x20] * s[0x11] - s[0xf] == 0x14af)
solver.add((s[0x20] + s[0x12] * s[0x17]) - s[5] == 0x133f)
solver.add(s[0x27] + s[3] + s[0x27] * s[8] == 0x1ce5)
solver.add(s[0x24] + (s[0x19] * s[7] - s[3]) == 0x15dd)
solver.add((s[9] - s[0x18]) - s[0x21] == -0x4f)
solver.add(s[0x24] * s[0xe] + s[0x1e] == 0x2015)

for (i, c) in enumerate('GLUG{C01nc1d'):
    solver.add(s[i] == ord(c))

solver.add(s[41] == ord('}'))

for i in range(42):
    solver.add(s[i] >= 0x21)
    solver.add(s[i] <= 0x7e)

print(solver.check())

model = solver.model()
for i in range(42):
    print(chr(solver.model().eval(s[i]).as_long()), end='')
```

This then gives the flag `GLUG{C01nc1d3nc3_c4n_b3_fr3aky_T6LSERDYB6}`.

