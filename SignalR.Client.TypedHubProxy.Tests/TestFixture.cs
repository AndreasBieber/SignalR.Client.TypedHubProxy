using System;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Owin.Hosting;
using Owin;

namespace SignalR.Client.TypedHubProxy.Tests
{
    public class TestFixture : IDisposable
    {
        private const string ADDR_BASE = "http://localhost:4711";
        private const string ADDR_SIGNALR = "/signalr";
        private const string ADDR_SERVER = ADDR_BASE + ADDR_SIGNALR;
        private const string HUBNAME = "testHub";

        private IDisposable _server;
        private HubConnection _hubConnection;

        public TestFixture()
        {
            _server = WebApp.Start(ADDR_BASE, builder =>
                                              {
                                                  var hubConfig = new HubConfiguration
                                                                  {
                                                                      EnableJavaScriptProxies = false,
                                                                      EnableJSONP = false,
                                                                      EnableDetailedErrors = true
                                                                  };
                                                  builder.UseErrorPage();
                                                  builder.MapSignalR(ADDR_SIGNALR, hubConfig);
                                              });

            _hubConnection = new HubConnection(ADDR_SERVER);
            this.HubProxy = _hubConnection.CreateHubProxy<ITestHub, ITestHubClientEvents>(HUBNAME);
            _hubConnection.Start().Wait();
        }

        public ITypedHubProxy<ITestHub, ITestHubClientEvents> HubProxy { get; private set; }

        public void Dispose()
        {
            this.HubProxy.Dispose();
            _hubConnection.Dispose();
            _server.Dispose();

            this.HubProxy = null;
            _hubConnection = null;
            _server = null;
        }
    }
}