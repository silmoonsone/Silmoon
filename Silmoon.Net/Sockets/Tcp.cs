using Silmoon.Threading;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Silmoon.Net.Sockets
{
    public class Tcp : IDisposable
    {
        public event TcpTransferEventHandler OnEvent = null;
        public event TcpTransferEventHandler OnDataReceived = null;
        public Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public List<Socket> ClientSockets = new List<Socket>();
        public bool IsClientMode { get; set; }
        public Tcp()
        {

        }

        public void Dispose()
        {
            if (socket.Connected)
                Disconnect();
            socket.Dispose();
        }

        public bool Connect(IPEndPoint endPoint)
        {
            OnEvent?.Invoke(this, new TcpEventArgs() { EventType = TcpEventType.ServerConnecting, IPEndPoint = endPoint });
            try
            {
                socket.Connect(endPoint);
                OnEvent?.Invoke(this, new TcpEventArgs() { EventType = TcpEventType.ServerConnected, IPEndPoint = endPoint, Socket = socket });
                ThreadHelper.ExecAsync(new ThreadStart(() =>
                {
                    EnableReceive(socket);
                }));
                IsClientMode = true;
                return true;
            }
            catch (Exception e)
            {
                OnEvent?.Invoke(this, new TcpEventArgs() { EventType = TcpEventType.ServerConnectFailed, IPEndPoint = endPoint, Socket = socket });
                return false;
            }
        }
        public void Disconnect()
        {
            var ep = socket.RemoteEndPoint;

            try
            {

                socket.Shutdown(SocketShutdown.Both);
                socket.Disconnect(true);
                OnEvent?.Invoke(this, new TcpEventArgs() { EventType = TcpEventType.ServerDisconnected, IPEndPoint = (IPEndPoint)ep, Socket = socket });
            }
            catch
            {
                OnEvent?.Invoke(this, new TcpEventArgs() { EventType = TcpEventType.ServerDisconnected, IPEndPoint = (IPEndPoint)ep, Socket = socket });
            }
        }
        public int SendData(byte[] data)
        {
            try
            {
                int i = socket.Send(data);
                return i;
            }
            catch
            {
                CloseClientSocket(socket);
                return -1;
            }
        }
        public int SendData(byte[] data, Socket clientSocket)
        {
            try
            {
                int i = clientSocket.Send(data);
                return i;
            }
            catch
            {
                CloseClientSocket(clientSocket);
                return -1;
            }
        }
        public bool StartListen(int backlog, IPEndPoint endPoint)
        {
            socket.Bind(endPoint);
            socket.Listen(backlog);
            OnEvent?.Invoke(this, new TcpEventArgs() { EventType = TcpEventType.ListenStarted, IPEndPoint = endPoint, Socket = socket });
            EnableAcceptAndReceive();
            IsClientMode = false;
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

                    OnEvent?.Invoke(this, new TcpEventArgs() { EventType = TcpEventType.ClientDisconnected, IPEndPoint = (IPEndPoint)ep, Socket = clientSocket });
                }
            }
        }

        void EnableAcceptAndReceive()
        {
            socket.BeginAccept(OnClientConnected, null);
        }
        void OnClientConnected(IAsyncResult ar)
        {
            var acceptedSocket = socket.EndAccept(ar);
            this.socket.BeginAccept(OnClientConnected, null);
            OnEvent?.Invoke(this, new TcpEventArgs() { EventType = TcpEventType.ClientConnected, IPEndPoint = (IPEndPoint)acceptedSocket.RemoteEndPoint, Socket = acceptedSocket });
            EnableReceive(acceptedSocket);
        }
        void EnableReceive(Socket socket)
        {
            lock (ClientSockets)
            {
                if (!IsClientMode) ClientSockets.Add(socket);
            }
            while (socket.Connected)
            {
                int recvLen = 0;
                byte[] recvBuff = new byte[1024];

                try
                {
                    recvLen = socket.Receive(recvBuff);
                }
                catch (Exception e)
                {
                    //if (!IsClientMode)
                    //    CloseClientSocket(socket);
                }

                if (recvLen == 0)
                {
                    if (!IsClientMode)
                        CloseClientSocket(socket);
                    else Disconnect();
                }
                else
                {
                    byte[] tdBuff = new byte[recvLen];
                    Array.Copy(recvBuff, tdBuff, recvLen);

                    recvDataProcess(socket, tdBuff);
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
            OnDataReceived?.Invoke(this, new TcpEventArgs() { EventType = TcpEventType.ReceivedData, IPEndPoint = (IPEndPoint)clientSocket.RemoteEndPoint, Data = data, Socket = clientSocket });
        }
    }
    public delegate void TcpTransferEventHandler(object sender, TcpEventArgs e);
}
