# 04 - Bowser

## Description

Level: Easy<br/>
Author: veganjay

Santa has heard that some kids appreciate a video game as a christmas gift. He would rather have the kids solve some CTF
challenges, so he took some inspiration and turned it into a challenge. Can you save the princess?

## Solution

We are given [bowser.elf](bowser.elf) an 64-bit ELF that is not stripped. Running it shows the following output:

```
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠲⣄⢀⡀⠀⠀⠀⠀⠀⠀⢀⠄⠀⣸⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡄⠈⠳⣕⢄⠀⠀⠀⠀⢠⣏⠀⠀⣹⡆⠀⠀⠀⠀⠀⠀⣀⡀⣀⠀⠀⠀⣄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡇⠀⠀⢸⡿⡷⣄⣤⣾⣿⣯⣿⣿⣿⣧⡀⠀⠀⢀⠀⠀⠈⣻⣿⣻⢿⣶⢿⣷⣄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣽⠀⡖⣯⢳⣿⣿⣿⡟⠛⡞⣿⣽⣿⣿⣧⣼⠃⢸⣧⣷⣿⡟⣷⣯⡟⣾⢻⡞⣿⡆⠀⠀⠀⠀⢠⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠠⠤⣀⡀⠀⠀⠀⠀⣀⣼⣧⠽⠒⠋⠉⠉⠉⠉⠉⠙⠓⠿⠿⠛⠋⠉⣄⠀⢻⣿⣿⡿⣽⣳⢯⡿⣽⢯⡿⣽⣷⠀⠀⠀⠀⢸⡄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠱⡀⠀⠈⠉⢓⢾⣿⡿⠋⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣀⣿⡄⠀⠐⢹⣿⡷⣯⢿⡽⣯⢿⡽⣷⣿⠀⢀⣤⣷⣼⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠘⢦⠀⣠⢯⡿⠋⠀⠀⠀⠀⢀⣀⠀⠀⢀⣠⣆⣴⡄⣀⠀⢄⠂⠄⡷⠻⣦⣤⣾⣿⣽⣯⡿⣽⢿⣾⡉⢏⡿⣿⣿⣻⣿⠋⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⢿⣵⠟⠀⢀⡠⠔⠚⠉⣡⡈⠉⠉⠛⠻⣿⣿⣿⣷⣮⣦⣴⣾⣷⣿⠿⠿⠾⣌⣛⡟⠉⣻⣯⣿⣧⠨⣽⣿⣞⣿⠟⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⣼⠏⠀⡔⠁⠀⠀⠀⣀⢴⣹⠶⢳⣀⠀⢻⣿⣛⡹⠿⠿⣿⣭⠝⠀⠀⠀⠀⠈⠹⣷⣤⣿⣈⣽⣻⠵⠿⠿⣭⣿⣤⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⣠⣼⡟⠀⣸⠀⠀⠀⠀⣦⣾⣿⣿⣿⣿⡿⠟⠚⠋⢄⡀⠀⢰⠋⢳⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⠀⠈⠀⠀⠐⠋⣟⣿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⢄⡀⠀⠀⠀⠈⡷⡿⠀⠀⡇⠀⠀⢠⣮⣁⣽⣿⣿⠟⠋⠁⠀⠀⢀⠞⠻⣦⢾⣦⡾⠁⠀⢠⢶⣷⡀⠀⠀⠀⠀⠀⠈⣇⠀⠀⠀⣠⡾⣼⡟⡆⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠉⠲⢤⣠⡴⣹⠃⠀⠀⣧⠀⢠⣾⣿⣿⣿⠏⠀⠀⠀⠱⣽⠞⢻⠦⡤⢿⣌⢿⣿⣤⠀⠈⣿⠿⣷⡄⣀⠀⠀⠀⣠⠹⣄⣠⠾⢋⡴⢇⢣⠇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠘⢷⡟⠀⠀⠀⣿⢤⠘⣿⣿⣿⡏⠀⢠⡀⠀⠀⣸⣷⢪⠝⣰⢃⡞⢮⣿⣿⡄⠀⢹⣶⣿⣿⣶⡴⢶⣿⣲⣯⣿⣿⡏⡙⣬⠼⠋⠀⠀⠀⠀⠀⠀⣠⡄⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⢸⠁⠀⠀⣸⠇⢸⣿⠏⣿⣿⡁⠀⠀⢿⣆⡾⠀⣿⣇⠹⣆⢏⡸⢆⡈⣹⣷⡀⢸⠏⢸⣿⣿⣷⣿⣿⣿⣿⣿⣾⣇⣾⢀⣶⣆⣀⣀⣀⣰⠶⡿⢱⠎⣀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠸⠀⠀⢀⡏⢀⣿⣽⠲⢾⣿⡇⠀⠠⢜⢢⠟⣦⡼⢧⢋⡖⢎⡱⠮⢵⡏⡹⡇⠀⠑⣿⡿⠛⣿⣿⣿⣿⡿⣭⣟⣹⣿⣿⣾⣿⡟⢏⡱⢌⢣⡱⢣⣫⢖⢧⣋⠖⠄
⠀⠀⠀⠀⢠⠀⠀⠀⡘⠁⣼⡿⠁⠀⠀⠉⠛⠦⣵⣎⣦⠕⢊⣀⣊⣜⠸⣏⡛⡛⠞⡹⠳⣷⠀⠀⠀⠁⠀⠋⠉⠉⠉⠀⠻⣧⣿⣿⣿⣿⢣⡙⣌⠲⣩⢲⡱⣣⠏⣎⣓⡬⠆⠀
⠀⠀⠀⠀⠎⠀⠀⠠⠁⢠⡿⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⡴⢢⡔⡿⠀⠑⠨⠙⠶⣥⣆⣑⠌⢣⡀⠀⠀⠀⢀⠀⣀⠂⣄⡾⢩⣿⣹⣻⣿⠋⠛⠛⠶⣇⢇⡚⡥⢞⡭⣚⠼⣱⡀
⠀⠀⠀⠀⣽⠀⠀⠄⢐⡾⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠑⣌⠳⣼⡅⠈⡲⢦⣉⠒⡰⢈⠉⡉⢉⣽⡷⣶⣟⡛⠻⢤⡃⠊⡤⣞⣿⣿⣿⣿⣆⠀⠀⣠⠞⢾⡴⡙⡮⠆⠉⢚⠀⠃
⠀⠀⠀⠀⣯⠽⠖⠖⢻⡁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠠⢌⠻⣜⢛⢦⡁⢆⡉⡙⠁⠂⠀⣴⡞⢯⡜⢧⡹⣛⣦⡀⠉⠓⠛⠶⠾⣿⣿⣿⣿⣷⣦⣽⣦⣤⠷⠋⠁⠀⠀⠀⠀⠀
⢀⣠⠴⠚⠁⢠⠠⡀⠼⡆⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠐⢌⢣⡝⡌⠦⡉⢆⡐⠄⠁⣴⣞⠳⣜⢣⠞⣥⠳⣍⠞⣵⡀⠀⠀⠀⠀⠀⠉⠙⠛⠋⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠘⠻⢦⣱⣌⢢⡑⣌⠲⡿⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠌⡜⡲⠜⢨⠓⠈⢄⣠⣴⢛⢧⠪⡝⣌⢧⣋⠶⡹⢌⡻⢼⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠈⠉⠉⠉⢉⡇⠀⠀⠀⠀⠀⠀⠀⠀⡀⢢⠑⡬⢱⢩⠟⠙⠛⠛⠒⣳⢏⠶⣙⠼⣘⠦⣍⢮⡱⣍⣾⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⡎⠀⠀⠀⠀⠀⠀⠀⠄⢢⢅⢣⠚⡔⢣⠏⠀⠀⠀⠀⠀⣟⢎⡳⣉⠮⢥⢫⠴⣢⢓⢾⣁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⡸⠀⠀⠀⠀⠀⠀⢀⠇⡸⢃⠼⡘⠟⣸⠟⠀⠀⠀⠀⠀⢸⣛⡜⢣⡛⡼⣃⢟⡼⣣⢟⡻⢼⢧⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⢰⠃⠀⠀⠀⠀⠠⠨⣐⢪⢑⡋⣎⣱⠽⠃⠀⠀⠀⠀⠀⠀⣿⢄⡏⢧⡙⢶⠩⡞⢴⢣⠎⣽⡷⣿⣻⡄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⣾⠀⠀⠀⠀⠀⠀⠀⠀⠈⠈⣏⠉⣻⣆⠀⠀⠀⠀⠀⠀⠀⠈⠚⠾⠧⠾⠥⠿⠼⠾⠾⠽⠾⠓⠓⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠘⠦⣀⠀⢀⡤⠒⢦⣠⠖⠚⣟⡎⠙⡏⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠉⠑⠒⠤⠞⠻⠦⢄⡟⠋⠒⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
Usage: ./bowser.elf password
```

