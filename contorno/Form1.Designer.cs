namespace contorno
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            panel1 = new Panel();
            button1 = new Button();
            label2 = new Label();
            panel2 = new Panel();
            modelImageFeedback = new Label();
            testImageFeedback = new Label();
            folderBrowserDialog1 = new FolderBrowserDialog();
            centroText = new Label();
            angoloText = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F);
            label1.Location = new Point(12, 4);
            label1.Name = "label1";
            label1.Size = new Size(293, 28);
            label1.TabIndex = 0;
            label1.Text = "Trascina qui l'immagine modello";
            // 
            // panel1
            // 
            panel1.AllowDrop = true;
            panel1.Anchor = AnchorStyles.None;
            panel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Location = new Point(12, 34);
            panel1.Name = "panel1";
            panel1.Size = new Size(200, 150);
            panel1.TabIndex = 1;
            panel1.DragDrop += panel1_DragDrop;
            panel1.DragEnter += panel1_DragEnter;
            // 
            // button1
            // 
            button1.Enabled = false;
            button1.Location = new Point(356, 264);
            button1.Name = "button1";
            button1.Size = new Size(156, 46);
            button1.TabIndex = 4;
            button1.Text = "Avvia";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F);
            label2.Location = new Point(321, 4);
            label2.Name = "label2";
            label2.Size = new Size(252, 28);
            label2.TabIndex = 12;
            label2.Text = "Trascina qui l'immagine test";
            // 
            // panel2
            // 
            panel2.AllowDrop = true;
            panel2.Anchor = AnchorStyles.None;
            panel2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel2.Location = new Point(336, 34);
            panel2.Name = "panel2";
            panel2.Size = new Size(200, 150);
            panel2.TabIndex = 13;
            panel2.DragDrop += panel2_DragDrop;
            panel2.DragEnter += panel2_DragEnter;
            // 
            // modelImageFeedback
            // 
            modelImageFeedback.AutoSize = true;
            modelImageFeedback.Location = new Point(12, 188);
            modelImageFeedback.Name = "modelImageFeedback";
            modelImageFeedback.Size = new Size(191, 20);
            modelImageFeedback.TabIndex = 14;
            modelImageFeedback.Text = "Nessuna immagine caricata";
            // 
            // testImageFeedback
            // 
            testImageFeedback.AutoSize = true;
            testImageFeedback.Location = new Point(321, 188);
            testImageFeedback.Name = "testImageFeedback";
            testImageFeedback.Size = new Size(191, 20);
            testImageFeedback.TabIndex = 15;
            testImageFeedback.Text = "Nessuna immagine caricata";
            // 
            // centroText
            // 
            centroText.AutoSize = true;
            centroText.Location = new Point(12, 242);
            centroText.Name = "centroText";
            centroText.Size = new Size(159, 20);
            centroText.TabIndex = 16;
            centroText.Text = "Coordinate del Centro:";
            // 
            // angoloText
            // 
            angoloText.AutoSize = true;
            angoloText.Location = new Point(12, 290);
            angoloText.Name = "angoloText";
            angoloText.Size = new Size(149, 20);
            angoloText.TabIndex = 17;
            angoloText.Text = "Angolo di rotazione: ";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(580, 322);
            Controls.Add(angoloText);
            Controls.Add(centroText);
            Controls.Add(testImageFeedback);
            Controls.Add(modelImageFeedback);
            Controls.Add(panel2);
            Controls.Add(label2);
            Controls.Add(button1);
            Controls.Add(panel1);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Applicazione";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Panel panel1;
        private Button button1;
        private Label label2;
        private Panel panel2;
        private Label modelImageFeedback;
        private Label testImageFeedback;
        private FolderBrowserDialog folderBrowserDialog1;
        public Label centroText;
        public Label angoloText;
    }
}
