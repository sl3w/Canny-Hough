using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edge_detection
{
    class Filters
    {
        //стандартная маска для Гауссовского размытия
        private static double[,] StandartGaussMatrix = new double[,] {
                                    { 2, 4, 5, 4, 2 },
                                    { 4, 9, 12, 9, 4 },
                                    { 5, 12, 15, 12, 5 },
                                    { 4, 9, 12, 9, 4 },
                                    { 2, 4, 5, 4, 2 } };

        public static Bitmap GaussianFilter(Bitmap bmp, double sigma, bool isColor)
        {
            return isColor ? GaussianFilterColor(bmp, sigma) : GaussianFilterGrayscale(bmp, sigma);
        }
        
        public static Bitmap GaussianFilterGrayscale(Bitmap bmp, double sigma)
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

                    int brightness = (int)MultGauss(mas, sigma);
                    resbmp.SetPixel(i, j, Color.FromArgb(brightness, brightness, brightness));
                }
            }
            return resbmp;
        }
        
        public static Bitmap GaussianFilterColor(Bitmap bmp, double sigma)
        {
            Bitmap resbmp = new Bitmap(bmp);
            for (int i = 2; i < bmp.Width - 2; i++)
            {
                for (int j = 2; j < bmp.Height - 2; j++)
                {
                    int[,] masR = new int[5, 5];
                    int[,] masG = new int[5, 5];
                    int[,] masB = new int[5, 5];

                    for (int k = -2; k < 3; k++)
                    for (int l = -2; l < 3; l++)
                    {
                        masR[k + 2, l + 2] = bmp.GetPixel(i + k, j + l).R;
                        masG[k + 2, l + 2] = bmp.GetPixel(i + k, j + l).G;
                        masB[k + 2, l + 2] = bmp.GetPixel(i + k, j + l).B;
                    }

                    int r = (int)MultGauss(masR, sigma);
                    int g = (int)MultGauss(masG, sigma);
                    int b = (int)MultGauss(masB, sigma);
                    resbmp.SetPixel(i, j, Color.FromArgb(r, g, b));
                }
            }
            return resbmp;
        }

        private static double MultGauss(int[,] matr, double sigma)
        {
            double br = 0;
            double[,] gauss;
            if (sigma == 0)
                gauss = StandartGaussMatrix;
            else
                gauss = GetGaussMatrBySigma(sigma);
            for (int i = 0; i < matr.GetLength(0); i++)
            {
                for (int j = 0; j < matr.GetLength(0); j++)
                {
                    br += (matr[i, j] * (gauss[i, j]));
                }
            }
            if (sigma == 0)
                br = br / 159;
            if (br > 255) br = 255;
            return br;
        }

        private static double[,] GetGaussMatrBySigma(double sigma)
        {
            double[,] gauss = new double[5, 5];
            for (int i = 1; i <= 5; i++)
                for (int j = 1; j <= 5; j++)
                {
                    double e = Math.Exp(-(Math.Pow(i - 3, 2) + Math.Pow(j - 3, 2)) / (2 * Math.Pow(sigma, 2)));
                    gauss[i - 1, j - 1] = (e / (2 * Math.PI * Math.Pow(sigma, 2)));
                }
            return gauss;
        }

        public static Bitmap MedianFilter(Bitmap src, int sizeFilter)
        {
            int offset = (int) Math.Ceiling((double)sizeFilter / 2);

            Bitmap dst = new Bitmap(src);
            for (int i = offset; i < src.Width - offset; i++)
            {
                for (int j = offset; j < src.Height - offset; j++)
                {
                    int[] mas = new int[sizeFilter * sizeFilter];

                    for (int k = -offset; k <= offset; k++)
                        for (int l = -offset; l <= offset; l++)
                            mas[k + offset + l + offset] = src.GetPixel(i + k, j + l).B;

                    Array.Sort(mas);
                    int middle = (int)Math.Ceiling((double)sizeFilter * sizeFilter / 2) - 1;
                    dst.SetPixel(i, j, Color.FromArgb(mas[middle], mas[middle], mas[middle]));
                }
            }

            return dst;
        }
    }
}
