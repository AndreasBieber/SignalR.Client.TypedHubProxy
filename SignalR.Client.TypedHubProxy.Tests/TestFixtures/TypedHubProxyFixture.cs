namespace SignalR.Client.TypedHubProxy.Tests.TestFixtures
{
    using Microsoft.AspNet.SignalR.Client;

    public class TypedHubProxyFixture : FixtureBase
    {
        public TypedHubProxyFixture()
        {
            this.Proxy = HubProxyMock.Object.CreateHubProxy<Contracts.IServerContract, Contracts.IClientContract>();
        }

        public IHubProxy<Contracts.IServerContract, Contracts.IClientContract> Proxy { get; private set; }
    }
}
