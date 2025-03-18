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
            string[] validExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff" };

            string extension = Path.GetExtension(filePath).ToLower();
            
            return validExtensions.Contains(extension);
        }
        public static void Avvio(string modelFilePath, string testFilePath, string savePath, string nome_operatore, Form1 form)
        {
            try
            {
                // Caricamento delle immagini
                Mat modelImg = CvInvoke.Imread(modelFilePath, ImreadModes.Color);
                Mat testImg = CvInvoke.Imread(testFilePath, ImreadModes.Color);

                if (modelImg.IsEmpty || testImg.IsEmpty)
                {
                    MessageBox.Show("Errore: Immagine non caricata correttamente.");
                    return;
                }

                // Crea una maschera nera delle stesse dimensioni dell'immagine
                Mat mask = new Mat(testImg.Size, DepthType.Cv8U, 1);
                mask.SetTo(new MCvScalar(0));

                // Calcola la larghezza della ROI (escludendo il 20% destro e sinistro)
                int roiX = (int)(testImg.Width * 0.15);    // Inizio al 20% della larghezza
                int roiWidth = (int)(testImg.Width * 0.7);  // Larghezza pari al 60% dell'immagine
                int roiHeight = testImg.Height;
                Rectangle roi = new Rectangle(roiX, 0, roiWidth, testImg.Height);

                // Crea le immagini ritagliate
                CvInvoke.Rectangle(mask, roi, new MCvScalar(255), -1);

                // Conversione in scala di grigi
                Mat modelGray = new Mat();
                Mat testGray = new Mat();
                CvInvoke.CvtColor(modelImg, modelGray, ColorConversion.Bgr2Gray);
                CvInvoke.CvtColor(testImg, testGray, ColorConversion.Bgr2Gray);

                // Regola contrasto (alpha) e luminosità (beta)
                double alpha = 3;  // Esempio: 1.2 aumenta contrasto del 20%
                int beta = -100;       // Esempio: 30 aumenta luminosità di 30 unità
                CvInvoke.ConvertScaleAbs(modelGray, modelGray, alpha, beta);
                CvInvoke.ConvertScaleAbs(testGray, testGray, alpha, beta);

                // Applica la maschera all'immagine in scala di grigi
                Mat testGrayMasked = new Mat();
                Mat modelGrayMasked = new Mat();
                CvInvoke.BitwiseAnd(modelGray, mask, modelGrayMasked);
                CvInvoke.BitwiseAnd(testGray, mask, testGrayMasked);

                // Applicazione del filtro GaussianBlur per ridurre il rumore
                CvInvoke.GaussianBlur(modelGrayMasked, modelGrayMasked, new Size(5, 5), 1);
                CvInvoke.GaussianBlur(testGrayMasked, testGrayMasked, new Size(5, 5), 1);

                // Thresholding con metodo Otsu per ottenere immagini binarie
                CvInvoke.Threshold(modelGrayMasked, modelGrayMasked, 0, 255, ThresholdType.Binary | ThresholdType.Otsu);
                CvInvoke.Threshold(testGrayMasked, testGrayMasked, 0, 255, ThresholdType.Binary | ThresholdType.Otsu);

                // Operazioni morfologiche per eliminare piccoli rumori (closing)
                Mat kernel = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(5, 5), new Point(-1, -1));
                CvInvoke.MorphologyEx(modelGrayMasked, modelGrayMasked, MorphOp.Close, kernel, new Point(-1, -1), 1, BorderType.Default, new MCvScalar());
                CvInvoke.MorphologyEx(testGrayMasked, testGrayMasked, MorphOp.Close, kernel, new Point(-1, -1), 1, BorderType.Default, new MCvScalar());

                //inverte i colori da bianco a nero
                CvInvoke.BitwiseNot(modelGrayMasked, modelGrayMasked);
                CvInvoke.BitwiseNot(testGrayMasked, testGrayMasked);

                // Ricerca dei contorni (usando il retrieval di contorni esterni)
                VectorOfVectorOfPoint modelContours = new VectorOfVectorOfPoint();
                VectorOfVectorOfPoint testContours = new VectorOfVectorOfPoint();
                Mat hierarchy = new Mat();
                CvInvoke.FindContours(modelGrayMasked, modelContours, hierarchy, RetrType.External, ChainApproxMethod.ChainApproxSimple);
                CvInvoke.FindContours(testGrayMasked, testContours, hierarchy, RetrType.External, ChainApproxMethod.ChainApproxSimple);

                if (modelContours.Size == 0 || testContours.Size == 0)
                {
                    MessageBox.Show("Errore: Nessun contorno trovato.");
                    return;
                }

                // Selezione del contorno più grande per il modello
                int modelLargestIdx = -1;
                double modelMaxArea = 0;
                for (int i = 0; i < modelContours.Size; i++)
                {
                    double area = CvInvoke.ContourArea(modelContours[i]);
                    if (area > modelMaxArea && area < (roiWidth * roiHeight * 0.8))
                    {
                        modelMaxArea = area;
                        modelLargestIdx = i;
                    }
                }

                // Selezione del contorno più grande per l'immagine di test
                int testLargestIdx = -1;
                double testMaxArea = 0;
                for (int i = 0; i < testContours.Size; i++)
                {
                    double area = CvInvoke.ContourArea(testContours[i]);
                    if (area > testMaxArea && area < (roiWidth * roiHeight * 0.8))
                    {
                        testMaxArea = area;
                        testLargestIdx = i;
                    }
                }

                if (modelLargestIdx == -1 || testLargestIdx == -1)
                {
                    MessageBox.Show("Errore: Nessun contorno valido trovato.");
                    return;
                }

                // Estrazione delle proprietà del contorno: orientamento e centro
                RotatedRect modelRect = CvInvoke.MinAreaRect(modelContours[modelLargestIdx]);
                RotatedRect testRect = CvInvoke.MinAreaRect(testContours[testLargestIdx]);

                // Calcolo del centro tramite i momenti (per il test)
                Moments momentsTest = CvInvoke.Moments(testContours[testLargestIdx]);
                double cX = momentsTest.M10 / momentsTest.M00;
                double cY = momentsTest.M01 / momentsTest.M00;

                // Calcolo angoli con correzione
                double modelAngle = modelRect.Angle;
                if (modelRect.Size.Width < modelRect.Size.Height) modelAngle += 90;

                double testAngle = testRect.Angle;
                if (testRect.Size.Width < testRect.Size.Height) testAngle += 90;

                // Normalizzazione dell'angolo
                double rotationDiff = testAngle - modelAngle;
                if (rotationDiff > 180) rotationDiff -= 360;
                else if (rotationDiff < -180) rotationDiff += 360;


                // Calcolo dello spostamento: differenza dei centri
                Moments momentsModel = CvInvoke.Moments(modelContours[modelLargestIdx]);
                double modelCX = momentsModel.M10 / momentsModel.M00;
                double modelCY = momentsModel.M01 / momentsModel.M00;
                double displacementX = cX - modelCX;
                double displacementY = cY - modelCY;

                // Disegno dei contorni e delle bounding box per visualizzare i risultati
                PointF[] modelBox = CvInvoke.BoxPoints(modelRect);
                for (int i = 0; i < 4; i++)
                {
                    CvInvoke.Line(modelImg,
                        new Point((int)modelBox[i].X, (int)modelBox[i].Y),
                        new Point((int)modelBox[(i + 1) % 4].X, (int)modelBox[(i + 1) % 4].Y),
                        new MCvScalar(255, 0, 0), 2);
                }
                PointF[] testBox = CvInvoke.BoxPoints(testRect);
                for (int i = 0; i < 4; i++)
                {
                    CvInvoke.Line(testImg,
                        new Point((int)testBox[i].X, (int)testBox[i].Y),
                        new Point((int)testBox[(i + 1) % 4].X, (int)testBox[(i + 1) % 4].Y),
                        new MCvScalar(0, 255, 0), 2);
                }
                // Disegna il centro dell'immagine di test
                CvInvoke.Circle(testImg, new Point((int)cX, (int)cY), 5, new MCvScalar(0, 0, 255), -1);







               
                List<ContourData> contourDataList = new List<ContourData>();


                string dataTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                ContourData newContourData = new ContourData
                {
                    NomeOperatore = nome_operatore,
                    DataTime = dataTime,
                    CentroidX = cX,
                    CentroidY = cY,
                    RotationAngle = rotationDiff
                };

                // Verifica se il dato esiste già
                bool isDuplicate = contourDataList.Any(existingData =>
                    Math.Abs(existingData.CentroidX - newContourData.CentroidX) < 1e-5 &&
                    Math.Abs(existingData.CentroidY - newContourData.CentroidY) < 1e-5 &&
                    Math.Abs(existingData.RotationAngle - newContourData.RotationAngle) < 1e-5);

                if (!isDuplicate)
                {
                    contourDataList.Add(newContourData);
                    try
                    {
                        //InsertDataIntoDatabase(nome_operatore, dataTime, cX, cY, rotationDiff);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Errore: " + ex);
                    }
                }
                CreateTestDirectory(savePath, modelFilePath, testFilePath, modelImg, testImg, contourDataList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore: {ex.Message}");
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