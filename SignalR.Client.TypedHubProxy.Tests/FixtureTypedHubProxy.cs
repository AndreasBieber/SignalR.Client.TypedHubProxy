namespace SignalR.Client.TypedHubProxy.Tests
{
    using Microsoft.AspNet.SignalR.Client;

    public class FixtureTypedHubProxy : FixtureBase
    {
        public FixtureTypedHubProxy()
        {
            this.Proxy = HubProxyMock.Object.CreateTypedProxy<ITestHub, ITestHubClientEvents>();
        }

        public ITypedHubProxy<ITestHub, ITestHubClientEvents> Proxy { get; private set; }
    }
}
