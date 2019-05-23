# Day 16: Pay 100 Bitcoins

In this challenge we get an ova file that has been infected by the ransomware Petya. The bootloader has been overwritten
and when booting it the master file table will be encrypted. Our goal is to safe the data and find the flag. We also
know that the OS is encrypted with the password `IWillNeverGetAVirus`.

With `tar-xvf HACKvent_thx_awesome_1n1k.ova` we extract the vmdk file and then we mount it. Then we have to decrypt the
volume using `cryptsetup`. Now we can access the files and get the flag.
