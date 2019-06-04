import ar.com.hjg.pngj.*;

import java.io.File;
import java.util.Arrays;

public class Filter {

    public static void main(String[] args) throws Exception {
        PngReader pngReader = new PngReaderByte(new File("eggdesign.png"));
        StringBuilder binary = new StringBuilder();

        for (int row = 0; row < pngReader.imgInfo.rows; row++) {
            IImageLine line = pngReader.readRow();
            FilterType filterType = ((ImageLineByte) line).getFilterType();

            if (filterType == FilterType.FILTER_NONE) {
                binary.append("0");
            } else if (filterType == FilterType.FILTER_SUB) {
                binary.append("1");
            } else {
                throw new Exception("Unknown filter type");
            }

        }

        pngReader.end();

        StringBuilder flag = new StringBuilder();

        Arrays.stream(binary.toString().split("(?<=\\G.{8})"))
                .forEach(chunk -> flag.append((char) Integer.parseInt(chunk, 2)));

        System.out.println(flag.toString());
    }

}
