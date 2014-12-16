namespace SignalR.Client.TypedHubProxy.Tests
{
    using Microsoft.AspNet.SignalR.Client;

    public class FixtureObservableHubProxy : FixtureBase
    {
        public FixtureObservableHubProxy()
        {
            this.Proxy = HubProxyMock.Object.CreateObservableProxy<IServerContract, IClientContract>();
        }

        public IObservableHubProxy<IServerContract, IClientContract> Proxy { get; private set; }
    }
}
