using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

public class HttpClientEx : HttpClient
{
    public event EventHandler DownloadStarted;
    public event EventHandler<DownloadProgressChangedEventArgs> DownloadProgressChanged;
    public event EventHandler DownloadCompleted;

    public HttpClientEx() : base()
    {
        Timeout = TimeSpan.FromSeconds(30);
    }

    public HttpClientEx(TimeSpan timeout) : base()
    {
        Timeout = timeout;
    }

    public async Task DownloadFileAsync(string requestUri, string outputPath)
    {
        // 触发下载开始事件
        DownloadStarted?.Invoke(this, EventArgs.Empty);

        using (var response = await GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead))
        {
            response.EnsureSuccessStatusCode();

            var contentLength = response.Content.Headers.ContentLength;

            using (var contentStream = await response.Content.ReadAsStreamAsync())
            using (var fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
            {
                var totalRead = 0L;
                var buffer = new byte[8192];
                var isMoreToRead = true;

                do
                {
                    var read = await contentStream.ReadAsync(buffer, 0, buffer.Length);
                    if (read == 0)
                    {
                        isMoreToRead = false;
                    }
                    else
                    {
                        await fileStream.WriteAsync(buffer, 0, read);

                        totalRead += read;

                        // 触发下载进度更新事件
                        DownloadProgressChanged?.Invoke(this, new DownloadProgressChangedEventArgs(totalRead, contentLength.HasValue ? contentLength.Value : -1));
                    }
                }
                while (isMoreToRead);

                // 触发下载完成事件
                DownloadCompleted?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}

public class DownloadProgressChangedEventArgs : EventArgs
{
    public long BytesReceived { get; }
    public long TotalBytesToReceive { get; }
    public double ProgressPercentage => TotalBytesToReceive > 0 ? (double)BytesReceived / TotalBytesToReceive * 100 : 0;

    public DownloadProgressChangedEventArgs(long bytesReceived, long totalBytesToReceive)
    {
        BytesReceived = bytesReceived;
        TotalBytesToReceive = totalBytesToReceive;
    }
}
