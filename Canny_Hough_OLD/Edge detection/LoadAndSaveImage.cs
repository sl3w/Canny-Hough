using System.Drawing;
using System.Windows.Forms;

namespace Edge_detection
{
    class LoadAndSaveImage
    {
        private static SaveFileDialog saveFileDialog = new SaveFileDialog();
        private static OpenFileDialog openFileDialog = new OpenFileDialog();
        public static void SaveImage(Image image)
        {
            saveFileDialog.FileName = "Обработанное изображение";
            saveFileDialog.Title = "Сохранение";
            //отображать ли предупреждение, если пользователь указывает имя уже существующего файла
            saveFileDialog.OverwritePrompt = true;
            //отображать ли предупреждение, если пользователь указывает несуществующий путь
            saveFileDialog.CheckPathExists = true;
            saveFileDialog.Filter = "JPEG (*.JPG)|*.jpg|PNG (*.PNG)|*.png|BMP (*.BMP)|*.bmp|GIF (*.GIF)|*.gif";
            if (saveFileDialog.ShowDialog() == DialogResult.OK) //если в диалоговом окне нажата кнопка "ОК"
            {
                try
                {
                    image.Save(saveFileDialog.FileName);
                }
                catch
                {
                    MessageBox.Show("Ошибка при сохранении изображения", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public static string UploadImage()
        {
            openFileDialog.FileName = "";
            openFileDialog.Filter = "Файлы изображений (*.jpg*,*.png,*.bmp,*.gif)|*.jpg; *.png; *.bmp; *.gif";
            openFileDialog.Title = "Выберите изображение";
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                return openFileDialog.FileName;
            }
            else
                return null;
        }
    }
}
