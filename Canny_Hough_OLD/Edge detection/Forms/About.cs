using System.Windows.Forms;

namespace Edge_detection
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenAboutFile();
        }

        private void OpenAboutFile()
        {
             try
            {
                System.Diagnostics.Process.Start(@"EdgeDetectionAbout.chm");
            }
            catch
            {
                MessageBox.Show("Файл справки не найден!", "",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
