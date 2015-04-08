using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Timers;
namespace Silmoon.Media
{
    [
ComVisible(true),
Guid("88884578-81ea-4850-9911-13ba2d71002b"),
]
    public class MMPlayer : IDisposable
    {
        private MMPlayerInfo _playerInfo = new MMPlayerInfo();
        public event MMPlayerHandler OnPlayerStateChange;
        public event MMPlayerHandler OnPlayContextChange;

        public MMPlayerInfo PlayerInfo
        {
            get { return _playerInfo; }
        }
        public struct MMPlayerInfo
        {
            public double PlayCurrentPosition;
            public string FileName;
            public PlayerState State;
        }
        MMPPlayerArgs _args = new MMPPlayerArgs();
        System.Windows.Forms.Timer _timer = new System.Windows.Forms.Timer();

        #region API
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetShortPathName(
         string lpszLongPath,
         StringBuilder shortFile,
         int cchBuffer
        );

        [DllImport("winmm.dll", EntryPoint = "mciSendString", CharSet = CharSet.Auto)]
        public static extern int mciSendString(
         string lpstrCommand,
         StringBuilder lpstrReturnString,
         int uReturnLength,
         int hwndCallback
        );
        #endregion

        public MMPlayer()
        {
            _playerInfo.State = PlayerState.Stop;
            _timer.Interval = 300;
            _timer.Tick += new EventHandler(_timer_Tick);
        }

        void _timer_Tick(object sender, EventArgs e)
        {
            refreshArgs();
            onPlayContextChange(_args);
            if (Duration == CurrentPosition)
            {
                chengeState(PlayerState.Stop);
            }
        }


        public string FilePath
        {
            get
            { return _playerInfo.FileName; }
            set
            { _playerInfo.FileName = value; }
        }
        public void Open()
        {
            if (string.IsNullOrEmpty(_playerInfo.FileName)) throw new ArgumentException("指定的文件名错误或者不存在的文件！", "FileName");
            StringBuilder sb = new StringBuilder(1024);
            StringBuilder sFile = new StringBuilder(1024);
            GetShortPathName(_playerInfo.FileName, sFile, sFile.Capacity);

            string CmdLine = "open \"" + sFile.ToString() + "\" alias media";
            mciSendString("close media", sb, sb.Capacity, 0);
            mciSendString("close all", sb, sb.Capacity, 0);
            mciSendString(CmdLine, sb, sb.Capacity, 0);
            mciSendString("set media time format milliseconds", sb, sb.Capacity, 0);
            chengeState(PlayerState.Stop);
        }
        public void Play()
        {
            if (PlayerInfo.State != PlayerState.Puase)
                Open();
            StringBuilder sb = new StringBuilder(1024);
            mciSendString("play media", sb, sb.Capacity, 0);
            chengeState(PlayerState.Playing);
        }
        public void Stop()
        {
            StringBuilder sb = new StringBuilder(1024);
            mciSendString("stop media", sb, 128, 0);
            mciSendString("close all", sb, 128, 0);
            chengeState(PlayerState.Stop);
        }
        public void Close()
        {
            StringBuilder sb = new StringBuilder(1024);
            mciSendString("close media", sb, 128, 0);
            mciSendString("close all", sb, 128, 0);
            chengeState(PlayerState.Stop);
        }
        public void Pause()
        {
            if (PlayerInfo.State != PlayerState.Playing) return;
            StringBuilder sb = new StringBuilder(1024);
            mciSendString("pause media", sb, sb.Capacity, 0);
            chengeState(PlayerState.Puase);
        }

        public void OpenCdRomDoor()
        {
            mciSendString("Set cdaudio door open wait", null, 0, 0);
        }
        public void CloseCdRomDoor()
        {
            mciSendString("Set cdaudio door closed wait", null, 0, 0);
        }

        public double Duration
        {
            get
            {
                StringBuilder sb = new StringBuilder(1024);
                int i = mciSendString("status media length", sb, sb.Capacity, 0);
                if (sb.Length == 0) return 0;
                return (Convert.ToDouble(sb.ToString()) / 1000);
            }
        }
        public double CurrentPosition
        {
            get
            {
                StringBuilder sb = new StringBuilder(1024);
                mciSendString("status media position", sb, sb.Capacity, 0);
                if (sb.Length == 0 || sb.ToString() == "0") return 0;
                _playerInfo.PlayCurrentPosition = (Convert.ToDouble(sb.ToString()) / 1000);
                return _playerInfo.PlayCurrentPosition;
            }
            set
            {
                StringBuilder sb = new StringBuilder(1024);
                int i = (int)(value * 1000);
                mciSendString("seek media to " + i, sb, sb.Capacity, 0);
                if (State == PlayerState.Playing)
                {
                    mciSendString("play media", sb, sb.Capacity, 0);
                }
            }
        }
        public double PlayeProgressPercentage
        {
            get
            { return PlayeDoublePercentage * 100; }
        }
        public double PlayeDoublePercentage
        {
            get
            {
                try
                { return CurrentPosition / Duration; }
                catch
                { return 0; }
            }
        }

        public PlayerState State
        {
            get
            {
                return _playerInfo.State;
            }
        }

        public int PlayerContextCycleInterval
        {
            get { return _timer.Interval; }
            set { _timer.Interval = value; }
        }

        void chengeState(PlayerState state)
        {
            if (_playerInfo.State != PlayerState.Playing && state == PlayerState.Playing)
                _timer.Start();
            else
                _timer.Stop();

            if (_playerInfo.State != state)
            {
                _playerInfo.State = state;
                if (OnPlayerStateChange != null)
                {
                    refreshArgs();
                    OnPlayerStateChange(this, _args);
                }
            }
        }
        void onPlayContextChange(MMPPlayerArgs args)
        {
            if (OnPlayContextChange != null)
            {
                OnPlayContextChange(this, args);
            }
        }

        private void refreshArgs()
        {
            try
            {
                _args.State = _playerInfo.State;
                _args.DoublePercentage = PlayeDoublePercentage;
                _args.ProgressPercentage = _args.DoublePercentage * 100;

                _args.CurrentPosition = CurrentPosition;
            }
            catch { }
        }


        #region IDisposable 成员

        public void Dispose()
        {
            Stop();
            Close();
            _timer.Stop();
            _timer.Dispose();
            _timer = null;
        }

        #endregion
    }
    public delegate void MMPlayerHandler(MMPlayer sender, MMPPlayerArgs e);
    [
ComVisible(true),
Guid("88884578-81ea-4850-9911-13ba2d710d2c"),
]
    public class MMPPlayerArgs : System.EventArgs
    {
        public PlayerState State;
        public double ProgressPercentage;
        public double DoublePercentage;
        public double CurrentPosition;
    }
}