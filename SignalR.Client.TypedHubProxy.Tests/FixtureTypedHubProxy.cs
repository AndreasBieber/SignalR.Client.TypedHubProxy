namespace SignalR.Client.TypedHubProxy.Tests
{
    using Microsoft.AspNet.SignalR.Client;

    public class FixtureTypedHubProxy : FixtureBase
    {
        public FixtureTypedHubProxy()
        {
            this.Proxy = HubProxyMock.Object.CreateTypedProxy<IServerContract, IClientContract>();
        }

        public ITypedHubProxy<IServerContract, IClientContract> Proxy { get; private set; }
    }
}
