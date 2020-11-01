using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Silmoon.Net.Transfer
{
    public class TcpObjectTransport<T> : Tcp where T : new()
    {
        Dictionary<IPEndPoint, List<byte>> clientCachedData = new Dictionary<IPEndPoint, List<byte>>();
        List<byte> serverCacheData = new List<byte>();
        public event TcpObjectReceiveHandler<T> OnObjectReceive;

        string headerStr = "\x1\0\0\0s\0i\0l\0m\0o\0o\0n\0\0";
        int objectDataSize = 0;
        T blockReadObjectCache = default;
        ManualResetEvent blockResetEvent = null;

        public Encoding Encoding { get; set; } = Encoding.UTF8;
        public TcpObjectTransport()
        {
            OnEvent += TcpObjectTransfer_OnEvent;
            OnDataReceived += TcpObjectTransfer_OnDataReceived;
        }

        void TcpObjectTransfer_OnEvent(object sender, TcpEventArgs e)
        {
            switch (e.EventType)
            {
                case TcpEventType.ListenStarted:
                    break;
                case TcpEventType.ListenStoped:
                    break;
                case TcpEventType.ServerConnecting:
                    break;
                case TcpEventType.ServerConnected:
                    clientCachedData.Add(e.IPEndPoint, new List<byte>());
                    break;
                case TcpEventType.ServerConnectFailed:
                    break;
                case TcpEventType.ServerDisconnected:
                    blockResetEvent?.Set();
                    clientCachedData.Remove(e.IPEndPoint);
                    break;
                case TcpEventType.ClientConnected:
                    lock (clientCachedData)
                    {
                        clientCachedData.Add(e.IPEndPoint, new List<byte>());
                    }
                    break;
                case TcpEventType.ClientDisconnected:
                    lock (clientCachedData)
                    {
                        clientCachedData.Remove(e.IPEndPoint);
                    }
                    break;
                case TcpEventType.ReceivedData:
                    break;
                default:
                    break;
            }
        }
        void TcpObjectTransfer_OnDataReceived(object sender, TcpEventArgs e)
        {
            var clitObj = clientCachedData[e.IPEndPoint];
            clitObj.AddRange(e.Data);

            string dataStr = null;
            if (objectDataSize == 0)
            {
                dataStr = Encoding.GetString(clitObj.ToArray());
                var ti = dataStr.IndexOf(headerStr);
                if (ti != -1)
                {
                    if (ti != 0)
                    {
                        ///如果协议头不是从接收到的数据开始算的，那么说明数据头之前有无用的数据，这里会清除无用的数据。
                        dataStr = dataStr.Substring(ti, dataStr.Length - ti);
                        clitObj.RemoveRange(0, ti);
                    }

                    byte[] data = clitObj.ToArray();
                    if (dataStr.Length >= 23)
                    {
                        ///在数据头完整的情况下（接收到的数据大于23（19+4））开始分析数据长度信息。
                        int len = BitConverter.ToInt32(data, 19);
                        objectDataSize = len;
                        //Console.WriteLine("header recv " + len);
                        clitObj.RemoveRange(0, 23);
                    }
                    if (dataStr.Length > 23)
                    {
                        ///这次分析数据头的时候，包含了数据主体。
                        if (clitObj.Count >= objectDataSize)
                        {
                            ///BUG!!!
                            ///如果收到的数据主体长度够了。
                            onReceiveObjectCompleted(e, clitObj);
                        }

                    }
                }
                else
                    if (clitObj.Count > 1024) clitObj.Clear();
            }
            else
                if (clitObj.Count >= objectDataSize) onReceiveObjectCompleted(e, clitObj);

        }

        public void SendObject(T obj)
        {
            int i = 0;
            if (!socket.Connected) return;
            if (typeof(T) == typeof(string))
                SendData(MakeData((string)(object)obj));
            else if (typeof(T) == typeof(byte[]))
                SendData(MakeData((byte[])(object)obj));
            else if (typeof(T) == typeof(object))
                SendData(MakeData(JsonConvert.SerializeObject(obj)));
            else
                SendData(MakeData(JsonConvert.SerializeObject(obj)));
        }
        public int SendObject(T obj, TcpObjectReceiveArgs<T> args)
        {
            return SendObject(obj, args.Socket);
        }
        public int SendObject(T obj, Socket clientSocket)
        {
            int i = 0;
            if (!clientSocket.Connected) return i;
            if (typeof(T) == typeof(string))
                SendData(MakeData((string)(object)obj), clientSocket);
            else if (typeof(T) == typeof(byte[]))
                SendData(MakeData((byte[])(object)obj), clientSocket);
            else if (typeof(T) == typeof(object))
                SendData(MakeData(JsonConvert.SerializeObject(obj)), clientSocket);
            else
                SendData(MakeData(JsonConvert.SerializeObject(obj)), clientSocket);
            return i;
        }
        public byte[] MakeData(string str)
        {
            byte[] data = Encoding.GetBytes(str);
            return MakeData(data);
        }
        public byte[] MakeData(byte[] data)
        {
            int len = data.Length;
            byte[] sendData = new byte[len + 23];
            Array.Copy(Encoding.GetBytes(headerStr), sendData, headerStr.Length);
            Array.Copy(BitConverter.GetBytes(len), 0, sendData, headerStr.Length, 4);
            Array.Copy(data, 0, sendData, headerStr.Length + 4, len);
            return sendData;
        }

        public T GetObject(int timeout = 3000)
        {
            if (blockReadObjectCache == null)
            {
                if (blockResetEvent == null) blockResetEvent = new ManualResetEvent(false);
                blockResetEvent.WaitOne(timeout);

                T obj = blockReadObjectCache;
                blockReadObjectCache = default;

                return obj;
            }
            else
            {
                T obj = blockReadObjectCache;
                blockReadObjectCache = default;

                return obj;
            }
        }
        void onReceiveObjectCompleted(TcpEventArgs e, List<byte> data)
        {
            if (typeof(T) == typeof(string))
            {
                T obj = (T)(object)Encoding.GetString(data.ToArray());
                blockReadObjectCache = obj;
                blockResetEvent?.Set();
                OnObjectReceive?.Invoke(this, new TcpObjectReceiveArgs<T>() { Data = e.Data, EventType = e.EventType, IPEndPoint = e.IPEndPoint, Object = obj, Socket = e.Socket, TcpArgs = e });
            }
            else if (typeof(T) == typeof(byte[]))
            {
                T obj = (T)(object)data.ToArray();
                blockReadObjectCache = obj;
                blockResetEvent?.Set();
                OnObjectReceive?.Invoke(this, new TcpObjectReceiveArgs<T>() { Data = e.Data, EventType = e.EventType, IPEndPoint = e.IPEndPoint, Object = obj, Socket = e.Socket, TcpArgs = e });
            }
            else if (typeof(T) == typeof(object))
            {
                T obj = (T)JsonConvert.DeserializeObject(Encoding.GetString(data.ToArray()), typeof(T));
                blockReadObjectCache = obj;
                blockResetEvent?.Set();
                OnObjectReceive?.Invoke(this, new TcpObjectReceiveArgs<T>() { Data = e.Data, EventType = e.EventType, IPEndPoint = e.IPEndPoint, Object = obj, Socket = e.Socket, TcpArgs = e });
            }
            else
            {
                T obj = (T)JsonConvert.DeserializeObject(Encoding.GetString(data.ToArray()), typeof(T));
                blockReadObjectCache = obj;
                blockResetEvent?.Set();
                OnObjectReceive?.Invoke(this, new TcpObjectReceiveArgs<T>() { Data = e.Data, EventType = e.EventType, IPEndPoint = e.IPEndPoint, Object = obj, Socket = e.Socket, TcpArgs = e });
            }

            data.Clear();
            objectDataSize = 0;

        }
    }
    public delegate void TcpObjectReceiveHandler<T>(object sender, TcpObjectReceiveArgs<T> e);

}
