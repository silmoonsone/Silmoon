using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX.AudioVideoPlayback;
using System.Runtime.InteropServices;


namespace Silmoon.Media.Controls
{
    [
    ComVisible(true),
    Guid("88884578-81ea-4850-9911-13ba2d71000c"),
    ]

    public partial class VideoControl : UserControl, Silmoon.Windows.ActiveX.IObjectSafety
    {
        public event VideoControlHander OnPlayContextChange;

        private string _filePath;
        private string _cachedPath;
        private Microsoft.DirectX.AudioVideoPlayback.Video _video;
        private Microsoft.DirectX.AudioVideoPlayback.Audio _audio;
        private System.Timers.Timer _timer;
        VideoControlArgs args = new VideoControlArgs();

        public object InitEvent
        {
            set
            {
                OnPlayContextChange = (VideoControlHander)value;
            }
        }

        private bool isVideo = true;

        public Microsoft.DirectX.AudioVideoPlayback.Audio A
        {
            get { return _audio; }
            set { _audio = value; }
        }
        public Microsoft.DirectX.AudioVideoPlayback.Video V
        {
            get { return _video; }
            set { _video = value; }
        }
        public double CurrentPosition
        {
            get
            {
                if (checkInit())
                {
                    if (isVideo) { return _video.CurrentPosition; }
                    else { return _audio.CurrentPosition; }
                }
                else return 0;
            }
            set
            {
                if (checkInit())
                {
                    if (isVideo) { _video.CurrentPosition = value; }
                    else { _audio.CurrentPosition = value; }
                }
            }
        }
        public double Duration
        {
            get
            {
                if (checkInit())
                {
                    if (isVideo) { return _video.Duration; }
                    else { return _audio.Duration; }
                }
                else return 0;
            }
        }
        public double PlayerDoublePercentage
        {
            get
            {
                try
                { return (double)this.CurrentPosition / (double)this.Duration; }
                catch
                { return 0; }
            }
        }
        public bool ShowTrackBar
        {
            get { return smTrackBar1.Visible; }
            set
            { smTrackBar1.Visible = value; }
        }


        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }
        public VideoControl()
        {
            InitializeComponent();
            _timer = new System.Timers.Timer();
            _timer.Interval = 100;
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(_timer_Elapsed);
            smTrackBar1.Button.BackColor = Color.Violet;
        }

