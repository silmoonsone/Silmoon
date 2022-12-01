using System;
using System.Collections.Generic;
using System.Text;
using Silmoon.Arrays;

namespace Silmoon.Threading
{
    /// <summary>
    /// 控制一个动作或者行为在一定的时间内可以执行几次。
    /// </summary>
    public class TimeLimit : IId
    {
        int countTimes = 0;
        DateTime startTime = DateTime.Now;

        /// <summary>
        /// 表示当前类型的id标记
        /// </summary>
        public int Id { get; set; } = 0;
        /// <summary>
        /// 限制次数
        /// </summary>
        public int LimitTimes { get; set; } = 1;
        /// <summary>
        /// 查询是否可以继续
        /// </summary>
        public bool CanDo()
        {

            if ((DateTime.Now - startTime).TotalMilliseconds < ResetMilliseconds)
            {
                if (countTimes >= LimitTimes)
                    return false;
                else
                {
                    countTimes++;
                    return true;
                }
            }
            startTime = DateTime.Now;
            countTimes = 0;
            return true;
        }
        /// <summary>
        /// 查询是否可以继续，查询一次次数加一
        /// </summary>
        public bool CanDo(bool addTimes)
        {

            if ((DateTime.Now - startTime).TotalMilliseconds < ResetMilliseconds)
            {
                if (countTimes >= LimitTimes)
                    return false;
                else
                {
                    if (addTimes)
                        countTimes++;
                    return true;
                }
            }
            startTime = DateTime.Now;
            countTimes = 0;
            return true;
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
        public TimeLimit(int id) => Id = id;
        /// <summary>
        /// 新实例TimeLimit
        /// </summary>
        public TimeLimit()
        {

        }
        public TimeLimit(ulong resetMillisecounds, int limitTimes)
        {
            ResetMilliseconds = resetMillisecounds;
            LimitTimes = limitTimes;
        }
        public TimeLimit(TimeSpan resetTimespan, int limitTimes)
        {
            ResetTimespan = resetTimespan;
            LimitTimes = limitTimes;
        }
        public override string ToString() => "TimeLimit(" + Id + ")" + ResetMilliseconds + "/" + LimitTimes + "(" + countTimes + ")";
    }
}
