# Day 23: muffinCTF

On the third day of the muffinCTF we got two new services as well: barracks and keep. The check if keep was available
didn't work at first because the folder wasn't owned by the user keep. To change this I ran `chown -R keep /home/keep/`.

I only ran the keep service to get the flag. No one was exploiting it so I left it unpatched. With the exploits from
above I was able to get the flag for day 3.
