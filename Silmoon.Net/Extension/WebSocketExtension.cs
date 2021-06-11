using Silmoon.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Silmoon.Net.Extension
{
    public static class WebSocketExtension
    {
        public static Task SendTask(this WebSocket webSocket, string content, Encoding encoding, CancellationToken? cancellationToken = null)
        {
            if (!cancellationToken.HasValue) cancellationToken = CancellationToken.None;

            return webSocket.SendAsync(new ArraySegment<byte>(encoding.GetBytes(content)), WebSocketMessageType.Text, true, cancellationToken.Value);
        }
        public static Task SendTask(this WebSocket webSocket, string content, CancellationToken? cancellationToken = null)
        {
            if (!cancellationToken.HasValue) cancellationToken = CancellationToken.None;

            return webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(content)), WebSocketMessageType.Text, true, cancellationToken.Value);
        }
        public static Task SendTask(this WebSocket webSocket, object jsonObject, Encoding encoding, CancellationToken? cancellationToken = null)
        {
            if (!cancellationToken.HasValue) cancellationToken = CancellationToken.None;

            return webSocket.SendAsync(new ArraySegment<byte>(encoding.GetBytes(jsonObject.ToJsonString())), WebSocketMessageType.Text, true, cancellationToken.Value);
        }
        public static Task SendTask(this WebSocket webSocket, object jsonObject, CancellationToken? cancellationToken = null)
        {
            if (!cancellationToken.HasValue) cancellationToken = CancellationToken.None;

            return webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(jsonObject.ToJsonString())), WebSocketMessageType.Text, true, cancellationToken.Value);
        }

        public static async Task<string> ReceiveTask(this WebSocket webSocket, long sizeLimit = 0, CancellationToken? cancellationToken = null, int bufferSize = 5120)
        {
            if (!cancellationToken.HasValue) cancellationToken = CancellationToken.None;

            try
            {
                var buffer = new byte[bufferSize];
                var array = new ArraySegment<byte>(buffer);
                var bufferList = new List<byte[]>();

                var receiveCount = 0;
                WebSocketReceiveResult result = null;
                do
                {
                    result = await webSocket.ReceiveAsync(array, cancellationToken.Value);
                    receiveCount += result.Count;
                    bufferList.Add(buffer.Take(result.Count).ToArray());
                    if (sizeLimit != 0 && receiveCount > sizeLimit) break;

                } while (!result.EndOfMessage);
                byte[] completedData = new byte[0];
                bufferList.ForEach(d => completedData = completedData.Concat(d).ToArray());
                return Encoding.UTF8.GetString(completedData, 0, receiveCount);
            }
            catch (Exception e) { throw e; }
        }
        public static async Task<string> ReceiveTask(this WebSocket webSocket, Encoding encoding, long sizeLimit = 0, CancellationToken? cancellationToken = null, int bufferSize = 5120)
        {
            if (!cancellationToken.HasValue) cancellationToken = CancellationToken.None;

            try
            {
                var buffer = new byte[bufferSize];
                var array = new ArraySegment<byte>(buffer);
                var bufferList = new List<byte[]>();

                var receiveCount = 0;
                WebSocketReceiveResult result = null;
                do
                {
                    result = await webSocket.ReceiveAsync(array, cancellationToken.Value);
                    receiveCount += result.Count;
                    bufferList.Add(buffer.Take(result.Count).ToArray());
                    if (sizeLimit != 0 && receiveCount > sizeLimit) break;

                } while (!result.EndOfMessage);
                byte[] completedData = new byte[0];
                bufferList.ForEach(d => completedData = completedData.Concat(d).ToArray());
                return encoding.GetString(completedData, 0, receiveCount);
            }
            catch (Exception e) { throw e; }
        }
        public static async Task<string> ReceiveOnceTask(this WebSocket webSocket, CancellationToken? cancellationToken = null, int bufferSize = 5120)
        {
            if (!cancellationToken.HasValue) cancellationToken = CancellationToken.None;

            var buffer = new byte[bufferSize];
            var array = new ArraySegment<byte>(buffer);
            var result = await webSocket.ReceiveAsync(array, cancellationToken.Value);
            return Encoding.UTF8.GetString(buffer, 0, result.Count);
        }
        public static async Task<string> ReceiveOnceTask(this WebSocket webSocket, Encoding encoding, CancellationToken? cancellationToken = null, int bufferSize = 5120)
        {
            if (!cancellationToken.HasValue) cancellationToken = CancellationToken.None;

            var buffer = new byte[bufferSize];
            var array = new ArraySegment<byte>(buffer);
            var result = await webSocket.ReceiveAsync(array, cancellationToken.Value);
            return encoding.GetString(buffer, 0, result.Count);
        }
    }
}
