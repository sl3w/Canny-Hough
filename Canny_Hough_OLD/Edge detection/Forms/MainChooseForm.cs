using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Edge_detection
{
    public partial class MainChooseForm : Form
    {
        public MainChooseForm()
        {
            InitializeComponent();
        }

        private Image uploadedImage;
        private string fileImagePath;

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
                        button2.Enabled = true;
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

        private void button1_Click(object sender, EventArgs e)
        {
            SelectImage();   
        }

        private void SelectImage()
        {
            fileImagePath = LoadAndSaveImage.UploadImage();
            if (fileImagePath != null)
                ShowUploadedImage();
        }

        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                fileImagePath = richTextBox1.Text;
                ShowUploadedImage();
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (label3.Visible)
                label3.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StartEdgeDetection();
        }

        private void StartEdgeDetection()
        {
            new ImageProcessingForm(uploadedImage).Show();
        }

        private void MainChooseForm_DragEnter(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            FileInfo fileInfo = new FileInfo(files[0]);
            string extension = fileInfo.Extension.ToLower();
            if (extension == ".png" || extension == ".jpg" || extension == ".bmp" || extension == ".gif")
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop) && files.Length == 1)
                {
                    e.Effect = DragDropEffects.All;
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void MainChooseForm_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            try
            {
                fileImagePath = files[0];
                ShowUploadedImage();
            }
            catch
            {
                label3.Visible = true;
            }
        }

        private void выйтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new About().ShowDialog();
        }
    }
}
