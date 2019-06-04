import javax.script.ScriptEngine;
import javax.script.ScriptEngineManager;
import javax.script.ScriptException;
import java.awt.*;
import java.io.IOException;
import java.net.*;
import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;
import java.nio.charset.StandardCharsets;
import java.util.ArrayList;
import java.util.List;
import java.util.Scanner;
import java.util.Stack;

public class Main {

    private static final String BASE = "http://whale.hacking-lab.com:5337/";

    private static String solveMathoymous(HttpClient client, String body) throws ScriptException, IOException, InterruptedException {
        String question = body.split("<code style=\"font-size: 1em\">")[1].split("=")[0];

        ScriptEngineManager mgr = new ScriptEngineManager();
        ScriptEngine engine = mgr.getEngineByName("JavaScript");

        Object result = engine.eval(question);

        HttpRequest resultRequest = HttpRequest
                .newBuilder()
                .GET()
                .uri(URI.create(BASE + "?result=" + result))
                .build();

        return client.send(resultRequest, HttpResponse.BodyHandlers.ofString()).body();
    }

    private static String solveRandonacci(HttpClient client) throws IOException, InterruptedException {
        HttpRequest randRequest = HttpRequest
                .newBuilder()
                .GET()
                .uri(URI.create(BASE + "?next=117780214897213996119"))
                .build();

        return client.send(randRequest, HttpResponse.BodyHandlers.ofString()).body();
    }

    private static String solveSimonsEyes(HttpClient client) throws IOException, InterruptedException {
        List<String> answer = new ArrayList<>();

        for (Point previousMove : allMoves) {
            int previousX = previousMove.x;
            int previousY = previousMove.y;

            if (previousX == 1 && previousY == 1) {
                answer.add("\"7\"");
            } else if (previousX == 1 && previousY == 0) {
                answer.add("\"4\"");
            } else if (previousX == 1 && previousY == -1) {
                answer.add("\"2\"");
            } else if (previousX == 0 && previousY == -1) {
                answer.add("\"1\"");
            } else if (previousX == -1 && previousY == -1) {
                answer.add("\"8\"");
            } else if (previousX == -1 && previousY == 0) {
                answer.add("\"3\"");
            } else if (previousX == -1 && previousY == 1) {
                answer.add("\"5\"");
            } else if (previousX == 0 && previousY == 1) {
                answer.add("\"6\"");
            } else {
                System.out.println("Unknown move");
            }
        }

        String path = "[" + String.join(",", answer) + "]";

        HttpRequest pathRequest = HttpRequest
                .newBuilder()
                .GET()
                .uri(URI.create(BASE + "?path=" + URLEncoder.encode(path, StandardCharsets.UTF_8)))
                .build();

        return client.send(pathRequest, HttpResponse.BodyHandlers.ofString()).body();
    }

    private static String solveJohn(HttpClient client) throws IOException, InterruptedException {
        HttpRequest secretRequest = HttpRequest
                .newBuilder()
                .GET()
                .uri(URI.create(BASE + "?secret=b4ByG14N7"))
                .build();

        return client.send(secretRequest, HttpResponse.BodyHandlers.ofString()).body();
    }

    private static String solveRanDee(HttpClient client) throws IOException, InterruptedException {
        HttpRequest solutionRequest = HttpRequest
                .newBuilder()
                .GET()
                .uri(URI.create(BASE + "?solution=RSA3ncrypt!onw!llneverd!e"))
                .build();

        return client.send(solutionRequest, HttpResponse.BodyHandlers.ofString()).body();
    }

    private static String solveOldRumpy(HttpClient client, String body) throws IOException, InterruptedException {
        System.out.println(body.split("<hr>")[2]);

        String input = new Scanner(System.in).nextLine();

        HttpRequest timeRequest = HttpRequest
                .newBuilder()
                .GET()
                .uri(URI.create(BASE + "?time=" + URLEncoder.encode(input, StandardCharsets.UTF_8)))
                .build();

        return client.send(timeRequest, HttpResponse.BodyHandlers.ofString()).body();
    }

