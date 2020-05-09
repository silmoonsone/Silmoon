using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Web;
using Silmoon.Types;

namespace Silmoon.MySilmoon
{
    /// <summary>
    /// SilmoonLogger日志器
    /// </summary>
    public class SL
    {
        public event LogHandler OnPushLogHandler;
        /// <summary>
        /// 创建一个日志器
        /// </summary>
        public SL()
        {

        }
        /// <summary>
        /// 向状态服务器推送日志
        /// </summary>
        /// <param name="sourceName">sourceName</param>
        /// <param name="sourceUserID">sourceUserID</param>
        /// <param name="shortLog">shortLog</param>
        /// <returns>推送结果状态</returns>
        public SystemStateCode PushStateLog(string sourceName, string sourceUserID, string shortLog)
        {
            return PushStateLog(sourceName, sourceUserID, shortLog, null);
        }
        /// <summary>
        /// 向状态服务器推送日志
        /// </summary>
        /// <param name="sourceName">sourceName</param>
        /// <param name="sourceUserID">sourceUserID</param>
        /// <param name="shortLog">shortLog</param>
        /// <param name="log">log</param>
        /// <param name="data">data</param>
        /// <param name="data1">data1</param>
        /// <param name="data2">data2</param>
        /// <param name="flag">flag</param>
        /// <param name="flag1">flag1</param>
        /// <param name="flag2">flag2</param>
        /// <returns>推送结果状态</returns>
        public SystemStateCode PushStateLog(string sourceName, string sourceUserID, string shortLog, string log = null, string data = null, string data1 = null, string data2 = null, string flag = null, string flag1 = null, string flag2 = null)
        {
            if (sourceName == null) return SystemStateCode.PARAM_ERROR;
            if (sourceUserID == null) return SystemStateCode.PARAM_ERROR;
            if (shortLog == null) return SystemStateCode.PARAM_ERROR;

            NameValueCollection para = new NameValueCollection();
            para.Add("sourceName", sourceName);
            para.Add("sourceUserID", sourceUserID);
            para.Add("shortLog", shortLog);
            if (log != null) para.Add("log", log);
            if (data != null) para.Add("data", data);
            if (data1 != null) para.Add("data1", data1);
            if (data2 != null) para.Add("data2", data2);
            if (flag != null) para.Add("flag", flag);
            if (flag1 != null) para.Add("flag1", flag1);
            if (flag2 != null) para.Add("flag2", flag2);
            using (WebClient wc = new WebClient())
            {
                wc.Proxy = null;
                wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                string result = wc.UploadString("https://stateserver.silmoon.com/pushStateLog", MakeQueryString(para));
                if (result.ToLower() == "success") return SystemStateCode.SUCCESS;
            }
            return SystemStateCode.FAIL;
        }
        string MakeQueryString(NameValueCollection parameters)
        {
            string result = string.Empty;
            for (int i = 0; i < parameters.Count; i++)
            {
                result += "&" + HttpUtility.UrlEncode(parameters.GetKey(i)) + "=" + HttpUtility.UrlEncode(parameters[i]);
            }
            return result.Substring(1, result.Length - 1);
        }
        void onPushLogHandler(string sourceName, string sourceUserID, string shortLog, string log = null, string data = null, string data1 = null, string data2 = null, string flag = null, string flag1 = null, string flag2 = null)
        {
            if (OnPushLogHandler != null) OnPushLogHandler(this, sourceName, sourceUserID, shortLog, log, data, data1, data2, flag, flag1, flag2);
        }

        public delegate void LogHandler(object sender, string sourceName, string sourceUserID, string shortLog, string log = null, string data = null, string data1 = null, string data2 = null, string flag = null, string flag1 = null, string flag2 = null);
    }

}
