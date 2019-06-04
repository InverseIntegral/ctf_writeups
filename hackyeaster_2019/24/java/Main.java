import java.io.FileOutputStream;
import java.io.OutputStream;
import java.net.CookieHandler;
import java.net.CookieManager;
import java.net.CookiePolicy;
import java.net.URI;
import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;
import java.time.Duration;
import java.time.LocalDateTime;
import java.util.Scanner;

public class Main {

    private static String BASE = "http://whale.hacking-lab.com:3555/";

    private static void getImage(HttpClient client) throws Exception {
        HttpRequest initialRequest = HttpRequest
                .newBuilder()
                .GET()
                .uri(URI.create(BASE)).build();

        client.send(initialRequest, HttpResponse.BodyHandlers.ofString());

        HttpRequest imageRequest = HttpRequest
                .newBuilder()
                .GET()
                .uri(URI.create(BASE + "picture")).build();

        HttpResponse<byte[]> imageResponse = client.send(imageRequest, HttpResponse.BodyHandlers.ofByteArray());
        new FileOutputStream("picture.jpg").write(imageResponse.body());
    }

    public static void main(String[] args) throws Exception {
        CookieManager cm = new CookieManager();
        cm.setCookiePolicy(CookiePolicy.ACCEPT_ALL);
        CookieHandler.setDefault(cm);

        HttpClient client = HttpClient
                .newBuilder()
                .cookieHandler(CookieHandler.getDefault())
                .followRedirects(HttpClient.Redirect.ALWAYS)
                .build();

        Scanner scanner = new Scanner(System.in);

        while (true) {
            LocalDateTime now = LocalDateTime.now();
            getImage(client);

            Process process = Runtime.getRuntime().exec("./capteg");
            Scanner processOutput = new Scanner(process.getInputStream());

            int foundNumber = processOutput.nextInt();

            System.out.println(">");
            String delta = scanner.nextLine();

            process.destroy();

            int deltaNr = Integer.valueOf(delta);

            int myAnswer = foundNumber + deltaNr;
            System.out.println("Sending: " + myAnswer);
            System.out.println("Took " + Duration.between(now, LocalDateTime.now()).getSeconds() + " seconds");

            HttpRequest verifyRequest = HttpRequest
                    .newBuilder()
                    .header("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8")
                    .POST(HttpRequest.BodyPublishers.ofString("s=" + myAnswer))
                    .uri(URI.create(BASE + "verify")).build();

            HttpResponse<String> verifyResponse = client.send(verifyRequest, HttpResponse.BodyHandlers.ofString());
            System.out.println(verifyResponse.body());
        }

    }

}
