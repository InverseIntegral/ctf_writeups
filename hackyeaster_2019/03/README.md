# 03 - Sloppy Encryption

## Description

Level: easy<br/>
Author: readmyusername

The easterbunny is not advanced at doing math and also really sloppy.
He lost the encryption script while hiding your challenge. Can you decrypt it?

```
K7sAYzGlYx0kZyXIIPrXxK22DkU4Q+rTGfUk9i9vA60C/ZcQOSWNfJLTu4RpIBy/27yK5CBW+UrBhm0= 
```

```ruby
require"base64"
puts"write some text and hit enter:"
input=gets.chomp
h=input.unpack('C'*input.length).collect{|x|x.to_s(16)}.join
ox='%#X'%h.to_i(16)
x=ox.to_i(16)*['5'].cycle(101).to_a.join.to_i
c=x.to_s(16).scan(/../).map(&:hex).map(&:chr).join
b=Base64.encode64(c)
puts"encrypted text:""#{b}"
```

## Solution

To solve this challenge I simply reversed the "encryption" script and ran it on the ciphertext:

```ruby
require"base64"
c = Base64.decode64("K7sAYzGlYx0kZyXIIPrXxK22DkU4Q+rTGfUk9i9vA60C/ZcQOSWNfJLTu4RpIBy/27yK5CBW+UrBhm0=")

x = c.split('').map(&:ord).map{|e| e.to_s(16).rjust(2, '0')}.join.hex
ox = (x / ['5'].cycle(101).to_a.join.to_i).to_s(16)
plaintext = ox.scan(/../).map(&:hex).map(&:chr).join
p plaintext
```

This gave me the plaintext *n00b_style_crypto*.
