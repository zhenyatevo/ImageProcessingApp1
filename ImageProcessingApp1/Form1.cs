using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ImageProcessingApp1
{
    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();
            InitializeTrackBar();
        }


        private void InitializeTrackBar()
        {
            trackBar7.Scroll += SolarizationParamsChanged;
            trackBar8.Scroll += SolarizationParamsChanged;
        }


        private void trackBar1_Scroll(object sender, EventArgs e) // 2. trackBar для яркости
        {
            if (grayscaleImage == null) return;

            int brightness = trackBar1.Value;
            pictureBox1.Image = AdjustBrightness(grayscaleImage, brightness);
        }


        public static Bitmap AdjustBrightness(Bitmap sourceImage, int brightnessDelta) //Яркость
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);

            // Используем Bitmap.LockBits в безопасном режиме
            BitmapData sourceData = sourceImage.LockBits(
                new Rectangle(0, 0, sourceImage.Width, sourceImage.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            BitmapData resultData = resultImage.LockBits(
                new Rectangle(0, 0, resultImage.Width, resultImage.Height),
                ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            // Создаем массивы для данных
            byte[] sourceBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] resultBuffer = new byte[resultData.Stride * resultData.Height];

            // Копируем данные изображения в массив
            System.Runtime.InteropServices.Marshal.Copy(
                sourceData.Scan0, sourceBuffer, 0, sourceBuffer.Length);

            // Обрабатываем пиксели
            for (int y = 0; y < sourceData.Height; y++)
            {
                for (int x = 0; x < sourceData.Width; x++)
                {
                    int offset = y * sourceData.Stride + x * 4; // 4 байта на пиксель (ARGB)

                    // Обрабатываем R, G, B каналы
                    for (int c = 0; c < 3; c++)
                    {
                        int newValue = sourceBuffer[offset + c] + brightnessDelta;
                        resultBuffer[offset + c] = (byte)Math.Clamp(newValue, 0, 255);
                    }

                    // Копируем альфа-канал без изменений
                    resultBuffer[offset + 3] = sourceBuffer[offset + 3];
                }
            }

            // Копируем результат обратно в изображение
            System.Runtime.InteropServices.Marshal.Copy(
                resultBuffer, 0, resultData.Scan0, resultBuffer.Length);

            // Разблокируем биты
            sourceImage.UnlockBits(sourceData);
            resultImage.UnlockBits(resultData);

            return resultImage;
        }


        private void trackBar2_Scroll(object sender, EventArgs e) // 3. trackBar для Негатива
        {

            if (grayscaleImage == null) return;


            int p = trackBar2.Value;
            grayscaleImage = ApplyNegativeEffect(grayscaleImage, p);
            pictureBox1.Image = grayscaleImage;

        }


        public static Bitmap ApplyNegativeEffect(Bitmap sourceImage, int threshold) // НЕГАТИВ
        {
            // Защита от null и проверка порога
            if (sourceImage == null || sourceImage.Width == 0 || sourceImage.Height == 0)
                return null;

            // Корректируем threshold, если он вышел за допустимые пределы
            threshold = Math.Max(0, Math.Min(255, threshold));

            try
            {
                Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);

                // Используем блокировку битов только если изображение достаточно большое
                if (sourceImage.Width * sourceImage.Height > 1000) // Порог для оптимизации
                {
                    BitmapData sourceData = null;
                    BitmapData resultData = null;

                    try
                    {
                        sourceData = sourceImage.LockBits(
                            new Rectangle(0, 0, sourceImage.Width, sourceImage.Height),
                            ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

                        resultData = resultImage.LockBits(
                            new Rectangle(0, 0, resultImage.Width, resultImage.Height),
                            ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

                        byte[] sourceBuffer = new byte[sourceData.Stride * sourceData.Height];
                        byte[] resultBuffer = new byte[resultData.Stride * resultData.Height];

                        System.Runtime.InteropServices.Marshal.Copy(sourceData.Scan0, sourceBuffer, 0, sourceBuffer.Length);

                        for (int i = 0; i < sourceBuffer.Length; i += 4)
                        {
                            for (int c = 0; c < 3; c++)
                            {
                                byte pixelValue = sourceBuffer[i + c];
                                resultBuffer[i + c] = pixelValue >= threshold ? (byte)(255 - pixelValue) : pixelValue;
                            }
                            resultBuffer[i + 3] = sourceBuffer[i + 3];
                        }

                        System.Runtime.InteropServices.Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
                    }
                    finally
                    {
                        if (sourceData != null) sourceImage.UnlockBits(sourceData);
                        if (resultData != null) resultImage.UnlockBits(resultData);
                    }
                }
                else
                {
                    //для маленьких изображений
                    for (int y = 0; y < sourceImage.Height; y++)
                    {
                        for (int x = 0; x < sourceImage.Width; x++)
                        {
                            Color pixel = sourceImage.GetPixel(x, y);
                            byte gray = (byte)(0.299 * pixel.R + 0.587 * pixel.G + 0.114 * pixel.B);
                            byte newValue = gray >= threshold ? (byte)(255 - gray) : gray;
                            resultImage.SetPixel(x, y, Color.FromArgb(pixel.A, newValue, newValue, newValue));
                        }
                    }
                }

                return resultImage;
            }
            catch
            {
                // В случае любой ошибки возвращаем null
                return null;
            }
        }


        private void trackBar3_Scroll(object sender, EventArgs e) // 4. trackBar для порогового преобразования
        {
            if (grayscaleImage == null) return;


            int p = trackBar3.Value;
            grayscaleImage = ApplyThreshold(grayscaleImage, p);
            pictureBox1.Image = grayscaleImage;
        }
        public static Bitmap ApplyThreshold(Bitmap sourceImage, int threshold)
        {
            // Проверка входных параметров
            if (sourceImage == null)
                throw new ArgumentNullException(nameof(sourceImage));

            if (threshold < 0 || threshold > 255)
                throw new ArgumentOutOfRangeException(nameof(threshold), "Порог должен быть между 0 и 255");

            // Создаем результирующее изображение
            Bitmap binaryImage = new Bitmap(sourceImage.Width, sourceImage.Height);

            // Блокируем биты для обработки
            BitmapData sourceData = sourceImage.LockBits(
                new Rectangle(0, 0, sourceImage.Width, sourceImage.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            BitmapData resultData = binaryImage.LockBits(
                new Rectangle(0, 0, binaryImage.Width, binaryImage.Height),
                ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            try
            {
                // Создаем буферы
                byte[] sourceBuffer = new byte[sourceData.Stride * sourceData.Height];
                byte[] resultBuffer = new byte[resultData.Stride * resultData.Height];

                // Копируем данные
                System.Runtime.InteropServices.Marshal.Copy(sourceData.Scan0, sourceBuffer, 0, sourceBuffer.Length);

                // Обрабатываем пиксели
                for (int i = 0; i < sourceBuffer.Length; i += 4)
                {
                    // Берем значение из синего канала (в grayscale все каналы одинаковы)
                    byte pixelValue = sourceBuffer[i];
                    byte binaryValue = pixelValue >= threshold ? (byte)255 : (byte)0;

                    // Заполняем все цветовые каналы
                    resultBuffer[i] = binaryValue; // B
                    resultBuffer[i + 1] = binaryValue; // G
                    resultBuffer[i + 2] = binaryValue; // R
                    resultBuffer[i + 3] = sourceBuffer[i + 3]; // Сохраняем альфа-канал
                }

                // Копируем результат обратно
                System.Runtime.InteropServices.Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            }
            finally
            {
                // Гарантированно разблокируем биты
                sourceImage.UnlockBits(sourceData);
                binaryImage.UnlockBits(resultData);
            }

            // Возвращаем результат (убрано изменение pictureBox1 - это должно быть в вызывающем коде)
            return binaryImage;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            ValidateInputs();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            ValidateInputs();
        }



        private void textBox1_KeyPress(object sender, KeyPressEventArgs e) // поле q1 для контраста
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }


        private void textBox2_KeyPress(object sender, KeyPressEventArgs e) // поле q2 для контраста
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }


        private void ValidateInputs()
        {

        }


        private void button1_Click(object sender, EventArgs e) // кнопка для увеличения контраста
        {
            if (grayscaleImage == null)
            {
                MessageBox.Show("Сначала загрузите изображение!");
                return;
            }

            if (!int.TryParse(textBox1.Text, out int q1) || !int.TryParse(textBox2.Text, out int q2))
            {
                MessageBox.Show("Введите корректные числовые значения для Q1 и Q2!");
                return;
            }

            if (q1 >= q2)
            {
                MessageBox.Show("Q1 должен быть меньше Q2!");
                return;
            }

            if (q1 < 0 || q2 > 255)
            {
                MessageBox.Show("Значения должны быть в диапазоне 0-255!");
                return;
            }

            try
            {
                Bitmap result = ApplyContrastStretching(grayscaleImage, q1, q2);
                pictureBox1.Image?.Dispose();
                pictureBox1.Image = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обработки: {ex.Message}");
            }
        }


        public static Bitmap ApplyContrastStretching(Bitmap sourceImage, int q1, int q2) // 5. УВЕЛИЧЕНИЕ КОНТРАСТА
        {
            if (sourceImage == null)
                throw new ArgumentNullException(nameof(sourceImage));

            if (q1 >= q2 || q1 < 0 || q2 > 255)
                throw new ArgumentException("Некорректные значения Q1 или Q2");

            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            double scale = 255.0 / (q2 - q1);

            BitmapData sourceData = sourceImage.LockBits(
                new Rectangle(0, 0, sourceImage.Width, sourceImage.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            BitmapData resultData = resultImage.LockBits(
                new Rectangle(0, 0, resultImage.Width, resultImage.Height),
                ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            try
            {
                int bytes = Math.Abs(sourceData.Stride) * sourceData.Height;
                byte[] sourceBuffer = new byte[bytes];
                byte[] resultBuffer = new byte[bytes];

                System.Runtime.InteropServices.Marshal.Copy(sourceData.Scan0, sourceBuffer, 0, bytes);

                for (int i = 0; i < bytes; i += 4)
                {
                    for (int c = 0; c < 3; c++) // Обрабатываем только R, G, B компоненты (не Alpha)
                    {
                        int pixelValue = sourceBuffer[i + c];
                        int stretchedValue;

                        if (pixelValue <= q1)
                            stretchedValue = 0;
                        else if (pixelValue >= q2)
                            stretchedValue = 255;
                        else
                            stretchedValue = (int)((pixelValue - q1) * scale);

                        resultBuffer[i + c] = (byte)Math.Clamp(stretchedValue, 0, 255);
                    }
                    resultBuffer[i + 3] = sourceBuffer[i + 3]; // Сохраняем альфа-канал без изменений
                }

                System.Runtime.InteropServices.Marshal.Copy(resultBuffer, 0, resultData.Scan0, bytes);
            }
            finally
            {
                sourceImage.UnlockBits(sourceData);
                resultImage.UnlockBits(resultData);
            }

            return resultImage;
        }


        private void button2_Click(object sender, EventArgs e) // кнопка для уменьшения контраста
        {
            if (grayscaleImage == null)
            {
                MessageBox.Show("Сначала загрузите изображение!");
                return;
            }

            if (!int.TryParse(textBox1.Text, out int q1) || !int.TryParse(textBox2.Text, out int q2))
            {
                MessageBox.Show("Введите корректные числовые значения для Q1 и Q2!");
                return;
            }

            if (q1 >= q2)
            {
                MessageBox.Show("Q1 должен быть меньше Q2!");
                return;
            }

            if (q1 < 0 || q2 > 255)
            {
                MessageBox.Show("Значения должны быть в диапазоне 0-255!");
                return;
            }

            try
            {
                Bitmap result = DecreaseContrast(grayscaleImage, q1, q2);
                pictureBox1.Image?.Dispose();
                pictureBox1.Image = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обработки: {ex.Message}");
            }
        }

        public static Bitmap DecreaseContrast(Bitmap sourceImage, int q1, int q2) // 6. УМЕНЬШЕНИЕ КОНТРАСТА
        {
            if (sourceImage == null)
                throw new ArgumentNullException(nameof(sourceImage));

            if (q1 >= q2 || q1 < 0 || q2 > 255)
                throw new ArgumentException("Некорректные значения Q1 или Q2");

            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            double scale = (q2 - q1) / 255.0;

            BitmapData sourceData = sourceImage.LockBits(
                new Rectangle(0, 0, sourceImage.Width, sourceImage.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            BitmapData resultData = resultImage.LockBits(
                new Rectangle(0, 0, resultImage.Width, resultImage.Height),
                ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            try
            {
                int bytes = Math.Abs(sourceData.Stride) * sourceData.Height;
                byte[] sourceBuffer = new byte[bytes];
                byte[] resultBuffer = new byte[bytes];

                System.Runtime.InteropServices.Marshal.Copy(sourceData.Scan0, sourceBuffer, 0, bytes);

                for (int i = 0; i < bytes; i += 4)
                {
                    for (int c = 0; c < 3; c++)
                    {
                        int pixelValue = sourceBuffer[i + c];
                        int compressedValue = q1 + (int)(pixelValue * scale);
                        resultBuffer[i + c] = (byte)Math.Clamp(compressedValue, q1, q2);
                    }
                    resultBuffer[i + 3] = sourceBuffer[i + 3];
                }

                System.Runtime.InteropServices.Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            }
            finally
            {
                sourceImage.UnlockBits(sourceData);
                resultImage.UnlockBits(resultData);
            }

            return resultImage;
        }


        private void trackBar4_Scroll(object sender, EventArgs e) // trackbar для гамма-преобразования
        {
            if (grayscaleImage == null) return;

            ApplyGammaCorrection();
        }


        private double GetCurrentGammaValue()
        {
            return trackBar4.Value / 10.0; // Преобразуем в диапазон 0.1-5.0
        }


        private void ApplyGammaCorrection()
        {
            try
            {
                double gamma = GetCurrentGammaValue();
                Bitmap result = ApplyGamma(grayscaleImage, gamma);
                pictureBox1.Image?.Dispose();
                pictureBox1.Image = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка гамма-коррекции: {ex.Message}");
            }
        }


        public static Bitmap ApplyGamma(Bitmap sourceImage, double gamma) // 7. GAMMA ПРЕОБРАЗОВАНИЕ
        {
            if (sourceImage == null)
                throw new ArgumentNullException(nameof(sourceImage));

            if (gamma <= 0)
                throw new ArgumentException("Гамма должна быть положительной");

            // Предварительно вычисляем таблицу преобразования для оптимизации
            byte[] gammaTable = new byte[256];
            for (int i = 0; i < 256; i++)
            {
                gammaTable[i] = (byte)Math.Clamp(255 * Math.Pow(i / 255.0, gamma), 0, 255);
            }

            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);

            BitmapData sourceData = sourceImage.LockBits(
                new Rectangle(0, 0, sourceImage.Width, sourceImage.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            BitmapData resultData = resultImage.LockBits(
                new Rectangle(0, 0, resultImage.Width, resultImage.Height),
                ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            try
            {
                int bytes = Math.Abs(sourceData.Stride) * sourceData.Height;
                byte[] sourceBuffer = new byte[bytes];
                byte[] resultBuffer = new byte[bytes];

                Marshal.Copy(sourceData.Scan0, sourceBuffer, 0, bytes);

                for (int i = 0; i < bytes; i += 4)
                {
                    for (int c = 0; c < 3; c++) // RGB каналы
                    {
                        resultBuffer[i + c] = gammaTable[sourceBuffer[i + c]];
                    }
                    resultBuffer[i + 3] = sourceBuffer[i + 3]; // Альфа-канал
                }

                Marshal.Copy(resultBuffer, 0, resultData.Scan0, bytes);
            }
            finally
            {
                sourceImage.UnlockBits(sourceData);
                resultImage.UnlockBits(resultData);
            }

            return resultImage;
        }


        private void trackBar5_Scroll(object sender, EventArgs e) // trackbar для квантования
        {
            if (grayscaleImage == null)
            {
                MessageBox.Show("Сначала загрузите изображение!");
                return;
            }

            // Применяем квантование
            Bitmap quantized = ApplyQuantization(grayscaleImage, trackBar5.Value);

            // Отображаем результат
            pictureBox1.Image?.Dispose();
            pictureBox1.Image = quantized;
        }


        private Bitmap ApplyQuantization(Bitmap source, int levels) // 8. КВАНТОВАНИЕ
        {
            if (source == null) return null;

            // Корректируем количество уровней
            levels = Math.Clamp(levels, 2, 256);

            // Создаем lookup-таблицу
            double step = 255.0 / (levels - 1);
            byte[] lut = new byte[256];
            for (int i = 0; i < 256; i++)
            {
                lut[i] = (byte)(Math.Round(i / step) * step);
            }

            Bitmap result = new Bitmap(source.Width, source.Height);

            // Блокируем биты для обработки
            BitmapData sourceData = source.LockBits(
                new Rectangle(0, 0, source.Width, source.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            BitmapData resultData = result.LockBits(
                new Rectangle(0, 0, result.Width, result.Height),
                ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            try
            {
                int bufferSize = sourceData.Stride * sourceData.Height;
                byte[] sourceBuffer = new byte[bufferSize];
                byte[] resultBuffer = new byte[bufferSize];

                // Копируем данные изображения
                System.Runtime.InteropServices.Marshal.Copy(sourceData.Scan0, sourceBuffer, 0, bufferSize);

                // Обрабатываем пиксели
                for (int y = 0; y < sourceData.Height; y++)
                {
                    int rowOffset = y * sourceData.Stride;
                    for (int x = 0; x < sourceData.Width; x++)
                    {
                        int pixelOffset = rowOffset + x * 3;
                        byte grayValue = sourceBuffer[pixelOffset];
                        byte quantizedValue = lut[grayValue];

                        resultBuffer[pixelOffset] = quantizedValue;     // B
                        resultBuffer[pixelOffset + 1] = quantizedValue; // G
                        resultBuffer[pixelOffset + 2] = quantizedValue; // R
                    }
                }

                // Копируем результат обратно
                System.Runtime.InteropServices.Marshal.Copy(resultBuffer, 0, resultData.Scan0, bufferSize);
            }
            finally
            {
                source.UnlockBits(sourceData);
                result.UnlockBits(resultData);
            }

            return result;
        }

        private void trackBar6_Scroll(object sender, EventArgs e) // trackbar для псевдораскрашивания
        {
            if (grayscaleImage == null)
            {
                MessageBox.Show("Сначала загрузите изображение!");
                return;
            }

            int schemeIndex = trackBar6.Value;
            Bitmap colored = ApplyColoringScheme(grayscaleImage, schemeIndex);

            pictureBox1.Image?.Dispose(); // Освобождаем предыдущее изображение
            pictureBox1.Image = colored;
        }


        public Bitmap ApplyColoringScheme(Bitmap grayscaleImage, int schemeIndex) // 9. ПСЕВДОРАСКРАШИВАНИЕ
        {
            // Определяем различные цветовые схемы
            Color[][] colorSchemes = new Color[][]
            {
        // Схема 0: Синий-Голубой-Зеленый-Желтый-Красный (термальная)
        new Color[]
        {
            Color.FromArgb(0, 0, 128),     // Тёмно-синий
            Color.FromArgb(0, 128, 255),    // Голубой
            Color.FromArgb(0, 255, 0),      // Зелёный
            Color.FromArgb(255, 255, 0),    // Жёлтый
            Color.FromArgb(255, 0, 0)       // Красный
        },
        // Схема 1: Фиолетовый-Розовый-Оранжевый (закатная)
        new Color[]
        {
            Color.FromArgb(25, 0, 51),
            Color.FromArgb(102, 0, 102),
            Color.FromArgb(204, 0, 102),
            Color.FromArgb(255, 102, 0),
            Color.FromArgb(255, 204, 0)
        },
        // Схема 2: Зеленый-Желтый (весенняя)
        new Color[]
        {
            Color.FromArgb(0, 64, 0),
            Color.FromArgb(0, 128, 0),
            Color.FromArgb(128, 192, 0),
            Color.FromArgb(255, 255, 0),
            Color.FromArgb(255, 255, 128)
        },
        // Схема 3: Серый-Красный (предупреждающая)
        new Color[]
        {
            Color.FromArgb(64, 64, 64),
            Color.FromArgb(128, 128, 128),
            Color.FromArgb(192, 64, 64),
            Color.FromArgb(255, 0, 0),
            Color.FromArgb(255, 128, 128)
        },
        // Схема 4: Радужная
        new Color[]
        {
            Color.FromArgb(148, 0, 211), // Фиолетовый
            Color.FromArgb(75, 0, 130),   // Индиго
            Color.FromArgb(0, 0, 255),    // Синий
            Color.FromArgb(0, 255, 0),    // Зеленый
            Color.FromArgb(255, 255, 0),  // Желтый
            Color.FromArgb(255, 127, 0),  // Оранжевый
            Color.FromArgb(255, 0, 0)     // Красный
        }
            };

            // Проверка и корректировка индекса схемы
            schemeIndex = Math.Clamp(schemeIndex, 0, colorSchemes.Length - 1);
            Color[] colorMap = colorSchemes[schemeIndex];

            Bitmap colorImage = new Bitmap(grayscaleImage.Width, grayscaleImage.Height);

            // Быстрая обработка через LockBits
            BitmapData srcData = grayscaleImage.LockBits(
                new Rectangle(0, 0, grayscaleImage.Width, grayscaleImage.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            BitmapData dstData = colorImage.LockBits(
                new Rectangle(0, 0, colorImage.Width, colorImage.Height),
                ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            try
            {
                int srcBytes = srcData.Stride * srcData.Height;
                byte[] srcBuffer = new byte[srcBytes];
                System.Runtime.InteropServices.Marshal.Copy(srcData.Scan0, srcBuffer, 0, srcBytes);

                int dstBytes = dstData.Stride * dstData.Height;
                byte[] dstBuffer = new byte[dstBytes];

                for (int y = 0; y < srcData.Height; y++)
                {
                    for (int x = 0; x < srcData.Width; x++)
                    {
                        int srcPos = y * srcData.Stride + x * 3;
                        int dstPos = y * dstData.Stride + x * 4;

                        byte grayValue = srcBuffer[srcPos]; // Для grayscale R=G=B, берем любой канал

                        // Выбираем цвет из карты
                        int colorIndex = grayValue * (colorMap.Length - 1) / 255;
                        Color color = colorMap[colorIndex];

                        // Записываем цвет в 32-битное изображение (ARGB)
                        dstBuffer[dstPos] = color.B;     // Blue
                        dstBuffer[dstPos + 1] = color.G; // Green
                        dstBuffer[dstPos + 2] = color.R; // Red
                        dstBuffer[dstPos + 3] = 255;    // Alpha (непрозрачность)
                    }
                }

                System.Runtime.InteropServices.Marshal.Copy(dstBuffer, 0, dstData.Scan0, dstBytes);
            }
            finally
            {
                grayscaleImage.UnlockBits(srcData);
                colorImage.UnlockBits(dstData);
            }

            return colorImage;
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }
        private void trackBar7_Scroll(object sender, EventArgs e)
        {

        }
        private void trackBar8_Scroll(object sender, EventArgs e)
        {

        }
        private void SolarizationParamsChanged(object sender, EventArgs e) // обработчик для trackbars соляризации
        {
            if (grayscaleImage == null) return;

            int threshold = trackBar7.Value;
            int range = trackBar8.Value;

            Bitmap result = ApplyAdvancedSolarization(grayscaleImage, threshold, range);

            pictureBox1.Image?.Dispose();
            pictureBox1.Image = result;
        }


        public Bitmap ApplyAdvancedSolarization(Bitmap source, int threshold, int range) // 10. СОЛЯРИЗАЦИЯ
        {
            Bitmap result = new Bitmap(source.Width, source.Height);

            BitmapData srcData = source.LockBits(
                new Rectangle(0, 0, source.Width, source.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            BitmapData resData = result.LockBits(
                new Rectangle(0, 0, result.Width, result.Height),
                ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            int bytes = Math.Abs(srcData.Stride) * srcData.Height;
            byte[] buffer = new byte[bytes];

            Marshal.Copy(srcData.Scan0, buffer, 0, bytes);

            for (int i = 0; i < bytes; i++)
            {
                int value = buffer[i];
                int distance = Math.Abs(value - threshold);

                if (distance < range)
                {
                    float factor = 0.5f * (1 + MathF.Sin(MathF.PI * (value - threshold) / (2 * range)));
                    buffer[i] = (byte)(value * factor + (255 - value) * (1 - factor));
                }
                else if (value > threshold)
                {
                    buffer[i] = (byte)(255 - value);
                }
            }

            Marshal.Copy(buffer, 0, resData.Scan0, bytes);

            source.UnlockBits(srcData);
            result.UnlockBits(resData);

            return result;
        }

        private void button3_Click(object sender, EventArgs e) // обработчик низкочастотного фильтра
        {
            if (grayscaleImage == null)
            {
                MessageBox.Show("Сначала загрузите изображение!");
                return;
            }

            // Создаем диалог для выбора типа фильтра
            using (var filterDialog = new Form())
            {
                filterDialog.Text = "Выберите тип фильтра";
                filterDialog.StartPosition = FormStartPosition.CenterParent;
                filterDialog.Width = 400;
                filterDialog.Height = 400;

                var rbH1 = new RadioButton { Text = "Простое усреднение (H₁)", Top = 20, Left = 20, Checked = true };
                var rbH2 = new RadioButton { Text = "Усиленный центр (H₂)", Top = 50, Left = 20 };
                var rbH3 = new RadioButton { Text = "Гауссово ядро (H₃)", Top = 80, Left = 20 };

                var btnOk = new Button { Text = "Применить", Top = 120, Left = 20, DialogResult = DialogResult.OK };
                var btnCancel = new Button { Text = "Отмена", Top = 120, Left = 120, DialogResult = DialogResult.Cancel };

                filterDialog.Controls.AddRange(new Control[] { rbH1, rbH2, rbH3, btnOk, btnCancel });

                if (filterDialog.ShowDialog(this) == DialogResult.OK)
                {
                    KernelType kernelType = KernelType.H1;
                    if (rbH2.Checked) kernelType = KernelType.H2;
                    else if (rbH3.Checked) kernelType = KernelType.H3;

                    // Применяем выбранный фильтр
                    Bitmap lowpassFilter = ApplyLowPassFilter(grayscaleImage, kernelType);

                    // Отображаем результат
                    pictureBox1.Image?.Dispose();
                    pictureBox1.Image = lowpassFilter;
                }
            }
        }

        public enum KernelType { H1, H2, H3 }


        public static Bitmap ApplyLowPassFilter(Bitmap source, KernelType kernelType = KernelType.H1) // 11. НИЗКОЧАСТОТНЫЙ ФИЛЬТР
        {
            Bitmap filteredImage = new Bitmap(source.Width, source.Height);
            float[,] kernel = GetKernel(kernelType);
            int radius = 1; // Радиус для ядра 3x3

            // Обрабатываем все пиксели, включая граничные (с зеркальным отражением)
            for (int y = 0; y < source.Height; y++)
            {
                for (int x = 0; x < source.Width; x++)
                {
                    float r = 0, g = 0, b = 0;

                    // Применяем ядро фильтра
                    for (int ky = -radius; ky <= radius; ky++)
                    {
                        for (int kx = -radius; kx <= radius; kx++)
                        {
                            // Обработка граничных пикселей через зеркальное отражение
                            int px = Math.Clamp(x + kx, 0, source.Width - 1);
                            int py = Math.Clamp(y + ky, 0, source.Height - 1);

                            Color pixel = source.GetPixel(px, py);
                            float weight = kernel[ky + radius, kx + radius];

                            r += pixel.R * weight;
                            g += pixel.G * weight;
                            b += pixel.B * weight;
                        }
                    }

                    // Обрезаем значения до допустимого диапазона
                    r = Math.Clamp(r, 0, 255);
                    g = Math.Clamp(g, 0, 255);
                    b = Math.Clamp(b, 0, 255);

                    filteredImage.SetPixel(x, y, Color.FromArgb((int)r, (int)g, (int)b));
                }
            }

            return filteredImage;
        }


        private static float[,] GetKernel(KernelType type) // вспомогательня ф-я с фильтрами
        {
            return type switch
            {
                KernelType.H1 => new float[,]
                {
            { 1f/9, 1f/9, 1f/9 },
            { 1f/9, 1f/9, 1f/9 },
            { 1f/9, 1f/9, 1f/9 }
                },

                KernelType.H2 => new float[,]
                {
            { 1f/10, 1f/10, 1f/10 },
            { 1f/10, 2f/10, 1f/10 },
            { 1f/10, 1f/10, 1f/10 }
                },

                KernelType.H3 => new float[,]
                {
            { 1f/16, 2f/16, 1f/16 },
            { 2f/16, 4f/16, 2f/16 },
            { 1f/16, 2f/16, 1f/16 }
                },

                _ => throw new ArgumentException("Unknown kernel type")
            };
        }


        private void button4_Click(object sender, EventArgs e) // button вызова высокочастотного фильтра
        {
            if (grayscaleImage == null)
            {
                MessageBox.Show("Сначала загрузите изображение!");
                return;
            }

            // Создаем диалог для выбора типа фильтра
            using (var filterDialog = new Form())
            {
                filterDialog.Text = "Выберите тип высокочастотного фильтра";
                filterDialog.StartPosition = FormStartPosition.CenterParent;
                filterDialog.Width = 400;
                filterDialog.Height = 400;

                var rbH1 = new RadioButton { Text = "Фильтр резкости (H₁)", Top = 20, Left = 20, Checked = true };
                var rbH2 = new RadioButton { Text = "Лапласиан (H₂)", Top = 50, Left = 20 };
                var rbH3 = new RadioButton { Text = "Усиленный лапласиан (H₃)", Top = 80, Left = 20 };

                var btnOk = new Button { Text = "Применить", Top = 140, Left = 20, DialogResult = DialogResult.OK };
                var btnCancel = new Button { Text = "Отмена", Top = 140, Left = 120, DialogResult = DialogResult.Cancel };

                filterDialog.Controls.AddRange(new Control[] { rbH1, rbH2, rbH3, btnOk, btnCancel });

                if (filterDialog.ShowDialog(this) == DialogResult.OK)
                {
                    HighPassKernelType kernelType = HighPassKernelType.H1;
                    if (rbH2.Checked) kernelType = HighPassKernelType.H2;
                    else if (rbH3.Checked) kernelType = HighPassKernelType.H3;

                    // Применяем выбранный фильтр
                    Bitmap highpassFilter = ApplyHighPassFilter(grayscaleImage, kernelType);

                    // Отображаем результат
                    pictureBox1.Image?.Dispose();
                    pictureBox1.Image = highpassFilter;
                }
            }
        }


        public enum HighPassKernelType { H1, H2, H3 }



        public static Bitmap ApplyHighPassFilter(Bitmap source, HighPassKernelType kernelType = HighPassKernelType.H1) // 12. ВЫСОКОЧАСТОТНЫЙ ФИЛЬТР
        {
            Bitmap filteredImage = new Bitmap(source.Width, source.Height);
            int[,] kernel = GetHighPassKernel(kernelType);
            int radius = 1; // Радиус для ядра 3x3

            for (int y = 0; y < source.Height; y++)
            {
                for (int x = 0; x < source.Width; x++)
                {
                    int sum = 0;

                    // Применяем ядро фильтра
                    for (int ky = -radius; ky <= radius; ky++)
                    {
                        for (int kx = -radius; kx <= radius; kx++)
                        {
                            // Обработка граничных пикселей через зеркальное отражение
                            int px = Math.Clamp(x + kx, 0, source.Width - 1);
                            int py = Math.Clamp(y + ky, 0, source.Height - 1);

                            Color pixel = source.GetPixel(px, py);
                            int grayValue = (int)(pixel.R * 0.299 + pixel.G * 0.587 + pixel.B * 0.114);
                            int weight = kernel[ky + radius, kx + radius];

                            sum += grayValue * weight;
                        }
                    }

                    // Обрезаем значения до допустимого диапазона (0-255)
                    sum = Math.Clamp(sum, 0, 255);

                    // Для высокочастотного фильтра используем только яркость
                    filteredImage.SetPixel(x, y, Color.FromArgb(sum, sum, sum));
                }
            }

            return filteredImage;
        }

        private static int[,] GetHighPassKernel(HighPassKernelType type) // вспомогательня ф-я с фильтрами
        {
            return type switch
            {
                HighPassKernelType.H1 => new int[,]
                {
            { -1, -1, -1 },
            { -1,  9, -1 },
            { -1, -1, -1 }
                },

                HighPassKernelType.H2 => new int[,]
                {
            {  0, -1,  0 },
            { -1,  5, -1 },
            {  0, -1,  0 }
                },

                HighPassKernelType.H3 => new int[,]
                {
            {  1, -2,  1 },
            { -2,  5, -2 },
            {  1, -2,  1 }
                },

                _ => throw new ArgumentException("Unknown kernel type")
            };
        }

        private void button5_Click(object sender, EventArgs e) // button вызова медианного фиьтра
        {
            if (grayscaleImage == null)
            {
                MessageBox.Show("Сначала загрузите изображение!");
                return;
            }

            // Создаем диалог для выбора размера окна
            using (var sizeDialog = new Form())
            {
                sizeDialog.Text = "Выберите размер окна медианного фильтра";
                sizeDialog.StartPosition = FormStartPosition.CenterParent;
                sizeDialog.Width = 400;
                sizeDialog.Height = 400;

                var rb3x3 = new RadioButton { Text = "3×3", Top = 20, Left = 20, Checked = true };
                var rb5x5 = new RadioButton { Text = "5×5", Top = 50, Left = 20 };
                var rb7x7 = new RadioButton { Text = "7×7", Top = 80, Left = 20 };
                var rb9x9 = new RadioButton { Text = "9×9", Top = 110, Left = 20 };

                var btnOk = new Button { Text = "Применить", Top = 140, Left = 20, DialogResult = DialogResult.OK };
                var btnCancel = new Button { Text = "Отмена", Top = 140, Left = 120, DialogResult = DialogResult.Cancel };

                sizeDialog.Controls.AddRange(new Control[] { rb3x3, rb5x5, rb7x7, rb9x9, btnOk, btnCancel });

                if (sizeDialog.ShowDialog(this) == DialogResult.OK)
                {
                    int windowSize = 3;
                    if (rb5x5.Checked) windowSize = 5;
                    else if (rb7x7.Checked) windowSize = 7;
                    else if (rb9x9.Checked) windowSize = 9;

                    // Применяем медианный фильтр
                    Bitmap medianFiltered = ApplyMedianFilter(grayscaleImage, windowSize);

                    // Отображаем результат
                    pictureBox1.Image?.Dispose();
                    pictureBox1.Image = medianFiltered;
                }
            }
        }


        public static Bitmap ApplyMedianFilter(Bitmap source, int windowSize) // 13. МЕДИАННЫЙ ФИЛЬТР
        {
            Bitmap filteredImage = new Bitmap(source.Width, source.Height);
            int radius = windowSize / 2;
            List<int> neighbors = new List<int>(windowSize * windowSize);

            for (int y = 0; y < source.Height; y++)
            {
                for (int x = 0; x < source.Width; x++)
                {
                    neighbors.Clear();

                    // Собираем соседей в окрестности
                    for (int ky = -radius; ky <= radius; ky++)
                    {
                        for (int kx = -radius; kx <= radius; kx++)
                        {
                            // Обработка граничных пикселей через зеркальное отражение
                            int px = Math.Clamp(x + kx, 0, source.Width - 1);
                            int py = Math.Clamp(y + ky, 0, source.Height - 1);

                            Color pixel = source.GetPixel(px, py);
                            int grayValue = (int)(pixel.R * 0.299 + pixel.G * 0.587 + pixel.B * 0.114);
                            neighbors.Add(grayValue);
                        }
                    }

                    // Сортируем и находим медиану
                    neighbors.Sort();
                    int median = neighbors[neighbors.Count / 2];

                    // Устанавливаем медианное значение
                    filteredImage.SetPixel(x, y, Color.FromArgb(median, median, median));
                }
            }

            return filteredImage;
        }
    }

}