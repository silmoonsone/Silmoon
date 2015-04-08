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
    public partial class ProcessWaitingBar : UserControl
    {
        Thread _th;
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProcessWaitingBar));
        public ProcessBarShowType ShowType = ProcessBarShowType.Marquee;
        public bool OnStartWaitingReset = true;
        public ProcessWaitingBar()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }
        public void Start()
        {
            _th = new Thread(_th_proc);
            _th.IsBackground = true;
            ctlProcessPictrue.Visible = true;
            _th.Start();
        }
        public void Stop(bool hideProcessPicture)
        {
            ctlProcessPictrue.Visible = !hideProcessPicture;
            Stop();
        }
        public void Stop()
        {
            if (_th != null) _th.Abort();
        }
        public void ShowError()
        {
            this.ctlProcessPictrue.Image = WindowsResource.WaitingProcessPictrue_Red;

        }
        void _th_proc()
        {
            if (OnStartWaitingReset)
            {
                ctlProcessPictrue.Location = new Point(-ctlProcessPictrue.Width, ctlProcessPictrue.Location.Y);
                this.ctlProcessPictrue.Image = WindowsResource.WaitingProcessPictrue;
            }
            while (true)
            {
                if (ctlProcessPictrue.Location.X < this.Width)
                    ctlProcessPictrue.Location = new Point(ctlProcessPictrue.Location.X + 3, ctlProcessPictrue.Location.Y);
                else
                    ctlProcessPictrue.Location = new Point(-ctlProcessPictrue.Width, ctlProcessPictrue.Location.Y);
                Thread.Sleep(12);
            }
        }

        public enum ProcessBarShowType
        {
            Process,
            Marquee,
        }

        private void ProcessWaitingBar_Load(object sender, EventArgs e)
        {

        }
    }
}