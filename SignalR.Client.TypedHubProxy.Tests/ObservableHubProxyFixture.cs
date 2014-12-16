using System;
using Microsoft.AspNet.SignalR.Client;

namespace SignalR.Client.TypedHubProxy.Tests
{
    public class ObservableHubProxyFixture : IDisposable
    {
        private const string HUBNAME = "testHub";

        private HubConnectionFixture _hubConnectionFixture;

        public IObservableHubProxy<ITestHub, ITestHubClientEvents> HubProxy { get; private set; }

        public ObservableHubProxyFixture()
        {
            _hubConnectionFixture = new HubConnectionFixture();
            var hubConnection = _hubConnectionFixture.HubConnection;
            this.HubProxy = hubConnection.CreateObservableHubProxy<ITestHub, ITestHubClientEvents>(HUBNAME);
            hubConnection.Start().Wait();
        }

        public void Dispose()
        {
            _hubConnectionFixture.Dispose();
            _hubConnectionFixture = null;
        }
    }
}