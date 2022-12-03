using Microsoft.AspNetCore.SignalR;

namespace Silmoon.AspNetCore.Test.Hubs
{
    public class DemoHub : Hub
    {
        public async void send(string symbol)
        {
            var client = Clients.Caller;

            await client.SendAsync("time", DateTime.Now.ToString());
            await Task.Delay(1000);
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