    private static String solveBunBun(HttpClient client) throws IOException, InterruptedException {
        HttpClient nonFollowingClient = HttpClient.newBuilder()
                .cookieHandler(CookieHandler.getDefault())
                .followRedirects(HttpClient.Redirect.NEVER)
                .build();

        HttpRequest watchRequest = HttpRequest
                .newBuilder()
                .GET()
                .uri(URI.create(BASE + "?action=watch"))
                .build();

        HttpResponse<String> watchResponse = nonFollowingClient.send(watchRequest, HttpResponse.BodyHandlers.ofString());

        while (watchResponse.statusCode() == 302) {
            final String item = watchResponse.headers().firstValue("Content-Type").orElse("Unknown");
            final String next = watchResponse.headers().firstValue("Location").orElse("Unknown");

            if (item.endsWith("teabag")) {
                break;
            }

            watchRequest = HttpRequest
                    .newBuilder()
                    .GET()
                    .uri(URI.create(next))
                    .build();

            watchResponse = nonFollowingClient.send(watchRequest, HttpResponse.BodyHandlers.ofString());
        }

        HttpRequest buyRequest = HttpRequest
                .newBuilder()
                .GET()
                .uri(URI.create(BASE + "?action=buy"))
                .build();

        return client.send(buyRequest, HttpResponse.BodyHandlers.ofString()).body();
    }

    private static String solveCottonTailCheck(HttpClient client, String body) throws IOException, InterruptedException {
        String img = body.split("img id=\"check\"")[1].split("src=\"")[1].split("\">")[0];
        System.out.println(img);

        final String input = new Scanner(System.in).nextLine();

        HttpRequest alphabetRequest = HttpRequest
                .newBuilder()
                .GET()
                .uri(URI.create(BASE + "?input=" + input))
                .build();

        return client.send(alphabetRequest, HttpResponse.BodyHandlers.ofString()).body();
    }

    private static boolean move(HttpClient client, int x, int y) throws IOException, InterruptedException, ScriptException {
        HttpRequest move = HttpRequest
                .newBuilder()
                .GET()
                .uri(URI.create(BASE + "move/" + x + "/" + y)).build();

        HttpResponse<String> response = client.send(move, HttpResponse.BodyHandlers.ofString());

        if (response.statusCode() != 200) {
            System.out.println("bad response code " + response.statusCode());
            return false;
        }

        String body = response.body();

        if (body.contains("alert")) {
            if (body.contains("Ouch! You would hit a wall")) {
                return false;
            }

            if (body.contains("We require you to prove your rabbitbility")) {
                System.out.println("Reached cottontail check");

                if (solveCottonTailCheck(client, body).contains("You solved it")) {
                    System.out.println("Solved cottontail check");
                    return true;
                } else {
                    System.out.println("Failed cottontail check");
                    System.exit(1);
                }
            }

            System.out.println(body);
            return false;
        }

        if (body.contains("Hey my friend, nice to meet you")) {
            System.out.println("Reached old rumpy");

            if (solveOldRumpy(client, body).contains("You solved it")) {
                System.out.println("Solved old rumpy");
                return true;
            } else {
                System.out.println("Failed old rumpy");
                System.exit(1);
            }
        }

        if (body.contains("one in mind")) {
            System.out.println("Reached Mathoymous");

            if (solveMathoymous(client, body).contains("You solved it")) {
                System.out.println("Solved Mathoymous");
                return true;
            } else {
                System.out.println("Failed Mathoymous");
                System.exit(1);
            }
        }

        if (body.contains("What a beautiful chain")) {
            System.out.println("Reached Randonacci");

            if (solveRandonacci(client).contains("You solved it")) {
                System.out.println("Solved randonacci");
                return true;
            } else {
                System.out.println("Failed randonacci");
                System.exit(1);
            }
        }

        if (body.contains("Hi, I'm Simon from the Security Team.")) {
            System.out.println("Reached simon's eyes");

            if (solveSimonsEyes(client).contains("You solved it")) {
                System.out.println("Solved simon's eyes");
                return true;
            } else {
                System.out.println("Failed simon's eyes");
                System.exit(1);

                return true;
            }
        }

        if (body.contains("Welcome Visitor. Feel free to take a look around my store.")) {
            System.out.println("Reached Bun Bun's Goods & Gadgets");

            if (solveBunBun(client).contains("You solved it")) {
                System.out.println("Solved Bun Bun's Goods & Gadgets");
                return true;
            } else {
                System.out.println("Failed Bun Bun's Goods & Gadgets");
                System.exit(1);
            }
        }

        if (body.contains("Ahoy sailor, my name is John")) {
            System.out.println("Reached sailor john");

            if (solveJohn(client).contains("You solved it")) {
                System.out.println("Solved sailor john");
                return true;
            } else {
                System.out.println("Failed sailor john");
                System.exit(1);
            }

            return true;
        }

        if (body.contains("Ran-Dee's Secret Algorithm")) {
            System.out.println("Reached Ran-Dee's Secret Algorithm");

            if (solveRanDee(client).contains("You solved it")) {
                System.out.println("Solved Ran-Dee's Secret Algorithm");
                return true;
            } else {
                System.out.println("Failed Ran-Dee's Secret Algorithm");
                System.exit(1);
            }
        }

        if (body.contains("A mysterious gate")) {
            System.out.println("Reached the gate");

            List<HttpCookie> cookies = cm.getCookieStore().getCookies();
            System.out.println(cookies.get(cookies.size() - 1));

            return true;
        }

        if (body.contains("Old Rumpy is gone")) {
            return true;
        }

        if (body.contains("He wants to give you another equation")) {
            return true;
        }

        if (body.contains("No more checks required!")) {
            return true;
        }

        if (body.contains("This looks wonderful, thank you!")) {
            return true;
        }

        if (body.contains("Simon let's you pass without any comment")) {
            return true;
        }

        if (body.contains("<h3>")) {
            System.out.println("Possible unknown challenge");
            System.out.println(cm.getCookieStore().getCookies().get(cm.getCookieStore().getCookies().size() - 1));
        }

        return true;
    }

