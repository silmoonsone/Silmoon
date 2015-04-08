using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Silmoon.Windows.Controls
{
    [
    ComVisible(true),
    Guid("88884578-81ea-4850-9911-13ba2d7100f0"),
    ]

    public partial class SmTrackBar : UserControl
    {
        int _x = 0;
        bool _isDown = false;
        public event SmTrackBarHander OnPercentageChange;
        public PictureBox TrackButton
        {
            get { return Button; }
            set { Button = value; }
        }
        public double Value
        {
            get
            {
                return (double)(((double)Button.Location.X / (double)(this.Width - Button.Width)));
            }
            set
            {
                double x = ((double)(this.Width - Button.Width) * value);
                Button.Location = new Point((int)x, Button.Location.Y);
            }
        }
        public int ProgressPercentage
        {
            get { return (int)(Value * 100); }
            set { Value = value / 100; }
        }
        public SmTrackBar()
        {
            InitializeComponent();
            Value = 0;
        }

        private void SmTrackBar_Load(object sender, EventArgs e)
        {

        }

        private void S_MouseDown(object sender, MouseEventArgs e)
        {
            Button.BackgroundImage = WindowsResource.SmTrackBarBottionImage1;
            _isDown = true;
            _x = e.X;
        }
        private void S_MouseMove(object sender, MouseEventArgs e)
        {
            bool b1 = ((this.Width - Button.Width) > Button.Location.X);
            bool b2 = Button.Location.X > 0;
            bool bb = _x < e.X;
            if ((bb||b2)&&(!bb||b1) && _isDown)
            {
                Button.Location = new Point((e.X - _x) + Button.Location.X, Button.Location.Y);
            }
            else if (Button.Location.X > (this.Width - Button.Width))
            {
                Button.Location = new Point(this.Width - Button.Width, Button.Location.Y);
            }
            else if (Button.Location.X < 0)
            {
                Button.Location = new Point(0, Button.Location.Y);
            }
        }
        private void S_MouseUp(object sender, MouseEventArgs e)
        {
            Button.BackgroundImage = WindowsResource.SmTrackBarBottionImage2;
            if (OnPercentageChange != null) { OnPercentageChange(Value); }
            _isDown = false;
        }
        private void S_MouseEnter(object sender, EventArgs e)
        {
            Button.BackgroundImage = WindowsResource.SmTrackBarBottionImage2;
        }
        private void S_MouseLeave(object sender, EventArgs e)
        {
            Button.BackgroundImage = WindowsResource.SmTrackBarBottionImage;
        }
        
        private void S_Click(object sender, EventArgs e)
        {
        }


    }
    public delegate void SmTrackBarHander(double nowValue);
}
