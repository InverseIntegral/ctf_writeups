# Day 08: Advent Snail

In this challenge we get a scrambled picture and we have to reverse it to get a valid QR code. The name of the challenge
hints at a spiral pattern. After trying to read the pixels from the upper left corner I tried to read them from the
center. This worked quite well.

```java
private static boolean[][] loadPixelsFromImage(File file) throws IOException {
    BufferedImage image = ImageIO.read(file);
    boolean[][] colours = new boolean[25][25];

    for (int x = 0; x < 25; x++) {
        for (int y = 0; y < 25; y++) {
            int colour = image.getRGB(x, y);

            if (colour == -14803426) {
                colours[x][y] = true; // black pixel
            } else if (colour == -263173) {
                colours[x][y] = false; // white pixel
            } else {
                throw new IllegalArgumentException("Unknown colour at position (" + x + ", " + y + ")");
            }
        }
    }

    return colours;
}

private static List<Point> generateSnailPattern() {
    List<Point> points = new ArrayList<>();
    int x = 12, y = 12;
    int distance = 1;
    
    points.add(new Point(x, y));

    while (distance != 27) {
        for (int i = 0; i < distance; i++) {
            y--;
            points.add(new Point(x, y));
        }

        for (int i = 0; i < distance; i++) {
            x++;
            points.add(new Point(x, y));
        }

        distance++;

        for (int i = 0; i < distance; i++) {
            y++;
            points.add(new Point(x, y));
        }

        for (int i = 0; i < distance; i++) {
            x--;
            points.add(new Point(x, y));
        }

        distance++;
    }

    // Remove superfluous points
    points.removeIf(p -> p.x < 0 || p.y < 0 || p.x > 24 || p.y > 24);

    return points;
}

private static void reassemble(List<Point> order, boolean[][] colors) throws IOException {
    BufferedImage b = new BufferedImage(25, 25, BufferedImage.TYPE_INT_RGB);
    int x = 0, y = 0;

    while (!order.isEmpty()) {
        Point pos = order.remove(0);
        b.setRGB(x, y, colors[pos.x][pos.y] ? 0 : 16777215);

        if (++x == 25) {
            x = 0;
            y++;
        }
    }

    ImageIO.write(b, "png", new File("unsnailed.png"));
}

public static void main(String[] args) throws IOException {
    reassemble(generateSnailPattern(), loadPixelsFromImage(new File("snail.png")));
}
```
