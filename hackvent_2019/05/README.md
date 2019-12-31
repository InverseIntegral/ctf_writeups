# 05 - Santa Parcel Tracking

## Description

Level: Easy<br/>
Author: inik

To handle the huge load of parcels Santa introduced this year a parcel tracking system. He didn't like the black and
white barcode, so he invented a more solemn barcode. Unfortunately the common barcode readers can't read it anymore, it
only works with the pimped models santa owns. Can you read the barcode

![Barcode](157de28f-2190-4c6d-a1dc-02ce9e385b5c.png)

## Solution

By checking the RGB values of the bars I realized that the blue component of each bar contained a character of the flag.
The following haskell program extracts those values:

```haskell
import Codec.Picture
import Data.Maybe
import Data.List
import Data.Char

main = do
   img <- readImage "157de28f-2190-4c6d-a1dc-02ce9e385b5c.png"
   case img of
      Left _ -> putStrLn "Could not read image" 
      Right image' -> putStrLn $ readFlag image'

readFlag :: DynamicImage -> String
readFlag image = decimalToString flagPixels
                 where pixels = fromMaybe [] $ firstRowPixels image
                       bluePixels = map getBlue pixels
                       flagPixels = map fromIntegral $ filter (/= 255) $ removeDuplicates bluePixels

firstRowPixels :: DynamicImage -> Maybe [PixelRGB8]
firstRowPixels (ImageRGB8 image@(Image w _ _)) =
   Just $ map (\x -> pixelAt image x 0) [0..w]
firstRowPixels _ = Nothing

getBlue :: PixelRGB8 -> Pixel8
getBlue (PixelRGB8 r g b) = b

removeDuplicates :: Eq t => [t] -> [t]
removeDuplicates values = map head $ group values

decimalToString :: [Int] -> String
decimalToString values = show $ map chr values
```

and it prints the flag `"X8YIOF0ZP4S8HV19{D1fficult_to_g3t_a_SPT_R3ader}S1090OMZE0E3NFP6E"`.
