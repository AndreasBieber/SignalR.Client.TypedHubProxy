namespace SignalR.Client.TypedHubProxy.Tests.TestFixtures
{
    using Microsoft.AspNet.SignalR.Client;

    public class ObservableHubProxyFixture : FixtureBase
    {
        public ObservableHubProxyFixture()
        {
            this.Proxy = HubProxyMock.Object.AsObservableHubProxy<Contracts.IServerContract, Contracts.IClientContract>();
        }

        public IObservableHubProxy<Contracts.IServerContract, Contracts.IClientContract> Proxy { get; private set; }
    }
}
