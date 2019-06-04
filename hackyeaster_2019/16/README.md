# 16 - Every-Thing

## Description
Level: medium<br/>
Author: inik

After the brilliant idea from [here](http://geek-and-poke.com/geekandpoke/2013/7/22/future-proof-your-data-model).

The data model is stable and you can really store Every-Thing.

[EveryThing.zip](EveryThing.zip)

## Solution

From the zip archive we get a MySQL dump.
I quickly wrote [a script](run.sh) to start a docker container containing a MySQL instance.
After that I imported the MySQL dump with `source EveryThing.sql`.

The schema is quite simple:
```mysql
CREATE TABLE `Thing` (
  `id` binary(16) NOT NULL,
  `ord` int(11) NOT NULL,
  `type` varchar(255) NOT NULL,
  `value` varchar(1024) DEFAULT NULL,
  `pid` binary(16) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `FKfaem61vklu1cjw9ckunvpicgi` (`pid`),
  CONSTRAINT `FKfaem61vklu1cjw9ckunvpicgi` FOREIGN KEY (`pid`) REFERENCES `Thing` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
```

`ord` probably defines the order and `pid` is a parent id. To get an overview I used the following query:

```mysql
SELECT DISTINCT type FROM Thing
```

Some of the types included the word `png`. It looks like the database stores images (and other data).
To extract the images I wrote a Java program:

```java
public class EveryThing {

    static String url = "jdbc:mysql://localhost:3306/everything?useSSL=false&allowPublicKeyRetrieval=true";
    static String user = "root";
    static String password = "password";

    private static void getIdat(Connection con, String id, BufferedOutputStream os) throws SQLException, IOException {
        String query = "SELECT ord, type, FROM_BASE64(value) FROM Thing WHERE HEX(pid) = '" + id + "' ORDER BY ord";

        try (Statement st = con.createStatement();
             ResultSet rs = st.executeQuery(query)) {

            while (rs.next()) {
                os.write(rs.getBytes(3));
            }
        }
    }

    private static void extractImage(Connection con, String id) throws SQLException, IOException {
        String query = "SELECT ord, type, FROM_BASE64(value), HEX(id) FROM Thing WHERE HEX(pid) = '" + id + "' ORDER BY ord";

        FileOutputStream fos;
        BufferedOutputStream bos;
        fos = new FileOutputStream(new File(id + ".png"));
        bos = new BufferedOutputStream(fos);

        try (Statement st = con.createStatement(); ResultSet rs = st.executeQuery(query)) {

            while (rs.next()) {

                String type = rs.getString("type");
                if (!type.equals("png.idat")) {
                    bos.write(rs.getBytes(3));
                } else {
                    getIdat(con, rs.getString(4), bos);
                }
            }
        }

        bos.flush();
        bos.close();
        fos.close();
    }

    public static void main(String[] args) throws SQLException, IOException {
        String query = "SELECT HEX(id) FROM Thing WHERE type = 'png'";

        try (Connection con = DriverManager.getConnection(url, user, password);
             Statement st = con.createStatement();
             ResultSet rs = st.executeQuery(query)) {

            while (rs.next()) {
                extractImage(con, rs.getString(1));
            }
        }
    }

}
```

It uses the the foreign key relation to navigate through the tree.
The [IDAT chunks](http://www.libpng.org/pub/png/spec/1.2/PNG-Chunks.html#C.IDAT) had to be extracted in a slightly different way.

Running the program creates multiple images, one of which was the flag.

