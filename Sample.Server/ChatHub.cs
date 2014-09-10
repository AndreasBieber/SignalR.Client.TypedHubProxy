using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Sample.Shared;

namespace Sample.Server
{
    public class ChatHub : Hub<IChatSubscriber>, IChatHub
    {
        static ChatHub()
        {
            new Timer(BroadcastMessage, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
        }

        /// <summary>
        ///     Interface implementation of IChatHub.
        ///     This method can be called from a client.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public Task SendMessage(string msg)
        {
            return Task.Factory.StartNew(() => Clients.All.NewMessage(msg));
        }

        private static void BroadcastMessage(object state)
        {
            IHubContext<IChatSubscriber> hubContext =
                GlobalHost.ConnectionManager.GetHubContext<ChatHub, IChatSubscriber>();
            hubContext.Clients.All.NewMessage(string.Format("Hello client {0}", DateTime.Now));
        }
    }
}