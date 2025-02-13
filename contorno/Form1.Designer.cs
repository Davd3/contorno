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
            nome_operatore_label = new Label();
            nome_operatore_box = new TextBox();
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
            // nome_operatore_label
            // 
            nome_operatore_label.AutoSize = true;
            nome_operatore_label.Location = new Point(12, 242);
            nome_operatore_label.Name = "nome_operatore_label";
            nome_operatore_label.Size = new Size(123, 20);
            nome_operatore_label.TabIndex = 16;
            nome_operatore_label.Text = "Nome operatore:";
            // 
            // nome_operatore_box
            // 
            nome_operatore_box.Location = new Point(12, 274);
            nome_operatore_box.Name = "nome_operatore_box";
            nome_operatore_box.PlaceholderText = "Nome operatore";
            nome_operatore_box.Size = new Size(125, 27);
            nome_operatore_box.TabIndex = 17;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(580, 322);
            Controls.Add(nome_operatore_box);
            Controls.Add(nome_operatore_label);
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
        public Label nome_operatore_label;
        public TextBox nome_operatore_box;
    }
}
