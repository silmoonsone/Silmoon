using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Silmoon.Windows.Forms
{
    public partial class OkConfirmDialog : Form
    {
        bool _ok = false;

        public bool Ok
        {
            get { return _ok; }
        }
        public string Title
        {
            get { return this.Text; }
            set { this.Text = value; }
        }

        public OkConfirmDialog()
        {
            InitializeComponent();
        }
        public OkConfirmDialog(string title)
        {
            InitializeComponent();
            Title = title;
        }
        private void ConfirmButton_Click(object sender, EventArgs e)
        {
            if (ConfirmTextBox.Text == "OK") _ok = true;
            else
            {
                MessageBox.Show("¥ÌŒÛ ‰»ÎOK£°", "£°", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ConfirmTextBox.Text = "";
            }
            if (Ok) DialogResult = DialogResult.OK;
        }

        private void ConfirmTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ConfirmTextBox.Text == "OK") ConfirmButton.Enabled = true;
            else ConfirmButton.Enabled = false;
        }
    }
}