using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;

namespace Silmoon.Net.Protocol
{
    public class HttpLikePacket
    {
        /// <summary>
        /// 数据的第一行标题栏
        /// </summary>
        public string TitleLine { get; set; }
        /// <summary>
        /// 数据的消息集合
        /// </summary>
        public NameValueCollection Message { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public byte[] Data { get; set; }

        public Encoding Encoding { get; set; }

        public HttpLikePacket()
        {
            Init();
        }
        public HttpLikePacket(Encoding encoding)
        {
            Init();
            Encoding = encoding;
        }
        public HttpLikePacket(byte[] data)
        {
            Init();
            ReadFromBytes(data);
        }
        public HttpLikePacket(byte[] data, Encoding encoding)
        {
            Init();
            ReadFromBytes(data);
            Encoding = encoding;
        }

        void Init()
        {
            Encoding = Encoding.UTF8;
            Message = new NameValueCollection();
            if (Data == null) Data = new byte[0];
        }

        public string GetDataString()
        {
            if (Data != null)
                return Encoding.GetString(Data);
            else return null;
        }
        public void SetByteData(string data)
        {
            Data = Encoding.GetBytes(data);
        }
        public byte[] GetBytes()
        {
            if (Message == null) return null;
            if (Data == null) Data = new byte[0];

            string headerString = TitleLine + "\r\n";
            for (int i = 0; i < Message.Count; i++)
            {
                headerString += Message.GetKey(i) + ":" + Message[i] + "\r\n";
            }

            byte[] headerData = Encoding.GetBytes(headerString);
            byte[] data = new byte[4 + headerData.Length + Data.Length];
            Array.Copy(BitConverter.GetBytes(headerData.Length), data, 4);
            Array.Copy(headerData, 0, data, 4, headerData.Length);
            Array.Copy(Data, 0, data, 4 + headerData.Length, Data.Length);
            return data;
        }
        public bool ReadFromBytes(byte[] rawData)
        {
            if (rawData.Length < 5) return false;
            int headerLenght = BitConverter.ToInt32(rawData, 0);
            if (rawData.Length < headerLenght + 4) return false;
            string headerString = Encoding.GetString(rawData, 4, headerLenght);
            string[] headerStrings = headerString.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            TitleLine = headerStrings[0];
            if (Message == null) Message = new NameValueCollection();
            else Message.Clear();
            for (int i = 1; i < headerStrings.Length; i++)
            {
                string[] lineArr = headerStrings[i].Split(new string[] { ":" }, 2, StringSplitOptions.None);
                if (lineArr.Length == 2)
                    Message[lineArr[0]] = lineArr[1];
            }

            Data = new byte[rawData.Length - 4 - headerLenght];
            Array.Copy(rawData, rawData.Length - Data.Length, Data, 0, Data.Length);

            return true;
        }
        public void Clear()
        {
            TitleLine = "";
            Message.Clear();
            Data = new byte[0];
        }
        public void Clear(string title)
        {
            TitleLine = title;
            Message.Clear();
            Data = new byte[0];
        }
    }
}
