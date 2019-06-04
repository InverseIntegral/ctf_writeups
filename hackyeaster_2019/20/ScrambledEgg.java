import javax.imageio.ImageIO;
import java.awt.*;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Deque;
import java.util.LinkedList;
import java.util.List;

public class ScrambledEgg {

    private static BufferedImage getRow(BufferedImage whole, int row) {
        return whole.getSubimage(0, row, 259, 1);
    }

    private static int getScore(BufferedImage row) {
        for (var x = 0; x < 259; x++) {
            var colour = new Color(row.getRGB(x, 0), true);

            if (colour.getAlpha() == 0) {
                if (colour.getRed() != 0) {
                    return colour.getRed();
                }

                if (colour.getGreen() != 0) {
                    return colour.getGreen();
                }

                if (colour.getBlue() != 0) {
                    return colour.getBlue();
                }
            }
        }

        return 0;
    }

    private static int getRed(int rgb) {
        return new Color(rgb, true).getRed();
    }

    private static int getGreen(int rgb) {
        return new Color(rgb, true).getGreen();
    }

    private static int getBlue(int rgb) {
        return new Color(rgb, true).getBlue();
    }

    enum ColourMode {
        RED,
        GREEN,
        BLUE,
        OTHER
    }

    private static void mixColoursOfChunks(Deque<List<Integer>> allPixels, Color[] colours, ColourMode mode) {
        var i = 0;

        for (var chunk : allPixels) {
            for (var element : chunk) {
                var colour = colours[i];

                var cRed = colour.getRed();
                var cGreen = colour.getGreen();
                var cBlue = colour.getBlue();

                switch (mode) {
                    case RED:
                        colours[i++] = new Color(getRed(element) + cRed, cGreen, cBlue);
                        break;
                    case BLUE:
                        colours[i++] = new Color(cRed, cGreen, getBlue(element) + cBlue);
                        break;
                    case GREEN:
                        colours[i++] = new Color(cRed, getGreen(element) + cGreen, cBlue);
                        break;
                }
            }
        }
    }

    private static int[] transformPixels(BufferedImage img) {
        List<Integer> red = new LinkedList<>();
        List<Integer> green = new LinkedList<>();
        List<Integer> blue = new LinkedList<>();
        List<Integer> other = new LinkedList<>();

        List<ColourMode> previousModes = new ArrayList<>(3);
        var currentMode = ColourMode.OTHER;

        for (var columnIndex = 0; columnIndex < img.getWidth(); columnIndex++) {
            var rgb = img.getRGB(columnIndex, 0);
            var colour = new Color(rgb, true);

            if (colour.getAlpha() != 0) {
                switch (currentMode) {
                    case RED:
                        red.add(rgb);
                        break;
                    case BLUE:
                        blue.add(rgb);
                        break;
                    case GREEN:
                        green.add(rgb);
                        break;
                    case OTHER:
                        other.add(rgb);
                        break;
                }
            } else {
                // Change the current mode
                if (colour.getRed() != 0) {
                    currentMode = ColourMode.RED;
                }

                if (colour.getGreen() != 0) {
                    currentMode = ColourMode.GREEN;
                }

                if (colour.getBlue() != 0) {
                    currentMode = ColourMode.BLUE;
                }

                previousModes.add(currentMode);
            }
        }

        // All pixels that were added before a transparent one get appended to the last chunk
        switch (currentMode) {
            case RED:
                red.addAll(other);
                break;
            case BLUE:
                blue.addAll(other);
                break;
            case GREEN:
                green.addAll(other);
                break;
        }

        // Contains all pixels in order of appearance
        Deque<List<Integer>> allPixels = new LinkedList<>();

        for (ColourMode colourMode : previousModes) {
            switch (colourMode) {
                case RED:
                    allPixels.add(red);
                    break;
                case BLUE:
                    allPixels.add(blue);
                    break;
                case GREEN:
                    allPixels.add(green);
                    break;
            }
        }

        // width minus three because we remove the transparent pixels
        var reorderedPixels = new int[img.getWidth() - 3];

        if (allPixels.size() == 0) {
            return reorderedPixels;
        }

        // Helper array that is default initialized with black
        var colours = new Color[img.getWidth() - 3];

        for (var i = 0; i < colours.length; i++) {
            colours[i] = new Color(0, 0, 0);
        }

        // Go over the chunks in order of the transparent pixels.
        // Only take the component (R, G or B) of the current mode
        for (var i = 0; i < 3; i++) {
            mixColoursOfChunks(allPixels, colours, previousModes.get(i));

            // Move the first chunk to the end
            allPixels.add(allPixels.removeFirst());
        }

        for (var i = 0; i < colours.length; i++) {
            reorderedPixels[i] = colours[i].getRGB();
        }

        return reorderedPixels;
    }

    public static void main(String[] args) throws IOException {
        var eggImage = ImageIO.read(new File("egg.png"));
        var output = new BufferedImage(256, 256, eggImage.getType());

        for (var rowIndex = 0; rowIndex < 256; rowIndex++) {
            var row = getRow(eggImage, rowIndex);

            var pixels = transformPixels(row);
            var score = getScore(row);

            // Put the rows in their correct order
            for (var x = 0; x < 256; x++) {
                output.setRGB(x, score, pixels[x]);
            }
        }

        ImageIO.write(output, "png", new File("output.png"));
    }

}
