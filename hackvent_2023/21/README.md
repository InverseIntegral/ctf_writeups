# 21 - Shopping List

## Description

Level: Hard<br/>
Author: Fabi_07

Santa still needs to buy some gifts, but he tends to forget things easily. That's why he created his own application: A
shopping list with state-of-the-art hacker protection.

## Solution

For this hard pwn challenge we were given [shopping-list.zip](shopping-list.zip) which contained a Dockerfile as well as
an ELF binary. We are also given a remote target to attack. Opening the binary in Ghidra and doing some reversing, we
get:

```c
void main() {
  setvbuf(stdin, 0, _IONBF, 0);
  setvbuf(stdout, 0, _IONBF, 0);
  setvbuf(stderr, 0, _IONBF, 0);
  puts("Shoppinglist creator ...");
  items = malloc(800);
  do {
    menu();
  } while( true );
}

void menu() {
  puts("What do you want to do?");
  puts("[a]dd a item");
  puts("[l]ist items");
  puts("[f]etch shopping list from file");
  puts("[s]ave your shopping list");
  puts("[e]dit a item");
  puts("[c]hange the quantity of a item");
  puts("[r]emove a item");
  puts("[q]uit");
  
  char option = getchr(" > ");
  
  switch(uVar1) {
  case 'a':
    int i = 0;
    while (i < 100 && *(items + i * 8) != 0) {
      i++;
    }
    
    byte item[3]= malloc(24);
    char* name = getstr("Name of the item: );
    size_t lentgh = strlen(name);
    long int amount = getNum("How many of this item do you need?");
    
    item[0] = name;
    item[1] = length;
    item[2] = amount;
    
    *(i * 8 + items) = item;
    printf("Added %ldx %s to your shopping list.", item[2], item[0]);
    itemCount++;
    break;
  default:
    puts("Please chose one of the options above);
    break;
  case 'c:
    char* name = getstr("Which item quantity do you want to change?");
    int found = -1;
    
    for (int i = 0; i < 100; i++) {
      if (*(items + i * 8) != 0) && (index = strstr(**(items + i * 8), name), index != 0)) {
        found = index;
      }
    }
    
    if (found < 0) {
      printf("No item found with named %s.",name);
    } else {
      printf("How many do you need");
      scanf("%ld%*c", *(items + found * 8) + 16); // writes to amount
      
      if (*(items + found * 8) + 8== 1337) {
        printf("You\'ve found my little secret, as a reward you will get: %p\n\n", win);
      }
      
      free(name);
      puts("");
    }
    break;
  case 'e':
    char *name = getstr("Which item do you want to edit?");
    int found = -1;
    
    for (int i = 0; i < 100; i++) {
      if (*(items + i * 8) != 0) && (index = strstr(**(items + i * 8), name), index != 0)) {
        found = index;
      }
    }
    
    if (found < 0) {
      printf("No item found with named %s.", name);
    } else {
      free(name);
      printf("New name of the item:");
      gets(**(items + found * 8));
      puts("");
    }
    
    break;
  case 'f':
    puts("This method will be implemented in a future release.");
    break;
  case 'l':
    if (itemCount == 0) {
      puts("Please add a item to your shopping list first.");
    } else {
      printf("Your shopping list:");
      for (int i = 0; i < 100; i++) {
        if (*(items + i * 8) != 0) {
          printf(" - %ldx %s\n",
                    *(items + i * 8 + 16), // amount
                   **(items + i * 8)); // name
        }
      }
      puts("");
    }
    break;
  case 'q':
    puts("Bye.");
    exit(0);
  case 'r':
    char *name = getstr("Which item do you want to delete?");
    int found = -1;
    
    for (int i = 0; i < 100; i++) {
      if (*(items + i * 8) != 0) && (index = strstr(**(items + i * 8), name), index != 0)) {
        found = index;
      }
    }
    
    if (found < 0) {
      printf("No item found with named %s.", name);
    } else {
      printf("Do you really want to delete %s (y/n)?", name);
      if (getchr(&) == 'y') {
        printf("Removed %s.\n\n", name);
        free(**(items + found * 8));
        free(*(items + found * 8));
        free(name);
        *(items + found * 8) = 0;
        itemCount--;
      }
    }
    break;
  case 's':
    if (itemCount == 0) {
      puts("Please add a item to your shopping list first.");
    } else {
      char *fileName = getstr("Filename:");

      if (strchr(fileName, "/") != 0) {
        puts("hacker detected, you will be reported to the admin.");
        FILE *flag = fopen("flag","w");
        fwrite("I told you not to overstep any boundaries...", 1, 44, flag);
        fclose(flag);
      }
      
      FILE *file = fopen(fileName, "w");
      fwrite("# Shopping list\n", 1, 16, file);
      
      for (int i = 0; i < 100; i++) {
        if (*(items + i * 8) != 0) {
          fprintf(file, " - %ldx %s\n",
                    *(items + i * 8 + 16), // amount
                   **(items + i * 8)); // name
        }
      }
      
      fclose(file);
      printf("Created File %s.", fileName);
      free(fileName);
    }
  }
  return;
}

void win() {
  execve("/bin/sh", 0, 0);
}
```

