# 07 - The golden book of Santa

## Description

Level: Easy<br/>
Author: darkstar

An employee found out that someone is selling secret information from Santa's golden book. For security reasons, the
service for accessing the book was immediately stopped and there is now only a note about the maintenance work. However,
it still seems possible that someone is leaking secret data.

## Solution

For this challenge we are given a server that listens on TCP port 80. We also know that this challenge is about someone
exfiltrating data. I tried to connect to the server using `nc` and quickly realized that it was indeed a custom
webserver. I used wireshark and curl to get a response and pretty soon realized that the response used a chunked
encoding. Strangely enough, each chunk had a different size. I decided to simply dump the content of the chunk encoded
response into a file [response.raw](response.raw).

[The format of the chunked transfer encoding](https://en.wikipedia.org/wiki/Chunked_transfer_encoding#Format) is quite
simple: Each chunk starts with the number of bytes of data it embeds expressed as a hexadecimal number, followed by the
chunk data. The chunk is terminated by `\r\n`. If we open the file and simply read all lines and try to parse each one
as a number, we obtain exactly the lines that contain a number of bytes in the chunk (i.e., the flag part). After some
experimenting with this, I realized that the first character of the length field always started with `9` which seemed
out of place. Instead of taking the whole lines, I dropped the first character.

```pthon
flag = ""

with open('response.raw', mode='rb') as file:
    lines = file.readlines()

    for line in lines:
        try:
            flag += chr(int(line[1:], 16))
        except:
            pass

print(flag)
```

As described above, the script tries to parse all byte length fields of the chunked transfer encoding. If it's able to
parse the hex representation of the line (minus the first char) then it adds it to the flag. With this we get the
output: `HV23{here_is_your_gift_in_small_pieces}`.
