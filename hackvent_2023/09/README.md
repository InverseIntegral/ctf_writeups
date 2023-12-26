# 09 - Passage encryption

## Description

Level: Medium<br/>
Author: dr_nick

Santa looked at the network logs of his machine and noticed that one of the elves browsed a weird website. He managed to
get the pcap of it, and it seems as though there is some sensitive information in there?!

## Solution

We are given [a pcap capture file](secret_capture.pcapng) that recorded a few HTTP requests. First, a few pictures of
doors are loaded and then
specific doors are requested (via GET parameters) in some order. At first, I thought that the order of requested doors
would hint at the flag. This didn't lead anywhere but the ports that are involved in the request seem interesting too.
Now the whole door theme makes more sense, it's about port knocking. Extracting the list of involved ports gives us:

```
56626
56772
56786
56750
56751
56823
56776
56811
56748
56807
56749
56810
56803
56795
56802
56811
56814
56795
56812
56811
56814
56816
56753
56795
56810
56811
56755
56795
56800
56811
56748
56814
56736
56825
56700
```

A typical flag starts with `HV23` or `72 86 50 51 123` in decimal. Comparing this to our numbers, we can see that
they differ by exactly 56700. Subtracting this value from all the numbers and printing them as chars:

```python
ports = [56772, 56786, 56750, 56751, 56823, 56776, 56811, 56748, 56807, 56749, 56810, 56803, 56795, 56802, 56811, 56814,
         56795, 56812, 56811, 56814, 56816, 56753, 56795, 56810, 56811, 56755, 56795,
         56800, 56811, 56748, 56814, 56736, 56825]

for port in ports:
    print(chr(port - 56700), end='')
```

gives the flag `HV23{Lo0k1ng_for_port5_no7_do0r$}`.