We are asked for a password, if we enter something as a password, the message becomes:

```
Sorry, that is not the correct password.
```

Decompiling the binary shows us the expected password (`mario`). Providing that as input still shows:

```
Sorry, your flag is in another castle.
```

Decompiling the code using Ghidra we see this:

```c
void main() {
  char* flag = { -0x54, -0x70, -0x73, -0x73, -0x7a, -0x2d, -0x21, -0x7a, -0x70, -0x76, -0x73, -0x21, -0x67, -0x6d, -0x62, -0x68, -0x21, -0x6a, -0x74, -0x21, -0x6a, -0x6f, -0x21, -0x62, -0x6f, -0x70, -0x75, -0x69, -0x66, -0x73, -0x21, -100, -0x62, -0x74, -0x75, -0x6d, -0x66, -0x2f, -1, -0x49, -0x57, -0x33, -0x34, -0x7c, -0x5a, -0x70, -0x76, -0x60, -0x49, -0x62, -0x77, -0x66, -0x60, -0x54, -0x62, -0x77, -0x66, -0x65, -0x60, -0x75, -0x69, -0x66, -0x60, -0x51, -0x73, -0x6a, -0x6f, -100, -0x66, -0x74, -0x74, -0x7e };
  
  bowser();
  
  if (argc == 2) {
    if (strcmp(argv[1],"mario") == 0) {
      for (uint8_t* c = &flag; *c != '\0'; c++) {
        *c = ~*c;
      }
      puts((char *) &flag);
    } else {
      puts("Sorry, that is not the correct password.");
    }
  } else {
    printf("Usage: %s password\n",*argv);
  }
}
```