    private static CookieManager cm;
    private static Stack<Point> allMoves = new Stack<>();

    private static void setStaticCookie(String cookieValue) {
        HttpCookie cookie = new HttpCookie("session", cookieValue);
        cookie.setPath("/");
        cookie.setPortlist("5337");
        cm.getCookieStore().add(URI.create("http://whale.hacking-lab.com"), cookie);
    }

    public static void main(String[] args) throws IOException, ScriptException, InterruptedException {
        cm = new CookieManager();
        cm.setCookiePolicy(CookiePolicy.ACCEPT_ALL);
        CookieHandler.setDefault(cm);

        HttpClient client = HttpClient.newBuilder()
                .cookieHandler(CookieHandler.getDefault())
                .followRedirects(HttpClient.Redirect.ALWAYS)
                .build();

        HttpRequest mapSelectionRequest = HttpRequest
                .newBuilder()
                .GET()
                .uri(URI.create(BASE + "7fde33818c41a1089088aa35b301afd9")).build();

        client.send(mapSelectionRequest, HttpResponse.BodyHandlers.ofString());

        HttpRequest initialRequest = HttpRequest
                .newBuilder()
                .GET()
                .uri(URI.create("http://whale.hacking-lab.com:5337")).build();

        client.send(initialRequest, HttpResponse.BodyHandlers.ofString());

        final List<Point> points = new ArrayList<>();
        final Stack<Point> moves = new Stack<>();

        Point currentPosition = new Point(0, 0);
        points.add(currentPosition);

        boolean moved = false;

        while (true) {
            outer:
            for (int x = -1; x < 2; x++) {
                for (int y = -1; y < 2; y++) {
                    if (x == 0 && y == 0) {
                        continue;
                    }

                    Point cloned = new Point(currentPosition);
                    cloned.translate(x, y);

                    if (!points.contains(cloned)) {
                        points.add(cloned);

                        allMoves.add(new Point(x, y));

                        if (move(client, x, y)) {
                            currentPosition = cloned;

                            moved = true;
                            moves.add(new Point(x, y));

                            break outer;
                        } else {
                            allMoves.pop();
                        }
                    }
                }
            }

            if (moved) {
                moved = false;
            } else {
                if (moves.empty()) {
                    System.out.println("Unable to backtrack further at point " + currentPosition);
                    System.out.println(cm.getCookieStore().getCookies().get(cm.getCookieStore().getCookies().size() - 1));
                    System.exit(1);
                }

                Point lastMove = moves.pop();
                int x = lastMove.x * -1;
                int y = lastMove.y * -1;

                Point cloned = new Point(currentPosition);
                cloned.translate(x, y);
                currentPosition = cloned;

                if (move(client, x, y)) {
                    allMoves.push(new Point(x, y));
                }

            }
        }

    }

}
