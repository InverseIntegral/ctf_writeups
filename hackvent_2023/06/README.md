# 06 - Santa should use a password manager

## Description

Level: Easy<br/>
Author: wangibangi

Santa is getting old and has troubles remembering his password. He said password Managers are too complicated for him
and he found a better way. So he screenshotted his password and decided to store it somewhere handy, where he can always
find it and where its easy to access.

## Solution

For this challenge we are given a huge (2.1G) raw file. Checking the file type with `file memory.raw`:

```
memory.raw: Windows Event Trace Log
```

It turns out that the file is a snapshot of a Windows instance. The
tool [Volatility](https://github.com/volatilityfoundation/volatility) came in handy to extract the files of the
snapshot. I used version 3 of volatility since version 2 doesn't support Python 3 (😮‍💨).

```
python3 vol.py -f memory.raw windows.filescan.FileScan
```

This gave me a huge list of files present in the snapshot. Since the description hints at a picture, we can look for
interesting images. I've also tried to dump the contents of files that have been opened with `notepad.exe` which didn't
lead anywhere.

```
...
0x918b760e88e0  \Users\santa\AppData\Local\Packages\Microsoft.Windows.Photos_8wekyb3d8bbwe\LocalState\PhotosAppBackground\wallpaper.png 216
...
```

Now, we can dump the file at the address:

```
python3 vol.py -f memory.raw windows.dumpfiles --virtaddr 0x918b760e88e0
```

And we obtain the wallpaper of santa which contains the flag as a QR code:

![The wallpaper containing the flag as a QR code](solution.png)

Scanning it gives `HV23{FANCY-W4LLP4p3r}`.