If the password was correct, there seems to be a loop that then prints the flag. If we write the same in Python to
obtain the actual flag:

```python
ct = oo = [-0x54, -0x70, -0x73, -0x73, -0x7a, -0x2d, -0x21, -0x7a, -0x70, -0x76, -0x73, -0x21, -0x67, -0x6d, -0x62,
           -0x68, -0x21, -0x6a, -0x74, -0x21, -0x6a, -0x6f, -0x21, -0x62, -0x6f, -0x70, -0x75, -0x69, -0x66, -0x73,
           -0x21, -100, -0x62, -0x74, -0x75, -0x6d, -0x66, -0x2f, -1, -0x49, -0x57, -0x33, -0x34, -0x7c, -0x5a, -0x70,
           -0x76, -0x60, -0x49, -0x62, -0x77, -0x66, -0x60, -0x54, -0x62, -0x77, -0x66, -0x65, -0x60, -0x75, -0x69,
           -0x66, -0x60, -0x51, -0x73, -0x6a, -0x6f, -100, -0x66, -0x74, -0x74, -0x7e]
''.join([chr(~c) for c in ct])
```

This prints `Sorry, your flag is in another castle.\x00HV23{You_Have_Saved_the_Princess}` which explains why the flag
was not printed, there's a null byte in the middle of the string.

Alternatively, we can solve this challenge using GDB. We simply set a breakpoint before the puts call and print
the stack variable:

```console
pwndbg> x/2s $rbp - 0x60

0x7fffffffdd80: "Sorry, your flag is in another castle."
0x7fffffffdda7: "HV23{You_Have_Saved_the_Princess}"
```

We print the two strings (due to the null terminator) at `$rbp - 0x60`. I really enjoyed this challenge, especially
since it can be solved using different tools. 
