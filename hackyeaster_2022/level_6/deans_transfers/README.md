# Dean's Transfers

## Description
Level: medium<br/>
Author: PS

Dean just launched his taxi business named Dean's Transfers.

For his website, he first wanted to register `deans-transfers.com`, but then found out there are so many fancy top-level
domains out there. You found a service running on his server - find a flag there!

The service is running on port 2211 on 46.101.107.117.

## Solution

For this challenge we are given a remote service as well as a hint that we have to find a domain name. I immediately
thought that the service would be a DNS server. To confirm this I performed a check using nmap:

``` 
$ nmap 46.101.107.117 -p 2211 -sV

PORT     STATE SERVICE VERSION
2211/tcp open  domain  ISC BIND 9.11.5-P4-5.1+deb10u6 (Debian Linux)
```

That seems to be correct. Now it's time to find the correct TLD for the name `deans-transfers`. To do this I wrote a
script that enumerates all TLDs and performs an AXFR query.

```python
import subprocess

# all TLDs
endings = []

for end in endings:
    cmd = f"drill AXFR deans-transfers.{end} @46.101.107.117 -p2211 "
    res = subprocess.getoutput(cmd)
    print(res)
```

One TLD was interesting:

```
;; ANSWER SECTION:
;; AUTHORITY SECTION:
deans-transfers.express.	302400	IN	SOA	deans-transfers.express. admin.deans-transfers.express.deans-transfers.express. 2 302400 43200 302400 302400
```

I reran the command and got the flag as a base64 encoded subdomain:

```
$ drill AXFR deans-transfers.express @46.101.107.117 -p2211

deans-transfers.express.	302400	IN	SOA	deans-transfers.express. admin.deans-transfers.express.deans-transfers.express. 2 302400 43200 302400 302400
deans-transfers.express.	302400	IN	NS	ns.deans-transfers.express.
aGUyMDIye2QzNG5fZHIxdjNzX3lvdV8zdjNyeXdoM3IzISF9.deans-transfers.express.	302400	IN	A	10.0.0.8
base64decode.deans-transfers.express.	302400	IN	A	10.0.13.9
ns.deans-transfers.express.	302400	IN	A	10.0.0.2
deans-transfers.express.	302400	IN	SOA	deans-transfers.express. admin.deans-transfers.express.deans-transfers.express. 2 302400 43200 302400 302400
```

With `echo "aGUyMDIye2QzNG5fZHIxdjNzX3lvdV8zdjNyeXdoM3IzISF9" | base64 -d` we get the flag `he2022{d34n_dr1v3s_you_3v3rywh3r3!!}`.

