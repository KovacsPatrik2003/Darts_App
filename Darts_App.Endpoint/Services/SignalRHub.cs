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
            Clients.Caller.SendAsync("Connected", Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Clients.Caller.SendAsync("Disconnected", Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }




        // Tárolja az aktív callback-eket
        private static Dictionary<string, TaskCompletionSource<int>> Callbacks = new Dictionary<string, TaskCompletionSource<int>>();

        // Hívás a kliensre, hogy kérjen be egy pontszámot
        public async Task<int> RequestThrowedPointsAsync()
        {
            string callbackId = Guid.NewGuid().ToString();
            var tcs = new TaskCompletionSource<int>();

            // Tárolja a callback-et
            Callbacks.Add(callbackId, tcs);

            // Jelzés a kliensnek
            await Clients.All.SendAsync("RequestThrowedPoints", callbackId);

            // Várakozás a válaszra
            return await tcs.Task;
        }

        // Kliens hívja, hogy visszaküldje a pontszámot
        public void ReceiveThrowedPoints(string callbackId, int throwedPoint)
        {
            if (Callbacks.TryGetValue(callbackId, out var tcs))
            {
                tcs.SetResult(throwedPoint);
                Callbacks.Remove(callbackId);
            }
        }
    }
}
