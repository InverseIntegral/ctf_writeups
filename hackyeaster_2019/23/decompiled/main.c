void main() {
  nameAndWelcome();
  printf("\x1B[H\x1B[J");
  fflush(stdout);

  while (true) {
    printf("\x1B[0;0H");
    puts("Choose:");
    puts("[1] Change User");
    puts("[2] Help");
    puts("[3] Play");
    puts("[4] Exit");
    printf("> ");
    fflush(stdout);

    int selection = -1;
    scanf("%d", &selection);

    char currentChar;

    do {
      currentChar = fgetc(stdin);
    } while (currentChar != 10 && currentChar != -1);

    fflush(stdin);

    printf("\x1B[H\x1B[J", &selection);
    printf("\x1B[8;0H");
    fflush(stdout);

    if (selection <= 4) {
      void (*function)(FILE *) = &pointerTable[8 * selection];
      function(stdout);
    } else {
      error();
    }

    selection = 0;
  }
}
