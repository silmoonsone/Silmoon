using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Types
{
    /// <summary>
    /// 以实际的时间表示一个时间段，比如上午10点到下午2点。
    /// </summary>
    public class TimeSection
    {
        DateTime _index;
        TimeSpan _length;

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime Index
        {
            get { return _index; }
            set { _index = value; }
        }
        /// <summary>
        /// 开始时间到结束时间的时长
        /// </summary>
        public TimeSpan Length
        {
            get { return _length; }
            set { _length = value; }
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime
        {
            get
            {
                DateTime dt = Index.Add(Length);
                return dt;
            }
            set { Length = ((TimeSpan)(EndTime - Index)); }
        }
        public TimeSection()
        {

        }
        public TimeSection(DateTime index, TimeSpan length)
        {
            Index = index;
            Length = length;
        }
        public TimeSection(DateTime index, DateTime endTime)
        {
            Index = index;
            EndTime = endTime;
        }
        public TimeSection(TimeSpan length, DateTime endTime)
        {
            Index = endTime - length;
        }
        public override string ToString()
        {
            return Index.ToString("yyyy-MM-dd HH:mm:ss") + "->" + EndTime.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
