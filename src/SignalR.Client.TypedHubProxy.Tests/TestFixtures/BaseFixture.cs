using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SignalR.Client.TypedHubProxy.Tests.Contracts;
using SignalR.Client.TypedHubProxy.Tests.Mocks;

namespace SignalR.Client.TypedHubProxy.Tests.TestFixtures
{
    public abstract class FixtureBase
    {
        private readonly HubProxy _hubProxy;

        protected FixtureBase()
        {
            var hubConnectionMock = new Mock<IHubConnection>();
            hubConnectionMock.SetupGet(m => m.JsonSerializer).Returns(new JsonSerializer());

            _hubProxy = new HubProxy(hubConnectionMock.Object, "whatEver");
            HubProxyMock = new Mock<MockedHubProxy>(_hubProxy);
            HubProxyMock.SetupGet(m => m.JsonSerializer).Returns(_hubProxy.JsonSerializer);
            HubProxyMock.Setup(m => m.Subscribe(It.IsAny<string>()))
                .Returns((string eventName) => _hubProxy.Subscribe(eventName));

            HubProxyMock.Setup(m => m.InvokeEvent(It.IsAny<Expression<Action<IClientContract>>>()))
                .Callback<Expression<Action<IClientContract>>>(call =>
                {
                    var invocation = call.GetActionDetails();
                    _hubProxy.InvokeEvent(invocation.MethodName,
                        invocation.Parameters.Select(JToken.FromObject).ToList());
                });
        }

        public Mock<MockedHubProxy> HubProxyMock { get; }
    }
}