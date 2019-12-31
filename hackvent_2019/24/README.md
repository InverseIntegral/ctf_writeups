# 24 - ham radio

## Description

Level: leet<br/>
Author: DrSchottky

Elves built for santa a special radio to help him coordinating today's presents delivery.

[HV19-ham radio.zip](19bf7592-f3ee-474c-bf82-233f270bbf70.zip)

## Solution

For this challenge we get a file called brcmfmac43430-sdio.bin. A GitHub search shows that this is a firmware for a
Broadcom BCM43430 wifi chip. Such chips are apparently used in certain Raspberry Pi models. By running `strings
brcmfmac43430-sdio.bin` I found that the firmware was patched using [nexmon](https://github.com/seemoo-lab/nexmon). This
will be useful later on. The next thing I did was to open the file in Ghidra and search for interesting strings. The
first one that I found was
`Um9zZXMgYXJlIHJlZCwgVmlvbGV0cyBhcmUgYmx1ZSwgRHJTY2hvdHRreSBsb3ZlcyBob29raW5nIGlvY3Rscywgd2h5IHNob3VsZG4ndCB5b3U` and
using [CyberChef](https://gchq.github.io/CyberChef/)'s magic function I found out that this was Base64 for `Roses are
red, Violets are blue, DrSchottky loves hooking ioctls, why shouldn't you`. Search for references to this string lead to
the following function:

```c
if (cmd == 0xcafe) {
  strncpy(ioctl_buffer, roses, length);
  return 0;
}

if (cmd == 0xd00d) {
   memcpy(data, 0x800000, 0x17);
   return 0;
}

if (cmd == 0x1337) {
  do {
    data  = data + 1;
    stack = stack + 1;
    *data = *data ^ *stack;
  } while (data != stack + 2));
  strncpy(ioctl_buffer, data, length);
  return 0;
}

return wlc_ioctl(wlc, cmd, ioctl_buffer, length, wlc_if);
```

Note that I renamed some functions and restructured the code to make it more readable. I mainly used [this
example](https://github.com/seemoo-lab/nexmon/blob/master/patches/bcm43430a1/7_45_41_46/nexmon/src/ioctl.c#L50) as a
reference of an ioctl hook and [the address to function
mapping](https://github.com/seemoo-lab/nexmon/blob/master/patches/common/wrapper.c). I assumed that the function was
first called with the command `0xd00d` and then with `0x1337`. The first command simply copies data from `0x800000` to a
location and the second one XORs it with data from the stack. [From
here](https://github.com/seemoo-lab/nexmon/blob/d5fd58656942d770e59764d88130b09d0a701c77/firmwares/bcm43430a1/7_45_41_26/definitions.mk#L13)
I knew that the ROM would start at `0x800000`. From [this repository](https://github.com/seemoo-lab/bcm_misc) I was able
to get a dumped ROM and I then extracted the first few bytes `41ea000313439b0730b510d10c680368634013604c6843`. Knowing
that `HV19{ XOR 41ea000313439b0730b510d10c680368634013604c6843 == ciphertext` I was able to search for the ciphertext in
the binary which was `09bc313a681aab7247867ee64a1d6f042e74500d78063e`. Finally, I could calculate the flag as
`41ea000313439b0730b510d10c680368634013604c6843 XOR 09bc313a681aab7247867ee64a1d6f042e74500d78063e` which gave me
`HV19{Y0uw3n7FullM4Cm4n}`.
