#include <stdio.h>
#include <string.h>
#include <stdint.h>
#include <pthread.h>

#define ECB 1
#define THREADS_NUM 26

#include "aes.h"

uint8_t cipherText[16] = { 0x71, 0x31, 0xAD, 0x54, 0xEF, 0x04, 0xDB, 0xA5, 0x03, 0x30, 0x0C, 0x0F, 0xF7, 0xBD, 0x83, 0x8E };
uint8_t kp[8] = { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };

void transform_key(char const * input, uint8_t *output) {
    int* ptr = (int *) output;

    for (size_t i = 0; i <= 3; i++) {
        ptr[i] = (input[4 * i + 1] << 8) | (input[4 * i + 2] << 16) | (input[4 * i + 3] << 24) | input[4 * i];
    }
}

void * decrypt(void *start) {
    char starting_letter = *(char *) start;
    char key[16] = {starting_letter, '\0', '\0', '\0', '\0', '\0', '\0', '\0', 'W', 'I', 'T', 'H', 'C', 'U', 'D', 'A'};
    uint8_t output_key[16];
    uint8_t tmp_ciphertext[16];

    printf("Thread %c\n", starting_letter);

    for (char a = 'A'; a <= 'Z'; a++) {
        key[1] = a;

        for (char b = 'A'; b <= 'Z'; b++) {
            key[2] = b;

            for (char c = 'A'; c <= 'Z'; c++) {
                key[3] = c;

                for (char d = 'A'; d <= 'Z'; d++) {
                    key[4] = d;

                    for (char e = 'A'; e <= 'Z'; e++) {
                        key[5] = e;

                        for (char f = 'A'; f <= 'Z'; f++) {
                            key[6] = f;

                            for (char g = 'A'; g <= 'Z'; g++) {
                                key[7] = g;

                                transform_key(key, output_key);
                                struct AES_ctx ctx;
                                AES_init_ctx(&ctx, output_key);

                                memcpy(tmp_ciphertext, cipherText, 16);
                                AES_ECB_decrypt(&ctx, tmp_ciphertext);

                                if (!memcmp(tmp_ciphertext, kp, 8)) {
                                    char printableKey[17];
                                    memcpy(printableKey, key, 16);
                                    printableKey[16] = '\0';

                                    printf("Found: %s\n", printableKey);
                                    return NULL;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

int main() {
    setbuf(stdout, NULL);
    pthread_t threads[THREADS_NUM];
    char starts[THREADS_NUM];

    size_t i;
    for (i = 0; i < THREADS_NUM; i++) {
        starts[i] = (char) (i + 65);
        pthread_create(&threads[i], NULL, decrypt, (void *) &starts[i]);
    }

    for (i = 0; i < THREADS_NUM; i++) {
        pthread_join(threads[i], NULL);
    }

    return 0;
}
