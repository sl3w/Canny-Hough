using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Edge_detection
{
    static class ImageProcessing
    {
        //private static int[,] laplas = new int[,] {
          //                       { 0, 1, 0 },
          //                       { 1, -4, 1 },
           //                      { 0, 1, 0 } };

        public static Bitmap ImageToGrey(Bitmap bmp)
        {
            Bitmap resbmp = new Bitmap(bmp.Width, bmp.Height);
            byte brightness;
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    brightness = (byte)(0.299 * bmp.GetPixel(i, j).R + 0.587 * bmp.GetPixel(i, j).G + 0.114 * bmp.GetPixel(i, j).B);
                    resbmp.SetPixel(i, j, Color.FromArgb(brightness, brightness, brightness));
                }
            }
            return resbmp;
        }

        public static Bitmap ImageToColor(Bitmap bmp)
        {
            Bitmap resbmp = new Bitmap(bmp.Width, bmp.Height);
            byte brightness;
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    brightness = bmp.GetPixel(i, j).B;
                    var colorAr = BrootForceColor(brightness);
                    resbmp.SetPixel(i, j, Color.FromArgb(colorAr[0], colorAr[1], colorAr[2]));
                }
            }
            return resbmp;
        }

        public static byte[] BrootForceColor(byte brightness)
        {
            for (byte i = 0; i <= 255; i++)
            {
                for (byte j = 0; j <= 255; j++)
                {
                    for (byte k = 0; k <= 255; k++)
                    {
                        var brightness1 = (byte)(0.299 * i + 0.587 * j + 0.114 * k);
                        if (brightness == brightness1)
                            return new [] {i, j, k};
                    }
                }
            }

            return null;
        }

        
        public static Bitmap SingleThreshold(Bitmap bmp, int border)
        {
            Bitmap resbmp = new Bitmap(bmp);
            for (int i = 0; i < bmp.Width - 1; i++)
            {
                for (int j = 0; j < bmp.Height - 1; j++)
                {
                    var brightness = bmp.GetPixel(i, j).B;
                    if (brightness < border)
                        brightness = 0;
                    else
                        brightness = 255;
                    resbmp.SetPixel(i, j, Color.FromArgb(brightness, brightness, brightness));
                }
            }
            return resbmp;
        }
        
        public static Bitmap SingleThresholdColor(Bitmap bmp, int border)
        {
            Bitmap resbmp = new Bitmap(bmp);
            for (int i = 0; i < bmp.Width - 1; i++)
            {
                for (int j = 0; j < bmp.Height - 1; j++)
                {
                    byte colorR = bmp.GetPixel(i, j).R;
                    byte colorG = bmp.GetPixel(i, j).G;
                    byte colorB = bmp.GetPixel(i, j).B;
                    colorR = colorR < border ? (byte) 0 : (byte) 255;
                    colorG = colorG < border ? (byte) 0 : (byte) 255;
                    colorB = colorB < border ? (byte) 0 : (byte) 255;
                    resbmp.SetPixel(i, j, Color.FromArgb(colorR, colorG, colorB));
                }
            }
            return resbmp;
        }


        public static Bitmap ReverseColor(Bitmap bmp)
        {
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    byte B = bmp.GetPixel(i, j).B;
                    B = (byte)Math.Abs(255 - B);
                    bmp.SetPixel(i, j, Color.FromArgb(B, B, B));
                }
            }
            return bmp;
        }

        public static Bitmap ImposeContours(Bitmap src, Bitmap contours, int threshold)
        {
            Bitmap dst = new Bitmap(src);
            for (int i = 0; i < src.Width; i++)
            {
                for (int j = 0; j < src.Height; j++)
                {
                    var pix = contours.GetPixel(i, j);
                    if (pix.B > threshold)
                        dst.SetPixel(i,j, Color.FromArgb(pix.B,0,255,0));
                }
            }

            return dst;
        }
    }
}
