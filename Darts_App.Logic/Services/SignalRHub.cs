using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
namespace Darts_App.Endpoint.Services
{
    public class SignalRHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            Clients.All.SendAsync("ReceiveMessage", $"{Context.ConnectionId} has joined");
            Clients.Caller.SendAsync("Connected", Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Clients.Caller.SendAsync("Disconnected", Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
        public static TaskCompletionSource<string> tcs;
        public async Task SendData(string data)
        {
            try
            {
                // A beérkező adat beállítása a tcs-be
                if (tcs != null && !tcs.Task.IsCompleted)
                {
                    tcs.SetResult(data);
                }

                await Clients.Caller.SendAsync("DataReceived", data);
            }
            catch (Exception e)
            {
                // Hiba logolása
                Console.WriteLine("Hiba történt a SendData metódusban: " + e.Message);
                throw;  // Dobd tovább a kivételt, hogy a SignalR megfelelően kezelje
            }
           
        }
    }
}
