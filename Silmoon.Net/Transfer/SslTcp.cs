using Silmoon.Threading;
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
        public Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public List<Socket> ClientSockets = new List<Socket>();
        public int BufferSize { get; set; } = 2048;
        public bool IsClientMode { get; set; }
        public TcpEventState State { get; set; }
        public SslTcp()
        {

        }

        public void Dispose()
        {
            if (socket.Connected) Disconnect();
            socket.Dispose();
            sslStream?.Dispose();
            networkStream?.Dispose();
        }
        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;

            Console.WriteLine("Certificate error: {0}", sslPolicyErrors);

            // Do not allow this client to communicate with unauthenticated servers.
            return false;
        }

        public bool Connect(IPEndPoint endPoint)
        {
            onEvent(TcpEventState.ServerConnecting, endPoint, socket);
            try
            {
                socket.Connect(endPoint);
                onEvent(TcpEventState.ServerConnected, endPoint, socket);
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
                    EnableReceive(socket);
                });

                IsClientMode = true;
                return true;
            }
            catch (Exception e)
            {
                onEvent(TcpEventState.ServerConnectFailed, endPoint, socket);

                return false;
            }
        }
        public void Disconnect()
        {
            if (State == TcpEventState.ServerDisconnected) return;

            try
            {
                var ep = socket.RemoteEndPoint;

                try
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Disconnect(true);
                }
                catch
                { }
                finally
                {
                    onEvent(TcpEventState.ServerDisconnected, (IPEndPoint)ep, socket);
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
            socket.Bind(endPoint);
            socket.Listen(backlog);

            onEvent(TcpEventState.ListenStarted, endPoint, socket);

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
                    clientSocket.Dispose();
                }
                finally
                {
                    clientSocketCloseProcess(clientSocket);
                    onEvent(TcpEventState.ClientDisconnected, (IPEndPoint)ep, clientSocket);
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
            socket.BeginAccept(OnClientConnected, null);
            onEvent(TcpEventState.ClientConnected, (IPEndPoint)acceptedSocket.RemoteEndPoint, acceptedSocket);

            EnableReceive(acceptedSocket);
        }
        void EnableReceive(Socket socket)
        {
            lock (ClientSockets)
            {
                if (!IsClientMode) ClientSockets.Add(socket);
            }

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
                        byte[] tdBuff = new byte[recvLen];
                        Array.Copy(recvBuff, tdBuff, recvLen);
                        onDataReceived(socket, tdBuff);

                    }
                } while (socket.Connected && recvLen != 0);

                if (!IsClientMode)
                    CloseClientSocket(socket);
                else Disconnect();
            }
        }


        void clientSocketCloseProcess(Socket clientSocket)
        {
            lock (ClientSockets)
            {
                ClientSockets.Remove(clientSocket);
            }
        }
        void onEvent(TcpEventState eventType, IPEndPoint endPoint, Socket socket)
        {
            State = eventType;
            OnEvent?.Invoke(this, new TcpEventArgs() { EventType = eventType, IPEndPoint = endPoint, Socket = socket });
        }
        void onDataReceived(Socket clientSocket, byte[] data)
        {
            OnDataReceived?.Invoke(this, new TcpEventArgs() { EventType = TcpEventState.ReceivedData, IPEndPoint = (IPEndPoint)clientSocket.RemoteEndPoint, Data = data, Socket = clientSocket });
        }
    }
}
