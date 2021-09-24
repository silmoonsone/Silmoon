using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Silmoon.Net.Transfer
{
    /// <summary>
    /// 还未完成的类
    /// </summary>
    public class TcpServer : ITcp
    {
        public TcpListener listener { get; set; }
        public TcpState State { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event TcpTransferEventHandler OnEvent;
        public event TcpTransferEventHandler OnDataReceived;

        public TcpServer()
        {
        }

        public void CloseAllClientSockets()
        {

        }

        public void CloseClientSocket(Socket clientSocket)
        {

        }

        public bool Connect(IPEndPoint endPoint)
        {
            throw new NotImplementedException();
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {

        }

        public void SendData(byte[] data, int offset = 0, int size = -1)
        {

        }

        public void SendData(byte[] data, Socket clientSocket, int offset = 0, int size = -1)
        {

        }

        public bool StartListen(int backlog, IPEndPoint endPoint)
        {
            if (!(listener is null))
            {
                throw new Exception("TCP服务器正在监听！不可尝试新的监听动作。");
            }

            listener = new TcpListener(endPoint);
            listener.Start(backlog);
            return true;
        }
    }
}
