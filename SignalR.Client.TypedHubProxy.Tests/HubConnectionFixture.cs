using System;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Owin.Hosting;
using Owin;

namespace SignalR.Client.TypedHubProxy.Tests
{
    public class HubConnectionFixture : IDisposable
    {
        private const string ADDR_BASE = "http://localhost:4711";
        private const string ADDR_SIGNALR = "/signalr";
        private const string ADDR_SERVER = ADDR_BASE + ADDR_SIGNALR;

        private IDisposable _server;
   
        public HubConnection HubConnection { get; private set; }

        public HubConnectionFixture()
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

            this.HubConnection = new HubConnection(ADDR_SERVER);
        }

        public virtual void Dispose()
        {
            this.HubConnection.Dispose();
            _server.Dispose();

            this.HubConnection = null;
            _server = null;
        }
    }
}