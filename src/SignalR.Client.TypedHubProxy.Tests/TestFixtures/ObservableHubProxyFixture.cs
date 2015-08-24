using Microsoft.AspNet.SignalR.Client;
using SignalR.Client.TypedHubProxy.Tests.Contracts;

namespace SignalR.Client.TypedHubProxy.Tests.TestFixtures
{
    public class ObservableHubProxyFixture : FixtureBase
    {
        public ObservableHubProxyFixture()
        {
            Proxy = HubProxyMock.Object.AsObservableHubProxy<IServerContract, IClientContract>();
        }

        public IObservableHubProxy<IServerContract, IClientContract> Proxy { get; private set; }
    }
}