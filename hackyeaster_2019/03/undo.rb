require"base64"
c = Base64.decode64("K7sAYzGlYx0kZyXIIPrXxK22DkU4Q+rTGfUk9i9vA60C/ZcQOSWNfJLTu4RpIBy/27yK5CBW+UrBhm0=")

x = c.split('').map(&:ord).map{|e| e.to_s(16).rjust(2, '0')}.join.hex
ox = (x / ['5'].cycle(101).to_a.join.to_i).to_s(16)
plaintext = ox.scan(/../).map(&:hex).map(&:chr).join
p plaintext
