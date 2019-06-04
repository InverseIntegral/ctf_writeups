unsigned int compareXOR(char* a1, char* a2) {
  char v5;
  char v6;

  do {
      v6 = *(a1++);
      v5 = *(a2++) ^ 0x42;

      if (!v6 && !v5) {
         return 0;
      }
   } while (v6 == v5);

  return 1;
}
