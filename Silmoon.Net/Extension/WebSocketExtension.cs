using Silmoon.Extension;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Silmoon.Net.Extension
{
    public static class WebSocketExtension
    {
        public static Task SendTask(this WebSocket webSocket, string content, Encoding encoding)
        {
            return webSocket.SendAsync(new ArraySegment<byte>(encoding.GetBytes(content)), WebSocketMessageType.Text, true, CancellationToken.None);
        }
        public static Task SendTask(this WebSocket webSocket, string content)
        {
            return webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(content)), WebSocketMessageType.Text, true, CancellationToken.None);
        }
        public static Task SendTask(this WebSocket webSocket, object jsonObject, Encoding encoding)
        {
            return webSocket.SendAsync(new ArraySegment<byte>(encoding.GetBytes(jsonObject.ToJsonString())), WebSocketMessageType.Text, true, CancellationToken.None);
        }
        public static Task SendTask(this WebSocket webSocket, object jsonObject)
        {
            return webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(jsonObject.ToJsonString())), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public static Task<string> ReceiveTask(this WebSocket webSocket, int bufferSize = 5120)
        {
            return Task.Run(async () =>
            {
                var buffer = new byte[bufferSize];
                var array = new ArraySegment<byte>(buffer);
                var result = await webSocket.ReceiveAsync(array, CancellationToken.None);
                return Encoding.UTF8.GetString(buffer, 0, result.Count);
            });
        }
        public static Task<string> ReceiveTask(this WebSocket webSocket, Encoding encoding, int bufferSize = 5120)
        {
            return Task.Run(async () =>
            {
                var buffer = new byte[bufferSize];
                var array = new ArraySegment<byte>(buffer);
                var result = await webSocket.ReceiveAsync(array, CancellationToken.None);
                return encoding.GetString(buffer, 0, result.Count);
            });
        }
    }
}
