import java.io.File;
import java.io.FileReader;
import java.io.IOException;
import java.nio.file.*;
import java.util.Arrays;
import java.util.Collections;
import java.util.List;

public class Mirror {

    private static void reverse(byte[] data) {
        for (int i = 0; i < data.length / 2; i++) {
            byte temp = data[i];

            data[i] = data[data.length - i - 1];
            data[data.length - i - 1] = temp;
        }
    }

    private static void reverseFile(String name, String ending) throws IOException {
        Path fileLocation = Paths.get(name + "." + ending);
        byte[] data = Files.readAllBytes(fileLocation);

        reverse(data);

        Path outputPath = Paths.get(new StringBuilder(name).reverse().toString() + "." + (new StringBuilder(ending).reverse().toString()));
        Files.write(outputPath, (byte[]) data, StandardOpenOption.CREATE_NEW);
    }

    public static void main(String[] args) throws IOException {
        reverseFile("evihcra", "piz");
    }

}
