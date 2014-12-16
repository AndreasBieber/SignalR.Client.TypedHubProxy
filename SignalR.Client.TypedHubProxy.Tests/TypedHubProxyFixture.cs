using System;
using Microsoft.AspNet.SignalR.Client;

namespace SignalR.Client.TypedHubProxy.Tests
{
    public class TypedHubProxyFixture : IDisposable
    {
        private const string HUBNAME = "testHub";

        private HubConnectionFixture _hubConnectionFixture;

        public ITypedHubProxy<ITestHub, ITestHubClientEvents> HubProxy { get; private set; }

        public TypedHubProxyFixture()
        {
            _hubConnectionFixture = new HubConnectionFixture();
            var hubConnection = _hubConnectionFixture.HubConnection;
            this.HubProxy = hubConnection.CreateHubProxy<ITestHub, ITestHubClientEvents>(HUBNAME);
            hubConnection.Start().Wait();
        }

        public void Dispose()
        {
            this.HubProxy.Dispose();
            _hubConnectionFixture.Dispose();

            this.HubProxy = null;
            _hubConnectionFixture = null;
        }
    }
}