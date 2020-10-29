using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Vote.Hubs
{
    public class StatHub : Hub
    {
        public async Task Stat(string message)
        {
            await Clients.All.SendAsync("Stat", message);
        }
    }
}