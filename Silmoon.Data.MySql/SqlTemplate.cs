using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Data
{
    /// <summary>
    /// 公用的SQL数据源模版
    /// </summary>
    public class SqlCommonTemplate
    {
        /// <summary>
        /// 检查SQL字段字符串是否是安全的。
        /// </summary>
        /// <param name="sqlString">SQL字段字符串</param>
        /// <returns></returns>
        public static bool CheckSqlStringSecurity(string sqlString)
        {
            sqlString = sqlString.Replace("''", "");
            return !(sqlString.IndexOf("'") != -1);
        }
        /// <summary>
        /// 检查SQL字段字符串是否是安全的。
        /// </summary>
        /// <param name="sqlString">SQL字段字符串</param>
        /// <returns></returns>
        public static string GetSecuritySqlString(string sqlString)
        {
            if (CheckSqlStringSecurity(sqlString))
                throw new System.Security.SecurityException("检测到存在危险的SQL查询语句。");
            else return sqlString;
        }
        /// <summary>
        /// 过滤和隔离SQL危险字符串，过滤前不检查原本是否安全，全部强行过滤。
        /// </summary>
        /// <param name="sqlString">SQL字段字符串</param>
        /// <returns></returns>
        public string InjectFieldReplace(string sqlString)
        {
            if (sqlString == null) return sqlString;
            return sqlString.Replace("'", "''");
        }
    }
}
