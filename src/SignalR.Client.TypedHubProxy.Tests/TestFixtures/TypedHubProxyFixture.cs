using Microsoft.AspNet.SignalR.Client;
using SignalR.Client.TypedHubProxy.Tests.Contracts;

namespace SignalR.Client.TypedHubProxy.Tests.TestFixtures
{
    public class TypedHubProxyFixture : FixtureBase
    {
        public TypedHubProxyFixture()
        {
            Proxy = HubProxyMock.Object.AsHubProxy<IServerContract, IClientContract>();
            WrongProxy = HubProxyMock.Object.AsHubProxy<IServerContract, IWrongClientContract>();
        }

        public IHubProxy<IServerContract, IClientContract> Proxy { get; private set; }

        public IHubProxy<IServerContract, IWrongClientContract> WrongProxy { get; private set; }
    }
}