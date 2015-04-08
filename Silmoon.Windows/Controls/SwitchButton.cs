using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Silmoon.Windows.Controls
{
    public partial class SwitchButton : UserControl
    {
        SwitchStateType switchState = SwitchStateType.Off;
        public event EventHandler OnSwitch;
        System.Windows.Forms.Timer timerOff = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer timerOn = new System.Windows.Forms.Timer();

        int offset = 0;

        public SwitchStateType SwitchState
        {
            get
            {
                { return switchState; }
            }
            set
            {
                if (switchState == value) return;
                switchState = value;
                _switch(value == SwitchStateType.On);
            }
        }


        bool mouseDown = false;
        public SwitchButton()
        {
            InitializeComponent();
            timerOff.Interval = 1;
            timerOff.Tick += new EventHandler(timerOff_Tick);
            timerOn.Interval = 1;
            timerOn.Tick += new EventHandler(timerOn_Tick);
        }

        void timerOn_Tick(object sender, EventArgs e)
        {
            offset = offset + 3;
            pictureBox1.Location = new Point(offset, 0);
            ctlEnableBIMG.Location = new Point(-30 + offset, 0);
            ctlDisableBIMG.Location = new Point(0 + offset, 0);

            if (offset == 30)
            {
                timerOn.Stop();
                offset = 0;
            }
        }
        void timerOff_Tick(object sender, EventArgs e)
        {
            offset = offset + 3;
            pictureBox1.Location = new Point(30 - offset, 0);
            ctlEnableBIMG.Location = new Point(0 - offset, 0);
            ctlDisableBIMG.Location = new Point(30 - offset, 0);

            if (offset == 30)
            {
                timerOff.Stop();
                offset = 0;
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            pictureBox1.BackgroundImage = WindowsResource.SwitchButton_ButtonDown;
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
            pictureBox1.BackgroundImage = WindowsResource.SwitchButton_ButtonUp;
        }

        public enum SwitchStateType
        {
            Off=0,On=1
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (SwitchState == SwitchStateType.Off)
                SwitchState = SwitchStateType.On;
            else SwitchState = SwitchStateType.Off;
        }


        void _switch(bool on)
        {
            Thread _t = null;
            if (on) timerOn.Start();
            else timerOff.Start();
            if (OnSwitch != null) OnSwitch(this, EventArgs.Empty);
        }

        private void ctlDisableBIMG_Click(object sender, EventArgs e)
        {
            if (SwitchState == SwitchStateType.Off)
                SwitchState = SwitchStateType.On;
            else SwitchState = SwitchStateType.Off;
        }

        private void ctlEnableBIMG_Click(object sender, EventArgs e)
        {
            if (SwitchState == SwitchStateType.Off)
                SwitchState = SwitchStateType.On;
            else SwitchState = SwitchStateType.Off;
        }
    }
}