Okay, that's quite a lot to understand. The challenge looks like a heap challenge where we can abuse the structure of
the items to:

- Leak a libc address by modifying the string pointer of a neighboring item
- Create an item of size `1337` to get the address of `win`
- [Leak a stack address via environ](https://github.com/Naetw/CTF-pwn-tips#leak-stack-address)
- Finally, modify the return address to get RCE via `win`

That would have been the intended solution. Luckily, there's this neat function `s` that allows us to write an almost
arbitrary file. Initially, I overwrote `vuln` to see what would happen. Reconnecting to the remote no longer worked.
This confirmed my suspicion. All I had to do at this point was to write a valid shell script via the `s`
function into `vuln` and then reconnect to get the flag. I chose the name `|| cat *` and the amount `1` and wrote this
file:

```
# Shopping list
 - 1x || cat *
```

The first row starting with `#` and is therefore ignored. The `||` ignores the preceding command that is invalid
, then I simply cat all files of the current directory. With this I get the flag:

```
█▀▀▀▀▀█ ▀▀█▀▄█▄▀▄▀▀█  █▀▀▀▀▀█
█ ███ █ ▀▀█ ▄▀█▄▀▄▀█▀ █ ███ █
█ ▀▀▀ █ ▀ ▀█▄ ▄ ▀ █▀▀ █ ▀▀▀ █
▀▀▀▀▀▀▀ █ ▀ █ ▀▄▀ ▀ █ ▀▀▀▀▀▀▀
▄▄▄▄▄█▀▄▄▄▄▄▄█ ▄ ▄▀ █ █ █▄▀▄█
▀ ▀█▀▀▀ ▀▄ ▀▄ ▀ ▀▀▀ ▀▀▀█▀█▀  
 ▀▄▄  ▀▀█▄ ▀▄ █▀▀▄▄█  ██▄▄ █ 
 ▄▀  ▄▀█▀▀▀▄▄▀ ▀█ ▀ ▄ ▀▀  █▄▄
█▀█ ▄ ▀█▀█▄█ ▄ ▄ ▀▄ ▄ ▀█▄█▀  
█▀█▄ ▄▀██  ▄ ██▀█▀ ▄█▄▀█ ▄  █
▀  ▀▀▀▀▀▄▄▄▄██▄▄▀▀▄██▀▀▀█ █▀ 
█▀▀▀▀▀█ ▄ ▄ ▀▄▄▄▄█ ██ ▀ █ ▀  
█ ███ █  ███▀▄█▄ █▄█▀█▀█▀ ▄█ 
█ ▀▀▀ █  ▀  █▄▀ ▄▄█▄▄▀██▀█▄▀▄
▀▀▀▀▀▀▀  ▀▀  ▀▀▀▀▀      ▀ ▀  
```

Which decodes to `HV23{heap4the_win}`. Well, I think the intended solution would have been a lot harder, so I was happy
to get some rest. The next challenge ([HV22](../22/README.md)) would prove to be a tricky pwn challenge too.