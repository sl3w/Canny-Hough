using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edge_detection
{
    class LoG
    {
        private static double[,] MarrHildrethMatrix = new double[,] {
                                    { 0, 0, -1, 0, 0 },
                                    { 0, -1, -2, -1, 0 },
                                    { -1, -2, 16, -2, -1 },
                                    { 0, -1, -2, -1, 0 },
                                    { 0, 0, -1, 0, 0 } };

        public static Bitmap MarrHildrethEdge(Bitmap bmp)
        {
            Bitmap resbmp = new Bitmap(bmp);
            for (int i = 2; i < bmp.Width - 2; i++)
            {
                for (int j = 2; j < bmp.Height - 2; j++)
                {
                    int[,] mas = new int[5, 5];

                    for (int k = -2; k < 3; k++)
                        for (int l = -2; l < 3; l++)
                            mas[k + 2, l + 2] = bmp.GetPixel(i + k, j + l).B;

                    int brightness = (int)MultMatrix(mas);
                    if (brightness > 255) brightness = 255;
                    else if (brightness < 0) brightness = 0;
                    resbmp.SetPixel(i, j, Color.FromArgb(brightness, brightness, brightness));
                }
            }
            return resbmp;
        }

        private static double MultMatrix(int[,] matr)
        {
            double br = 0;
            double[,] gauss;
                gauss = MarrHildrethMatrix;
 
            for (int i = 0; i < matr.GetLength(0); i++)
            {
                for (int j = 0; j < matr.GetLength(0); j++)
                {
                    br += (matr[i, j] * (gauss[i, j]));
                }
            }
            //br = br / 159;
            return br;
        }
    }
}
