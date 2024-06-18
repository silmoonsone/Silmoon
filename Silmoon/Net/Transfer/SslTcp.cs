using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Silmoon.Net.Transfer
{
    public class SslTcp : ITcp
    {
        SslStream sslStream = null;
        NetworkStream networkStream = null;

        public event TcpTransferEventHandler OnEvent = null;
        public event TcpTransferEventHandler OnDataReceived = null;
        public Socket socket = null;
        public List<Socket> ClientSockets = new List<Socket>();
        public int BufferSize { get; set; } = 2048;
        public bool IsClientMode { get; set; }
        public TcpState State { get; set; }
        public SslTcp()
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
        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
            //if (sslPolicyErrors == SslPolicyErrors.None)
            //    return true;

            //Console.WriteLine("Certificate error: {0}", sslPolicyErrors);
            //return false;
        }

        public bool Connect(IPEndPoint endPoint)
        {
            try
            {
                onEvent(TcpEventType.ServerConnecting, endPoint, socket);
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(endPoint);
                onEvent(TcpEventType.ServerConnected, endPoint, socket);
                if (networkStream != null)
                {
                    networkStream.Close();
                    networkStream.Dispose();
                }

                networkStream = new NetworkStream(socket);
                sslStream = new SslStream(networkStream, false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);
                X509Store store = new X509Store(StoreName.Root);
                store.Open(OpenFlags.ReadWrite);
                X509Certificate2Collection certs = store.Certificates.Find(X509FindType.FindBySubjectName, "TestClient", false);
                sslStream.AuthenticateAsClient("TestServer", certs, SslProtocols.Tls, false);

                Task.Run(() =>
                {
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
            if (socket.Connected)
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
                sslStream.Write(data, offset, size);
            }
            catch
            {
                CloseClientSocket(socket);
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
                    clientSocket.Dispose();
                    ClientSockets.Remove(clientSocket);
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
        Task Receive2(Socket socket)
        {
            return Task.Run(() =>
            {
                byte[] recvBuff = new byte[BufferSize];

                SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                args.SetBuffer(recvBuff, 0, BufferSize);

                args.Completed += (s, e) =>
                {
                    Console.WriteLine($"len={e.BytesTransferred}, error={e.SocketError}");
                    if (args.BytesTransferred > 0) onDataReceived(socket, recvBuff, args.BytesTransferred);
                    else
                    {
                        if (!IsClientMode) CloseClientSocket(socket); else readSocketCloseProcess();
                        return;
                    }

                    while (!socket.ReceiveAsync(args))
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
        void Receive(Socket socket)
        {
            if (socket.Connected)
            {
                int recvLen;
                do
                {
                    byte[] recvBuff = new byte[BufferSize];
                    try
                    {
                        recvLen = sslStream.Read(recvBuff, 0, recvBuff.Length);
                    }
                    catch { recvLen = 0; }
                    if (recvLen != 0)
                    {
                        onDataReceived(socket, recvBuff, recvLen);
                    }
                } while (socket.Connected && recvLen != 0);

                if (!IsClientMode)
                    CloseClientSocket(socket);
                else readSocketCloseProcess();
            }
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
            Console.WriteLine($"event recv len={len}");
            byte[] copiedBuffer = new byte[len];
            Array.Copy(buffer, copiedBuffer, len);

            OnDataReceived?.Invoke(this, new TcpEventArgs() { EventType = TcpEventType.ReceivedData, IPEndPoint = (IPEndPoint)clientSocket.RemoteEndPoint, Data = copiedBuffer, Socket = clientSocket });
        }
    }
}
