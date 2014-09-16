# SignalR.Client.TypedHubProxy
SignalR.Client.TypedHubProxy is a library which extends the SignalR.Client components.

## What can it be used for?
One thing that really bugged me out about SignalR is the lack of strongly typed hub proxies.
This library will enable this feature via interface implementations.

## Get it on NuGet!

    Install-Package SignalR.Client.TypedHubProxy

## LICENSE
[Apache 2.0 License](https://github.com/Gandalis/SignalR.Client.TypedHubProxy/blob/master/LICENSE.md)

## Example
### Prepare interfaces
#### IChatEvents
This interface will be used by the server to call methods from the client.
```csharp
namespace Sample.Shared
{
    public interface IChatEvents
    {
        void NewMessage(string msg);
    }
}
```
#### IChatHub
This interface will be implemented by the serverhub. The client will use this interface also to call these defined interface methods on the server.
```csharp
using System.Threading.Tasks;

namespace Sample.Shared
{
    public interface IChatHub
    {
        void SendMessage(string msg);
        int GetConnectedClients();
    }
}
```
### The Hub
#### ChatHub
This is the chathub inside of the server, which implements IChatHub and uses IChatEvents.
```csharp
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
```
### The Client
### Program.cs
This is the chatclient.
```csharp
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
            var hubProxy = connection.CreateHubProxy<IChatHub, IChatEvents>("chatHub");

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
```