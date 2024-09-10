using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace PhotoEditor00002
{
    public partial class myHistogramForm : Form
    {
        #region parameterDef
        string activeFileNamePath;
        public Bitmap activeBitmap;
//        Bitmap histogramBitmap;
        int[] alphaArray = new int[256];
        int[] redArray = new int[256];
        int[] greenArray = new int[256];
        int[] blueArray = new int[256];
        int[] grayArray = new int[256];
        int setThresHold = 0;
        public int setMinLimit = 0;
        public int setMaxLimit = 255;
        int calcMaxValue = 0;
        public bool validImage = false;
        #endregion
        // --- --- Support methods --- ---
        private void drawHistogram()
        {
            histogramChart.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
            histogramChart.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;
            histogramChart.ChartAreas[0].AxisX.Interval = 20;
            histogramChart.ChartAreas[0].AxisY2.Enabled = AxisEnabled.False;
            if (redChRB.Checked)
                {
                    try { histogramChart.Series.Clear(); } catch { }
                    var dataPointSeriesRed = new Series
                    {
                        Name = "Red Channel Histogram",
                        Color = Color.Red,
                        ChartType = SeriesChartType.Line
                    };
                    for (int i = 0; i < 256; i++)
                    {
                        dataPointSeriesRed.Points.AddXY(i, redArray[i]);
                    }
                    //redChRB.Checked = false;
                    greenChRB.Checked = false;
                    blueChRB.Checked = false;
                    grayRB.Checked = false;
                    alphaChRB.Checked = false;
                    histogramChart.Series.Add(dataPointSeriesRed);
                    histogramChart.Series["Red Channel Histogram"]["PixelPointWidth"] = "1";
                }
            if (greenChRB.Checked)
                {
                    try { histogramChart.Series.Clear(); } catch { }
                    var dataPointSeriesGreen = new Series
                    {
                        Name = "Green Channel Histogram",
                        Color = Color.Green,
                        ChartType = SeriesChartType.Line
                    };
                    for (int i = 0; i < 256; i++)
                    {
                        dataPointSeriesGreen.Points.AddXY(i, greenArray[i]);
                    }
                    redChRB.Checked = false;
                    //greenChRB.Checked = false;
                    blueChRB.Checked = false;
                    grayRB.Checked = false;
                    alphaChRB.Checked = false;
                    histogramChart.Series.Add(dataPointSeriesGreen);
                    histogramChart.Series["Green Channel Histogram"]["PixelPointWidth"] = "1";
                }
            if (blueChRB.Checked)
                {
                    try { histogramChart.Series.Clear(); } catch { }
                    var dataPointSeriesBlue = new Series
                    {
                        Name = "Blue Channel Histogram",
                        Color = Color.Blue,
                        ChartType = SeriesChartType.Line
                    };
                    for (int i = 0; i < 256; i++)
                    {
                        dataPointSeriesBlue.Points.AddXY(i, blueArray[i]);
                    }
                    redChRB.Checked = false;
                    greenChRB.Checked = false;
                    //blueChRB.Checked = false;
                    grayRB.Checked = false;
                    alphaChRB.Checked = false;
                    histogramChart.Series.Add(dataPointSeriesBlue);
                    histogramChart.Series["Blue Channel Histogram"]["PixelPointWidth"] = "1";
                }
            if (grayRB.Checked)
                {
                    try { histogramChart.Series.Clear(); } catch { }
                    var dataPointSeriesGray = new Series
                    {
                        Name = "Gray Channel Histogram",
                        Color = Color.DarkGray,
                        ChartType = SeriesChartType.Line
                    };
                    for (int i = 0; i < 256; i++)
                    {
                        dataPointSeriesGray.Points.AddXY(i, grayArray[i]);
                    }
                    redChRB.Checked = false;
                    greenChRB.Checked = false;
                    blueChRB.Checked = false;
                    //grayRB.Checked = false;
                    alphaChRB.Checked = false;
                    histogramChart.Series.Add(dataPointSeriesGray);
                    histogramChart.Series["Gray Channel Histogram"]["PixelPointWidth"] = "1";
                }
            if (alphaChRB.Checked)
                {
                    try { histogramChart.Series.Clear(); } catch { }
                    var dataPointSeriesAlpha = new Series
                    {
                        Name = "Alpha Channel Histogram",
                        Color = Color.Black,
                        ChartType = SeriesChartType.Line
                    };
                    for (int i = 0; i < 256; i++)
                    {
                        dataPointSeriesAlpha.Points.AddXY(i, alphaArray[i]);
                    }
                    redChRB.Checked = false;
                    greenChRB.Checked = false;
                    blueChRB.Checked = false;
                    grayRB.Checked = false;
                    //alphaChRB.Checked = false;
                    histogramChart.Series.Add(dataPointSeriesAlpha);
                    histogramChart.Series["Alpha Channel Histogram"]["PixelPointWidth"] = "1";
                }
        }
        public void histogramFilter()
        {
            Bitmap bmp = new Bitmap(activeFileNamePath);
            Color pixel;
            float amplifier = (float)((float)255 / (setMaxLimit - setMinLimit));
            if (bmp.Height > bmp.Width)
            {
                for (int i = 0; i <= bmp.Width - 1; i++)
                {
                    for (int j = 0; j < bmp.Height - 1; j++)
                    {
                        pixel = bmp.GetPixel(i, j);
                        int crv = Math.Max(0, Math.Min(255, Convert.ToInt32(pixel.R * amplifier)));
                        int cgv = Math.Max(0, Math.Min(255, Convert.ToInt32(pixel.G * amplifier)));
                        int cbv = Math.Max(0, Math.Min(255, Convert.ToInt32(pixel.B * amplifier)));
                        Color newPixel = Color.FromArgb(pixel.A, crv, cgv, cbv);
                        try { bmp.SetPixel(i, j, newPixel); } catch { }
                    }
                }
            }
            else
            {
                for (int i = 0; i < bmp.Height; i++)
                {
                    for (int j = 0; j < bmp.Width; j++)
                    {
                        pixel = bmp.GetPixel(j, i);
                        int crv = Math.Max(0, Math.Min(255, Convert.ToInt32(pixel.R * amplifier)));
                        int cgv = Math.Max(0, Math.Min(255, Convert.ToInt32(pixel.G * amplifier)));
                        int cbv = Math.Max(0, Math.Min(255, Convert.ToInt32(pixel.B * amplifier)));
                        Color newPixel = Color.FromArgb(pixel.A, crv, cgv, cbv);
                        try { bmp.SetPixel(j, i, newPixel); } catch { }
                    }
                }
            }
            imagePicBx.SizeMode = PictureBoxSizeMode.Zoom;
            imagePicBx.Image = bmp;
        }
        private void calcLimitations()
        {
            for (int i = 0; i < 256; i++)
            {
                calcMaxValue = Math.Max(calcMaxValue, grayArray[i]);
                if ((setMinLimit == 0) && (grayArray[i] >= setThresHold))
                    setMinLimit = i;
                if ((setMaxLimit == 256) && (grayArray[255 - i] >= setThresHold))
                    setMaxLimit = 255 - i;
                minValNUD.Value = setMinLimit;
                maxValNUD.Value = setMaxLimit;
            }
        }
        // --- --- Class Method --- ---
        public myHistogramForm(string currentImage)
        {
            activeFileNamePath = currentImage;
            Color currPix;
            InitializeComponent();
            activeBitmap = new Bitmap(currentImage);
            if (activeBitmap.Height > activeBitmap.Width)
            {
                for (int i = 0; i <= activeBitmap.Width - 1; i++)
                {
                    for (int j = 0; j < activeBitmap.Height - 1; j++)
                    {
                        currPix = activeBitmap.GetPixel(i, j);
                        alphaArray[currPix.A]++;
                        redArray[currPix.R]++;
                        greenArray[currPix.G]++;
                        blueArray[currPix.B]++;
                        // Grayscale are calculated with the formula: 0.2989 * R(x,y) + 0.5870 * G(x,y) + 0.1140 * B(x,y)
                        grayArray[(int)((currPix.R * 0.2989) + (currPix.G * 0.5870) + (currPix.B * 0.1140))]++;
                    }
                }
            }
            else
            {
                for (int i = 0; i <= activeBitmap.Height - 1; i++)
                {
                    for (int j = 0; j < activeBitmap.Width - 1; j++)
                    {
                        currPix = activeBitmap.GetPixel(j, i);
                        alphaArray[currPix.A]++;
                        redArray[currPix.R]++;
                        greenArray[currPix.G]++;
                        blueArray[currPix.B]++;
                        // Grayscale are calculated with the formula: 0.2989 * R(x,y) + 0.5870 * G(x,y) + 0.1140 * B(x,y)
                        grayArray[(int)((currPix.R * 0.2989) + (currPix.G * 0.5870) + (currPix.B * 0.1140))]++;
                    }
                }
            }
            calcLimitations();
            imagePicBx.SizeMode = PictureBoxSizeMode.Zoom;
            imagePicBx.Image = activeBitmap;
            minValNUD.Value = setMinLimit;
            maxValNUD.Value = setMaxLimit;
            histogramFilter();
        }
        // --- --- GUI Event Methods --- ---
        private void minValNud_UpButton(object sender, EventArgs e)
        {
            setMinLimit = Math.Max(0, Math.Min(255, setMinLimit + 1));
            drawHistogram();
            histogramFilter();

        }
        private void minValNUD_ValueChanged(object sender, EventArgs e)
        {
            setMinLimit = Convert.ToInt32(minValNUD.Value);
            drawHistogram();
            histogramFilter();
        }
        private void maxValNUD_ValueChanged(object sender, EventArgs e)
        {
            setMaxLimit = Convert.ToInt32(maxValNUD.Value);
            histogramFilter();
        }
        private void redChRB_CheckedChanged(object sender, EventArgs e)
        {
            drawHistogram();
        }
        private void greenChRB_CheckedChanged(object sender, EventArgs e)
        {
            drawHistogram();
        }
        private void blueChRB_CheckedChanged(object sender, EventArgs e)
        {
            drawHistogram();
        }
        private void grayRB_CheckedChanged(object sender, EventArgs e)
        {
            drawHistogram();
        }
        private void alphaChRB_CheckedChanged(object sender, EventArgs e)
        {
            drawHistogram();
        }
        private void histogramPicBx_Click(object sender, EventArgs e)
        {
            // This is not handled.
        }
        private void imagePicBx_Click(object sender, EventArgs e)
        {
            // TODO - start a 'marking cross' as one corner, take the next click as the other corner.
            //        after second click zoom in on the selected area.
            //        Double-click resumes full image size.
        }
        private void activBtn_Click(object sender, EventArgs e)
        {
            validImage = true;
            this.Hide();
        }
        private void discBtn_Click(object sender, EventArgs e)
        {
            validImage = false;
            this.Hide();
        }
        private void thrsHldNUD_ValueChanged(object sender, EventArgs e)
        {
            setThresHold = Convert.ToInt32(thrsHldNUD.Value);
            calcLimitations();
            drawHistogram();
            histogramFilter();
        }
    }
}