        private void 播放PToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Play();
        }
        private void 停止SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stop();
        }
        private void 暂停PToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Pause();
        }
        private void 全屏FToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FullScreen();
        }

        public void Play()
        {
            if (checkFileName())
            {

                InitVideo();

                if (isVideo) { _video.Play(); }
                else { _audio.Play(); }
                FixSize();
            }
        }
        public void Pause()
        {
            if (checkInit())
            {
                if (isVideo) { _video.Pause(); }
                else { _audio.Pause(); }
            }
        }
        public void Stop()
        {
            if (checkInit())
            {
                if (isVideo) { _video.Stop(); }
                else { _audio.Stop(); }
            }
        }
        public void Open()
        {
            if (_video == null && _audio == null)
            { InitVideo(); }
            else
            { OpenVideo(); }
        }
        public void FullScreen()
        {
            if (_video != null)
            {
                if (_video.State == StateFlags.Running)
                {
                    if (!_video.Fullscreen)
                        _video.Fullscreen = true;
                    else
                        _video.Fullscreen = false;
                }
            }
        }

        public new void Dispose()
        {
            base.Dispose();
            args = null;
            _timer.Dispose();
            _timer = null;

            if (A != null)
            {
                A.Dispose();
                A = null;
            }
            if (V != null)
            {
                V.Dispose();
                V = null;
            }
            base.Dispose();
        }

        private void OpenVideo()
        {
            try
            {
                if (_video == null) { InitVideo(); }
                else { _video.Open(_filePath); }
                if (_audio != null) { _audio.Stop(); }
                isVideo = true;
            }
            catch
            {
                if (_audio == null) { InitVideo(); }
                else { _audio.Open(_filePath); }
                if (_video != null) { _video.Stop(); }
                isVideo = false;
            }
        }
        private void InitVideo()
        {
            if (checkFileName())
            {
                try
                {
                    if (_video != null)
                    {
                        if (_cachedPath != _filePath)
                        {
                            _video.Dispose();
                            _video = new Video(_filePath);
                            _cachedPath = _filePath;
                        }
                    }
                    else
                    {
                        _video = new Video(_filePath);
                        _cachedPath = _filePath;
                    }

                    _video.Owner = VideoPanel;
                    isVideo = true;
                    if (_audio != null) { _audio.Dispose(); _audio = null; }
                }
                catch
                {
                    if (_audio != null)
                    {
                        if (_cachedPath != _filePath)
                        {
                            _audio.Dispose();
                            _video = new Video(_filePath);
                            _cachedPath = _filePath;
                        }
                    }
                    else
                    {
                        _audio = new Audio(_filePath);
                        _cachedPath = _filePath;
                    }
                    isVideo = false;
                    if (_video != null) { _video.Dispose(); _video = null; }
                }
                InitEvents();
            }
        }
        private void InitEvents()
        {
            if (isVideo)
            {
                _video.Ending += new EventHandler(a_Ending);
                _video.Starting += new EventHandler(a_Starting);
            }
            else
            {
                _audio.Ending += new EventHandler(a_Ending);
                _audio.Starting += new EventHandler(a_Starting);
            }
        }

        private void PlayerControl_SizeChanged(object sender, EventArgs e)
        {
            FixSize();
        }
        private void FixSize()
        {
            if (_video != null)
            {
                if (_video.Playing)
                {
                    if ((double)((double)this.Width / (double)this.Height) > (double)((double)_video.DefaultSize.Width / (double)_video.DefaultSize.Height))
                    {
                        VideoPanel.Height = this.Height;
                        VideoPanel.Width = (int)((double)_video.DefaultSize.Width / (double)_video.DefaultSize.Height * (double)VideoPanel.Height);
                    }
                    else
                    {
                        VideoPanel.Width = this.Width;
                        VideoPanel.Height = (int)((double)_video.DefaultSize.Height / (double)_video.DefaultSize.Width * (double)VideoPanel.Width);
                    }
                }
            }
            VideoPanel.Location = new Point((this.Size.Width - VideoPanel.Size.Width) / 2, (this.Size.Height - VideoPanel.Size.Height) / 2);
            ReplayButton.Location = new Point((VideoPanel.Size.Width - ReplayButton.Size.Width) / 2, (VideoPanel.Size.Height - ReplayButton.Size.Height) / 2);
        }
        private void ClearAudio()
        {
            if (_audio != null) { _audio.Dispose(); _audio = null; }
        }
        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            RefreshArgs();
            if (smTrackBar1.Visible)
                smTrackBar1.Value = PlayerDoublePercentage;
            if (OnPlayContextChange != null) { OnPlayContextChange(this, args); }
        }
        private void a_Starting(object sender, EventArgs e)
        {
            _timer.Start();
        }
        private void a_Ending(object sender, EventArgs e)
        {
            ReplayButton.Visible = true;
            _timer.Interval = 500;
            Stop();
        }
        private bool checkInit()
        {
            if (_video == null && _audio == null)
                return false;
            else return true;
        }
        private bool checkFileName()
        {
            if (string.IsNullOrEmpty(_filePath))
            {
                label1.Visible = true;
                label1.Text = "空的URI！";
                return false;
            }
            else
            {
                return true;
            }
        }
        private void RefreshArgs()
        {
            try
            {
                if (isVideo)
                {
                    if (_video.Playing) { _timer.Interval = 100; }
                    args.CurrentPosition = _video.CurrentPosition;
                    args.Duration = _video.Duration;
                }
                else
                {
                    if (_audio.Playing) { _timer.Interval = 100; }
                    args.CurrentPosition = _audio.CurrentPosition;
                    args.Duration = _audio.Duration;
                }
                args.DoublePercentage = PlayerDoublePercentage;
                args.ProgressPercentage = (int)(args.DoublePercentage * 100);
            }
            catch { }
        }

        private void Replay_Click(object sender, EventArgs e)
        {
            ReplayButton.Visible = false;
            Play();
        }

        #region IObjectSafety 成员

        public void GetInterfacceSafyOptions(int riid, out int pdwSupportedOptions, out int pdwEnabledOptions)
        {
            pdwSupportedOptions = 1;
            pdwEnabledOptions = 2;
        }

        public void SetInterfaceSafetyOptions(int riid, int dwOptionsSetMask, int dwEnabledOptions)
        {

        }

        #endregion

        private void smTrackBar1_OnPercentageChange(double nowValue)
        {
            CurrentPosition = nowValue * Duration;
        }
    }
    public delegate void VideoControlHander(object sender, VideoControlArgs e);
    public class VideoControlArgs
    {
        public double Duration;
        public double CurrentPosition;
        public int ProgressPercentage;
        public double DoublePercentage;
    }
}