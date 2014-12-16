namespace SignalR.Client.TypedHubProxy.Tests
{
    using Microsoft.AspNet.SignalR.Client;

    public class FixtureObservableHubProxy : FixtureBase
    {
        public FixtureObservableHubProxy()
        {
            this.Proxy = HubProxyMock.Object.CreateObservableProxy<ITestHub, ITestHubClientEvents>();
        }

        public IObservableHubProxy<ITestHub, ITestHubClientEvents> Proxy { get; private set; }
    }
}
