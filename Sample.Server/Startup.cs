using Microsoft.AspNet.SignalR;
using Owin;

namespace Sample.Server
{
    internal class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var hubConfig = new HubConfiguration
                            {
                                EnableJavaScriptProxies = false,
                                EnableJSONP = false,
                                EnableDetailedErrors = true
                            };

            app.MapSignalR("/signalr", hubConfig);
        }
    }
}