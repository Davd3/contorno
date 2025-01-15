using System.Diagnostics;
using System.Drawing.Imaging;
using System.Security.Cryptography.X509Certificates;

namespace contorno
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void panel1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                // Verifica che tutti i file abbiano estensioni valide
                if (files.All(file => IsValidExtension(file)))
                {
                    e.Effect = DragDropEffects.Copy;
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
                if (IsValidExtension(file))
                {
                    MessageBox.Show($"File caricato con successo: {Path.GetFileName(file)}");
                    imagePath = file;
                    sizeBox.Enabled = true;
                    // Gestisci l'immagine
                    panel1.AllowDrop = false;
                }
                else
                {
                    MessageBox.Show($"File non valido: {Path.GetFileName(file)}");
                }
            }
        }

        private bool IsValidExtension(string filePath)
        {
            // Elenco delle estensioni accettate
            string[] validExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff" };

            // Ottieni l'estensione del file
            string extension = Path.GetExtension(filePath).ToLower();

            // Verifica se l'estensione è tra quelle valide
            return validExtensions.Contains(extension);
        }

        private void sizeBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(sizeBox.Text))
            {
                size = 5;
                return;
            }
            size = Convert.ToInt32(sizeBox.Text);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(imagePath))
                {
                    MessageBox.Show("Seleziona un'immagine prima di avviare il processo.");
                    return;
                }
                button1.Enabled = false;
                // Carica l'immagine bitmap
                Bitmap originalBitmap = new Bitmap(imagePath);

                // Reset ProgressBar
                progressBar1.Value = 0;

                // Avvia l'elaborazione
                Bitmap processedBitmap = await Task.Run(() =>
                {
                    return ProcessBitmapWithProgress(originalBitmap);
                });

                // Salva l'immagine elaborata
                string outputImagePath = "processed_image.bmp";
                processedBitmap.Save(outputImagePath, ImageFormat.Bmp);

                // Mostra i risultati
                MessageBox.Show("Elaborazione completata!");
                Process.Start(new ProcessStartInfo
                {
                    FileName = outputImagePath,
                    UseShellExecute = true
                });

                // Resetta ProgressBar
                progressBar1.Value = 0;
                panel1.AllowDrop = true;
                button1.Enabled = true;
            }

        private Bitmap ProcessBitmapWithProgress(Bitmap bitmap)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;

            // Bitmap per l'elaborazione
            Bitmap processedBitmap = new Bitmap(width, height);

            bool[,] binaryImage = new bool[width, height];
            double area = 0;
            double perimeter = 0;
            int sumX = 0, sumY = 0;

            int threshold = 128;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color pixel = bitmap.GetPixel(x, y);
                    int grayValue = (pixel.R + pixel.G + pixel.B) / 3;
                    bool isForeground = grayValue < threshold;
                    binaryImage[x, y] = isForeground;

                    if (isForeground)
                    {
                        area++;
                        sumX += x;
                        sumY += y;

                        if (IsEdge(binaryImage, x, y, width, height))
                        {
                            processedBitmap.SetPixel(x, y, Color.Green);
                            perimeter++;
                        }
                        else
                        {
                            processedBitmap.SetPixel(x, y, Color.Black);
                        }
                    }
                    else
                    {
                        processedBitmap.SetPixel(x, y, Color.White);
                    }
                }

                // Aggiorna la ProgressBar
                UpdateProgress((y + 1) * 100 / height);
            }

            Point centroid = new Point((int)(sumX / area), (int)(sumY / area));
            DrawCentroid(processedBitmap, centroid, size);

            return processedBitmap;
        }

        private void UpdateProgress(int progress)
        {
            if (progressBar1.InvokeRequired)
            {
                progressBar1.Invoke(new Action(() => progressBar1.Value = progress));
            }
            else
            {
                progressBar1.Value = progress;
            }
        }

        static bool IsEdge(bool[,] binaryImage, int x, int y, int width, int height)
        {
            int[] dx = { -1, 1, 0, 0 };
            int[] dy = { 0, 0, -1, 1 };

            for (int i = 0; i < 4; i++)
            {
                int nx = x + dx[i];
                int ny = y + dy[i];
                if (nx >= 0 && ny >= 0 && nx < width && ny < height && !binaryImage[nx, ny])
                {
                    return true;
                }
            }

            return false;
        }

        static void DrawCentroid(Bitmap bitmap, Point centroid, int size)
        {
            for (int dy = -size; dy <= size; dy++)
            {
                for (int dx = -size; dx <= size; dx++)
                {
                    int cx = centroid.X + dx;
                    int cy = centroid.Y + dy;
                    if (cx >= 0 && cy >= 0 && cx < bitmap.Width && cy < bitmap.Height)
                    {
                        bitmap.SetPixel(cx, cy, Color.Blue);
                    }
                }
            }
        }
    }
}