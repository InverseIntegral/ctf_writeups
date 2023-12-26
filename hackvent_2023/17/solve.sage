from Crypto.Util.number import bytes_to_long, long_to_bytes
from PIL import Image

im = Image.open('key.png')
rgb_im = im.convert('RGB')
width, height = im.size
sys.set_int_max_str_digits(999999999)
pq = im.tobytes()
size = len(pq) // 2

p = bytes_to_long(pq[:size])
q = bytes_to_long(pq[size:])

in_file = open("flag.enc", "rb")
data = in_file.read()

phi = (p - 1) * (q - 1)
n = p * q
ct = mod(bytes_to_long(data), n)

e = 65537
d = inverse_mod(e, phi)

c = pow(int(ct), int(d), int(n))
flag = long_to_bytes(c)

with open('flag.png', 'wb') as f:
    f.write(flag)