using System;
using Microsoft.AspNet.SignalR.Client;
using Sample.Shared;

namespace Sample.Client
{
    internal class Chat : IChatSubscriber
    {
        public Chat()
        {
            var connection = new HubConnection("http://localhost:1337/signalr");

            var hubProxy = connection.CreateHubProxy<IChatHub>("chatHub");
            hubProxy.SubscribeOn<IChatSubscriber>(this);

            connection.Start().Wait();
            
            hubProxy.SendMessage("I'm the client!");
        }

        /// <summary>
        ///     Interface implementation of IChatSubscriber.
        ///     This method can be called from the server.
        /// </summary>
        /// <param name="msg">The incoming chat message.</param>
        public void NewMessage(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}