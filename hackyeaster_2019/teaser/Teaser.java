import javax.imageio.ImageIO;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;
import java.util.Arrays;
import java.util.Comparator;
import java.util.LinkedList;
import java.util.List;

public class Main {

    private static int getColour(File file) throws IOException {
        return ImageIO
                .read(file)
                .getRGB(0, 0);
    }

    private static List<Integer> getColours() {
        List<File> files = Arrays.asList(new File("./frames").listFiles());
        List<Integer> colours = new LinkedList<>();

        files.sort(Comparator.naturalOrder());
        files.forEach(f -> {
            try {
                colours.add(getColour(f));
            } catch (IOException e) {
                e.printStackTrace();
            }
        });

        return colours;
    }

    private static void reassemble(List<Integer> colours) throws IOException {
        BufferedImage image = new BufferedImage(480, 480, BufferedImage.TYPE_INT_RGB);
        int x = 0, y = 0;

        for (int colour : colours) {
            image.setRGB(x, y, colour);
            x++;

            if (x == 480) {
                x = 0;
                y++;
            }
        }

        ImageIO.write(image, "png", new File("solved.png"));
    }

    public static void main(String[] args) throws IOException {
        reassemble(getColours());
    }

}
