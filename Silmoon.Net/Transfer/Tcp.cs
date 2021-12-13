using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Net;
using System.Net.Sockets;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Silmoon.Net.Transfer
{
    public class Tcp : ITcp
    {
        public event TcpTransferEventHandler OnEvent = null;
        public event TcpTransferEventHandler OnDataReceived = null;
        public Socket socket = null;
        public List<Socket> ClientSockets = new List<Socket>();
        public int BufferSize { get; set; } = 20480;
        public bool IsClientMode { get; set; }
        public TcpState State { get; set; }
        public Tcp()
        {

        }

        public void Dispose()
        {
            try
            {
                Disconnect();
                socket?.Close();
                socket?.Dispose();
                CloseAllClientSockets();
                socket = null;
            }
            catch { }
        }


        public bool Connect(IPEndPoint endPoint)
        {
            try
            {
                onEvent(TcpEventType.ServerConnecting, endPoint, socket);
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(endPoint);
                onEvent(TcpEventType.ServerConnected, endPoint, socket);
                Task.Run(() =>
                {
                    Thread.CurrentThread.Name = "socket async thread(" + socket.RemoteEndPoint + ")";
                    Receive(socket);
                });
                IsClientMode = true;
                return true;
            }
            catch
            {
                onEvent(TcpEventType.ServerConnectFailed, endPoint, socket);
                return false;
            }
        }
        public void Disconnect()
        {
            if (socket?.Connected ?? false)
            //if (socket.Connected)
            {
                try
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Disconnect(false);
                }
                catch
                { }
            }
        }
        void readSocketCloseProcess()
        {
            if (State == TcpState.ServerDisconnected || socket == null) return;

            try
            {
                //这里在断开连接的时候，如果同时Dispose掉，可能会获取不到RemoteEndPoint，所以try外定义ep，try内获取。
                EndPoint ep = default;
                try
                {
                    ep = socket.RemoteEndPoint;
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                    socket.Dispose();
                    socket = null;
                }
                catch { }
                finally
                {
                    onEvent(TcpEventType.ServerDisconnected, (IPEndPoint)ep, socket);
                }
            }
            catch { }
        }
        public void SendData(byte[] data, int offset = 0, int size = -1)
        {
            try
            {
                if (size == -1) size = data.Length;
                int i = socket.Send(data, offset, size, SocketFlags.None);
                //return i;
            }
            catch
            {
                CloseClientSocket(socket);
                //return -1;
            }
        }
        public void SendData(byte[] data, Socket clientSocket, int offset = 0, int size = -1)
        {
            try
            {
                if (size == -1) size = data.Length;
                int i = clientSocket.Send(data, offset, size, SocketFlags.None);
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
            if (socket is null)
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Bind(endPoint);
                socket.Listen(backlog);
                onEvent(TcpEventType.ListenStarted, endPoint, socket);
                socket.BeginAccept(OnClientConnected, null);

                IsClientMode = false;
                return true;
            }
            else
            {
                return false;
            }
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
            lock (ClientSockets)
            {
                if (!ClientSockets.Contains(clientSocket)) return;
                EndPoint ep = default;
                try
                {
                    ep = clientSocket.RemoteEndPoint;
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                }
                catch { }
                finally
                {
                    ClientSockets.Remove(clientSocket);
                    clientSocket.Dispose();
                    onEvent(TcpEventType.ClientDisconnected, (IPEndPoint)ep, clientSocket);
                }
            }
        }

        void OnClientConnected(IAsyncResult ar)
        {
            socket.BeginAccept(OnClientConnected, null);
            var acceptedSocket = socket.EndAccept(ar);
            lock (ClientSockets)
            {
                if (!IsClientMode) ClientSockets.Add(acceptedSocket);
            }
            onEvent(TcpEventType.ClientConnected, (IPEndPoint)acceptedSocket.RemoteEndPoint, acceptedSocket);
            Receive(acceptedSocket);
        }

        Task Receive(Socket socket)
        {
            return Task.Run(() =>
            {
                byte[] recvBuff = new byte[BufferSize];

                SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                args.SetBuffer(recvBuff, 0, BufferSize);

                args.Completed += (s, e) =>
                {
                    if (args.BytesTransferred > 0) onDataReceived(socket, recvBuff, args.BytesTransferred);
                    else
                    {
                        if (!IsClientMode) CloseClientSocket(socket); else readSocketCloseProcess();
                        return;
                    }
                    while (socket.Connected && !socket.ReceiveAsync(args))
                    {
                        if (args.BytesTransferred > 0) onDataReceived(socket, recvBuff, args.BytesTransferred);
                        else
                        {
                            if (!IsClientMode) CloseClientSocket(socket); else readSocketCloseProcess();
                            break;
                        }
                    }
                };
                while (!socket.ReceiveAsync(args))
                {
                    if (args.BytesTransferred > 0) onDataReceived(socket, recvBuff, args.BytesTransferred);
                    else
                    {
                        if (!IsClientMode) CloseClientSocket(socket); else readSocketCloseProcess();
                        break;
                    }
                }
            });
        }
        Task Receive2(Socket socket)
        {
            return Task.Run(() =>
            {
                try
                {
                    int recvLen;
                    byte[] recvBuff = new byte[BufferSize];
                    do
                    {
                        recvLen = socket.Receive(recvBuff);
                        if (recvLen != 0)
                        {
                            onDataReceived(socket, recvBuff, recvLen);
                        }
                    } while (socket.Connected && recvLen != 0);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    if (!IsClientMode) CloseClientSocket(socket);
                    else readSocketCloseProcess();
                }
            });


        }

        void onEvent(TcpEventType eventType, IPEndPoint endPoint, Socket socket)
        {
            switch (eventType)
            {
                case TcpEventType.ListenStarted:
                    State = TcpState.ListenStarted;
                    break;
                case TcpEventType.ListenStoped:
                    State = TcpState.ListenStoped;
                    break;
                case TcpEventType.ServerConnecting:
                    State = TcpState.ServerConnecting;
                    break;
                case TcpEventType.ServerConnected:
                    State = TcpState.ServerConnected;
                    break;
                case TcpEventType.ServerConnectFailed:
                    State = TcpState.ServerConnectFail;
                    break;
                case TcpEventType.ServerDisconnected:
                    State = TcpState.ServerDisconnected;
                    break;
                default:
                    break;
            };
            OnEvent?.Invoke(this, new TcpEventArgs() { EventType = eventType, IPEndPoint = endPoint, Socket = socket });
        }
        void onDataReceived(Socket clientSocket, byte[] buffer, int len)
        {
            byte[] copiedBuffer = new byte[len];
            Array.Copy(buffer, copiedBuffer, len);

            OnDataReceived?.Invoke(this, new TcpEventArgs() { EventType = TcpEventType.ReceivedData, IPEndPoint = (IPEndPoint)clientSocket.RemoteEndPoint, Data = copiedBuffer, Socket = clientSocket });
        }
    }
    public delegate void TcpTransferEventHandler(object sender, TcpEventArgs e);
}
