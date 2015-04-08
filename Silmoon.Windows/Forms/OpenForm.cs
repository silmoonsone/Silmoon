using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Silmoon.Windows.Forms
{
    public partial class OpenForm : Form
    {
        public event ConfirmOpenFileHander OnConfirmOpen;
        public OpenFileDialog _ofd = new OpenFileDialog();

        public OpenForm()
        {
            InitializeComponent();
        }
        private void OpenWindow_Load(object sender, EventArgs e)
        {

        }

        private void OpenOKButton_Click(object sender, EventArgs e)
        {
            if (OnConfirmOpen != null) { OnConfirmOpen(OpenTextBox.Text); }
            this.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            _ofd.ShowDialog();
            if (_ofd.FileName != "")
            {
                OpenTextBox.Text = _ofd.FileName;
            }
        }
    }
    public delegate void ConfirmOpenFileHander(string filePath);
}