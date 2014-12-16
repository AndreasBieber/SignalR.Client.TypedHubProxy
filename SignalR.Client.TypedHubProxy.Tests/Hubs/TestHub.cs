using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace SignalR.Client.TypedHubProxy.Tests.Hubs
{
    public class TestHub: Hub<ITestHubClientEvents>, ITestHub
    {
        public TestHub()
        {
            Observable.Timer(TimeSpan.Zero, TimeSpan.FromMilliseconds(10), NewThreadScheduler.Default)
                .ObserveOn(NewThreadScheduler.Default)
                .Subscribe(tick => Clients.All.Timer(tick));
        }

        public void ThrowAway(Guid guid)
        {
            
        }

        public Guid PingBackGuid(Guid guid)
        {
            return guid;
        }

        public void SendDelayedWithMs(Guid guid, int ms)
        {
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(ms);
                Clients.Caller.DelayedAnswerFromServer(guid);
            });
        }

        public void SendDelayed()
        {
            Task.Factory.StartNew(() => Clients.Caller.DelayedAnswer());
        }

        public void SendDelayed(int param1)
        {
            Task.Factory.StartNew(() => Clients.Caller.DelayedAnswer(param1));
        }

        public void SendDelayed(int param1, int param2)
        {
            Task.Factory.StartNew(() => Clients.Caller.DelayedAnswer(param1, param2));
        }

        public void SendDelayed(int param1, int param2, int param3)
        {
            Task.Factory.StartNew(() => Clients.Caller.DelayedAnswer(param1, param2, param3));
        }

        public void SendDelayed(int param1, int param2, int param3, int param4)
        {
            Task.Factory.StartNew(() => Clients.Caller.DelayedAnswer(param1, param2, param3, param4));
        }

        public void SendDelayed(int param1, int param2, int param3, int param4, int param5)
        {
            Task.Factory.StartNew(() => Clients.Caller.DelayedAnswer(param1, param2, param3, param4, param5));
        }

        public void SendDelayed(int param1, int param2, int param3, int param4, int param5, int param6)
        {
            Task.Factory.StartNew(() => Clients.Caller.DelayedAnswer(param1, param2, param3, param4, param5, param6));
        }

        public void SendDelayed(int param1, int param2, int param3, int param4, int param5, int param6, int param7)
        {
            Task.Factory.StartNew(() => Clients.Caller.DelayedAnswer(param1, param2, param3, param4, param5, param6, param7));
        }
    }
}
