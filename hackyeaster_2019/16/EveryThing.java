import java.io.*;
import java.sql.*;

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
