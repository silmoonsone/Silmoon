using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace Silmoon.Net.SmProtocol
{
    /// <summary>
    /// SM协议处理机智
    /// </summary>
    public class SmPackectProtocol
    {
        /// <summary>
        /// 新实例协议处理机制
        /// </summary>
        public SmPackectProtocol()
        {

        }
        /// <summary>
        /// 获取SM协议头结构数据
        /// </summary>
        /// <param name="packect">数据</param>
        /// <returns></returns>
        public ProtocalHeader IsProtocolHeader(byte[] packect)
        {
            ProtocalHeader result;
            result.PackectLength = 0;
            result.IsSmProtocol = false;


            string stringData = Encoding.Default.GetString(packect);
            if (stringData.Length < 5) return result;

            int startC = stringData.IndexOf("_sm_");
            if (startC == -1) return result;

            stringData = stringData.Substring(startC, stringData.Length - startC);

            if (stringData.Substring(0, 4) != "_sm_") return result;
            if (stringData.Substring(stringData.Length - 4, 4) != "_end") return result;
            string[] packInfoArr = stringData.Split(new string[] { "_" }, StringSplitOptions.None);
            if (packInfoArr.Length < 4) return result;
            if (Convert.ToInt64(packInfoArr[2]) == 0) return result;
            try
            {
                result.IsSmProtocol = true;
                result.PackectLength = Convert.ToInt64(packInfoArr[2]);
            }
            catch
            {
                result.IsSmProtocol = false;
                result.PackectLength = 0;
            }

            return result;
        }
        /// <summary>
        /// 根据SM协议头保存的状态读取数据
        /// </summary>
        /// <param name="status">SM协议装状态数据</param>
        /// <param name="byteData">数据</param>
        /// <returns></returns>
        public byte[] ReadFormSmProtocol(ref ProtocalStatusInfo status, byte[] byteData)
        {
            if (byteData.Length == status.PackectLength)
            {
                status.Received = false;
                return byteData;
            }
            return null;
        }
        /// <summary>
        /// 构建一个带有SM协议头的数据包
        /// </summary>
        /// <param name="byteData">数据包的内容数据</param>
        /// <returns></returns>
        public byte[] MakeByteData(byte[] byteData)
        {
            if (byteData.Length == 0) { return null; }

            byte[] headerData = Encoding.Default.GetBytes("_sm_" + byteData.Length + "_end");
            ArrayList resultArr = new ArrayList();
            foreach (byte b in headerData) resultArr.Add(b);
            foreach (byte b in byteData) resultArr.Add(b);
            byte[] resultBytes = (byte[])resultArr.ToArray(typeof(byte));
            resultArr.Clear();
            return resultBytes;
        }
    }
    /// <summary>
    /// 用于保存SM协议的状态
    /// </summary>
    public struct ProtocalStatusInfo
    {
        /// <summary>
        /// 是否已经接受到了SM协议头
        /// </summary>
        public bool Received;
        /// <summary>
        /// SM协议头中表示的内容长度
        /// </summary>
        public long PackectLength;
    }
    /// <summary>
    /// SM协议头结构
    /// </summary>
    public struct ProtocalHeader
    {
        public long PackectLength;
        public bool IsSmProtocol;
    }
}
