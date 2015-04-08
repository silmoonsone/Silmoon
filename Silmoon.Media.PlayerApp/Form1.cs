using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Silmoon.Windows.Forms;
using System.IO;
using System.Threading;
using Silmoon.Windows.Controls;

namespace Silmoon.Media.Video.Player
{
    public partial class Form1 : Form
    {
        string[] _args;
        string _filePath;
        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            smTraceBar1.OnPercentageChange += new SmTrackBarHander(OnPercentageChange_Click);
            playerControl1.OnPlayContextChange += new Silmoon.Media.Controls.VideoControlHander(playerControl1_OnPlayContextChange);
        }

        void playerControl1_OnPlayContextChange(object sender, Silmoon.Media.Controls.VideoControlArgs e)
        {
            smTraceBar1.Value = e.CurrentPosition / e.Duration;
            Menu_ProcLabel.Text = ((int)(smTraceBar1.Value * 100)).ToString() + "%";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _args = Environment.GetCommandLineArgs();
            InitForm();
        }
        private void InitForm()
        {
            smTraceBar1.TrackButton.BackColor = System.Drawing.Color.White;
            if (_args.Length != 1)
            {
                _filePath = _args[1];
                DoPlay();
            }
        }

        void OnPercentageChange_Click(double nowValue)
        {
            playerControl1.CurrentPosition = smTraceBar1.Value * playerControl1.Duration;
        }
        private void Tool_WMPOpenButton_Click(object sender, EventArgs e)
        {
            UsingWmpOpen();
        }


        private void 使用WindowsMediaPlayer打开WToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            UsingWmpOpen();
        }

        private void 打开OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm open = new OpenForm();
            open.OnConfirmOpen += new ConfirmOpenFileHander(open_OnConfirmOpen);
            open.Show();
        }
        private void 工具栏ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (工具栏ToolStripMenuItem.Checked) { 工具栏ToolStripMenuItem.Checked = false; }
            else { 工具栏ToolStripMenuItem.Checked = true; }
            ToolMenu.Visible = 工具栏ToolStripMenuItem.Checked;
        }

        void DoPlay()
        {
            playerControl1.FilePath = _filePath;
            playerControl1.Play();
            this.Text = Path.GetFileName(_filePath) + " - SmPlayer 播放器";
        }
        void DoOpen()
        {
            playerControl1.FilePath = _filePath;
            playerControl1.Open();
            playerControl1.Play();
            this.Text = Path.GetFileName(_filePath) + " - SmPlayer 播放器";
        }
        void UsingWmpOpen()
        {
            Process p = new Process();
            p.EnableRaisingEvents = true;
            p.Exited += new EventHandler(p_Exited);
            p.StartInfo.FileName = "wmplayer.exe";
            p.StartInfo.Arguments = "\"" + _filePath + "\"";
            p.Start();
            if (playerControl1.V != null)
            {
                playerControl1.V.Pause();
            }
            this.Visible = false;
        }
        void p_Exited(object sender, EventArgs e)
        {
            this.Visible = true;
            if (playerControl1.V != null)
            {
                if (playerControl1.V.State == Microsoft.DirectX.AudioVideoPlayback.StateFlags.Paused)
                {
                    playerControl1.V.Play();
                }
            }
        }
        void open_OnConfirmOpen(string filePath)
        {
            _filePath = filePath;
            DoOpen();
        }

        private void playerControl1_Paint(object sender, PaintEventArgs e)
        {
            foreach (Control C in this.Controls)
            {
                if (C is Label)
                {
                    Label L = (Label)C;
                    L.Visible = false;
                    e.Graphics.DrawString(L.Text, L.Font, new SolidBrush(L.ForeColor), L.Left - playerControl1.Left, L.Top - playerControl1.Top);
                }

            }
        }
    }
}