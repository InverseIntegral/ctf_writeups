import java.net.CookieManager;
import java.net.URI;
import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;
import java.security.MessageDigest;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class Memeory {

    private static HttpClient client;
    private static MessageDigest messageDigest;

    private final static String BASE = "http://whale.hacking-lab.com:1111/";
    private final static String PIC = "pic/";
    private final static String SOLVE = "solve";

    private static String convertByteToHex(byte[] byteData) {
        StringBuilder sb = new StringBuilder();

        for (byte byteDatum : byteData) {
            sb.append(Integer.toString((byteDatum & 0xff) + 0x100, 16).substring(1));
        }

        return sb.toString();
    }

    private static void playRound() throws Exception {
        Map<String, Integer> hashTable = new HashMap<>();
        List<Pair> pairs = new ArrayList<>();

        HttpRequest roundRequest = HttpRequest.newBuilder(URI.create(BASE)).GET().build();
        client.send(roundRequest, HttpResponse.BodyHandlers.ofString());

        for (int i = 1; i <= 98; i++) {
            HttpRequest imageRequest = HttpRequest.newBuilder(URI.create(BASE + PIC + i)).GET().build();

            byte[] body = client.send(imageRequest, HttpResponse.BodyHandlers.ofByteArray()).body();
            String imageHash = convertByteToHex(messageDigest.digest(body));

            if (hashTable.containsKey(imageHash)) {
                Integer previous = hashTable.get(imageHash);
                pairs.add(new Pair(previous, i));
            } else {
                hashTable.put(imageHash, i);
            }
        }

        for (Pair pair : pairs) {
            HttpRequest solveRequest = HttpRequest.newBuilder(URI.create(BASE + SOLVE))
                    .header("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8")
                    .POST(HttpRequest.BodyPublishers.ofString("first=" + pair.first + "&second=" + pair.second))
                    .build();

            HttpResponse<String> solveResponse = client.send(solveRequest, HttpResponse.BodyHandlers.ofString());
            System.out.println(solveResponse.body());
        }
    }

    public static void main(String[] args) throws Exception {
        CookieManager.setDefault(new CookieManager());
        client = HttpClient.newBuilder()
                .cookieHandler(CookieManager.getDefault())
                .followRedirects(HttpClient.Redirect.ALWAYS)
                .build();

        messageDigest = MessageDigest.getInstance("MD5");

        for (int i = 0; i < 10; i++) {
            System.out.println("Playing round " + i);
            playRound();
        }
    }

    private static final class Pair {

        private Integer first;
        private Integer second;

        private Pair(Integer first, Integer second) {
            this.first = first;
            this.second = second;
        }

    }

}
