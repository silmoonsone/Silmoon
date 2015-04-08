using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Silmoon.Windows.Forms
{
    public partial class GenieForm : Silmoon.Windows.Forms.ScrollForm
    {
        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        //public bool PanelContainVisual
        //{
        //    get { return panel1.Visible; }
        //    set { panel1.Visible = value; }
        //}

        public GenieForm()
        {
            InitializeComponent();
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            foreach (Control control in this.Controls)
            {
                control.Visible = false;
            }
            base.OnFormClosing(e);
        }
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCHITTEST:
                    base.WndProc(ref m);
                    if ((int)m.Result == HTCLIENT)
                        m.Result = (IntPtr)HTCAPTION;
                    return;
            }
            base.WndProc(ref m);
        }
        protected override void OnTextChanged(EventArgs e)
        {
            ctlTitleLabel.Text = this.Text;
            base.OnTextChanged(e);
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            //panel1.Height = this.Height - 17;
            if (ctlClosePanelButton != null)
                ctlClosePanelButton.Location = new Point(this.Width - 17, 0);
            base.OnSizeChanged(e);
        }

        private void 关闭XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ctlClosePanelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
