using Silmoon.Extension;
using Silmoon.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Silmoon.Net
{
    public class SocketHelper
    {
        static Socket socket = null;
        public static int UdpSendTo(string s)
        {
            if (socket is null) socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            lock (socket)
            {
                var i = socket.SendTo(Encoding.UTF8.GetBytes(s), new IPEndPoint(IPAddress.Loopback, 20001));
                return i;
            }
        }
        public static int UdpSendTo(string s, IPAddress address, int port)
        {
            if (socket is null) socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            lock (socket)
            {
                var i = socket.SendTo(Encoding.UTF8.GetBytes(s), new IPEndPoint(address, port));
                return i;
            }
        }
        public static async Task<StateSet<bool, Exception>> TcpConnectTestAsync(IPEndPoint endPoint, int timeoutMilliseconds, Func<byte[], byte[]> func = null)
        {
            using (var tcpClient = new TcpClient())
            {
                try
                {
                    // 异步连接，带超时处理
                    var connectTask = tcpClient.ConnectAsync(endPoint.Address, endPoint.Port);
                    if (await Task.WhenAny(connectTask, Task.Delay(timeoutMilliseconds)) != connectTask)
                    {
                        // 超时逻辑
                        return StateSet<bool, Exception>.Create(false, null, "connect timeout");
                    }

                    // 设置发送和接收超时
                    tcpClient.ReceiveTimeout = timeoutMilliseconds;
                    tcpClient.SendTimeout = timeoutMilliseconds;

                    if (func != null)
                    {
                        using (var stream = tcpClient.GetStream())
                        {
                            // 读取数据
                            var buffer = new byte[1024];
                            int read = await stream.ReadAsync(buffer, 0, buffer.Length);
                            if (read == 0)
                            {
                                // 无数据读取，连接可能已关闭
                                return StateSet<bool, Exception>.Create(false, null, "read == 0");
                            }

                            // 调用回调函数处理数据并发送结果
                            var result = func(buffer);
                            await stream.WriteAsync(result, 0, result.Length);
                        }
                    }

                    return StateSet<bool, Exception>.Create(true);
                }
                catch (Exception e)
                {
                    // 处理异常
                    return StateSet<bool, Exception>.Create(false, e);
                }
            }
        }
    }
}
