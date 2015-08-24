using Microsoft.AspNet.SignalR.Client;
using SignalR.Client.TypedHubProxy.Tests.Contracts;

namespace SignalR.Client.TypedHubProxy.Tests.TestFixtures
{
    public class TypedHubProxyFixture : FixtureBase
    {
        public TypedHubProxyFixture()
        {
            Proxy = HubProxyMock.Object.AsHubProxy<IServerContract, IClientContract>();
        }

        public IHubProxy<IServerContract, IClientContract> Proxy { get; private set; }
    }
}