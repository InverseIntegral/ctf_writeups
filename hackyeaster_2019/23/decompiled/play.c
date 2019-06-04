int play() {
  // generates a random key and a random maze
  generateKey();
  initMaze();

  printf("\x1B[H\x1B[J");
  fflush(stdout);

  for (signed int i = 0; i <= 624; i++) {
    if (maze[i] == 2) {
      col = i % 25;
      row = i / 25;
      break;
    }
  }

  signed int shouldPickup = 0;

  while (true) {
    while (true) {
      printPosition();

      printf("\x1B[20;0H");
      printf("Enter your command:\n> ");
      fflush(stdout);
      fgets(userInput, 16, stdin);

      if (!strchr(userInput, 10)) {
        char readChar;

        do {
          readChar = fgetc(stdin);
        } while (readChar != 10 && readChar != -1);
      }

      fflush(stdin);
      printf("\x1B[H\x1B[J", 10LL);
      puts("\x1B[16;0H");

      if (!compareXOR(userInput, "':+6HB")) {
        printf("\x1B[H\x1B[J", "':+6HB");
        return fflush(stdout);
      }

      if (!compareXOR(userInput, "%-b,-06*HB")) {
        if (maze[25 * (row - 1) + col]) {
          row--;
          shouldPickup = 0;
        } else {
          printf("There is a wall!", "%-b,-06*HB");
        }

        goto END_LOOP;
      }

      if (!compareXOR(userInput, "%-b1-76*HB")) {
        if (maze[25 * (row + 1) + col]) {
          row++;
          shouldPickup = 0;
        } else {
          printf("There is a wall!", "%-b1-76*HB");
        }

        goto END_LOOP;
      }

      if (!compareXOR(userInput, "%-b5'16HB")) {
        if (maze[25 * row - 1 + col]) {
          col--;
          shouldPickup = 0;
        } else {
          printf("There is a wall!", "%-b5'16HB");
        }

        goto END_LOOP;
      }

      if (!compareXOR(userInput, "%-b'#16HB")) {
        if (maze[25 * row + 1 + col]) {
          col++;
          shouldPickup = 0;
        } else {
          printf("There is a wall!", "%-b'#16HB");
        }

        goto END_LOOP;
      }

      if (!compareXOR(userInput, "1'#0!*HB")) {
        shouldPickup = 1;

        if (maze[25 * row + col] == 3) {
          printf("You found a key!", "1'#0!*HB");
        } else if ( maze[25 * row + col] == 4) {
          printf("You found a locked chest!", "1'#0!*HB");
        } else if (rand() % 3) {
          puts(messages[0]);
        } else {
          puts(messages[rand() % 9]);
        }

        goto END_LOOP;
      }

      if (compareXOR(userInput, "2+!)b72HB")) {
        break;
      }

      if (shouldPickup) {
        if (maze[25 * row + col] == 3) {
          printf("You pick up the key: %s", s1);
        } else if (maze[25 * row + col] == 4) {
          printf("This is to heavy! You can't pick up that.", "2+!)b72HB");
        } else {
          printf("There is nothing you want to pick up!", "2+!)b72HB");
        }
      } else {
        printf("Maybe you should search first", "2+!)b72HB");
      }

      END_LOOP:
      fflush(stdout);
    }

    if (!compareXOR(userInput, "-2',HB")) {
      break;
    }

    if (!compareXOR(userInput, "5*-#/+HB")) {
      printf(&name, "5*-#/+HB"); // string format
      goto END_LOOP;
    }

    error();
  }

  if (!shouldPickup) {
    printf("Maybe you should search first", "-2',HB");
    goto END_LOOP;
  }

  if (maze[25 * row + col] != 4) {
    printf("There is nothing you can open!", "-2',HB");
    goto END_LOOP;
  }

  completePrintPosition();
  printf("\x1B[20;0H", "-2',HB");
  fflush(stdout);

  signed int tries = 3;
  while (true) {
    tries--;

    if (!tries) {
      printf("Next time get the right key!");
      printf("For now get out of here! Quickly!");

      fflush(stdin);
      exit(0);
    }

    printf("The chest is locked. Please enter the key:\n> ");
    fflush(stdout);
    fgets(keyInput, 40, stdin); // buffer overflow

    if (!strchr(keyInput, 10)) {
      while (fgetc(stdin) != 10);
    }

    fflush(stdin);

    if (!strncmp(s1, keyInput, strlen(s1))) {
      break;
    }

    puts("Sorry but that was the wrong key.");
  }

  printf("\x1B[H\x1B[J", keyInput);
  puts("Congratulation, you solved the maze. Here is your reward:");

  char* s = (char *) malloc(0x400);
  FILE* stream = fopen("egg.txt", "r");

  while (fgets(s, 1024, stream)) {
    printf("%s", s);
  }

  fclose(stream);
  printf("Press enter to return to the menue", 1024LL);
  fflush(stdout);

  char readChar2;

  do {
    readChar2 = fgetc(stdin);
  } while (readChar2 != 10 && readChar2 != -1);

  fflush(stdin);
  printf("\x1B[H\x1B[J");

  return fflush(stdout);
}
