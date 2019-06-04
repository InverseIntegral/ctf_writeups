int nameAndWelcome() {
  printf("\x1B[H\x1B[J");
  printf("Please enter your name:\n> ");
  fflush(stdout);

  fgets(&name, 16, stdin);

  if (!strchr(&name, 10)) {
    char readChar;
    do {
      readChar = fgetc(stdin);
    } while (readChar != 10 && readChar != -1 );
  }

  fflush(stdin);
  size_t nameLength = strlen(&name);

  printf("Welcome %s.\n\n", &name, nameLength);
  fflush(stdout);
  printf("\x1B[H\x1B[J");

  return fflush(stdout);
}
