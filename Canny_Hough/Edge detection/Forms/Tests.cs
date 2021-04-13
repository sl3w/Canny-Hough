using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Edge_detection.Forms
{
    public partial class Tests : Form
    {
        public Tests()
        {
            InitializeComponent();
        }

        private Image uploadedImage;
        private string fileImagePath;

        private Image uploadedImage2;
        private string fileImagePath2;

        private void button1_Click(object sender, EventArgs e)
        {
            fileImagePath = LoadAndSaveImage.UploadImage();
            if (fileImagePath != null)
                ShowUploadedImage();
        }

        private void ShowUploadedImage()
        {
            try
            {
                uploadedImage = Image.FromFile(fileImagePath);
                if (uploadedImage.Height >= 200 && uploadedImage.Height <= 1500)
                {
                    if (uploadedImage.Width >= 200 && uploadedImage.Width <= 2000)
                    {
                        pictureBox1.Image = uploadedImage;
                        button3.Enabled = true;
                        richTextBox1.Text = fileImagePath;
                    }
                    else
                    {
                        label3.Text = "Неверные размеры изображения.";
                        label3.Visible = true;
                    }
                }
                else
                {
                    label3.Text = "Неверные размеры изображения.";
                    label3.Visible = true;
                }
            }
            catch
            {
                label3.Text = "Ошибка при загрузке изображения.";
                label3.Visible = true;
            }
        }

        private void ShowUploadedImage2()
        {
            try
            {
                uploadedImage2 = Image.FromFile(fileImagePath2);
                if (uploadedImage2.Height >= 200 && uploadedImage2.Height <= 1500)
                {
                    if (uploadedImage2.Width >= 200 && uploadedImage2.Width <= 2000)
                    {
                        pictureBox2.Image = uploadedImage2;
                        button3.Enabled = true;
                        richTextBox2.Text = fileImagePath2;
                    }
                    else
                    {
                        label1.Text = "Неверные размеры изображения.";
                        label1.Visible = true;
                    }
                }
                else
                {
                    label1.Text = "Неверные размеры изображения.";
                    label1.Visible = true;
                }
            }
            catch
            {
                label1.Text = "Ошибка при загрузке изображения.";
                label1.Visible = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            fileImagePath2 = LoadAndSaveImage.UploadImage();
            if (fileImagePath2 != null)
                ShowUploadedImage2();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var Test = new TestsProcessing(new Bitmap(uploadedImage), new Bitmap(uploadedImage2));
            label2.Text = "PCO:" + Test.CalcPCO();
            label4.Text = "Pnd:" + Test.CalcPnd();
            label5.Text = "Pfa:" + Test.CalcPfa();
        }
    }
}
