using System;

namespace SignalR.Client.TypedHubProxy.Tests
{
    public interface ITestHubClientEvents
    {
        void DelayedAnswer(Guid guid);
    }
}
