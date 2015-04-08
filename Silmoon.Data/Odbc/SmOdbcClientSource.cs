using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Data.Odbc
{
    /// <summary>
    /// 用于自己创建引用类型使用SmSqlClient数据源的控制与引用。
    /// </summary>
    public abstract class SmOdbcClientSource : IDisposable
    {
        SmOdbcClient _ssc;
        /// <summary>
        /// 设置或获取数据源
        /// </summary>
        public SmOdbcClient DataSource
        {
            get { return _ssc; }
            set { _ssc = value; }
        }
        /// <summary>
        /// 关闭数据连接
        /// </summary>
        public void Close()
        {
            _ssc.Close();
        }

        #region IDisposable 成员

        public void Dispose()
        {
            _ssc.Dispose();
        }

        #endregion

        /// <summary>
        /// 实例化数据类型和数据源
        /// </summary>
        /// <param name="open">是否在实例的时候打开数据库</param>
        /// <param name="conStr">指定连接数据库的数据库连接字符串</param>
        public void InitData(bool open, string conStr)
        {
            _ssc = new SmOdbcClient(conStr);
            if (open) { _ssc.Open(); }
        }
    }
}