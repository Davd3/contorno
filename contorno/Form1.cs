using System.Diagnostics;
using System.Drawing.Imaging;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace contorno
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string modelPath;
        private string testPath;
        private string selectedPath;
        private bool buttonCheck1 = false;
        private bool buttonCheck2 = false;

        private void panel1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                // Verifica che tutti i file abbiano estensioni valide
                if (files.All(file => Metodi.IsValidExtension(file)))
                {
                    e.Effect = DragDropEffects.Copy;
                    buttonCheck1 = true;
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
        }
        private void panel2_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                // Verifica che tutti i file abbiano estensioni valide
                if (files.All(file => Metodi.IsValidExtension(file)))
                {
                    e.Effect = DragDropEffects.Copy;
                    buttonCheck2 = true;
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
        }
        private void panel1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            foreach (string file in files)
            {
                modelPath = file;

                modelImageFeedback.Text = Path.GetFileName(modelPath);
                Image modelImage = Image.FromFile(modelPath);
                panel1.BackgroundImage = modelImage;
                panel1.BackgroundImageLayout = ImageLayout.Zoom;
            }
            button1.Enabled = buttonCheck1 == true && buttonCheck2 == true;
        }

        private void panel2_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                testPath = file;

                testImageFeedback.Text = Path.GetFileName(testPath);
                Image testImage = Image.FromFile(testPath);
                panel2.BackgroundImage = testImage;
                panel2.BackgroundImageLayout = ImageLayout.Zoom;
            }
            button1.Enabled = buttonCheck1 == true && buttonCheck2 == true;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                // Directory di default
                folderDialog.SelectedPath = @"C:\";


                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    selectedPath = folderDialog.SelectedPath;

                }
            }
            Metodi.Avvio(modelPath, testPath, selectedPath, this);
        }
    }
}