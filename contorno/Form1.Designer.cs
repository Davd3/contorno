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
            sizeBox = new TextBox();
            label2 = new Label();
            button1 = new Button();
            progressBar1 = new ProgressBar();
            perimetroLabel = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F);
            label1.Location = new Point(3, 4);
            label1.Name = "label1";
            label1.Size = new Size(215, 28);
            label1.TabIndex = 0;
            label1.Text = "Trascina qui l'immagine";
            // 
            // panel1
            // 
            panel1.AllowDrop = true;
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Location = new Point(3, 35);
            panel1.Name = "panel1";
            panel1.Size = new Size(215, 189);
            panel1.TabIndex = 1;
            panel1.DragDrop += panel1_DragDrop;
            panel1.DragEnter += panel1_DragEnter;
            // 
            // sizeBox
            // 
            sizeBox.BorderStyle = BorderStyle.FixedSingle;
            sizeBox.Enabled = false;
            sizeBox.Location = new Point(528, 9);
            sizeBox.Name = "sizeBox";
            sizeBox.PlaceholderText = "5";
            sizeBox.Size = new Size(60, 27);
            sizeBox.TabIndex = 2;
            sizeBox.TextChanged += sizeBox_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10F);
            label2.Location = new Point(244, 9);
            label2.Name = "label2";
            label2.Size = new Size(278, 23);
            label2.TabIndex = 3;
            label2.Text = "Grandezza del punto centrale in px";
            // 
            // button1
            // 
            button1.Location = new Point(474, 172);
            button1.Name = "button1";
            button1.Size = new Size(114, 34);
            button1.TabIndex = 4;
            button1.Text = "Avvia";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(228, 212);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(360, 15);
            progressBar1.TabIndex = 5;
            // 
            // perimetroLabel
            // 
            perimetroLabel.AutoSize = true;
            perimetroLabel.Font = new Font("Segoe UI", 12F);
            perimetroLabel.Location = new Point(421, 55);
            perimetroLabel.Name = "perimetroLabel";
            perimetroLabel.Size = new Size(75, 28);
            perimetroLabel.TabIndex = 6;
            perimetroLabel.Text = "Centro:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F);
            label3.Location = new Point(421, 111);
            label3.Name = "label3";
            label3.Size = new Size(101, 28);
            label3.TabIndex = 7;
            label3.Text = "Perimetro:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F);
            label4.Location = new Point(421, 83);
            label4.Name = "label4";
            label4.Size = new Size(56, 28);
            label4.TabIndex = 8;
            label4.Text = "Area:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 12F);
            label5.Location = new Point(528, 55);
            label5.Name = "label5";
            label5.Size = new Size(23, 28);
            label5.TabIndex = 9;
            label5.Text = "n";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 12F);
            label6.Location = new Point(528, 83);
            label6.Name = "label6";
            label6.Size = new Size(23, 28);
            label6.TabIndex = 10;
            label6.Text = "n";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 12F);
            label7.Location = new Point(528, 111);
            label7.Name = "label7";
            label7.Size = new Size(23, 28);
            label7.TabIndex = 11;
            label7.Text = "n";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(597, 236);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(perimetroLabel);
            Controls.Add(progressBar1);
            Controls.Add(button1);
            Controls.Add(label2);
            Controls.Add(sizeBox);
            Controls.Add(panel1);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Applicazione";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Panel panel1;
        private TextBox sizeBox;
        private Label label2;
        private Button button1;
        private ProgressBar progressBar1;
        private Label perimetroLabel;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
    }
}
