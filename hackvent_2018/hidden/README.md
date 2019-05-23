# Hidden

## Hidden 1

Similar to last years hidden challenge we had to use `telnet challenges.hackvent.hacking-lab.com`. This time we didn't
get a flag directly though. When looking at the snow animation we can see some characters flying by. We can write the
telnet output to a file and read the password.

The `challenges.hackvent.hacking-lab.com` host also runs an FTP server. With the password from before we can log into
Santas FTP. From there we can download the file named `flag`.

## Hidden 2

The second hidden flag can be found by checking the TXT entry of the `www.hackvent.org` domain. This subdomain can be found
when solving the challenge of day 5.

## Hidden 3

This one was hidden in the challenge of day 14. We have a value n and a thumbprint in the file:

```powershell
#thumbprint 1398ED7F59A62962D5A47DD0D32B71156DD6AF6B46BEA949976331B8E1
$n = [System.Numerics.BigInteger]::Parse("0D8A7A45D9BE42BB3F03F710CF105628E8080F6105224612481908DC721", 'AllowHexSpecifier');
```

This is an RSA encrypted message. We can factorize n into p and q using [factordb](http://factordb.com/index.php):

```
p = 73197682537506302133452713613304371
q = 79797652691134123797009598697137499
```

With the help of CrypTool it's now easy to decrypt the message and get the hidden flag.

![](images/cryptool.png)
