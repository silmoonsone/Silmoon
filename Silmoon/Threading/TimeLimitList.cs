using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silmoon.Threading
{
    public class TimeLimitList
    {
        private static readonly Lazy<TimeLimitList> sharedTimeLimitList = new Lazy<TimeLimitList>(() => new TimeLimitList());
        public static TimeLimitList SharedTimeLimitList => sharedTimeLimitList.Value;

        List<TimeLimit> timeLimitList { get; set; } = new List<TimeLimit>();
        public bool CanDo(string name, TimeSpan timeSpan, int times)
        {
            lock (timeLimitList)
            {
                List<TimeLimit> pendingOfRemove = new List<TimeLimit>();
                foreach (var item in timeLimitList)
                {
                    if (item.OutOfTime()) pendingOfRemove.Add(item);
                }
                foreach (var item in pendingOfRemove)
                {
                    timeLimitList.Remove(item);
                }

                var timeLimit = timeLimitList.Where(x => x.Name == name).FirstOrDefault();
                if (timeLimit is null)
                {
                    timeLimit = new TimeLimit(timeSpan, times, name);
                    timeLimitList.Add(timeLimit);

                    return timeLimit.CanDo(true);
                }
                else
                {
                    return timeLimit.CanDo(true);
                }
            }
        }
    }
}
