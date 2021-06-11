using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edge_detection
{
    class Edges
    {
        private static double[,] angles;

        private static Color WhiteBorder = Color.FromArgb(255, 255, 255);
        private static Color NotBorder = Color.FromArgb(0, 0, 0);

        private static int[,] SobelX = {
                                 { 1, 2, 1 },
                                 { 0, 0, 0 },
                                 { -1, -2, -1 } };

        private static int[,] SobelY = {
                                 { -1, 0, 1 },
                                 { -2, 0, 2 },
                                 { -1, 0, 1 } };

        private static int[,] newSobelX = { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
        private static int[,] newSobelY = { { 1, 2, 1 }, { 0, 0, 0 }, { -1, -2, -1 } };

        private static int[,] PrewittX = {
            { 1, 1, 1 },
            { 0, 0, 0 },
            { -1, -1, -1 } };

        private static int[,] PrewittY = {
            { -1, 0, 1 },
            { -1, 0, 1 },
            { -1, 0, 1 } };
        
        //фильтр Собеля
        public static Bitmap SobelConvolve(Bitmap bmp)
        {
            Bitmap resbmp = new Bitmap(bmp);
            angles = new double[bmp.Width, bmp.Height];
            for (int i = 1; i < bmp.Width - 1; i++)
            {
                for (int j = 1; j < bmp.Height - 1; j++)
                {
                    int[,] mas = new int[3, 3];

                    for (int k = -1; k < 2; k++)
                        for (int l = -1; l < 2; l++)
                            mas[k + 1, l + 1] = bmp.GetPixel(i + k, j + l).B;

                    int ggx = SobelPrewittMult(mas, SobelX);
                    int ggy = SobelPrewittMult(mas, SobelY);

                    int brightness = (int)Math.Sqrt(Math.Pow(ggx, 2) + Math.Pow(ggy, 2));
                    if (brightness > 255) brightness = 255;
                    else if (brightness < 0) brightness = 0;
                    resbmp.SetPixel(i, j, Color.FromArgb(brightness, brightness, brightness));

                    double a = Math.Atan2(ggy, ggx) * 180 / Math.PI;
                    angles[i, j] = GetAngleMult45(a);
                }
            }
            return resbmp;
        }
        
        public static Bitmap SobelConvolveColor(Bitmap bmp)
        {
            Bitmap resbmp = new Bitmap(bmp);
            angles = new double[bmp.Width, bmp.Height];
            for (int i = 1; i < bmp.Width - 1; i++)
            {
                for (int j = 1; j < bmp.Height - 1; j++)
                {
                    int[,] masR = new int[3, 3];
                    int[,] masG = new int[3, 3];
                    int[,] masB = new int[3, 3];

                    for (int k = -1; k < 2; k++)
                    for (int l = -1; l < 2; l++)
                    {
                        masR[k + 1, l + 1] = bmp.GetPixel(i + k, j + l).R;
                        masG[k + 1, l + 1] = bmp.GetPixel(i + k, j + l).G;
                        masB[k + 1, l + 1] = bmp.GetPixel(i + k, j + l).B;
                    }

                    int ggxR = SobelPrewittMult(masR, SobelX);
                    int ggyR = SobelPrewittMult(masR, SobelY);
                    
                    int red = (int)Math.Sqrt(Math.Pow(ggxR, 2) + Math.Pow(ggyR, 2));
                    if (red > 255) red = 255;
                    else if (red < 0) red = 0;
                    
                    int ggxG = SobelPrewittMult(masG, SobelX);
                    int ggyG = SobelPrewittMult(masG, SobelY);
                    
                    int green = (int)Math.Sqrt(Math.Pow(ggxG, 2) + Math.Pow(ggyG, 2));
                    if (green > 255) green = 255;
                    else if (green < 0) green = 0;
                    
                    int ggxB = SobelPrewittMult(masB, SobelX);
                    int ggyB = SobelPrewittMult(masB, SobelY);

                    int blue = (int)Math.Sqrt(Math.Pow(ggxB, 2) + Math.Pow(ggyB, 2));
                    if (blue > 255) blue = 255;
                    else if (blue < 0) blue = 0;
                    
                    resbmp.SetPixel(i, j, Color.FromArgb(red, green, blue));

                    //double a = Math.Atan2(ggy, ggx) * 180 / Math.PI;
                    //angles[i, j] = GetAngleMult45(a);
                }
            }
            return resbmp;
        }
        
        public static Bitmap PrewittConvolve(Bitmap bmp)
        {
            Bitmap resbmp = new Bitmap(bmp);
            angles = new double[bmp.Width, bmp.Height];
            for (int i = 1; i < bmp.Width - 1; i++)
            {
                for (int j = 1; j < bmp.Height - 1; j++)
                {
                    int[,] mas = new int[3, 3];

                    for (int k = -1; k < 2; k++)
                        for (int l = -1; l < 2; l++)
                            mas[k + 1, l + 1] = bmp.GetPixel(i + k, j + l).B;

                    int ggx = SobelPrewittMult(mas, PrewittX);
                    int ggy = SobelPrewittMult(mas, PrewittY);

                    int brightness = (int)Math.Sqrt(Math.Pow(ggx, 2) + Math.Pow(ggy, 2));
                    if (brightness > 255) brightness = 255;
                    else if (brightness < 0) brightness = 0;
                    resbmp.SetPixel(i, j, Color.FromArgb(brightness, brightness, brightness));

                    //double a = Math.Atan2(ggy, ggx) * 180 / Math.PI;
                    //angles[i, j] = GetAngleMult45(a);
                }
            }
            return resbmp;
        }
        
        public static Bitmap PrewittConvolveColor(Bitmap bmp)
        {
            Bitmap resbmp = new Bitmap(bmp);
            angles = new double[bmp.Width, bmp.Height];
            for (int i = 1; i < bmp.Width - 1; i++)
            {
                for (int j = 1; j < bmp.Height - 1; j++)
                {
                    int[,] masR = new int[3, 3];
                    int[,] masG = new int[3, 3];
                    int[,] masB = new int[3, 3];

                    for (int k = -1; k < 2; k++)
                    for (int l = -1; l < 2; l++)
                    {
                        masR[k + 1, l + 1] = bmp.GetPixel(i + k, j + l).R;
                        masG[k + 1, l + 1] = bmp.GetPixel(i + k, j + l).G;
                        masB[k + 1, l + 1] = bmp.GetPixel(i + k, j + l).B;
                    }

                    int ggxR = SobelPrewittMult(masR, PrewittX);
                    int ggyR = SobelPrewittMult(masR, PrewittY);
                    
                    int red = (int)Math.Sqrt(Math.Pow(ggxR, 2) + Math.Pow(ggyR, 2));
                    if (red > 255) red = 255;
                    else if (red < 0) red = 0;
                    
                    int ggxG = SobelPrewittMult(masG, PrewittX);
                    int ggyG = SobelPrewittMult(masG, PrewittY);
                    
                    int green = (int)Math.Sqrt(Math.Pow(ggxG, 2) + Math.Pow(ggyG, 2));
                    if (green > 255) green = 255;
                    else if (green < 0) green = 0;
                    
                    int ggxB = SobelPrewittMult(masB, PrewittX);
                    int ggyB = SobelPrewittMult(masB, PrewittY);

                    int blue = (int)Math.Sqrt(Math.Pow(ggxB, 2) + Math.Pow(ggyB, 2));
                    if (blue > 255) blue = 255;
                    else if (blue < 0) blue = 0;
                    
                    resbmp.SetPixel(i, j, Color.FromArgb(red, green, blue));

                    //double a = Math.Atan2(ggy, ggx) * 180 / Math.PI;
                    //angles[i, j] = GetAngleMult45(a);
                }
            }
            return resbmp;
        }

        private static int SobelPrewittMult(int[,] matr, int[,] g)
        {
            int br = 0;
            for (int i = 0; i < g.GetLength(0); i++)
                for (int j = 0; j < g.GetLength(0); j++)
                    br += (matr[i, j] * g[i, j]);
            return br;
        }

        public static Bitmap Sobel(Bitmap src)
        {
            Bitmap dst = new Bitmap(src.Width, src.Height);
            angles = new double[src.Width, src.Height];

            //оператор Собеля

            int sumX, sumY, sum;
            //'цикл прохода по всему изображению
            for (int y = 0; y < src.Height - 1; y++)
                for (int x = 0; x < src.Width - 1; x++)
                {
                    sumX = sumY = 0;
                    if (y == 0 || y == src.Height - 1) sum = 0;
                    else if (x == 0 || x == src.Width - 1) sum = 0;
                    else
                    {
                        //цикл свертки оператором Собеля
                        for (int i = -1; i < 2; i++)
                            for (int j = -1; j < 2; j++)
                            {
                                //взять значение пикселя
                                int c = src.GetPixel(x + i, y + j).R;
                                //найти сумму произведений пикселя на значение из матрицы по X
                                sumX += c * newSobelX[i + 1, j + 1];
                                //и сумму произведений пикселя на значение из матрицы по Y
                                sumY += c * newSobelY[i + 1, j + 1];
                            }
                        //найти приближенное значение величины градиента
                        //sum = Math.Abs(sumX) + Math.Abs(sumY);
                        sum = (int)Math.Sqrt(Math.Pow(sumX, 2) + Math.Pow(sumY, 2));
                        double a = Math.Atan2(sumY, sumX) * 180 / Math.PI;
                        angles[x, y] = GetAngleMult45(a);
                    }
                    //провести нормализацию
                    if (sum > 255) sum = 255;
                    else if (sum < 0) sum = 0;
                    //записать результат в выходное изображение
                    dst.SetPixel(x, y, Color.FromArgb(255, sum, sum, sum));
                }
            //Binarization(dst);
            return dst;
        }

        private static double GetAngleMult45(double angle)
        {
            double a = Math.Round(angle / 45) * 45;
            if (a < 0)
                a += 360;
            return a;
        }

        //подавление не-максимумов
        public static Bitmap NonMaximumSuppression(Bitmap bmp)
        {
            Bitmap resbmp = new Bitmap(bmp);
            for (int i = 1; i < bmp.Width - 1; i++)
            {
                for (int j = 1; j < bmp.Height - 1; j++)
                {
                    double angle = angles[i, j];
                    if (angle == 0 || angle == 180)
                    {
                        if ((bmp.GetPixel(i - 1, j).B > bmp.GetPixel(i, j).B || bmp.GetPixel(i, j).B < bmp.GetPixel(i + 1, j).B))
                        {
                            resbmp.SetPixel(i, j, NotBorder);
                        }
                    }
                    if (angle == 90 || angle == 270)
                    {
                        if ((bmp.GetPixel(i, j - 1).B > bmp.GetPixel(i, j).B || bmp.GetPixel(i, j).B < bmp.GetPixel(i, j + 1).B))
                        {
                            resbmp.SetPixel(i, j, NotBorder);
                        }
                    }
                    if (angle == 45 || angle == 225)
                    {
                        if ((bmp.GetPixel(i - 1, j + 1).B > bmp.GetPixel(i, j).B || bmp.GetPixel(i, j).B < bmp.GetPixel(i + 1, j - 1).B))
                        {
                            resbmp.SetPixel(i, j, NotBorder);
                        }
                    }
                    if (angle == 135 || angle == 315)
                    {
                        if ((bmp.GetPixel(i + 1, j + 1).B > bmp.GetPixel(i, j).B || bmp.GetPixel(i, j).B < bmp.GetPixel(i - 1, j - 1).B))
                        {
                            resbmp.SetPixel(i, j, NotBorder);
                        }
                    }
                }
            }
            return resbmp;
        }


        //двойная пороговая фильтрация
        public static Bitmap DoubleThreshold(Bitmap bmp, int top, int low)
        {
            Bitmap resbmp = new Bitmap(bmp);
            byte brightness;
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    brightness = bmp.GetPixel(i, j).G;
                    Color color;
                    if (brightness < low)
                    {
                        color = NotBorder;
                    }
                    else
                    {
                        if (brightness > top)
                        {
                            color = WhiteBorder;
                        }
                        else
                            color = Color.FromArgb(100, 100, 100);
                    }
                    resbmp.SetPixel(i, j, color);
                }
            }
            return resbmp;
        }


        //трассировка области неоднозначности
        public static Bitmap EdgeTracking(Bitmap bmp)
        {
            Bitmap resbmp = new Bitmap(bmp);

            for (int i = 1; i < bmp.Width - 1; i++)
            {
                for (int j = 1; j < bmp.Height - 1; j++)
                {
                    byte br = bmp.GetPixel(i, j).B;
                    if (br == 100)
                    {
                        bool check = true;
                        for (int x = -1; x < 2; x++)
                            if (check)
                            {
                                for (int y = -1; y < 2; y++)
                                    if (angles[i, j] == angles[i + x, j + y] && bmp.GetPixel(i + x, j + y).B == 255)
                                    {
                                        check = true;
                                    }
                                    else
                                    {
                                        check = false;
                                        break;
                                    }
                            }
                            else
                                break;
                        if (check)
                            resbmp.SetPixel(i, j, WhiteBorder);
                        else
                            resbmp.SetPixel(i, j, NotBorder);
                    }
                }
            }
            return resbmp;
        }

        //восстановление границ
        public static Bitmap BorderRestoration(Bitmap bmp, int rad)
        {
            Bitmap resbmp = new Bitmap(bmp);

            for (int i = rad; i < bmp.Width - rad; i++)
            {
                for (int j = rad; j < bmp.Height - rad; j++)
                {
                    if (bmp.GetPixel(i, j).B == 255)
                    {
                        for (int n = 1; n < rad; n++)
                        {
                            if (bmp.GetPixel(i - n, j - n).B == 255)
                                for (int k = 1; k < n; k++)
                                    if (bmp.GetPixel(i - n + k, j - n + k).B == 0)
                                        resbmp.SetPixel(i - n + k, j - n + k, WhiteBorder);
                            if (bmp.GetPixel(i - n, j).B == 255)
                                for (int k = 1; k < n; k++)
                                    if (bmp.GetPixel(i - n + k, j).B == 0)
                                        resbmp.SetPixel(i - n + k, j, WhiteBorder);
                            if (bmp.GetPixel(i - n, j + n).B == 255)
                                for (int k = 1; k < n; k++)
                                    if (bmp.GetPixel(i - n + k, j + n - k).B == 0)
                                        resbmp.SetPixel(i - n + k, j + n - k, WhiteBorder);

                            if (bmp.GetPixel(i, j - n).B == 255)
                                for (int k = 1; k < n; k++)
                                    if (bmp.GetPixel(i + k, j - n + k).B == 0)
                                        resbmp.SetPixel(i + k, j - n + k, WhiteBorder);
                            if (bmp.GetPixel(i, j + n).B == 255)
                                for (int k = 1; k < n; k++)
                                    if (bmp.GetPixel(i + k, j + n - k).B == 0)
                                        resbmp.SetPixel(i + k, j + n - k, WhiteBorder);

                            if (bmp.GetPixel(i + n, j - n).B == 255)
                                for (int k = 1; k < n; k++)
                                    if (bmp.GetPixel(i + n - k, j - n + k).B == 0)
                                        resbmp.SetPixel(i - n + k, j - n + k, WhiteBorder);
                            if (bmp.GetPixel(i + n, j).B == 255)
                                for (int k = 1; k < n; k++)
                                    if (bmp.GetPixel(i + n - k, j).B == 0)
                                        resbmp.SetPixel(i + n - k, j, WhiteBorder);
                            if (bmp.GetPixel(i - n, j + n).B == 255)
                                for (int k = 1; k < n; k++)
                                    if (bmp.GetPixel(i + n - k, j + n - k).B == 0)
                                        resbmp.SetPixel(i + n - k, j + n - k, WhiteBorder);
                        }
                    }
                }
            }
            return resbmp;
        }

        //градиентный метод
        public static Bitmap FadeDetection(Bitmap bmp)
        {
            Bitmap resbmp = new Bitmap(bmp);

            angles = new double[bmp.Width, bmp.Height];

            //byte brightness = (byte)Math.Sqrt(Math.Pow((bmp.GetPixel(0, 0).G - bmp.GetPixel(1, 0).G), 2) + Math.Pow((bmp.GetPixel(0, 0).G - bmp.GetPixel(0, 1).G), 2));
            //resbmp.SetPixel(0, 0, Color.FromArgb(brightness, brightness, brightness));

            byte brightness;
            for (int i = 1; i < bmp.Width; i++)
            {
                for (int j = 1; j < bmp.Height; j++)
                {

                    int gx = (bmp.GetPixel(i, j).G - bmp.GetPixel(i - 1, j).G);
                    int gy = (bmp.GetPixel(i, j).G - bmp.GetPixel(i, j - 1).G);

                    brightness = (byte)Math.Sqrt(Math.Pow(gx, 2) + Math.Pow(gy, 2));
                    resbmp.SetPixel(i, j, Color.FromArgb(brightness, brightness, brightness));

                    double a = Math.Atan((double)gy / gx) * 180 / Math.PI;
                    angles[i, j] = GetAngleMult45(a);
                }
            }

            //Крайние точки
            //нижняя и верхняя часть
            for (int i = 1; i < bmp.Width - 1; i++)
            {
                brightness = (byte)Math.Sqrt(Math.Pow((bmp.GetPixel(i, bmp.Height - 1).G - bmp.GetPixel(i - 1, bmp.Height - 1).G), 2));
                resbmp.SetPixel(i, bmp.Height - 1, Color.FromArgb(brightness, brightness, brightness));

                brightness = (byte)Math.Sqrt(Math.Pow((bmp.GetPixel(i, 0).G - bmp.GetPixel(i - 1, 0).G), 2) + Math.Pow((bmp.GetPixel(i, 0).G - bmp.GetPixel(i, 1).G), 2));
                resbmp.SetPixel(i, 0, Color.FromArgb(brightness, brightness, brightness));
            }

            //правая и левая часть
            for (int i = 1; i < bmp.Height - 1; i++)
            {
                brightness = (byte)Math.Sqrt(Math.Pow((bmp.GetPixel(bmp.Width - 1, i).G - bmp.GetPixel(bmp.Width - 1, i - 1).G), 2) + Math.Pow((bmp.GetPixel(bmp.Width - 2, i).G - bmp.GetPixel(bmp.Width - 2, i - 1).G), 2));
                resbmp.SetPixel(bmp.Width - 1, i, Color.FromArgb(brightness, brightness, brightness));

                brightness = (byte)Math.Sqrt(Math.Pow((bmp.GetPixel(0, i).G - bmp.GetPixel(0, i - 1).G), 2) + Math.Pow((bmp.GetPixel(0, i).G - bmp.GetPixel(1, i).G), 2));
                resbmp.SetPixel(0, i, Color.FromArgb(brightness, brightness, brightness));
            }
            return resbmp;
        }
        
        public static Bitmap FadeDetectionColor(Bitmap bmp)
        {
            Bitmap resbmp = new Bitmap(bmp);

            //angles = new double[bmp.Width, bmp.Height];

            //byte brightness = (byte)Math.Sqrt(Math.Pow((bmp.GetPixel(0, 0).G - bmp.GetPixel(1, 0).G), 2) + Math.Pow((bmp.GetPixel(0, 0).G - bmp.GetPixel(0, 1).G), 2));
            //resbmp.SetPixel(0, 0, Color.FromArgb(brightness, brightness, brightness));

            //byte brightness;
            for (int i = 1; i < bmp.Width; i++)
            {
                for (int j = 1; j < bmp.Height; j++)
                {

                    int gxR = (bmp.GetPixel(i, j).R - bmp.GetPixel(i - 1, j).R);
                    int gyR = (bmp.GetPixel(i, j).R - bmp.GetPixel(i, j - 1).R);
                    int red = (int) Math.Sqrt(Math.Pow(gxR, 2) + Math.Pow(gyR, 2));
                    if (red > 255) red = 255;
                    else if (red < 0) red = 0;
                    
                    int gxG = (bmp.GetPixel(i, j).G - bmp.GetPixel(i - 1, j).G);
                    int gyG = (bmp.GetPixel(i, j).G - bmp.GetPixel(i, j - 1).G);
                    int green = (int) Math.Sqrt(Math.Pow(gxG, 2) + Math.Pow(gyG, 2));
                    if (green > 255) green = 255;
                    else if (green < 0) green = 0;
                    
                    int gxB= (bmp.GetPixel(i, j).B - bmp.GetPixel(i - 1, j).B);
                    int gyB = (bmp.GetPixel(i, j).B - bmp.GetPixel(i, j - 1).B);
                    int blue = (int) Math.Sqrt(Math.Pow(gxB, 2) + Math.Pow(gyB, 2));
                    if (blue > 255) blue = 255;
                    else if (blue < 0) blue = 0;
                    
                    resbmp.SetPixel(i, j, Color.FromArgb(red, green, blue));

                    //double a = Math.Atan((double)gy / gx) * 180 / Math.PI;
                    //angles[i, j] = GetAngleMult45(a);
                }
            }

            //Крайние точки
            //нижняя и верхняя часть
            // for (int i = 1; i < bmp.Width - 1; i++)
            // {
            //     brightness = (byte)Math.Sqrt(Math.Pow((bmp.GetPixel(i, bmp.Height - 1).G - bmp.GetPixel(i - 1, bmp.Height - 1).G), 2));
            //     resbmp.SetPixel(i, bmp.Height - 1, Color.FromArgb(brightness, brightness, brightness));
            //
            //     brightness = (byte)Math.Sqrt(Math.Pow((bmp.GetPixel(i, 0).G - bmp.GetPixel(i - 1, 0).G), 2) + Math.Pow((bmp.GetPixel(i, 0).G - bmp.GetPixel(i, 1).G), 2));
            //     resbmp.SetPixel(i, 0, Color.FromArgb(brightness, brightness, brightness));
            // }
            //
            // //правая и левая часть
            // for (int i = 1; i < bmp.Height - 1; i++)
            // {
            //     brightness = (byte)Math.Sqrt(Math.Pow((bmp.GetPixel(bmp.Width - 1, i).G - bmp.GetPixel(bmp.Width - 1, i - 1).G), 2) + Math.Pow((bmp.GetPixel(bmp.Width - 2, i).G - bmp.GetPixel(bmp.Width - 2, i - 1).G), 2));
            //     resbmp.SetPixel(bmp.Width - 1, i, Color.FromArgb(brightness, brightness, brightness));
            //
            //     brightness = (byte)Math.Sqrt(Math.Pow((bmp.GetPixel(0, i).G - bmp.GetPixel(0, i - 1).G), 2) + Math.Pow((bmp.GetPixel(0, i).G - bmp.GetPixel(1, i).G), 2));
            //     resbmp.SetPixel(0, i, Color.FromArgb(brightness, brightness, brightness));
            // }
            return resbmp;
        }

        //метод на основе лапласиана
        public static Bitmap LaplacianDetection(Bitmap bmp)
        {
            Bitmap resbmp = new Bitmap(bmp);

            //byte brightness = (byte)Math.Sqrt(Math.Pow((bmp.GetPixel(0, 0).G - bmp.GetPixel(1, 0).G), 2) + Math.Pow((bmp.GetPixel(0, 0).G - bmp.GetPixel(0, 1).G), 2));
            //resbmp.SetPixel(0, 0, Color.FromArgb(brightness, brightness, brightness));

            byte brightness;
            for (int i = 1; i < bmp.Width - 1; i++)
            {
                for (int j = 1; j < bmp.Height - 1; j++)
                {

                    int gx = (bmp.GetPixel(i + 1, j).G - 2 * bmp.GetPixel(i, j).G + bmp.GetPixel(i - 1, j).G);
                    int gy = (bmp.GetPixel(i, j + 1).G - 2 * bmp.GetPixel(i, j).G + bmp.GetPixel(i, j - 1).G);

                    brightness = (byte)(Math.Abs(gx + gy));
                    resbmp.SetPixel(i, j, Color.FromArgb(brightness, brightness, brightness));
                }
            }

            //Крайние точки
            //нижняя и верхняя часть
            for (int i = 1; i < bmp.Width - 1; i++)
            {
                int j = bmp.Height - 1;
                int gx = (bmp.GetPixel(i + 1, j).G - 2 * bmp.GetPixel(i, j).G + bmp.GetPixel(i - 1, j).G);
                int gy = (bmp.GetPixel(i, j).G - 2 * bmp.GetPixel(i, j).G + bmp.GetPixel(i, j - 1).G);

                brightness = (byte)(Math.Abs(gx + gy));
                resbmp.SetPixel(i, j, Color.FromArgb(brightness, brightness, brightness));

                j = 0;
                gx = (bmp.GetPixel(i + 1, j).G - 2 * bmp.GetPixel(i, j).G + bmp.GetPixel(i - 1, j).G);
                gy = (bmp.GetPixel(i, j).G - 2 * bmp.GetPixel(i, j).G + bmp.GetPixel(i, j).G);

                brightness = (byte)(Math.Abs(gx + gy));
                resbmp.SetPixel(i, j, Color.FromArgb(brightness, brightness, brightness));
            }

            //правая и левая часть
            for (int j = 1; j < bmp.Height - 1; j++)
            {
                int i = bmp.Width - 1;
                int gx = (bmp.GetPixel(i, j).G - 2 * bmp.GetPixel(i, j).G + bmp.GetPixel(i - 1, j).G);
                int gy = (bmp.GetPixel(i, j + 1).G - 2 * bmp.GetPixel(i, j).G + bmp.GetPixel(i, j - 1).G);

                brightness = (byte)(Math.Abs(gx + gy));
                resbmp.SetPixel(i, j, Color.FromArgb(brightness, brightness, brightness));


                i = 0;
                gx = (bmp.GetPixel(i, j).G - 2 * bmp.GetPixel(i, j).G + bmp.GetPixel(i, j).G);
                gy = (bmp.GetPixel(i, j + 1).G - 2 * bmp.GetPixel(i, j).G + bmp.GetPixel(i, j).G);

                brightness = (byte)(Math.Abs(gx + gy));
                resbmp.SetPixel(i, j, Color.FromArgb(brightness, brightness, brightness));
            }
            return resbmp;
        }
        
        public static Bitmap LaplacianDetectionColor(Bitmap bmp)
        {
            Bitmap resbmp = new Bitmap(bmp);

            //byte brightness = (byte)Math.Sqrt(Math.Pow((bmp.GetPixel(0, 0).G - bmp.GetPixel(1, 0).G), 2) + Math.Pow((bmp.GetPixel(0, 0).G - bmp.GetPixel(0, 1).G), 2));
            //resbmp.SetPixel(0, 0, Color.FromArgb(brightness, brightness, brightness));

            //byte brightness;
            for (int i = 1; i < bmp.Width - 1; i++)
            {
                for (int j = 1; j < bmp.Height - 1; j++)
                {

                    int gxR = bmp.GetPixel(i + 1, j).R - 2 * bmp.GetPixel(i, j).R + bmp.GetPixel(i - 1, j).R;
                    int gyR = bmp.GetPixel(i, j + 1).R - 2 * bmp.GetPixel(i, j).R + bmp.GetPixel(i, j - 1).R;
                    int red = Math.Abs(gxR + gyR);
                    
                    if (red > 255) red = 255;

                    int gxG = bmp.GetPixel(i + 1, j).G - 2 * bmp.GetPixel(i, j).G + bmp.GetPixel(i - 1, j).G;
                    int gyG = bmp.GetPixel(i, j + 1).G - 2 * bmp.GetPixel(i, j).G + bmp.GetPixel(i, j - 1).G;
                    int green = Math.Abs(gxG + gyG);
                    
                    if (green > 255) green = 255;

                    int gxB = bmp.GetPixel(i + 1, j).B - 2 * bmp.GetPixel(i, j).B + bmp.GetPixel(i - 1, j).B;
                    int gyB = bmp.GetPixel(i, j + 1).B - 2 * bmp.GetPixel(i, j).B + bmp.GetPixel(i, j - 1).B;
                    int blue = Math.Abs(gxB + gyB);
                    
                    if (blue > 255) blue = 255;

                    resbmp.SetPixel(i, j, Color.FromArgb(red, green, blue));
                }
            }

            //Крайние точки
            //нижняя и верхняя часть
            // for (int i = 1; i < bmp.Width - 1; i++)
            // {
            //     int j = bmp.Height - 1;
            //     int gx = (bmp.GetPixel(i + 1, j).G - 2 * bmp.GetPixel(i, j).G + bmp.GetPixel(i - 1, j).G);
            //     int gy = (bmp.GetPixel(i, j).G - 2 * bmp.GetPixel(i, j).G + bmp.GetPixel(i, j - 1).G);
            //
            //     brightness = (int)(Math.Abs(gx + gy));
            //     resbmp.SetPixel(i, j, Color.FromArgb(brightness, brightness, brightness));
            //
            //     j = 0;
            //     gx = (bmp.GetPixel(i + 1, j).G - 2 * bmp.GetPixel(i, j).G + bmp.GetPixel(i - 1, j).G);
            //     gy = (bmp.GetPixel(i, j).G - 2 * bmp.GetPixel(i, j).G + bmp.GetPixel(i, j).G);
            //
            //     brightness = (byte)(Math.Abs(gx + gy));
            //     resbmp.SetPixel(i, j, Color.FromArgb(brightness, brightness, brightness));
            // }
            //
            // //правая и левая часть
            // for (int j = 1; j < bmp.Height - 1; j++)
            // {
            //     int i = bmp.Width - 1;
            //     int gx = (bmp.GetPixel(i, j).G - 2 * bmp.GetPixel(i, j).G + bmp.GetPixel(i - 1, j).G);
            //     int gy = (bmp.GetPixel(i, j + 1).G - 2 * bmp.GetPixel(i, j).G + bmp.GetPixel(i, j - 1).G);
            //
            //     brightness = (byte)(Math.Abs(gx + gy));
            //     resbmp.SetPixel(i, j, Color.FromArgb(brightness, brightness, brightness));
            //
            //
            //     i = 0;
            //     gx = (bmp.GetPixel(i, j).G - 2 * bmp.GetPixel(i, j).G + bmp.GetPixel(i, j).G);
            //     gy = (bmp.GetPixel(i, j + 1).G - 2 * bmp.GetPixel(i, j).G + bmp.GetPixel(i, j).G);
            //
            //     brightness = (byte)(Math.Abs(gx + gy));
            //     resbmp.SetPixel(i, j, Color.FromArgb(brightness, brightness, brightness));
            // }
            return resbmp;
        }
    }
}
