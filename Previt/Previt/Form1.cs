using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Previt
{
    public partial class Form1 : Form
    {
        private int[,] xKernel = new int[,]
        {
            { -1, 0, 1 },
            { -1, 0, 1 },
            { -1, 0, 1 }
        };

        private int[,] yKernel = new int[,]
        {
            { -1, -1, -1 },
            { 0, 0, 0 },
            { 1, 1, 1 }
        };

        public Form1()
        {
            InitializeComponent();
            trackBar1.Minimum = 1;
            trackBar1.Maximum = 255;
            trackBar1.Value = 80;
            trackBar1.TickFrequency = 1;
            trackBar1.SmallChange = 1;
            trackBar1.LargeChange = 1;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(openFileDialog.FileName);
                detectEdges(trackBar1.Value);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    pictureBox2.Image.Save(saveFileDialog.FileName);
                    MessageBox.Show("Image saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while saving the image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void detectEdges(int magnitudeLimit)
        {
                if (pictureBox1.Image != null)
                {
                    Bitmap originalImage = (Bitmap)pictureBox1.Image;
                    Bitmap resultImage = new Bitmap(originalImage.Width, originalImage.Height);

                    for (int y = 1; y < originalImage.Height - 1; y++)
                    {
                        for (int x = 1; x < originalImage.Width - 1; x++)
                        {
                            int gx = 0, gy = 0;

                            for (int j = -1; j <= 1; j++)
                            {
                                for (int i = -1; i <= 1; i++)
                                {
                                    Color pixel = originalImage.GetPixel(x + i, y + j);
                                    int r = pixel.R;

                                    gx += xKernel[i + 1, j + 1] * r;
                                    gy += yKernel[i + 1, j + 1] * r;
                                }
                            }

                            int magnitude = (int)Math.Sqrt(gx * gx + gy * gy);
                            magnitude = Math.Min(magnitudeLimit, magnitude);

                            Color newPixel = Color.FromArgb(magnitude, magnitude, magnitude);
                            resultImage.SetPixel(x, y, newPixel);
                        }
                    }

                    pictureBox2.Image = resultImage;
                }
            }
            private void trackBar1_Scroll(object sender, EventArgs e)
        {
            detectEdges(trackBar1.Value);
            label4.Text = trackBar1.Value.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
        }
    }
}



