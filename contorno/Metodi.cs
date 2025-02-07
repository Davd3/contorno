using System;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Newtonsoft.Json;
using contorno;

namespace contorno {
    
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
        public static void Avvio(string modelFilePath, string testFilePath, string savePath, Form1 form) { 
            //--------------------------------PERCORSO FILE DELLE IMMAGINI--------------------------------

            string modelFilePathExtension = Path.GetExtension(modelFilePath);
            string testFilePathExtension = Path.GetExtension(testFilePath);
            // Lettura delle immagini
            Mat modelImg = CvInvoke.Imread(modelFilePath, ImreadModes.Color);
            Mat testImg = CvInvoke.Imread(testFilePath, ImreadModes.Color);

            // Conversione in scala di grigi
            Mat modelGray = new Mat();
            Mat testGray = new Mat();
            CvInvoke.CvtColor(modelImg, modelGray, ColorConversion.Bgr2Gray);
            CvInvoke.CvtColor(testImg, testGray, ColorConversion.Bgr2Gray);

            // Applicazione del filtro Canny
            Mat modelEdges = new Mat();
            Mat testEdges = new Mat();
            CvInvoke.Canny(modelGray, modelEdges, 50, 200, 3, true);
            CvInvoke.Canny(testGray, testEdges, 50, 200, 3, true);

            // Ricerca dei contorni
            Mat hierarchy = new Mat();
            VectorOfVectorOfPoint modelContours = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(modelEdges, modelContours, hierarchy, RetrType.External, ChainApproxMethod.ChainApproxNone);

            VectorOfVectorOfPoint testContours = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(testEdges, testContours, hierarchy, RetrType.External, ChainApproxMethod.ChainApproxNone);

            // Lista per salvare i dati dei contorni
            List<ContourData> contourDataList = new List<ContourData>();

            Console.WriteLine($"Numero di contorni trovati nell'immagine Tes: {testContours.Size}");
            if (testContours.Size > 0)
            {
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

                            // Disegna centroide su testImg
                            CvInvoke.Circle(testImg, new Point((int)cX, (int)cY), 5, new MCvScalar(0, 0, 255), -1);
                            Console.WriteLine($"Posizione del centro nell'immagine di test: ({cX}, {cY})");

                            // Trova il rettangolo minimo attorno al contorno
                            RotatedRect testRotatedRect = CvInvoke.MinAreaRect(contour);
                            PointF[] testBox = CvInvoke.BoxPoints(testRotatedRect);
                            for (int j = 0; j < 4; j++)
                            {
                                // Disegna il rettangolo minimo sull'immagine di test
                                CvInvoke.Line(testImg, new Point((int)testBox[j].X, (int)testBox[j].Y),
                                            new Point((int)testBox[(j + 1) % 4].X, (int)testBox[(j + 1) % 4].Y),
                                            new MCvScalar(255, 0, 0), 2);
                            }

                            // Ora possiamo confrontare i rettangoli tra modello e test
                            for (int k = 0; k < modelContours.Size; k++)
                            {
                                var modelContour = modelContours[k];
                                double modelArea = CvInvoke.ContourArea(modelContour);
                                if (modelArea > 50) // Ignora contorni troppo piccoli
                                {
                                    // Trova il rettangolo minimo attorno al contorno del modello
                                    RotatedRect modelRotatedRect = CvInvoke.MinAreaRect(modelContour);
                                    PointF[] modelBox = CvInvoke.BoxPoints(modelRotatedRect);
                                    for (int j = 0; j < 4; j++)
                                    {
                                        // Disegna il rettangolo minimo sull'immagine del modello
                                        CvInvoke.Line(modelImg, new Point((int)modelBox[j].X, (int)modelBox[j].Y),
                                                new Point((int)modelBox[(j + 1) % 4].X, (int)modelBox[(j + 1) % 4].Y),
                                                new MCvScalar(255, 0, 0), 2);
                                    }

                                    // Calcola l'angolo di rotazione tra il modello e il test
                                    double rotationAngle = modelRotatedRect.Angle - testRotatedRect.Angle;

                                    // Mostra l'angolo di rotazione tra i due rettangoli
                                    Console.WriteLine($"Angolo di rotazione tra il modello e il test: {rotationAngle}°");

                                    ContourData newContourData = new ContourData
                                    {
                                        CentroidX = cX,
                                        CentroidY = cY,
                                        RotationAngle = testRotatedRect.Angle
                                    };

                                    bool isDuplicate = false;
                                    foreach (var existingData in contourDataList)
                                    {
                                        if (Math.Abs(existingData.CentroidX - newContourData.CentroidX) < 1e-5 &&
                                            Math.Abs(existingData.CentroidY - newContourData.CentroidY) < 1e-5 &&
                                            Math.Abs(existingData.RotationAngle - newContourData.RotationAngle) < 1e-5)
                                        {
                                            isDuplicate = true;
                                            break;
                                        }
                                    }

                                    // Aggiungi il record solo se non è duplicato
                                    if (!isDuplicate)
                                    {
                                        contourDataList.Add(newContourData);
                                    }

                                    form.centroText.Text = $"Coordinate del centro: X: {cX}: Y: {cY}";
                                    form.angoloText.Text = $"Angolo di rotazione: {testRotatedRect.Angle}";
                                }
                            }
                        }
                    }
                }
            }

            string percorso = savePath;
            int contatore = 1;

            while (Directory.Exists(percorso))
            {
                percorso = savePath + @"\test_" + contatore;
                contatore++;
            }
            Directory.CreateDirectory(percorso);

            // Salvataggio delle immagini
            CvInvoke.Imwrite($@"{percorso}\model_with_centroids{modelFilePathExtension}", modelImg);
            CvInvoke.Imwrite($@"{percorso}\test_with_centroids{testFilePathExtension}", testImg);
            Console.WriteLine($"Immagini salvate in {percorso}.");


            // Salvataggio dei dati in un file JSON
            string jsonFilePath = Path.Combine(percorso, "contour_data.json");
            string json = JsonConvert.SerializeObject(contourDataList, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(jsonFilePath, json);

            MessageBox.Show($"Dati salvati in {percorso}");
        }
    }
}