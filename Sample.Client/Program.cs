using System;
using Microsoft.AspNet.SignalR.Client;
using Sample.Shared;

namespace Sample.Client
{
    internal class Program
    {
        private static void Main()
        {
            // Create the connection
            var connection = new HubConnection("http://localhost:1337/signalr");

            // Create the hubproxy
            ITypedHubProxy<IChatHub, IChatEvents> hubProxy = connection.CreateHubProxy<IChatHub, IChatEvents>("chatHub");

            // Subscribe on the event IChatEvents.NewMessage.
            // When the event was fired through the server, the static method Program.NewMessage(string msg) will be invoked.
            hubProxy.SubscribeOn<string>(hub => hub.NewMessage, NewMessage);

            // Start the connection
            connection.Start().Wait();

            // Call the method IChatHub.GetConnectedClients() on the server and get the result.
            int clientCount = hubProxy.Call(hub => hub.GetConnectedClients());
            Console.WriteLine("Connected clients: {0}", clientCount);

            // Call the method IChatHub.SendMessage with no result.
            hubProxy.Call(hub => hub.SendMessage("Hi, i'm the client."));
            Console.ReadKey();
            connection.Stop();
        }

        /// <summary>
        ///     This method can be called from the server.
        /// </summary>
        /// <param name="msg">The incoming chat message.</param>
        public static void NewMessage(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}