using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Silmoon.Net.Transfer
{
    public interface ITcp : IDisposable
    {
        TcpEventState State { get; set; }
        bool Connect(IPEndPoint endPoint);
        void Disconnect();
        void SendData(byte[] data, int offset = 0, int size = -1);
        void SendData(byte[] data, Socket clientSocket, int offset = 0, int size = -1);
        bool StartListen(int backlog, IPEndPoint endPoint);
        void CloseAllClientSockets();
        void CloseClientSocket(Socket clientSocket);
        event TcpTransferEventHandler OnEvent;
        event TcpTransferEventHandler OnDataReceived;

    }
}
