# 07 - Grinch's Portscan

## Description

The elves port-scanned grinch's server and noticed something strange.
There's a secret message hidden in the [packet capture](a343d168-3e6e-42b7-af30-b27c1c03de12.pcap), can you find it?

## Solution

We are given a packet capture file which we can view in wireshark. The packet capture recorded lots of TCP requests from
various source and destination ports. In a first step I filtered the packets to only include TCP packets being sent from the
client to the server `tcp && ip.src==172.16.66.10`. Looking at the source and destination ports as decimal ASCII values,
I could see that the source ports contained the flag. But there was a lot of superfluous packages in the current filter.
After a while I came up with the filter `!(frame.len == 66) && tcp && ip.src==172.16.66.10 && tcp.flags == 0x010 &&
tcp.seq == 1` that seemd to do the trick. The source ports of the packages with this filter were:

```
72
86
50
49
123
123
99
48
110
102
117
115
101
95
80
111
114
116
115
99
52
110
110
51
114
115
125
```

This gave the flag `HV21{c0nfuse_Portsc4nn3rs}`.

