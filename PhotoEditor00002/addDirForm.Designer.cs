namespace PhotoEditor00002
{
    partial class addDirForm
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
            this.newDirNameLbl = new System.Windows.Forms.Label();
            this.newDirNameTxtBx = new System.Windows.Forms.TextBox();
            this.discardBtn = new System.Windows.Forms.Button();
            this.createNewDirBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // newDirNameLbl
            // 
            this.newDirNameLbl.AutoSize = true;
            this.newDirNameLbl.Location = new System.Drawing.Point(7, 6);
            this.newDirNameLbl.Name = "newDirNameLbl";
            this.newDirNameLbl.Size = new System.Drawing.Size(107, 13);
            this.newDirNameLbl.TabIndex = 0;
            this.newDirNameLbl.Text = "New directory name: ";
            // 
            // newDirNameTxtBx
            // 
            this.newDirNameTxtBx.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.newDirNameTxtBx.Location = new System.Drawing.Point(110, 3);
            this.newDirNameTxtBx.Name = "newDirNameTxtBx";
            this.newDirNameTxtBx.Size = new System.Drawing.Size(243, 20);
            this.newDirNameTxtBx.TabIndex = 1;
            this.newDirNameTxtBx.TextChanged += new System.EventHandler(this.newDirNameTxtBx_TextChanged);
            // 
            // discardBtn
            // 
            this.discardBtn.Location = new System.Drawing.Point(10, 25);
            this.discardBtn.Name = "discardBtn";
            this.discardBtn.Size = new System.Drawing.Size(170, 30);
            this.discardBtn.TabIndex = 2;
            this.discardBtn.Text = "Discard new directory";
            this.discardBtn.UseVisualStyleBackColor = true;
            this.discardBtn.Click += new System.EventHandler(this.discardBtn_Click);
            // 
            // createNewDirBtn
            // 
            this.createNewDirBtn.Location = new System.Drawing.Point(183, 25);
            this.createNewDirBtn.Name = "createNewDirBtn";
            this.createNewDirBtn.Size = new System.Drawing.Size(170, 30);
            this.createNewDirBtn.TabIndex = 3;
            this.createNewDirBtn.Text = "Create new directory";
            this.createNewDirBtn.UseVisualStyleBackColor = true;
            this.createNewDirBtn.Visible = false;
            this.createNewDirBtn.Click += new System.EventHandler(this.createNewDirBtn_Click);
            // 
            // addDirForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(365, 62);
            this.Controls.Add(this.createNewDirBtn);
            this.Controls.Add(this.discardBtn);
            this.Controls.Add(this.newDirNameTxtBx);
            this.Controls.Add(this.newDirNameLbl);
            this.Name = "addDirForm";
            this.Text = "addDirForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label newDirNameLbl;
        private System.Windows.Forms.TextBox newDirNameTxtBx;
        private System.Windows.Forms.Button discardBtn;
        private System.Windows.Forms.Button createNewDirBtn;
    }
}