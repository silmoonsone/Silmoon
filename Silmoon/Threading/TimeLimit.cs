using System;
using System.Collections.Generic;
using System.Text;
using Silmoon.Arrays;

namespace Silmoon.Threading
{
    /// <summary>
    /// 控制一个动作或者行为在一定的时间内可以执行几次。
    /// </summary>
    public class TimeLimit
    {
        int countTimes = 0;
        DateTime startTime = DateTime.Now;

        public string Name { get; set; }
        /// <summary>
        /// 限制次数
        /// </summary>
        public int LimitTimes { get; set; } = 1;
        /// <summary>
        /// 查询是否可以继续
        /// </summary>
        /// <param name="addTimes">查询一次，行为增加一次</param>
        /// <returns></returns>
        public bool CanDo(bool addTimes = false)
        {
            if ((DateTime.Now - startTime).TotalMilliseconds < ResetMilliseconds)
            {
                //在时间范围内
                if (countTimes >= LimitTimes)
                    return false;
                else
                {
                    if (addTimes) AddTimes(1);
                    return true;
                }
            }
            else
            {
                startTime = DateTime.Now;
                countTimes = 0;
                if (addTimes) AddTimes(1);
                return true;
            }
        }
        /// <summary>
        /// 添加动作次数
        /// </summary>
        /// <param name="times">次数</param>
        public void AddTimes(int times) => countTimes += times;
        /// <summary>
        /// 控制的时间范围，以毫秒为单位的设置
        /// </summary>
        public ulong ResetMilliseconds { get; set; } = 1000;
        /// <summary>
        /// 控制的时间范围，以时间间隔为单位的设置
        /// </summary>
        public TimeSpan ResetTimespan
        {
            set
            {
                ResetMilliseconds = (ulong)value.TotalMilliseconds;
            }
        }

        /// <summary>
        /// 以一个id号开始的新实例TimeLimit
        /// </summary>
        /// <param name="id"></param>
        public TimeLimit(string name) => Name = name;
        /// <summary>
        /// 新实例TimeLimit
        /// </summary>
        public TimeLimit()
        {

        }
        public TimeLimit(ulong resetMillisecounds, int limitTimes, string name = null)
        {
            ResetMilliseconds = resetMillisecounds;
            LimitTimes = limitTimes;
            Name = name;
        }
        public TimeLimit(TimeSpan resetTimespan, int limitTimes, string name = null)
        {
            ResetTimespan = resetTimespan;
            LimitTimes = limitTimes;
            Name = name;
        }
        public override string ToString() => "TimeLimit(" + Name + "), " + LimitTimes + "/" + ResetMilliseconds + "ms (" + countTimes + ")";
        public bool OutOfTime()
        {
            return startTime.AddMilliseconds(ResetMilliseconds) < DateTime.Now;
        }
    }
}
