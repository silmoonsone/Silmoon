using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Silmoon.Net.Transfer
{
    public interface ITcp : IDisposable
    {
        bool Connect(IPEndPoint endPoint);
        void Disconnect();
        void SendData(byte[] data);
        void SendData(byte[] data, Socket clientSocket);
        bool StartListen(int backlog, IPEndPoint endPoint);
        void CloseAllClientSockets();
        void CloseClientSocket(Socket clientSocket);
        event TcpTransferEventHandler OnEvent;
        event TcpTransferEventHandler OnDataReceived;

    }
}
