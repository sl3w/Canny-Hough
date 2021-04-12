using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edge_detection
{
    class Tests
    {
        //загруженно изображение
        private Bitmap UploadedImage;
        //эталонное изображение
        private Bitmap EthalonImage;

        //количество граничных точек в эталонном контуре
        private int NI;

        //количество граничных точек в полученном контуре
        private int NB;

        public Tests(Bitmap upl)
        {
            UploadedImage = upl;
            NI = CountBorderPixel(EthalonImage);
            NB = CountBorderPixel(UploadedImage);
        }

        //подсчет количества граничных точек
        private int CountBorderPixel(Bitmap bitmap)
        {
            int counter = 0;
            for (int i = 0; i < UploadedImage.Width - 1; i++)
            {
                for (int j = 0; j < EthalonImage.Height - 1; j++)
                {
                    if (UploadedImage.GetPixel(i, j).R == 255)
                    {
                        counter++;
                    }
                }
            }
            return counter;
        }

        //вероятность верного детектирования
        private double CalcPCO()
        {
            double TP = 0;
            for (int i = 0; i < UploadedImage.Width - 1; i++)
            {
                for (int j = 0; j < EthalonImage.Height - 1; j++)
                {
                    if (EthalonImage.GetPixel(i, j).R == 255 && UploadedImage.GetPixel(i, j).R == 255)
                    {
                        TP++;
                    }
                }
            }

            return TP / Math.Max(NI, NB);
        }

        //вероятность ошибки первого рода
        private double CalcPnd()
        {
            double FN = 0;
            for (int i = 0; i < UploadedImage.Width - 1; i++)
            {
                for (int j = 0; j < EthalonImage.Height - 1; j++)
                {
                    if (EthalonImage.GetPixel(i, j).R == 255 && UploadedImage.GetPixel(i, j).R == 0)
                    {
                        FN++;
                    }
                }
            }

            return FN / Math.Max(NI, NB);
        }

        //вероятность ошибки второго рода
        private double CalcPfa()
        {
            double FP = 0;
            for (int i = 0; i < UploadedImage.Width - 1; i++)
            {
                for (int j = 0; j < EthalonImage.Height - 1; j++)
                {
                    if (EthalonImage.GetPixel(i, j).R == 0 && UploadedImage.GetPixel(i, j).R == 255)
                    {
                        FP++;
                    }
                }
            }

            return FP / Math.Max(NI, NB);
        }

        //метрика Прэтта
        private double CalcIMP()
        {
            double s = 0;
            double alpha = 0.1;
            int k = 1;
            int l = 1;
            for (int i = 0; i < UploadedImage.Width - 1; i++)
            {
                for (int j = 0; j < EthalonImage.Height - 1; j++)
                {
                    if (EthalonImage.GetPixel(i, j).R != UploadedImage.GetPixel(i, j).R )
                    {
                        if (UploadedImage.GetPixel(i, j - l).R == 255 || UploadedImage.GetPixel(i - k, j).R == 255)
                        {
                            double di = Math.Sqrt(Math.Pow(k, 2) + Math.Pow(l, 2));
                            s += 1 / ((double)(1 + alpha * di));
                        }
                    } 
                    else
                    {
                        s += 1;
                    }
                }
            }

            return s / Math.Max(NI, NB);
        }

        //комплексная метрика
        private double CalcD4()
        {
            double D4 = Math.Pow(CalcPCO() - 1, 2) + Math.Pow(CalcIMP() - 1, 2) + CalcPnd() + CalcPfa();
            return Math.Sqrt(D4);
        }
    }
}
