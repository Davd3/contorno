using System;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Newtonsoft.Json;
using contorno;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace contorno
{
    public class Metodi
    {
        public static bool IsValidExtension(string filePath)
        {
            // Elenco delle estensioni accettate
            string[] validExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff" };

            // Ottieni l'estensione del file
            string extension = Path.GetExtension(filePath).ToLower();

            // Verifica se l'estensione è tra quelle valide
            return validExtensions.Contains(extension);
        }
        public static void Avvio(string modelFilePath, string testFilePath, string savePath, string nome_operatore, Form1 form)
        {
            try
            {
                // Lettura delle immagini
                Mat modelImg = CvInvoke.Imread(modelFilePath, ImreadModes.Color);
                Mat testImg = CvInvoke.Imread(testFilePath, ImreadModes.Color);

                if (modelImg.IsEmpty || testImg.IsEmpty)
                {
                    MessageBox.Show("Errore: Immagine non caricata correttamente.");
                    return;
                }

                // Conversione in scala di grigi
                Mat modelGray = new Mat();
                Mat testGray = new Mat();
                CvInvoke.CvtColor(modelImg, modelGray, ColorConversion.Bgr2Gray);
                CvInvoke.CvtColor(testImg, testGray, ColorConversion.Bgr2Gray);

                // Applicazione del filtro GaussianBlur + Threshold per migliorare il rilevamento dei contorni
                CvInvoke.GaussianBlur(testGray, testGray, new Size(5, 5), 1.5);
                CvInvoke.Threshold(testGray, testGray, 100, 255, ThresholdType.Binary);
                CvInvoke.GaussianBlur(modelGray, modelGray, new Size(5, 5), 1.5);
                CvInvoke.Threshold(modelGray, modelGray, 100, 255, ThresholdType.Binary);

                // Applicazione del filtro Canny
                Mat modelEdges = new Mat();
                Mat testEdges = new Mat();
                CvInvoke.Canny(modelGray, modelEdges, 100, 200);
                CvInvoke.Canny(testGray, testEdges, 100, 200);

                // Ricerca dei contorni
                Mat hierarchy = new Mat();
                VectorOfVectorOfPoint modelContours = new VectorOfVectorOfPoint();
                VectorOfVectorOfPoint testContours = new VectorOfVectorOfPoint();
                CvInvoke.FindContours(modelEdges, modelContours, hierarchy, RetrType.External, ChainApproxMethod.ChainApproxSimple);
                CvInvoke.FindContours(testEdges, testContours, hierarchy, RetrType.External, ChainApproxMethod.ChainApproxSimple);

                if (modelContours.Size == 0 || testContours.Size == 0)
                {
                    MessageBox.Show("Errore: Nessun contorno trovato.");
                    return;
                }

                List<ContourData> contourDataList = new List<ContourData>();

                for (int i = 0; i < testContours.Size; i++)
                {
                    var contour = testContours[i];
                    double area = CvInvoke.ContourArea(contour);
                    if (area > 50) // Ignora contorni troppo piccoli
                    {
                        var moments = CvInvoke.Moments(contour);
                        if (moments.M00 != 0)
                        {
                            double cX = moments.M10 / moments.M00;
                            double cY = moments.M01 / moments.M00;
                            CvInvoke.Circle(testImg, new Point((int)cX, (int)cY), 5, new MCvScalar(0, 0, 255), -1);

                            RotatedRect testRotatedRect = CvInvoke.MinAreaRect(contour);
                            PointF[] testBox = CvInvoke.BoxPoints(testRotatedRect);
                            for (int j = 0; j < 4; j++)
                            {
                                CvInvoke.Line(testImg, new Point((int)testBox[j].X, (int)testBox[j].Y),
                                            new Point((int)testBox[(j + 1) % 4].X, (int)testBox[(j + 1) % 4].Y),
                                            new MCvScalar(255, 0, 0), 2);
                            }

                            double testAngle = testRotatedRect.Angle;
                            if (testRotatedRect.Size.Width < testRotatedRect.Size.Height)
                                testAngle += 90;

                            for (int j = 0; j < modelContours.Size; j++)
                            {
                                var modelContour = modelContours[j];
                                double modelArea = CvInvoke.ContourArea(modelContour);
                                if (modelArea > 50)
                                {
                                    RotatedRect modelRotatedRect = CvInvoke.MinAreaRect(modelContour);
                                    PointF[] modelBox = CvInvoke.BoxPoints(modelRotatedRect);
                                    for (int k = 0; k < 4; k++)
                                    {
                                        CvInvoke.Line(modelImg, new Point((int)modelBox[k].X, (int)modelBox[k].Y),
                                                    new Point((int)modelBox[(k + 1) % 4].X, (int)modelBox[(k + 1) % 4].Y),
                                                    new MCvScalar(255, 0, 0), 2);
                                    }

                                    double modelAngle = modelRotatedRect.Angle;
                                    if (modelRotatedRect.Size.Width < modelRotatedRect.Size.Height)
                                        modelAngle += 90;

                                    double rotationAngle = testAngle - modelAngle;
                                    if (rotationAngle > 180) rotationAngle -= 360;
                                    if (rotationAngle < -180) rotationAngle += 360;
                                    string dataTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                                    ContourData newContourData = new ContourData
                                    {
                                        NomeOperatore= nome_operatore,
                                        DataTime = dataTime,
                                        CentroidX = cX,
                                        CentroidY = cY,
                                        RotationAngle = rotationAngle
                                    };

                                    // Verifica se il dato esiste già
                                    bool isDuplicate = contourDataList.Any(existingData =>
                                        Math.Abs(existingData.CentroidX - newContourData.CentroidX) < 1e-5 &&
                                        Math.Abs(existingData.CentroidY - newContourData.CentroidY) < 1e-5 &&
                                        Math.Abs(existingData.RotationAngle - newContourData.RotationAngle) < 1e-5);

                                    if (!isDuplicate)
                                    {
                                        contourDataList.Add(newContourData);
                                        InsertDataIntoDatabase(nome_operatore, dataTime, cX, cY, rotationAngle);
                                    }
                                }
                            }
                        }
                    }
                }

                CreateTestDirectory(savePath, modelFilePath, testFilePath, modelImg, testImg, contourDataList);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore: {ex}");
            }
            form.nome_operatore_box.Clear();
        }




        public static void CreateTestDirectory(string savePath, string modelFilePath, string testFilePath, Mat modelImg, Mat testImg, List<ContourData> contourDataList)
        {
            string percorso = savePath;
            int contatore = 1;

            while (Directory.Exists(percorso))
            {
                percorso = savePath + @"\test_" + contatore;
                contatore++;
            }
            Directory.CreateDirectory(percorso);

            CvInvoke.Imwrite($@"{percorso}\model_with_centroids{Path.GetExtension(modelFilePath)}", modelImg);
            CvInvoke.Imwrite($@"{percorso}\test_with_centroids{Path.GetExtension(testFilePath)}", testImg);

            string jsonFilePath = Path.Combine(percorso, "contour_data.json");
            string json = JsonConvert.SerializeObject(contourDataList, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(jsonFilePath, json);

            MessageBox.Show($"Dati salvati in {percorso}");
        }
        public static void InsertDataIntoDatabase(string operatore, string dateTime, double centroidX, double centroidY, double rotationAngle)
        {
            string connectionString = "Server=DAVID3\\SQLEXPRESS;Database=contorno;User Id=root;Password=toor;TrustServerCertificate=True;";
            string query = "INSERT INTO Contorno (date_time, operatore, centroid_x, centroid_y, rotation_angle) VALUES (@dateTime, @operatore, @centroidX, @centroidY, @rotationAngle)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@dateTime", dateTime);
                        command.Parameters.AddWithValue("@operatore", operatore);
                        command.Parameters.AddWithValue("@centroidX", centroidX);
                        command.Parameters.AddWithValue("@centroidY", centroidY);
                        command.Parameters.AddWithValue("@rotationAngle", rotationAngle);

                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Errore durante l'inserimento nel database: {ex.Message}");
                }
            }

        }
    }
}
