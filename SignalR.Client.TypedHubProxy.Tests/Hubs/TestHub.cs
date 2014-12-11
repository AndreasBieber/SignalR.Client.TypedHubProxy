using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace SignalR.Client.TypedHubProxy.Tests.Hubs
{
    public class TestHub: Hub<ITestHubClientEvents>, ITestHub
    {
        public void ThrowAway(Guid guid)
        {
            
        }

        public Guid PingBackGuid(Guid guid)
        {
            return guid;
        }

        public void SendDelayed(Guid guid, int ms)
        {
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(ms);
                Clients.Caller.DelayedAnswer(guid);
            });
        }
    }
}
