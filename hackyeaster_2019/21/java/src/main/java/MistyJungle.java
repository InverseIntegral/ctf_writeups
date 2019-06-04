import org.apache.commons.text.WordUtils;
import org.json.JSONObject;

import javax.imageio.ImageIO;
import javax.script.ScriptEngine;
import javax.script.ScriptEngineManager;
import javax.script.ScriptException;
import java.awt.*;
import java.awt.image.BufferedImage;
import java.io.*;
import java.net.*;
import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;
import java.nio.charset.StandardCharsets;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.util.*;
import java.util.List;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class MistyJungle {

    private static final String BASE = "http://whale.hacking-lab.com:5337/";

    static class MyPoint extends Point {

        MyPoint(int x, int y) {
            super(x, y);
        }

        @Override
        public String toString() {
            return "[" + x + "," + y + "]";
        }

    }

    private static List<MyPoint> solveWarmup(String url) throws IOException {
        BufferedImage original = ImageIO.read(new File("original.png"));
        BufferedImage modified = ImageIO.read(new URL(BASE + "static/img/ch11/challenges/" + url));

        List<MyPoint> diffs = new ArrayList<>();

        for (int i = 0; i < original.getHeight(); i++) {
            for (int j = 0; j < original.getWidth(); j++) {

                int rgb = original.getRGB(j, i);
                int rgb2 = modified.getRGB(j, i);

                if (rgb != rgb2) {
                    diffs.add(new MyPoint(i, j));
                }
            }
        }

        return diffs;
    }

    private static String solveCottonTail(HttpClient client, String body) throws IOException, InterruptedException {
        HttpResponse<String> resultResponse = null;

        for (int i = 0; i < 10; i++) {
            String result = body.substring(body.indexOf("captcha") + 50, body.indexOf("captcha") + 75).split("-")[1];

            HttpRequest resultRequest = HttpRequest
                    .newBuilder()
                    .GET()
                    .uri(URI.create(BASE + "?result=" + result))
                    .build();

            resultResponse = client.send(resultRequest, HttpResponse.BodyHandlers.ofString());
            body = resultResponse.body();
        }

        return resultResponse.body();
    }

    private static String calculateMaths(List<Integer> operands, String current, double expected) throws ScriptException {
        // Base case
        if (operands.isEmpty()) {
            ScriptEngineManager manager = new ScriptEngineManager();
            ScriptEngine engine = manager.getEngineByName("JavaScript");

            Object result = engine.eval(current);
            Double calculatedResult;

            if (result instanceof Integer) {
                calculatedResult = ((Integer) result).doubleValue();
            } else {
                calculatedResult = (Double) result;
            }

            if (Math.abs(expected - calculatedResult) < 0.1) {
                return "";
            } else {
                return "NOK";
            }
        }

        Integer firstOperand = operands.remove(0);

        // Recursive cases
        String plusResult = calculateMaths(new ArrayList<>(operands), current + "+" + firstOperand, expected);
        String minusResult = calculateMaths(new ArrayList<>(operands), current + "-" + firstOperand, expected);
        String multiplyResult = calculateMaths(new ArrayList<>(operands), current + "*" + (double) firstOperand, expected);
        String divideResult = calculateMaths(new ArrayList<>(operands), current + "/" + (double) firstOperand, expected);

        if (!plusResult.contains("NOK")) {
            return "+" + plusResult;
        } else if (!minusResult.contains("NOK")) {
            return "-" + minusResult;
        } else if (!multiplyResult.contains("NOK")) {
            return "*" + multiplyResult;
        } else if (!divideResult.contains("NOK")) {
            return "/" + divideResult;
        } else {
            return "NOK";
        }
    }

    private static String splitFrames() throws IOException, InterruptedException {
        Runtime.getRuntime().exec("convert frames/initial.gif frames/frame.png").waitFor();

        File[] files = new File("frames/").listFiles((file, s) -> s.contains("frame"));

        Arrays.sort(files, (file, t1) -> {
            Pattern p = Pattern.compile("frame-(.+).png");
            String first = file.getName();
            String second = t1.getName();

            Matcher firstMatcher = p.matcher(first);
            firstMatcher.find();
            firstMatcher.group(1);
            Integer firstNr = Integer.valueOf(firstMatcher.group(1));

            Matcher secondMatcher = p.matcher(second);
            secondMatcher.find();
            Integer secondNr = Integer.valueOf(secondMatcher.group(1));

            return firstNr - secondNr;
        });

        StringBuilder binary = new StringBuilder();

        for (File file : files) {
            if (ImageIO.read(file).getRGB(0, 0) == -1) {
                binary.append("1");
            } else {
                binary.append("0");
            }
        }

        StringBuilder ascii = new StringBuilder();

        Arrays.stream(binary.toString().split("(?<=\\G.{8})"))
                .forEach(s -> ascii.append((char) Integer.parseInt(s, 2)));

        return ascii.toString();
    }

    private static void increaseMap(JSONObject jsonObject, Map<String, Integer> map) {
        String see = jsonObject.getString("see");
        String hear = jsonObject.getString("hear");
        String taste = jsonObject.getString("taste");
        String smell = jsonObject.getString("smell");
        String touch = jsonObject.getString("touch");

        map.put(see, map.getOrDefault(see, 0) + 1);
        map.put(hear, map.getOrDefault(hear, 0) + 1);
        map.put(taste, map.getOrDefault(taste, 0) + 1);
        map.put(smell, map.getOrDefault(smell, 0) + 1);
        map.put(touch, map.getOrDefault(touch, 0) + 1);
    }

    private static String solveCLC32(HttpClient client) throws IOException, InterruptedException {
        StringBuilder builder = new StringBuilder();

        outerLoop:
        while (true) {
            HttpRequest solutionRequest = HttpRequest
                    .newBuilder()
                    .header("Content-Type", "application/json")
                    .POST(HttpRequest.BodyPublishers.ofString("{\n" +
                            "  \"query\": \"{ In { Out { see, hear, taste, smell, touch }, see, hear, taste, smell, touch } }\"\n" +
                            "}"))
                    .uri(URI.create("http://whale.hacking-lab.com:5337/live/a/life"))
                    .build();

            HttpResponse<String> response = client.send(solutionRequest, HttpResponse.BodyHandlers.ofString());

            JSONObject jsonObject = new JSONObject(response.body());

            Map<String, Integer> outerMap = new HashMap<>();
            JSONObject in = jsonObject.getJSONObject("data").getJSONObject("In");

            increaseMap(in, outerMap);

            for (String key : outerMap.keySet()) {
                if (outerMap.get(key) >= 3) {
                    builder.append(key);

                    if (builder.length() >= 4) {
                        break outerLoop;
                    }
                }
            }

            Map<String, Integer> innerMap = new HashMap<>();
            JSONObject out = jsonObject.getJSONObject("data").getJSONObject("In").getJSONObject("Out");
            increaseMap(out, innerMap);

            for (String key : innerMap.keySet()) {
                if (innerMap.get(key) >= 3) {
                    builder.append(key);

                    if (builder.length() >= 4) {
                        break outerLoop;
                    }
                }
            }
        }

        return builder.toString();
    }

    private static String solvePumplesPuzzle(String[] hints) {
        String[] names = new String[5];
        String[] colours = new String[5];
        String[] characteristics = new String[5];
        String[] starSigns = new String[5];
        String[] patterns = new String[5];

        // The backpack of b3_name is b3_colour
        Pattern p1 = Pattern.compile("The backpack of (.+) is (.+)\\.");
        Matcher m1 = p1.matcher(hints[2]);
        m1.find();

        names[2] = m1.group(1);
        colours[2] = m1.group(2);

        // b5_name's star sign is b5_sign.
        Pattern p2 = Pattern.compile("(.+)&#39;s star sign is (.+)\\.");
        Matcher m2 = p2.matcher(hints[3]);
        m2.find();

        names[4] = m2.group(1);
        starSigns[4] = m2.group(2);

        // The b4_pattern backpack is also b4_colour.
        Pattern p3 = Pattern.compile("The (.+) backpack is also (.+)\\.");
        Matcher m3 = p3.matcher(hints[4]);
        m3.find();

        patterns[3] = m3.group(1);
        colours[3] = m3.group(2);

        // The b2_pattern backpack by b2_name was expensive.
        Pattern p4 = Pattern.compile("The (.+) backpack by (.+) was expensive\\.");
        Matcher m4 = p4.matcher(hints[5]);
        m4.find();

        patterns[1] = m4.group(1);
        names[1] = m4.group(2);

        // The bunny with the b4_colour backpack sits next to the bunny with the b5_colour backpack, on the left.
        Pattern p5 = Pattern.compile("The bunny with the (.+) backpack sits next to the bunny with the (.+) backpack, on the left\\.");
        Matcher m5 = p5.matcher(hints[6]);
        m5.find();

        colours[3] = m5.group(1);
        colours[4] = m5.group(2);

        // The b3_sign is also b3_char.
        Pattern p6 = Pattern.compile("The (.+) is also (.+)\\.");
        Matcher m6 = p6.matcher(hints[7]);
        m6.find();

        starSigns[2] = m6.group(1);
        characteristics[2] = m6.group(2);

        // The b1_char bunny has a b1_colour backpack.
        Pattern p7 = Pattern.compile("The (.+) bunny has a (.+) backpack\\.");
        Matcher m7 = p7.matcher(hints[8]);
        m7.find();

        characteristics[0] = m7.group(1);
        colours[0] = m7.group(2);

        // The bunny with the b3_pattern backpack sits in the middle.
        Pattern p8 = Pattern.compile("The bunny with the (.+) backpack sits in the middle\\.");
        Matcher m8 = p8.matcher(hints[9]);
        m8.find();

        patterns[2] = m8.group(1);

        // b1_name is the first bunny.
        Pattern p9 = Pattern.compile("(.+) is the first bunny\\.");
        Matcher m9 = p9.matcher(hints[10]);
        m9.find();

        names[0] = m9.group(1);

        // The bunny with a b1_pattern backpack sits next to the b2_char bunny.
        Pattern p10 = Pattern.compile("The bunny with a (.+) backpack sits next to the (.+) bunny\\.");
        Matcher m10 = p10.matcher(hints[11]);
        m10.find();

        patterns[0] = m10.group(1);
        characteristics[1] = m10.group(2);

        // The b2_char bunny sits also next to the b1_sign.
        Pattern p11 = Pattern.compile("The (.+) bunny sits also next to the (.+)\\.");
        Matcher m11 = p11.matcher(hints[12]);
        m11.find();

        characteristics[1] = m11.group(1);
        starSigns[0] = m11.group(2);

        // The b1_char bunny sits next to the b2_sign.
        Pattern p12 = Pattern.compile("The (.+) bunny sits next to the (.+)\\.");
        Matcher m12 = p12.matcher(hints[13]);
        m12.find();

        characteristics[0] = m12.group(1);
        starSigns[1] = m12.group(2);

        // The backpack of the b5_char bunny is p5_pattern.
        Pattern p13 = Pattern.compile("The backpack of the (.+) bunny is (.+)\\.");
        Matcher m13 = p13.matcher(hints[14]);
        m13.find();

        characteristics[4] = m13.group(1);
        patterns[4] = m13.group(2);

        // b4_name is a b4_char bunny.
        Pattern p14 = Pattern.compile("(.+) is a (.+) bunny\\.");
        Matcher m14 = p14.matcher(hints[15]);
        m14.find();

        names[3] = m14.group(1);
        characteristics[3] = m14.group(2);

        // b1_name sits next to the bunny with a b2_colour backpack.
        Pattern p15 = Pattern.compile("(.+) sits next to the bunny with a (.+) backpack\\.");
        Matcher m15 = p15.matcher(hints[16]);
        m15.find();

        names[0] = m15.group(1);
        colours[1] = m15.group(2);

        List<String> possibleSigns = new ArrayList<>(Arrays.asList("virgo", "capricorn", "aquarius", "pisces", "taurus"));

        possibleSigns.remove(starSigns[0]);
        possibleSigns.remove(starSigns[1]);
        possibleSigns.remove(starSigns[2]);
        possibleSigns.remove(starSigns[4]);

        starSigns[3] = possibleSigns.get(0);

        StringBuilder builder = new StringBuilder("Name");

        for (String name : names) {
            builder.append(",").append(name);
        }

        builder.append(",Color");

        for (String colour : colours) {
            builder.append(",").append(WordUtils.capitalize(colour));
        }

        builder.append(",Characteristic");

        for (String characteristic : characteristics) {
            builder.append(",").append(WordUtils.capitalize(characteristic));
        }

        builder.append(",Starsign");

        for (String starSign : starSigns) {
            builder.append(",").append(WordUtils.capitalize(starSign));
        }

        builder.append(",Mask");

        for (String patten : patterns) {
            builder.append(",").append(WordUtils.capitalize(patten));
        }

        return builder.toString();
    }

    private static boolean move(HttpClient client, int x, int y) throws IOException, InterruptedException, ScriptException {
        HttpRequest move = HttpRequest
                .newBuilder()
                .GET()
                .uri(URI.create(BASE + "move/" + x + "/" + y)).build();

        HttpResponse<String> response = client.send(move, HttpResponse.BodyHandlers.ofString());

        if (response.statusCode() != 200 && response.statusCode() != 302) {
            System.out.println("bad response code " + response.statusCode());
            return false;
        }

        String body = response.body();

        if (body.contains("alert")) {
            if (body.contains("alert-warning")) {
                return false;
            } else if (body.contains("alert-success")) {
                return true;
            } else if (body.contains("alert alert-danger")) {
                return false;
            } else {
                System.out.println(body);
                return false;
            }
        } else {
            if (body.contains("Weeeeelcoooome")) {
                System.out.println("Reached Warmup");

                List<MyPoint> diff = solveWarmup(body.substring(body.indexOf("ch11/challenges/") + 16).substring(0, 40));
                String diffString = diff.toString();

                HttpRequest pixelRequest = HttpRequest
                        .newBuilder()
                        .GET()
                        .uri(URI.create(BASE + "?pixels=" + URLEncoder.encode(diffString, StandardCharsets.UTF_8)))
                        .build();

                HttpResponse<String> pixelsResponse = client.send(pixelRequest, HttpResponse.BodyHandlers.ofString());

                if (pixelsResponse.body().contains("Bye")) {
                    System.out.println("Solved solveWarmup");
                    return true;
                } else {
                    System.out.println("Warmup failed");
                    System.exit(1);
                }
            } else if (body.contains("WARNING!")) {
                if (body.contains("Nothing happens")) {
                    return true; // Already solved
                }

                System.out.println("Reached C0tt0nt4il");

                if (solveCottonTail(client, body).contains("You solved it")) {
                    System.out.println("Solved C0tt0nt4il");
                    return true;
                } else {
                    System.out.println("C0tt0nt4il failed");
                    System.exit(1);
                }
            } else if (body.contains("Myterious Circle")) {
                System.out.println("Reached the mysterious circle");

                if (body.contains("you seem to be at a completely different place")) {
                    System.out.println("I have teleported");

                    points = new ArrayList<>();
                    currentPosition = new Point(0, 0);
                    moves = new Stack<>();
                    moved = false;

                    return true;
                }

                return true;
            } else if (body.contains("One in mind")) {
                System.out.println("Reached Mathonymous 2.0");

                String[] htmlOperands = body.split("<code style=\"font-size: 1em; margin: 10px\">");
                List<Integer> operands = new ArrayList<>();

                for (int i = 1; i < htmlOperands.length; i++) {
                    String stringOperand = htmlOperands[i].strip().split("</code>")[0].strip();
                    operands.add(Integer.valueOf(stringOperand));
                }

                String stringResult = body.split("<code style=\"font-size: 1em\">=")[1].split("</code>")[0];
                double numResult = Double.valueOf(stringResult);

                String result = calculateMaths(operands, String.valueOf(operands.remove(0)), numResult);

                HttpRequest resultRequest = HttpRequest
                        .newBuilder()
                        .GET()
                        .uri(URI.create(BASE + "?op=" + URLEncoder.encode(result, StandardCharsets.UTF_8)))
                        .build();

                HttpResponse<String> resultResponse = client.send(resultRequest, HttpResponse.BodyHandlers.ofString());

                if (resultResponse.body().contains("You solved it")) {
                    System.out.println("Solved Mathonymous 2.0");
                    return true;
                } else {
                    System.out.println("Failed Mathonymous 2.0");
                    System.exit(1);
                }
            } else if (body.contains("Hey I'm Pumple")) {
                if (body.contains("Pumple is still angry")) {
                    return true;
                }

                System.out.println("Solving Pumple's puzzle");

                String questionText = body.substring(body.indexOf("hr"));
                String[] hints = questionText.split("<pre class=\"mb-2\">");

                for (int i = 1; i < 17; i++) {
                    hints[i] = hints[i].substring(0, hints[i].indexOf("</pre>"));
                }

                String mySolution = solvePumplesPuzzle(hints);

                HttpRequest solutionRequest = HttpRequest
                        .newBuilder()
                        .GET()
                        .uri(URI.create(BASE + "?solution=" + mySolution))
                        .build();

                HttpResponse<String> solutionResponse = client.send(solutionRequest, HttpResponse.BodyHandlers.ofString());

                if (solutionResponse.body().contains("You solved it!")) {
                    System.out.println("Solved pumple's puzzle");
                    return true;
                } else {
                    System.out.println("Failed to solve pumple's puzzle");
                    System.exit(1);
                }
            } else if (body.contains("Nothing here")) {
                System.out.println("Nothing here");
                return true;
            } else if (body.contains("You didn't come here to make the choice")) {
                System.out.println("Solving oracle");

                String initialSeed = body.split("<code>")[2].split("</code>")[0];

                Process p = Runtime.getRuntime().exec("python oracle.py " + initialSeed);
                BufferedReader bfr = new BufferedReader(new InputStreamReader(p.getInputStream()));
                String theLastNumber = bfr.readLine();

                HttpRequest guessRequest = HttpRequest
                        .newBuilder()
                        .GET()
                        .uri(URI.create(BASE + "?guess=" + theLastNumber))
                        .build();

                HttpResponse<String> guessResponse = client.send(guessRequest, HttpResponse.BodyHandlers.ofString());

                if (guessResponse.body().contains("You solved it")) {
                    System.out.println("Solved oracle");
                    return true;
                } else {
                    System.out.println("Failed oracle");
                    System.exit(1);
                }
            } else if (body.contains("Do you know what to do with it")) {
                System.out.println("Started Punkt.Hase");

                String gifId = body.split("ch15/challenges/")[1].split(".gif")[0];

                try (InputStream in = new URL(BASE + "static/img/ch15/challenges/" + gifId + ".gif").openStream()) {
                    Files.copy(in, Paths.get("frames/initial.gif"));
                }

                String code = splitFrames();

                for (File file : new File("frames/").listFiles()) {
                    file.delete();
                }

                HttpRequest codeRequest = HttpRequest
                        .newBuilder()
                        .GET()
                        .uri(URI.create(BASE + "?code=" + code))
                        .build();

                HttpResponse<String> codeResponse = client.send(codeRequest, HttpResponse.BodyHandlers.ofString());

                if (codeResponse.body().contains("You solved it")) {
                    System.out.println("Solved Punkt.Hase");
                    return true;
                } else {
                    System.out.println("Failed Punkt.Hase");
                    System.exit(1);
                }
            } else if (body.contains("listen and answer me")) {
                System.out.println("Reached Pssst...");

                System.out.println(body.split("<pre>He: ")[1].split("<br>")[0]);
                Scanner scanner = new Scanner(System.in);
                String regexInput = scanner.next();

                HttpRequest answerRequest = HttpRequest
                        .newBuilder()
                        .GET()
                        .uri(URI.create(BASE + "?answer=" + URLEncoder.encode(regexInput, StandardCharsets.UTF_8)))
                        .build();

                HttpResponse<String> answerResponse = client.send(answerRequest, HttpResponse.BodyHandlers.ofString());

                if (answerResponse.body().contains("You solved it!")) {
                    System.out.println("Sovled pssst");
                    return true;
                } else {
                    System.out.println("Failed to solve pssst");
                    System.exit(1);
                }
            } else if (body.contains("Dreams ... just dreams")) {
                System.out.println("Reached CLC32");

                String checksum = solveCLC32(client);

                HttpRequest checksumRequest = HttpRequest
                        .newBuilder()
                        .GET()
                        .uri(URI.create(BASE + "?checksum=" + URLEncoder.encode(checksum, StandardCharsets.UTF_8)))
                        .build();

                HttpResponse<String> checksumResponse = client.send(checksumRequest, HttpResponse.BodyHandlers.ofString());

                if (checksumResponse.body().contains("You solved it!")) {
                    System.out.println("Sovled CLC32");
                    return true;
                } else {
                    System.out.println("Failed to solve CLC32");
                    System.exit(1);
                }
            } else if (body.contains("You are too late for their famous story telling")) {
                System.out.println(cm.getCookieStore().getCookies());
                System.out.println(body);
                System.exit(1);
            } else if (body.contains("He wants to give you another equation, but your navigator solved it already")) {
                System.out.println("Reached Mathonymous 2.0 again");
                return true;
            } else if (body.contains("The bunny is blinking repeatedly")) {
                System.out.println("Reached Punkt.Hase again");
                return true;
            } else if (body.contains("Nothing happens")) {
                System.out.println("Reached CLC32 again");
                return true;
            } else if (body.contains("not here anymore")) {
                System.out.println("He's gone");
                return true;
            } else if (body.contains("The Oracle is gone")) {
                System.out.println("Reached oracle again");
                return true;
            } else if (body.contains("ch16.jpg")) {
                System.out.println("Reached psst again");
                return true;
            }

            if (body.contains("<h3>")) {
                System.out.println("Possible unknown challenge");
                System.out.println(cm.getCookieStore().getCookies());
                System.exit(1);
            }

            return true;
        }
    }

    private static CookieManager cm;
    private static List<Point> points;
    private static Point currentPosition;
    private static Stack<Point> moves;
    private static boolean moved;

    private static void setStaticCookie(String cookieValue) {
        HttpCookie cookie = new HttpCookie("session", cookieValue);
        cookie.setPath("/");
        cookie.setPortlist("5337");
        cm.getCookieStore().add(URI.create("http://whale.hacking-lab.com"), cookie);
    }

    public static void main(String[] args) throws IOException, InterruptedException, ScriptException {
        cm = new CookieManager();
        cm.setCookiePolicy(CookiePolicy.ACCEPT_ALL);
        CookieHandler.setDefault(cm);

        HttpClient client = HttpClient
                .newBuilder()
                .cookieHandler(CookieHandler.getDefault())
                .followRedirects(HttpClient.Redirect.ALWAYS)
                .build();

        HttpRequest mapSelectionRequest = HttpRequest
                .newBuilder()
                .GET()
                .uri(URI.create(BASE + "1804161a0dabfdcd26f7370136e0f766")).build();

        client.send(mapSelectionRequest, HttpResponse.BodyHandlers.ofString());

        HttpRequest initialRequest = HttpRequest
                .newBuilder()
                .GET()
                .uri(URI.create("http://whale.hacking-lab.com:5337")).build();

        client.send(initialRequest, HttpResponse.BodyHandlers.ofString());

        move(client, 0, 0);

        points = new ArrayList<>();
        currentPosition = new Point(0, 0);
        points.add(currentPosition);
        moves = new Stack<>();
        moved = false;

        while (true) {
            moveLoop:
            for (int x = -1; x < 2; x++) {
                for (int y = -1; y < 2; y++) {
                    if (x == 0 && y == 0) {
                        continue;
                    }

                    Point cloned = new Point(currentPosition);
                    cloned.translate(x, y);

                    if (!points.contains(cloned)) {
                        points.add(cloned);

                        if (move(client, x, y)) {
                            currentPosition = cloned;

                            moved = true;

                            moves.add(new Point(x, y));
                            System.out.println(x + "\t" + y);

                            break moveLoop;
                        }
                    }
                }
            }

            if (moved) {
                moved = false;
            } else {
                if (moves.empty()) {
                    System.out.println("Unable to backtrack further at point " + currentPosition);
                    System.exit(1);
                }

                Point lastMove = moves.pop();
                int x = lastMove.x * -1;
                int y = lastMove.y * -1;

                Point cloned = new Point(currentPosition);
                cloned.translate(x, y);
                currentPosition = cloned;

                move(client, x, y);
            }
        }
    }

}
