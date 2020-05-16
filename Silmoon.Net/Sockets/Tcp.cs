using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Silmoon.Net.Sockets
{
    public class Tcp : IDisposable
    {
        public event TcpTransferEventHandler OnEvent = null;
        public event TcpTransferEventHandler OnDataReceived = null;
        public Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public List<Socket> ClientSockets = new List<Socket>();

        public Tcp()
        {

        }

        public void Dispose()
        {
            if (socket.Connected)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            socket.Dispose();
            socket = null;
        }

        public bool Connect(IPEndPoint endPoint)
        {
            OnEvent?.Invoke(this, new TcpEventArgs() { EventType = TcpEventType.ServerConnecting, IPEndPoint = endPoint });
            try
            {
                socket.Connect(endPoint);
                OnEvent?.Invoke(this, new TcpEventArgs() { EventType = TcpEventType.ServerConnected, IPEndPoint = endPoint });
                return true;
            }
            catch
            {
                OnEvent?.Invoke(this, new TcpEventArgs() { EventType = TcpEventType.ServerConnectFailed, IPEndPoint = endPoint });
                return false;
            }
        }
        public int SendData(byte[] data)
        {
            int i = socket.Send(data);
            return i;
        }
        public bool StartListen(int backlog, IPEndPoint endPoint)
        {
            socket.Bind(endPoint);
            socket.Listen(backlog);
            OnEvent?.Invoke(this, new TcpEventArgs() { EventType = TcpEventType.ListenStarted, IPEndPoint = endPoint });
            EnableReceive();
            return true;
        }
        public void CloseAllClientSockets()
        {
            List<Socket> readyCloseSocket = new List<Socket>();
            lock (ClientSockets)
            {
                foreach (var item in ClientSockets)
                {
                    readyCloseSocket.Add(item);
                }

                foreach (var item in readyCloseSocket)
                {
                    CloseClientSocket(item);
                }
            }
        }
        public void CloseClientSocket(Socket clientSocket)
        {
            if (!ClientSockets.Contains(clientSocket)) return;

            lock (clientSocket)
            {
                var ep = clientSocket.RemoteEndPoint;
                try
                {
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Disconnect(false);
                    clientSocket.Close();
                    clientSocket.Dispose();
                }
                finally
                {
                    clientSocketCloseProcess(clientSocket);

                    OnEvent?.Invoke(this, new TcpEventArgs() { EventType = TcpEventType.ClientDisconnected, IPEndPoint = (IPEndPoint)ep });
                }
            }
        }

        void EnableReceive()
        {
            socket.BeginAccept(OnClientConnected, null);
        }
        void OnClientConnected(IAsyncResult ar)
        {
            var csocket = socket.EndAccept(ar);
            socket.BeginAccept(OnClientConnected, null);

            OnEvent?.Invoke(this, new TcpEventArgs() { EventType = TcpEventType.ClientConnected, IPEndPoint = (IPEndPoint)csocket.RemoteEndPoint });
            ClientSockets.Add(csocket);
            while (csocket.Connected)
            {
                int recvLen = 0;
                byte[] recvBuff = new byte[1024];

                try
                {

                    recvLen = csocket.Receive(recvBuff);
                }
                catch
                {
                    CloseClientSocket(csocket);
                }

                if (recvLen == 0)
                {
                    CloseClientSocket(csocket);
                    break;
                }
                else
                {
                    byte[] tdBuff = new byte[recvLen];
                    Array.Copy(recvBuff, tdBuff, recvLen);

                    recvDataProcess(csocket, tdBuff);
                }

            }
        }


        void clientSocketCloseProcess(Socket clientSocket)
        {
            lock (ClientSockets)
            {
                ClientSockets.Remove(clientSocket);
            }
        }
        void recvDataProcess(Socket clientSocket, byte[] data)
        {
            OnDataReceived?.Invoke(this, new TcpEventArgs() { EventType = TcpEventType.ReceivedData, IPEndPoint = (IPEndPoint)clientSocket.RemoteEndPoint, Data = data });
        }
    }
    public delegate void TcpTransferEventHandler(object sender, TcpEventArgs e);
}
