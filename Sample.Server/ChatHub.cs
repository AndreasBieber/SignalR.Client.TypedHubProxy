using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Sample.Shared;

namespace Sample.Server
{
    public class ChatHub : Hub<IChatEvents>, IChatHub
    {
        private static int _connectedClients;

        static ChatHub()
        {
            new Timer(BroadcastMessage, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
        }

        public override Task OnConnected()
        {
            ++_connectedClients;
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            --_connectedClients;
            return base.OnDisconnected(stopCalled);
        }

        /// <summary>
        ///     Interface implementation of IChatHub.
        ///     This method can be called from a client.
        /// </summary>
        /// <param name="msg">The chat message.</param>
        /// <returns></returns>
        public void SendMessage(string msg)
        {
            Clients.All.NewMessage("CLIENT > " + msg);
        }

        public int GetConnectedClients()
        {
            return _connectedClients;
        }

        private static void BroadcastMessage(object state)
        {
            IHubContext<IChatEvents> hubContext =
                GlobalHost.ConnectionManager.GetHubContext<ChatHub, IChatEvents>();
            hubContext.Clients.All.NewMessage(string.Format("SERVER > Hello client {0}", DateTime.Now));
        }
    }
}