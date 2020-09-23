using Silmoon.Threading;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading;

namespace Silmoon.Net.Transfer
{
    public class Tcp : ITcp
    {
        public event TcpTransferEventHandler OnEvent = null;
        public event TcpTransferEventHandler OnDataReceived = null;
        public Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public List<Socket> ClientSockets = new List<Socket>();
        public int BufferSize { get; set; } = 2048;
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
            try
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
            catch { }
        }
        public void SendData(byte[] data)
        {
            try
            {
                int i = socket.Send(data);
                //return i;
            }
            catch
            {
                CloseClientSocket(socket);
                //return -1;
            }
        }
        public void SendData(byte[] data, Socket clientSocket)
        {
            try
            {
                int i = clientSocket.Send(data);
                //return i;
            }
            catch
            {
                CloseClientSocket(clientSocket);
                //return -1;
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

            using (NetworkStream stream = new NetworkStream(socket))
            {
                if (socket.Connected)
                {
                    int recvLen;
                    do
                    {
                        byte[] recvBuff = new byte[BufferSize];
                        try
                        {
                            recvLen = stream.Read(recvBuff, 0, recvBuff.Length);
                        }
                        catch { recvLen = 0; }
                        if (recvLen != 0)
                        {
                            //Console.WriteLine(recvLen);
                            byte[] tdBuff = new byte[recvLen];
                            Array.Copy(recvBuff, tdBuff, recvLen);
                            onDataReceived(socket, tdBuff);

                        }
                    } while (socket.Connected && recvLen != 0);

                    if (!IsClientMode)
                        CloseClientSocket(socket);
                    //else Disconnect();

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
        void onDataReceived(Socket clientSocket, byte[] data)
        {
            OnDataReceived?.Invoke(this, new TcpEventArgs() { EventType = TcpEventType.ReceivedData, IPEndPoint = (IPEndPoint)clientSocket.RemoteEndPoint, Data = data, Socket = clientSocket });
        }
    }
    public delegate void TcpTransferEventHandler(object sender, TcpEventArgs e);
}
