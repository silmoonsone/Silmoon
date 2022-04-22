using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Threading
{
    public class ClockTimer
    {
        public event Action SecoundChange;
        public event Action MinuteChange;
        public event Action HourChange;
        public event Action DayChange;
        public event Action MonthChange;
        public event Action YearChange;
        System.Timers.Timer timer = new System.Timers.Timer(1000);

        DateTime dateTime = DateTime.Now;
        public ClockTimer()
        {
            timer.Elapsed += Timer_Elapsed;
        }
        public void Start()
        {
            dateTime = DateTime.Now;
            timer.Start();
        }
        public void Stop()
        {
            timer.Stop();
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            SecoundChange?.Invoke();

            if (dateTime.ToString("yyyy MM dd HH mm") != DateTime.Now.ToString("yyyy MM dd HH mm")) MinuteChange?.Invoke();
            if (dateTime.ToString("yyyy MM dd HH") != DateTime.Now.ToString("yyyy MM dd HH")) HourChange?.Invoke();
            if (dateTime.ToString("yyyy MM dd") != DateTime.Now.ToString("yyyy MM dd")) DayChange?.Invoke();
            if (dateTime.ToString("yyyy MM") != DateTime.Now.ToString("yyyy MM")) MonthChange?.Invoke();
            if (dateTime.ToString("yyyy") != DateTime.Now.ToString("yyyy")) YearChange?.Invoke();


            dateTime = DateTime.Now;
        }
    }
}
