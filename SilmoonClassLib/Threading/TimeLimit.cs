using System;
using System.Collections.Generic;
using System.Text;
using Silmoon.Arrays;

namespace Silmoon.Threading
{
    /// <summary>
    /// 控制一个动作或者行为在一定的时间内可以执行几次。
    /// </summary>
    public class TimeLimit : IID
    {
        int iD = 0;
        ulong resetMilliseconds = 1000;
        int limitTimes = 1;
        int countTimes = 0;
        DateTime startTime = DateTime.Now;

        /// <summary>
        /// 表示当前类型的id标记
        /// </summary>
        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }
        /// <summary>
        /// 限制次数
        /// </summary>
        public int LimitTimes
        {
            get { return limitTimes; }
            set { limitTimes = value; }
        }
        /// <summary>
        /// 查询是否可以继续
        /// </summary>
        public bool CanDo()
        {

            if ((DateTime.Now - startTime).TotalMilliseconds < resetMilliseconds)
            {
                if (countTimes >= limitTimes)
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

            if ((DateTime.Now - startTime).TotalMilliseconds < resetMilliseconds)
            {
                if (countTimes >= limitTimes)
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
        public void AddTimes(int times)
        {
            countTimes = countTimes + times;
        }
        /// <summary>
        /// 控制的时间范围，以毫秒为单位的设置
        /// </summary>
        public ulong ResetMilliseconds
        {
            get { return resetMilliseconds; }
            set { resetMilliseconds = value; }
        }
        /// <summary>
        /// 控制的时间范围，以时间间隔为单位的设置
        /// </summary>
        public TimeSpan ResetTimespan
        {
            set
            {
                resetMilliseconds = (ulong)value.TotalMilliseconds;
            }
        }
        
        /// <summary>
        /// 以一个id号开始的新实例TimeLimit
        /// </summary>
        /// <param name="id"></param>
        public TimeLimit(int id)
        {
            iD = id;
        }
        /// <summary>
        /// 新实例TimeLimit
        /// </summary>
        public TimeLimit()
        {

        }
        public TimeLimit(ulong resetMillisecounds, int limitTimes)
        {
            ResetMilliseconds = resetMilliseconds;
            LimitTimes = limitTimes;
        }
        public TimeLimit(TimeSpan resetTimespan, int limitTimes)
        {
            ResetTimespan = resetTimespan;
            LimitTimes = limitTimes;
        }
        public override string ToString()
        {
            return "TimeLimit(" + iD + ")" + resetMilliseconds + "/" + limitTimes + "(" + countTimes + ")";
        }
    }
}
