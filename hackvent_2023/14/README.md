# 14 - Crypto Dump

## Description

Level: Medium<br/>
Author: LogicalOverflow

To keep today's flag save, Santa encrypted it, but now the elf cannot figure out how to decrypt it. The tool just
crashes all the time. Can you still recover the flag?

## Solution

For this challenge we are given a 64bit ELF binary as well as a coredump where the binary crashed. Opening the ELF in
Ghidra and doing some reversing, we can see this code:

```c
undefined8 main(int argc, char *argv[]) {  
  FILE* key_file = fopen("./key","r");
  fseek(key_file, 0, SEEK_END);
  long length = ftell(key_file);
  
  // key is 32 byte long
  if (length == 32) {
    rewind(key_file);
    char key[32]; // const uint8_t*
    fread_unlocked(key, 1, 32, key_file);
    fclose(key_file);
    
    // open first argument into file_content
    FILE* file = fopen(argv[1], "r");
    fseek(file, 0, SEEK_END);
    long file_length = ftell(file);
    rewind(file);
    
    byte* file_content = malloc(file_length);
    fread_unlocked(file_content, 1, file_length, file);
    fclose(file);
    
    struct aes256_ctx* context = malloc(240);
    nettle_aes256_set_encrypt_key(context, key);
    
    int length;
    byte* content;
    
    // crash here
    if (argv[2] == 'd') {
     length = file_length - 16; // when decrypting the first block is the IV
     content = malloc(length);
     
     nettle_ctr_crypt(context,
          nettle_aes256_encrypt,
          16, // block_size
          file_content, // IV
          length, // length of the src
          content, // destination
          file_content + 16);  // src
    } else if (argv[2] == 'e') { {
      length = file_length + 16; // when encrypting the first block is the IV (prepended to the cyphertext)
      content = malloc(length);
      byte* iv = malloc(16);
      
      FILE* random = fopen("/dev/random", "r");
      fread_unlocked(iv, 1, 16, random);
      fclose(random);
      
      // move 16 byte from iv to start of content
      // rbp points to the start of content
      // R12 is the IV
      
      // MOVDQU     XMM0,xmmword ptr [R12]
      // MOVUPS     xmmword ptr [RBP],XMM0
      
      nettle_ctr_crypt(context,
        nettle_aes256_encrypt,
        16, // block_size
        content, // IV
        file_length, // length of the src
        content + 16, // destination
        file_content); // src
    }
    
    FILE* out_file = fopen("./out","w");
    fwrite(content, 1, length, out_file);
    fclose(out_file);
  }
}
```

When checking out the coredump file, we can see the exact reason why the program crashed:

```
16:00b0│-008 0x7ffeef3dd710 ◂— 0x2
17:00b8│ rbp 0x7ffeef3dd718 —▸ 0x7ffeef3dda36 ◂— 'flagsave'
18:00c0│+008 0x7ffeef3dd720 —▸ 0x7ffeef3dda3f ◂— './flag.enc'
19:00c8│+010 0x7ffeef3dd728 ◂— 0x0
```

This is the content of the stack, we see that there are 2 arguments being passed to the program (indicated by the `0x2`,
the two pointers as well as the terminating `0x0`). Then this exact instruction crashes:

```
   0x401136 <main+246>        mov    rax, qword ptr [rbp + 0x10]
   0x40113a <main+250>        movzx  eax, byte ptr [rax]
   0x40113d <main+253>        cmp    al, 0x64
```

`rbp + 0x10` is just `0x0` which gets dereference and moved into `eax` (instruction at `main+250`) and results in an
invalid memory access. Essentially, the program was called with too few arguments.

All we need to do now is to find the key that is loaded into memory as well as the cyphertext and then decrypt the
file `flag.enc`. Let's first get the key which is stored at `RSP + 0x10` at that point:

```
 RSP  0x7ffeef3dd660 ◂— 0x0
...
pwndbg> x/4xg $rsp+0x10                                                                                                                                                                                       
0x7ffeef3dd670: 0xc84141ac5c7daf9b      0x4bfc70d23ffa8ccb                                                                                                                                                         
0x7ffeef3dd680: 0x0a25540a54cda0ee      0x910f40cb948f8fd8                                                                                                                                                                                           
```

We have to be careful about the endianness. If we interpret it as little endian, we get the
key `9baf7d5cac4141c8cb8cfa3fd270fc4beea0cd540a54250ad88f8f94cb400f91`. Now, we just need to find the cyphertext (that
contains the IV at the beginning). `RBX` is the total length of `flag.enc` which is `0x2b = 43`. The cyphertext can be
found at `R13`:

```
pwndbg> x/6g $r13
0x7fc80c16f040: 0x14c90896ad3871af      0x25289fbe19febdbe
0x7fc80c16f050: 0x58453afd0fa798bd      0x6615bbf88e8d8f18
0x7fc80c16f060: 0xb5be3581610b5f73      0x0000000000c9800d
```

The IV is `af7138ad9608c914bebdfe19be9f2825` and the cyhertext is the following `43 - 16 = 27`
bytes `bd98a70ffd3a4558188f8d8ef8bb1566735f0b618135beb50d80c9`. [We can now use Cyberchef](https://gchq.github.io/CyberChef/#recipe=AES_Decrypt(%7B'option':'Hex','string':'9baf7d5cac4141c8cb8cfa3fd270fc4beea0cd540a54250ad88f8f94cb400f91'%7D,%7B'option':'Hex','string':'af7138ad9608c914bebdfe19be9f2825'%7D,'CTR','Hex','Raw',%7B'option':'Hex','string':''%7D,%7B'option':'Hex','string':''%7D)&input=YmQ5OGE3MGZmZDNhNDU1ODE4OGY4ZDhlZjhiYjE1NjY3MzVmMGI2MTgxMzViZWI1MGQ4MGM5)
to decrypt using AES 256 CTR and obtain the flag `HV23{17's_4ll_ri6h7_7h3r3}`. This was a very interesting and well-made
challenge, I learned something new which is always great. 