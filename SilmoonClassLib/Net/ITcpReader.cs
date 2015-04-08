using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace Silmoon.Net
{
    public interface ITcpReader
    {
        event TcpReceiveDataEventHander OnReceivedData;

        int ClientID
        {
            get;
        }
        TcpClient Client
        {
            get;
        }
        StateFlag ObjectFlag
        {
            get;
            set;
        }

        void SendData(byte[] byteData);
        void SendString(string s);
        void CloseConnect();
    }
}
