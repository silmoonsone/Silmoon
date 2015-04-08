using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using WMPLib;
using System.Timers;
using System.Windows.Forms;

namespace Silmoon.Media
{
    [
    ComVisible(true),
    Guid("88884578-81ea-4850-9911-13ba2d71000b"),
    ]
    public class WMPPlayer : IDisposable
    {
        string _filePath;
        PlayerState _state;
        public WMPLib.WindowsMediaPlayerClass _wmp = new WMPLib.WindowsMediaPlayerClass();
        WMPPlayerArgs _args = new WMPPlayerArgs();
        System.Timers.Timer _timer = new System.Timers.Timer();

        public event WMPPlayerHander OnPlayerStateChange;
        public event WMPPlayerHander OnPlayContextChange;


        protected void onPlayerStateChange(WMPPlayerArgs args)
        {
            if (OnPlayerStateChange != null)
            {
                OnPlayerStateChange(this, args);
            }
        }
        protected void onPlayContextChange(WMPPlayerArgs args)
        {
            if (OnPlayContextChange != null)
            {
                System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
                OnPlayContextChange(this, args);
            }
        }


        public double Duration
        {
            get
            {
                return _wmp.currentMedia.duration;
            }
        }
        public string DurationString
        {
            get
            {
                return _wmp.currentMedia.durationString;
            }
        }
        public double CurrentPosition
        {
            get
            {
                return _wmp.currentPosition;
            }
            set
            {
                _wmp.currentPosition = value;
            }
        }
        public string CurrentPositionString
        {
            get
            {
                return _wmp.currentPositionString;
            }
        }

        public double PlayProgressPercentage
        {
            get
            { return PlayDoublePercentage * 100; }
        }
        public double PlayDoublePercentage
        {
            get
            {
                try
                { return CurrentPosition / Duration; }
                catch
                { return 0; }
            }
        }

        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }
        public PlayerState State
        {
            get { return _state; }
            set { _state = value; }
        }
        public WMPPlayState WMPState
        {
            get { return _wmp.playState; }
        }
        public double PlayerContextCycleInterval
        {
            get { return _timer.Interval; }
            set { _timer.Interval = value; }
        }
        public string MuiscName
        {
            get { return _wmp.currentMedia.name; }
        }

        public WMPPlayer()
        {
            InitClass();
        }
        public WMPPlayer(string filePath)
        {
            _filePath = filePath;
            InitClass();
        }
        private void InitClass()
        {
            //((System.ComponentModel.ISupportInitialize)(this._wmp)).BeginInit();
            //this.Controls.Add(this._wmp);
            //((System.ComponentModel.ISupportInitialize)(this._wmp)).EndInit();

            _wmp.settings.autoStart = false;
            _wmp.PlayStateChange += new _WMPOCXEvents_PlayStateChangeEventHandler(_wmp_PlayStateChange);
            _state = PlayerState.Null;
            _timer.Interval = 300;
            _timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
        }


        void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            refreshArgs();
            onPlayContextChange(_args);
        }

        void _wmp_PlayStateChange(int NewState)
        {
            switch (_wmp.playState)
            {
                case WMPPlayState.wmppsBuffering:
                    break;
                case WMPPlayState.wmppsLast:
                    break;
                case WMPPlayState.wmppsMediaEnded:
                    break;
                case WMPPlayState.wmppsPaused:
                    _state = PlayerState.Puase;
                    break;
                case WMPPlayState.wmppsPlaying:
                    _state = PlayerState.Playing;
                    break;
                case WMPPlayState.wmppsReady:
                    _state = PlayerState.Stop;
                    break;
                case WMPPlayState.wmppsReconnecting:
                    break;
                case WMPPlayState.wmppsScanForward:
                    break;
                case WMPPlayState.wmppsScanReverse:
                    break;
                case WMPPlayState.wmppsStopped:
                    _state = PlayerState.Stop;
                    break;
                case WMPPlayState.wmppsTransitioning:
                    break;
                case WMPPlayState.wmppsUndefined:
                    break;
                case WMPPlayState.wmppsWaiting:
                    break;
                default:
                    break;
            }
            if (_wmp.playState == WMPPlayState.wmppsPlaying)
            { _timer.Start(); }
            else { _timer.Stop(); }
            refreshArgs();
            onPlayerStateChange(_args);
        }

        public void Play()
        {
            _wmp.URL = _filePath;
            _wmp.play();
        }
        public bool GoOn()
        {
            if (_state == PlayerState.Puase)
            {
                _wmp.play();
                _state = PlayerState.Playing;
                return true;
            }
            return false;
        }
        public bool Pause()
        {
            if (_state == PlayerState.Playing)
            {
                _wmp.pause();
                return true;
            }
            else return false;
        }
        public void Stop()
        {
            _wmp.stop();
        }

        public void Close()
        {
            Stop();
            _wmp.close();
        }

        private void refreshArgs()
        {
            _args.State = _wmp.playState;
            _args.DoublePercentage = PlayDoublePercentage;
            _args.ProgressPercentage = _args.DoublePercentage * 100;

            _args.CurrentPosition = CurrentPosition;
        }
        public void Dispose()
        {
            _timer.Dispose();
            _timer = null;
            _wmp.close();
            _wmp = null;
            _args = null;
        }
    }
    public enum PlayerState
    {
        Null = 0,
        Loaded = 1,
        Playing = 2,
        Puase = 3,
        Stop = 4,
        Close = 5
    }
    public delegate void WMPPlayerHander(object sender, WMPPlayerArgs e);

    [
    ComVisible(true),
    Guid("88884578-81ea-4850-9911-13ba2d710d0c"),
    ]
    public class WMPPlayerArgs : System.EventArgs
    {
        public WMPPlayState State;
        public double ProgressPercentage;
        public double DoublePercentage;
        public double CurrentPosition;
    }
}