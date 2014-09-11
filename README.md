# SignalR.Client.TypedHubProxy
SignalR.Client.TypedHubProxy is a library which extends the SignalR.Client components.

## What can it be used for?
One thing that really bugged me about SignalR is the lack of strongly typed hub proxies.
This library will enable this feature via interface implementations.

## Get it on NuGet!

    Install-Package SignalR.Client.TypedHubProxy

## LICENSE
[Apache 2.0 License](https://github.com/Gandalis/SignalR.Client.TypedHubProxy/blob/master/LICENSE.md)

## Documentation
### Client Interface for the ChatHub
This interface will be used by the server to call methods from the client.
```csharp
namespace Sample.Shared
{
    public interface IChatSubscriber
    {
        void NewMessage(string msg);
    }
}
```
### ChatHub Interface
This interface will be implemented by the serverhub. The client will also use this interface to call these defined interface methods.
```csharp
using System.Threading.Tasks;

namespace Sample.Shared
{
    public interface IChatHub
    {
        Task SendMessage(string msg);
    }
}
```

### ChatHub
This is the chathub inside of the server, which implements IChatHub and uses IChatSubscriber.
```csharp
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
```
### ChatClient
This is the chatclient.
```csharp
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

            IChatHub hubProxy = connection.CreateHubProxy<IChatHub>("chatHub");
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
```
