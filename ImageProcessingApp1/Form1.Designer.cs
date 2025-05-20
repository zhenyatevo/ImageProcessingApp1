using System;
using System.Drawing;
using System.Windows.Forms;

namespace ImageProcessingApp1
{
    partial class Form1
    {
        private Bitmap originalImage;
        private Bitmap grayscaleImage;
        private Bitmap currentImage;  // Поле для текущего обработанного изображения
        private MenuStrip menuStrip1;
        private ToolStripMenuItem openToolStripMenuItem;
        private OpenFileDialog openFileDialog1;
        private PictureBox pictureBox1; // для изображения поле
        private PictureBox pictureBox2; // для гистограммы изображение
        private Button btnBuildHistogram; //кнопка для Гистограммы
        private Label lblInfo;

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
            menuStrip1 = new MenuStrip();
            openToolStripMenuItem = new ToolStripMenuItem();
            openFileDialog1 = new OpenFileDialog();
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            btnBuildHistogram = new Button();
            lblInfo = new Label();
            trackBar1 = new TrackBar();
            label1 = new Label();
            trackBar2 = new TrackBar();
            label2 = new Label();
            trackBar3 = new TrackBar();
            label3 = new Label();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            button1 = new Button();
            label4 = new Label();
            label5 = new Label();
            button2 = new Button();
            trackBar4 = new TrackBar();
            label6 = new Label();
            trackBar5 = new TrackBar();
            label7 = new Label();
            trackBar6 = new TrackBar();
            label8 = new Label();
            trackBar7 = new TrackBar();
            trackBar8 = new TrackBar();
            label9 = new Label();
            button3 = new Button();
            button4 = new Button();
            button5 = new Button();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar6).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar7).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar8).BeginInit();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(24, 24);
            menuStrip1.Items.AddRange(new ToolStripItem[] { openToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(9, 3, 0, 3);
            menuStrip1.Size = new Size(1143, 35);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(72, 29);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            // 
            // pictureBox1
            // 
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            pictureBox1.Location = new Point(17, 45);
            pictureBox1.Margin = new Padding(4, 5, 4, 5);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(400, 400);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.BorderStyle = BorderStyle.FixedSingle;
            pictureBox2.Location = new Point(425, 45);
            pictureBox2.Margin = new Padding(4, 5, 4, 5);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(400, 400);
            pictureBox2.TabIndex = 2;
            pictureBox2.TabStop = false;
            // 
            // btnBuildHistogram
            // 
            btnBuildHistogram.Enabled = false;
            btnBuildHistogram.Location = new Point(17, 455);
            btnBuildHistogram.Margin = new Padding(4, 5, 4, 5);
            btnBuildHistogram.Name = "btnBuildHistogram";
            btnBuildHistogram.Size = new Size(223, 50);
            btnBuildHistogram.TabIndex = 3;
            btnBuildHistogram.Text = "Построить гистограмму";
            btnBuildHistogram.UseVisualStyleBackColor = true;
            btnBuildHistogram.Click += btnBuildHistogram_Click;
            // 
            // lblInfo
            // 
            lblInfo.AutoSize = true;
            lblInfo.Location = new Point(240, 700);
            lblInfo.Margin = new Padding(4, 0, 4, 0);
            lblInfo.Name = "lblInfo";
            lblInfo.Size = new Size(0, 25);
            lblInfo.TabIndex = 4;
            // 
            // trackBar1
            // 
            trackBar1.LargeChange = 10;
            trackBar1.Location = new Point(17, 513);
            trackBar1.Maximum = 255;
            trackBar1.Minimum = -255;
            trackBar1.Name = "trackBar1";
            trackBar1.Size = new Size(223, 69);
            trackBar1.TabIndex = 5;
            trackBar1.Tag = "";
            trackBar1.TickFrequency = 50;
            trackBar1.Scroll += trackBar1_Scroll;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(240, 525);
            label1.Name = "label1";
            label1.Size = new Size(76, 25);
            label1.TabIndex = 6;
            label1.Text = "яркость";
            // 
            // trackBar2
            // 
            trackBar2.LargeChange = 16;
            trackBar2.Location = new Point(17, 575);
            trackBar2.Maximum = 255;
            trackBar2.Name = "trackBar2";
            trackBar2.Size = new Size(223, 69);
            trackBar2.TabIndex = 5;
            trackBar2.TickFrequency = 16;
            trackBar2.Value = 128;
            trackBar2.Scroll += trackBar2_Scroll;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(240, 586);
            label2.Name = "label2";
            label2.Size = new Size(74, 25);
            label2.TabIndex = 8;
            label2.Text = "негатив";
            // 
            // trackBar3
            // 
            trackBar3.LargeChange = 16;
            trackBar3.Location = new Point(17, 632);
            trackBar3.Maximum = 255;
            trackBar3.Name = "trackBar3";
            trackBar3.Size = new Size(223, 69);
            trackBar3.TabIndex = 5;
            trackBar3.TickFrequency = 16;
            trackBar3.Scroll += trackBar3_Scroll;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(240, 647);
            label3.Name = "label3";
            label3.Size = new Size(246, 25);
            label3.TabIndex = 10;
            label3.Text = "пороговое преобразование";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(277, 703);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(81, 31);
            textBox1.TabIndex = 11;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(633, 703);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(81, 31);
            textBox2.TabIndex = 12;
            textBox2.TextChanged += textBox2_TextChanged;
            // 
            // button1
            // 
            button1.Location = new Point(720, 697);
            button1.Name = "button1";
            button1.Size = new Size(186, 42);
            button1.TabIndex = 13;
            button1.Text = "увеличить контраст";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(17, 703);
            label4.Name = "label4";
            label4.Size = new Size(254, 25);
            label4.TabIndex = 14;
            label4.Text = "Введите q1 из отрезка [0,255]";
            label4.Click += label4_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(364, 703);
            label5.Name = "label5";
            label5.Size = new Size(263, 25);
            label5.TabIndex = 15;
            label5.Text = "; Введите q2 из отрезка [0,255]";
            label5.Click += label5_Click;
            // 
            // button2
            // 
            button2.Location = new Point(912, 696);
            button2.Name = "button2";
            button2.Size = new Size(195, 42);
            button2.TabIndex = 16;
            button2.Text = "уменьшить контраст";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // trackBar4
            // 
            trackBar4.Location = new Point(497, 513);
            trackBar4.Maximum = 50;
            trackBar4.Minimum = 1;
            trackBar4.Name = "trackBar4";
            trackBar4.Size = new Size(217, 69);
            trackBar4.TabIndex = 17;
            trackBar4.TickFrequency = 5;
            trackBar4.Value = 10;
            trackBar4.Scroll += trackBar4_Scroll;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(733, 525);
            label6.Name = "label6";
            label6.Size = new Size(206, 25);
            label6.TabIndex = 18;
            label6.Text = "гамма преобразование";
            // 
            // trackBar5
            // 
            trackBar5.Location = new Point(497, 575);
            trackBar5.Maximum = 32;
            trackBar5.Minimum = 2;
            trackBar5.Name = "trackBar5";
            trackBar5.Size = new Size(217, 69);
            trackBar5.TabIndex = 19;
            trackBar5.TickFrequency = 2;
            trackBar5.Value = 8;
            trackBar5.Scroll += trackBar5_Scroll;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(733, 575);
            label7.Name = "label7";
            label7.Size = new Size(116, 25);
            label7.TabIndex = 20;
            label7.Text = "квантование";
            // 
            // trackBar6
            // 
            trackBar6.Location = new Point(497, 632);
            trackBar6.Maximum = 4;
            trackBar6.Name = "trackBar6";
            trackBar6.Size = new Size(217, 69);
            trackBar6.TabIndex = 21;
            trackBar6.Scroll += trackBar6_Scroll;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(733, 632);
            label8.Name = "label8";
            label8.Size = new Size(199, 25);
            label8.TabIndex = 22;
            label8.Text = "псевдораскрашивание";
            // 
            // trackBar7
            // 
            trackBar7.Location = new Point(873, 183);
            trackBar7.Maximum = 255;
            trackBar7.Name = "trackBar7";
            trackBar7.Size = new Size(156, 69);
            trackBar7.TabIndex = 23;
            trackBar7.TickFrequency = 16;
            trackBar7.Value = 128;
            trackBar7.Scroll += trackBar7_Scroll;
            // 
            // trackBar8
            // 
            trackBar8.Location = new Point(873, 248);
            trackBar8.Maximum = 50;
            trackBar8.Name = "trackBar8";
            trackBar8.Size = new Size(156, 69);
            trackBar8.TabIndex = 24;
            trackBar8.TickFrequency = 5;
            trackBar8.Value = 10;
            trackBar8.Scroll += trackBar8_Scroll;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(846, 134);
            label9.Name = "label9";
            label9.Size = new Size(273, 25);
            label9.TabIndex = 25;
            label9.Text = "соляризация: порог и диапазон";
            label9.Click += label9_Click;
            // 
            // button3
            // 
            button3.Location = new Point(846, 323);
            button3.Name = "button3";
            button3.Size = new Size(217, 34);
            button3.TabIndex = 26;
            button3.Text = "низкочастотный фильтр";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Location = new Point(846, 363);
            button4.Name = "button4";
            button4.Size = new Size(230, 34);
            button4.TabIndex = 27;
            button4.Text = "высокочастотный фильтр";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.Location = new Point(846, 403);
            button5.Name = "button5";
            button5.Size = new Size(183, 34);
            button5.TabIndex = 28;
            button5.Text = "медианный фильтр";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1143, 750);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(label9);
            Controls.Add(trackBar8);
            Controls.Add(trackBar7);
            Controls.Add(label8);
            Controls.Add(trackBar6);
            Controls.Add(label7);
            Controls.Add(trackBar5);
            Controls.Add(label6);
            Controls.Add(trackBar4);
            Controls.Add(button2);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(button1);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(label3);
            Controls.Add(trackBar3);
            Controls.Add(label2);
            Controls.Add(trackBar2);
            Controls.Add(label1);
            Controls.Add(trackBar1);
            Controls.Add(lblInfo);
            Controls.Add(btnBuildHistogram);
            Controls.Add(pictureBox2);
            Controls.Add(pictureBox1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Margin = new Padding(4, 5, 4, 5);
            Name = "Form1";
            Text = "Анализатор изображений";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar1).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar2).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar3).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar4).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar5).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar6).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar7).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar8).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private void openToolStripMenuItem_Click(object sender, EventArgs e) //загрузка изображения
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    originalImage = new Bitmap(openFileDialog1.FileName);
                    grayscaleImage = ConvertToGrayscale(originalImage);
                    currentImage = new Bitmap(grayscaleImage);
                    pictureBox1.Image = currentImage;
                    trackBar1.Value = 0; // Сброс яркости
                    btnBuildHistogram.Enabled = true;
                    lblInfo.Text = $"Размер: {originalImage.Width}x{originalImage.Height}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки изображения: " + ex.Message);
                }
            }
        }


        private Bitmap ConvertToGrayscale(Bitmap original) // GRAYSCALE
        {
            Bitmap grayscale = new Bitmap(original.Width, original.Height);

            for (int y = 0; y < original.Height; y++)
            {
                for (int x = 0; x < original.Width; x++)
                {
                    Color c = original.GetPixel(x, y);
                    byte gray = (byte)(0.3 * c.R + 0.59 * c.G + 0.11 * c.B);
                    grayscale.SetPixel(x, y, Color.FromArgb(gray, gray, gray));
                }
            }

            return grayscale;
        }


        private void btnBuildHistogram_Click(object sender, EventArgs e) // 1. обработчик кнопки для гистограммы 
        {
            if (grayscaleImage == null)
            {
                MessageBox.Show("Сначала загрузите изображение");
                return;
            }

            int[] histogram = new int[256];

            for (int y = 0; y < grayscaleImage.Height; y++)
            {
                for (int x = 0; x < grayscaleImage.Width; x++)
                {
                    int brightness = grayscaleImage.GetPixel(x, y).R;
                    histogram[brightness]++;
                }
            }

            Bitmap histogramImage = DrawHistogram(histogram, pictureBox2.Width, pictureBox2.Height);
            pictureBox2.Image = histogramImage;
        }

        private Bitmap DrawHistogram(int[] histogram, int width, int height) // отрисовка гистограммы
        {
            Bitmap bmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);

                // Находим максимальное значение для масштабирования
                int maxCount = 0;
                for (int i = 0; i < 256; i++)
                {
                    if (histogram[i] > maxCount) maxCount = histogram[i];
                }

                // Рисуем оси
                Pen axisPen = new Pen(Color.Black, 2);
                int margin = 40;
                g.DrawLine(axisPen, margin, height - margin, width - margin / 2, height - margin); // X
                g.DrawLine(axisPen, margin, height - margin, margin, margin / 2); // Y

                // Подписи осей
                Font font = new Font("Arial", 10);
                g.DrawString("Яркость", font, Brushes.Black, width / 2 - 30, height - margin / 2);
                g.DrawString("Количество", font, Brushes.Black, margin / 4, height / 2 - 30);

                // Рисуем гистограмму
                float barWidth = (width - margin * 1.5f) / 256;
                for (int i = 0; i < 256; i++)
                {
                    float barHeight = (float)histogram[i] / maxCount * (height - margin * 1.5f);
                    float x = margin + i * barWidth;
                    float y = height - margin - barHeight;

                    g.FillRectangle(Brushes.SteelBlue, x, y, barWidth, barHeight);
                    g.DrawRectangle(Pens.DarkBlue, x, y, barWidth, barHeight);
                }

                // Метки на оси X
                for (int i = 0; i <= 255; i += 32)
                {
                    float x = margin + i * barWidth;
                    g.DrawLine(Pens.Black, x, height - margin, x, height - margin + 5);
                    g.DrawString(i.ToString(), font, Brushes.Black, x - 10, height - margin + 5);
                }
            }
            return bmp;
        }

        private TrackBar trackBar1; // Яркость
        private Label label1;
        private TrackBar trackBar2;// Негатив с порогом
        private Label label2;
        private TrackBar trackBar3;// Пороговое преобразование
        private Label label3;
        private TextBox textBox1;// q1 поле вводв
        private TextBox textBox2;// q2 поле ввода
        private Button button1;// Увеличение контраста
        private Label label4;
        private Label label5;
        private Button button2;// Уменьшение контраста
        private TrackBar trackBar4;// Гамма преобразование
        private Label label6;
        private TrackBar trackBar5;// Квантование
        private Label label7;
        private TrackBar trackBar6;// Псевдораскрашивание
        private Label label8;
        private TrackBar trackBar7;// Соляризация: порог
        private TrackBar trackBar8;// Соляризация: диапазон
        private Label label9;
        private Button button3; // Низкочастотный фильтр (3 х 3)
        private Button button4; // Высокочастотный фильтр (3 х 3)
        private Button button5;
    }
}