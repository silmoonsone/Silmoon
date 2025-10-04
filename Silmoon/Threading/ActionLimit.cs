using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Silmoon.Arrays;
using Silmoon.Types;

namespace Silmoon.Threading
{
    /// <summary>
    /// 对于多个时间段，一定时间内次数限制的控制类
    /// </summary>
    public class ActionLimit
    {
        private List<TimeLimit> timesLimit { get; set; } = new List<TimeLimit>();
        private List<TimeSection> timeSectionLimit { get; set; } = new List<TimeSection>();
        public string Name { get; set; }
        public ActionLimit(string name = null)
        {
            Name = name;
        }

        /// <summary>
        /// 当所有的时间段限制不统一的时候使用的默认值
        /// </summary>
        public bool DefaultTimeSectionPass { get; set; } = false;
        /// <summary>
        /// 当所有的次数限制不统一的时候使用的默认值
        /// </summary>
        public bool DefaultTimesLimitPass { get; set; } = false;
        /// <summary>
        /// 在整个时间计算中，忽略日期
        /// </summary>
        public bool IgnoreDate { get; set; } = true;
        /// <summary>
        /// 设置或获取是否在指定的时间内不允许策略，如果不是就是在指定的时间外不允许
        /// </summary>
        public bool BlackTimeSection { get; set; } = true;

        /// <summary>
        /// 获取所有对于时间次数设置的规则
        /// </summary>
        public TimeLimit[] TimeLimits
        {
            get
            {
                TimeLimit[] ret = new TimeLimit[timesLimit.Count];
                for (int i = 0; i < timesLimit.Count; i++)
                {
                    ret[i] = timesLimit[i];
                }
                return ret;
            }
        }
        /// <summary>
        /// 在规则中设置的时间段
        /// </summary>
        public TimeSection[] TimeSections
        {
            get
            {
                TimeSection[] ret = new TimeSection[timeSectionLimit.Count];
                for (int i = 0; i < timeSectionLimit.Count; i++)
                {
                    ret[i] = timeSectionLimit[i];
                }
                return ret;
            }
        }

        /// <summary>
        /// 添加一个时间限制规则
        /// </summary>
        /// <param name="timelimit"></param>
        public void AddTimeLimit(TimeLimit timelimit) => timesLimit.Add(timelimit);
        /// <summary>
        /// 添加一个时间段限制规则
        /// </summary>
        /// <param name="timesection"></param>
        public void AddTimeSection(TimeSection timesection) => timeSectionLimit.Add(timesection);

        public bool Pass => timeSectionPass() && timesLimitPass();

        bool timesLimitPass()
        {
            int passCount = 0;
            foreach (TimeLimit item in TimeLimits)
            {
                if (item.CanDo(false))
                    passCount++;
            }

            if (passCount == TimeLimits.Length)
                foreach (TimeLimit item in TimeLimits)
                    item.AddTimes(1);

            if (passCount == 0)
                return false;
            else if (passCount == TimeLimits.Length)
                return true;
            else
                return DefaultTimesLimitPass;
        }
        bool timeSectionPass()
        {
            int inTimeCount = 0;
            DateTime now = DateTime.Now;
            if (IgnoreDate) now = new DateTime(0001, 1, 1, now.Hour, now.Minute, now.Second, now.Millisecond);

            foreach (TimeSection item in TimeSections)
            {
                DateTime startTime = item.Index;
                DateTime endTime = item.EndTime;
                if (IgnoreDate)
                {
                    startTime = new DateTime(0001, 1, 1, startTime.Hour, startTime.Minute, startTime.Second, startTime.Millisecond);
                    endTime = new DateTime(0001, 1, 1, endTime.Hour, endTime.Minute, endTime.Second, endTime.Millisecond);
                }
                //Console.WriteLine("startTime(" + startTime + ");now(" + now + ");endTime(" + endTime + ")");
                if (startTime < now && now < endTime)
                    inTimeCount++;
            }

            if (inTimeCount == 0)
                return BlackTimeSection;
            else if (inTimeCount == TimeLimits.Length)
                return !BlackTimeSection;
            else
                return DefaultTimeSectionPass;

        }
    }
}
