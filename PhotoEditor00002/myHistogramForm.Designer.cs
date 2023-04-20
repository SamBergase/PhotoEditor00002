namespace PhotoEditor00002
{
    partial class myHistogramForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.imagePicBx = new System.Windows.Forms.PictureBox();
            this.minValNUD = new System.Windows.Forms.NumericUpDown();
            this.minValLbl = new System.Windows.Forms.Label();
            this.maxValLbl = new System.Windows.Forms.Label();
            this.maxValNUD = new System.Windows.Forms.NumericUpDown();
            this.redChRB = new System.Windows.Forms.RadioButton();
            this.greenChRB = new System.Windows.Forms.RadioButton();
            this.blueChRB = new System.Windows.Forms.RadioButton();
            this.grayRB = new System.Windows.Forms.RadioButton();
            this.alphaChRB = new System.Windows.Forms.RadioButton();
            this.activBtn = new System.Windows.Forms.Button();
            this.discBtn = new System.Windows.Forms.Button();
            this.trsHldLbl = new System.Windows.Forms.Label();
            this.thrsHldNUD = new System.Windows.Forms.NumericUpDown();
            this.histogramChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.imagePicBx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minValNUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxValNUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.thrsHldNUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.histogramChart)).BeginInit();
            this.SuspendLayout();
            // 
            // imagePicBx
            // 
            this.imagePicBx.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imagePicBx.Cursor = System.Windows.Forms.Cursors.Cross;
            this.imagePicBx.Location = new System.Drawing.Point(7, 7);
            this.imagePicBx.Name = "imagePicBx";
            this.imagePicBx.Size = new System.Drawing.Size(440, 313);
            this.imagePicBx.TabIndex = 0;
            this.imagePicBx.TabStop = false;
            this.imagePicBx.Click += new System.EventHandler(this.imagePicBx_Click);
            // 
            // minValNUD
            // 
            this.minValNUD.Location = new System.Drawing.Point(508, 238);
            this.minValNUD.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.minValNUD.Name = "minValNUD";
            this.minValNUD.Size = new System.Drawing.Size(68, 20);
            this.minValNUD.TabIndex = 2;
            this.minValNUD.ValueChanged += new System.EventHandler(this.minValNUD_ValueChanged);
            // 
            // minValLbl
            // 
            this.minValLbl.AutoSize = true;
            this.minValLbl.Location = new System.Drawing.Point(454, 240);
            this.minValLbl.Name = "minValLbl";
            this.minValLbl.Size = new System.Drawing.Size(51, 13);
            this.minValLbl.TabIndex = 3;
            this.minValLbl.Text = "Minimum:";
            // 
            // maxValLbl
            // 
            this.maxValLbl.AutoSize = true;
            this.maxValLbl.Location = new System.Drawing.Point(454, 267);
            this.maxValLbl.Name = "maxValLbl";
            this.maxValLbl.Size = new System.Drawing.Size(54, 13);
            this.maxValLbl.TabIndex = 5;
            this.maxValLbl.Text = "Maximum:";
            // 
            // maxValNUD
            // 
            this.maxValNUD.Location = new System.Drawing.Point(508, 265);
            this.maxValNUD.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.maxValNUD.Name = "maxValNUD";
            this.maxValNUD.Size = new System.Drawing.Size(68, 20);
            this.maxValNUD.TabIndex = 4;
            this.maxValNUD.ValueChanged += new System.EventHandler(this.maxValNUD_ValueChanged);
            // 
            // redChRB
            // 
            this.redChRB.AutoSize = true;
            this.redChRB.Location = new System.Drawing.Point(617, 238);
            this.redChRB.Name = "redChRB";
            this.redChRB.Size = new System.Drawing.Size(87, 17);
            this.redChRB.TabIndex = 6;
            this.redChRB.TabStop = true;
            this.redChRB.Text = "Red Channel";
            this.redChRB.UseVisualStyleBackColor = true;
            this.redChRB.CheckedChanged += new System.EventHandler(this.redChRB_CheckedChanged);
            // 
            // greenChRB
            // 
            this.greenChRB.AutoSize = true;
            this.greenChRB.Location = new System.Drawing.Point(710, 238);
            this.greenChRB.Name = "greenChRB";
            this.greenChRB.Size = new System.Drawing.Size(96, 17);
            this.greenChRB.TabIndex = 7;
            this.greenChRB.TabStop = true;
            this.greenChRB.Text = "Green Channel";
            this.greenChRB.UseVisualStyleBackColor = true;
            this.greenChRB.CheckedChanged += new System.EventHandler(this.greenChRB_CheckedChanged);
            // 
            // blueChRB
            // 
            this.blueChRB.AutoSize = true;
            this.blueChRB.Location = new System.Drawing.Point(812, 238);
            this.blueChRB.Name = "blueChRB";
            this.blueChRB.Size = new System.Drawing.Size(88, 17);
            this.blueChRB.TabIndex = 8;
            this.blueChRB.TabStop = true;
            this.blueChRB.Text = "Blue Channel";
            this.blueChRB.UseVisualStyleBackColor = true;
            this.blueChRB.CheckedChanged += new System.EventHandler(this.blueChRB_CheckedChanged);
            // 
            // grayRB
            // 
            this.grayRB.AutoSize = true;
            this.grayRB.Location = new System.Drawing.Point(710, 265);
            this.grayRB.Name = "grayRB";
            this.grayRB.Size = new System.Drawing.Size(72, 17);
            this.grayRB.TabIndex = 9;
            this.grayRB.TabStop = true;
            this.grayRB.Text = "Grayscale";
            this.grayRB.UseVisualStyleBackColor = true;
            this.grayRB.CheckedChanged += new System.EventHandler(this.grayRB_CheckedChanged);
            // 
            // alphaChRB
            // 
            this.alphaChRB.AutoSize = true;
            this.alphaChRB.Location = new System.Drawing.Point(812, 265);
            this.alphaChRB.Name = "alphaChRB";
            this.alphaChRB.Size = new System.Drawing.Size(94, 17);
            this.alphaChRB.TabIndex = 10;
            this.alphaChRB.TabStop = true;
            this.alphaChRB.Text = "Alpha Channel";
            this.alphaChRB.UseVisualStyleBackColor = true;
            this.alphaChRB.CheckedChanged += new System.EventHandler(this.alphaChRB_CheckedChanged);
            // 
            // activBtn
            // 
            this.activBtn.Location = new System.Drawing.Point(457, 293);
            this.activBtn.Name = "activBtn";
            this.activBtn.Size = new System.Drawing.Size(240, 26);
            this.activBtn.TabIndex = 11;
            this.activBtn.Text = "Activate";
            this.activBtn.UseVisualStyleBackColor = true;
            this.activBtn.Click += new System.EventHandler(this.activBtn_Click);
            // 
            // discBtn
            // 
            this.discBtn.Location = new System.Drawing.Point(701, 293);
            this.discBtn.Name = "discBtn";
            this.discBtn.Size = new System.Drawing.Size(218, 26);
            this.discBtn.TabIndex = 12;
            this.discBtn.Text = "Discard";
            this.discBtn.UseVisualStyleBackColor = true;
            this.discBtn.Click += new System.EventHandler(this.discBtn_Click);
            // 
            // trsHldLbl
            // 
            this.trsHldLbl.AutoSize = true;
            this.trsHldLbl.Location = new System.Drawing.Point(582, 267);
            this.trsHldLbl.Name = "trsHldLbl";
            this.trsHldLbl.Size = new System.Drawing.Size(57, 13);
            this.trsHldLbl.TabIndex = 14;
            this.trsHldLbl.Text = "Threshold:";
            // 
            // thrsHldNUD
            // 
            this.thrsHldNUD.Location = new System.Drawing.Point(641, 265);
            this.thrsHldNUD.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.thrsHldNUD.Name = "thrsHldNUD";
            this.thrsHldNUD.Size = new System.Drawing.Size(56, 20);
            this.thrsHldNUD.TabIndex = 13;
            this.thrsHldNUD.ValueChanged += new System.EventHandler(this.thrsHldNUD_ValueChanged);
            // 
            // histogramChart
            // 
            chartArea1.Name = "ChartArea1";
            this.histogramChart.ChartAreas.Add(chartArea1);
            this.histogramChart.Location = new System.Drawing.Point(457, 7);
            this.histogramChart.Name = "histogramChart";
            series1.ChartArea = "ChartArea1";
            series1.Name = "Series1";
            this.histogramChart.Series.Add(series1);
            this.histogramChart.Size = new System.Drawing.Size(462, 225);
            this.histogramChart.TabIndex = 8;
            this.histogramChart.Text = "Histogram Chart";
            // 
            // myHistogramForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(929, 329);
            this.Controls.Add(this.histogramChart);
            this.Controls.Add(this.trsHldLbl);
            this.Controls.Add(this.thrsHldNUD);
            this.Controls.Add(this.discBtn);
            this.Controls.Add(this.activBtn);
            this.Controls.Add(this.alphaChRB);
            this.Controls.Add(this.grayRB);
            this.Controls.Add(this.blueChRB);
            this.Controls.Add(this.greenChRB);
            this.Controls.Add(this.redChRB);
            this.Controls.Add(this.maxValLbl);
            this.Controls.Add(this.maxValNUD);
            this.Controls.Add(this.minValLbl);
            this.Controls.Add(this.minValNUD);
            this.Controls.Add(this.imagePicBx);
            this.Name = "myHistogramForm";
            this.Text = "myHistogramForm";
            ((System.ComponentModel.ISupportInitialize)(this.imagePicBx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minValNUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxValNUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.thrsHldNUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.histogramChart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox imagePicBx;
        private System.Windows.Forms.NumericUpDown minValNUD;
        private System.Windows.Forms.Label minValLbl;
        private System.Windows.Forms.Label maxValLbl;
        private System.Windows.Forms.NumericUpDown maxValNUD;
        private System.Windows.Forms.RadioButton redChRB;
        private System.Windows.Forms.RadioButton greenChRB;
        private System.Windows.Forms.RadioButton blueChRB;
        private System.Windows.Forms.RadioButton grayRB;
        private System.Windows.Forms.RadioButton alphaChRB;
        private System.Windows.Forms.Button activBtn;
        private System.Windows.Forms.Button discBtn;
        private System.Windows.Forms.Label trsHldLbl;
        private System.Windows.Forms.NumericUpDown thrsHldNUD;
        private System.Windows.Forms.DataVisualization.Charting.Chart histogramChart;
    }
}