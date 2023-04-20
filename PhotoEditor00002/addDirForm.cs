using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhotoEditor00002
{
    public partial class addDirForm : Form
    {
        public string newDirName;
        public string getNewDirName() { return newDirName; }
        public addDirForm()
        {
            InitializeComponent();
            newDirName = "";
        }
        private void createNewDirBtn_Click(object sender, EventArgs e)
        {
            newDirName = newDirNameTxtBx.Text;
            createNewDirBtn.Visible = false;
            this.Hide();
        }
        private void discardBtn_Click(object sender, EventArgs e)
        {
            newDirName = "";
            createNewDirBtn.Visible = false;
            this.Hide();
        }
        private void newDirNameTxtBx_TextChanged(object sender, EventArgs e)
        {
            createNewDirBtn.Visible = true;
        }
        public void MyDispose()
        {
            this.Close();
        }
    }
}
