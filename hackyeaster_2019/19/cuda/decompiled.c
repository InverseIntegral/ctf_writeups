int main() {
    unsigned __int8 *v4;
    unsigned __int8 *v5;
    unsigned int *v6;
    unsigned __int8 *v7;
    unsigned __int8 *v8;

    unsigned int *block_amount;
    unsigned int *plain_text;

    cudart::threadState **v14;
    cudart::threadState **v17;
    cudart::threadState **v20;

    __int64 v13;
    __int64 v16;
    __int64 v19;

    unsigned int v15;
    unsigned int v18;
    unsigned int v21;

    char input[16];
    int key[4];

    printf("Enter Password: ");
    fgets(input, 17, stdin);

    for (int i = 0; i <= 3; ++i) {
        key[i] = (input[4 * i + 1] << 8)
                 | (input[4 * i + 2] << 16)
                 | (input[4 * i + 3] << 24)
                 | input[4 * i];
    }

    cudaMalloc(&plain_text, 0x10);
    cudaMalloc(&block_amount, 0xB0);
    cudaMalloc(&v8, 0x100);
    cudaMalloc(&v7, 0x100);
    cudaMalloc(&v6, 0x28);
    cudaMalloc(&v5, 0x1000);
    cudaMalloc(&v4, 16 * block_amount);
    cudaMemcpy(plain_text, key, 16LL, 1LL);
    cudaMemcpy(v8, &v3, 256LL, 1LL);
    cudaMemcpy(v7, &v4, 256LL, 1LL);
    cudaMemcpy(v6, &v2, 40LL, 1LL);
    cudaMemcpy(v5, &v7, 4096LL, 1LL);
    cudaMemcpy(v4, &plain_text, 16 * block_amount, 1LL);

    dim3::dim3(&v13, 1u, 1u, 1u);
    dim3::dim3(&v14, 1u, 1u, 1u);

    if (!(unsigned int) _cudaPushCallConfiguration(v14, v15)) {
        f13(v8, v7, v6, v5, 1);
    }

    checkError();

    dim3::dim3(&v16, 1u, 1u, 1u);
    dim3::dim3(&v17, 1u, 1u, 1u);
    if (!(unsigned int) _cudaPushCallConfiguration(v17, v18)) {
        f3(plain_text, block_amount, v8, v6, 1);
    }

    dim3::dim3(&v19, 0x40u, 1u, 1u);
    dim3::dim3(&v20, 0x47u, 1u, 1u);

    if (!(unsigned int) _cudaPushCallConfiguration(v20, v21)) {
        f12(v4, block_amount, v7, v5, block_amount);
    }

    checkError();

    cudaMemcpy(&plain_text, v4, 16 * block_amount, 2LL);
    checkError();

    FILE *stream = fopen("egg", "wb");
    fwrite(&plain_text, 1, 16 * block_amount, stream);
    fclose(stream);

    return 0;
}
