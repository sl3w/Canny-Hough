using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edge_detection
{
    class Hough
    {
        public static int[,] Accum;

        /**** Преобразование в полутоновое ****/
        public static void GrayScale(Bitmap img)
        {
            for (int y = 0; y < img.Height; y++)
            for (int x = 0; x < img.Width; x++)
            {
                Color c = img.GetPixel(x, y);
                /* формула расчета */
                int px = (int) ((c.R * 0.3) + (c.G * 0.59) + (c.B * 0.11));
                img.SetPixel(x, y, Color.FromArgb(c.A, px, px, px));
            }
        }

        /**** Бинаризация изображения ****/
        public static Bitmap Binarization(Bitmap img)
        {
            double threshold = 0.7;
            for (int y = 0; y < img.Height; y++)
            for (int x = 0; x < img.Width; x++)
                img.SetPixel(x, y, img.GetPixel(x, y).GetBrightness() < threshold ? Color.Black : Color.White);
            return img;
        }

        /**** Выделение краев оператором Собеля ****/
        public static Bitmap Sobel(Bitmap src)
        {
            Bitmap dst = new Bitmap(src.Width, src.Height);
            //оператор Собеля
            int[,] dx = {{-1, 0, 1}, {-2, 0, 2}, {-1, 0, 1}};
            int[,] dy = {{1, 2, 1}, {0, 0, 0}, {-1, -2, -1}};

            //Преобразование в полутоновое изображение
            GrayScale(src);

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
                        sumX += c * dx[i + 1, j + 1];
                        //и сумму произведений пикселя на значение из матрицы по Y
                        sumY += c * dy[i + 1, j + 1];
                    }

                    //найти приближенное значение величины градиента
                    //sum = Math.Abs(sumX) + Math.Abs(sumY);
                    sum = (int) Math.Sqrt(Math.Pow(sumX, 2) + Math.Pow(sumY, 2));
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

        /**** Алгоритмы поиска локальных максимумов ****/

        public static Point SearchLine(Point Size, int tr)
        {
            int /*sum = 0,*/
                max = 0;
            Point pt = new Point(0, 0);

            for (int y = 0; y < Size.Y; y++)
            for (int x = 0; x < Size.X; x++)
            {
                //sum = 0;
                if (max < Accum[y, x])
                {
                    max = Accum[y, x];
                    pt.X = x;
                    pt.Y = y;
                }
            }

            if (max < tr) pt.X = -1;
            else Accum[pt.Y, pt.X] = 0;

            return pt;
        }

        public static Point SearchCircle(Point Size, int tr)
        {
            int sum = 0, max = 0;
            Point pt = new Point(0, 0);

            for (int y = 1; y < Size.Y - 1; y++)
            for (int x = 1; x < Size.X - 1; x++)
            {
                sum = 0;
                for (int i = -1; i <= 1; i++)
                for (int j = -1; j <= 1; j++)
                    sum += Accum[y + i, x + j];

                if (max < sum)
                {
                    max = sum;
                    pt.X = x;
                    pt.Y = y;
                }
            }

            if (max / 9 < tr) pt.X = -1;
            else
            {
                for (int i = -1; i <= 1; i++)
                for (int j = -1; j <= 1; j++)
                    Accum[pt.Y + i, pt.X + j] = 0;
            }

            return pt;
        }

        /**** Максимум в аккумуляторе ****/
        public static int AccumMax(Point Size)
        {
            int amax = 0;
            for (int y = 0; y < Size.Y; y++)
            for (int x = 0; x < Size.X; x++)
                if (Accum[y, x] > amax)
                    amax = Accum[y, x];
            return amax;
        }

        /**** Нормализация в аккумуляторе ****/
        public static void Normalize(Point Size, int amax)
        {
            for (int y = 0; y < Size.Y; y++)
            for (int x = 0; x < Size.X; x++)
            {
                int c = (int) (((double) Accum[y, x] / (double) amax) * 255.0);
                Accum[y, x] = c;
            }
        }

        public static Bitmap TransformLine(Bitmap img)
        {
            var Size = new Point();
            const int mang = 180;

            Size.Y = (int) Math.Round(Math.Sqrt(Math.Pow(img.Width, 2) + Math.Pow(img.Height, 2)));
            Size.X = 180;
            Accum = new int[Size.Y, mang];

            const double dt = Math.PI / 180.0;
            for (int y = 0; y < img.Height; y++)
            {
                for (int x = 0; x < img.Width; x++)
                {
                    if (img.GetPixel(x, y).R == 255)
                    {
                        for (int i = 0; i < mang; i++)
                        {
                            int row = (int) Math.Round(x * Math.Cos(dt * i) + y * Math.Sin(dt * i));
                            if (row < Size.Y && row > 0)
                                Accum[row, i]++;
                        }
                    }
                }
            }

            // Поиск максимума
            var amax = AccumMax(Size);
            // Нормализация 
            var img1 = new Bitmap(img.Width, img.Height);
            if (amax != 0)
            {
                img1 = new Bitmap(Size.X, Size.Y);
                // Нормализация в аккумулятор
                Normalize(Size, amax);
                for (int y = 0; y < Size.Y; y++)
                for (int x = 0; x < Size.X; x++)
                {
                    int c = Accum[y, x];
                    img1.SetPixel(x, y, Color.FromArgb(c, c, c));
                }
            }

            return img1;
        }

        public static Bitmap TransformCircle(Bitmap img, int r)
        {
            Point Size = new Point(img.Width, img.Height);
            int mang = 360;

            Accum = new int[Size.Y, Size.X];
            double dt = Math.PI / 180.0;

            for (int y = 0; y < img.Height; y++)
            for (int x = 0; x < img.Width; x++)
                if (img.GetPixel(x, y).R == 255)
                {
                    for (int i = 0; i < mang; i++)
                    {
                        int Tx = (int) Math.Round(x - r * Math.Cos(dt * i));
                        int Ty = (int) Math.Round(y + r * Math.Sin(dt * i));
                        if ((Tx < Size.X) && (Tx > 0) && (Ty < Size.Y) && (Ty > 0)) Accum[Ty, Tx]++;
                    }
                }

            // Поиск максимума
            int amax = AccumMax(Size);
            // Нормализация 
            Bitmap img1 = new Bitmap(img.Width, img.Height);
            if (amax != 0)
            {
                img1 = new Bitmap(Size.X, Size.Y);
                // Нормализация в аккумулятор
                Normalize(Size, amax);
                for (int y = 0; y < Size.Y; y++)
                for (int x = 0; x < Size.X; x++)
                {
                    int c = Accum[y, x];
                    img1.SetPixel(x, y, Color.FromArgb(c, c, c));
                }
            }

            return img1;
        }
    }
}