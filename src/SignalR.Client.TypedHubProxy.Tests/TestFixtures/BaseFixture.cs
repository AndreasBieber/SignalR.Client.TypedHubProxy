namespace SignalR.Client.TypedHubProxy.Tests.TestFixtures
{
    using System.Linq;
    using Microsoft.AspNet.SignalR.Client;
    using Moq;

    public abstract class FixtureBase
    {
        private readonly Microsoft.AspNet.SignalR.Client.Hubs.HubProxy _hubProxy;
        protected Mock<Microsoft.AspNet.SignalR.Client.Hubs.IHubConnection> HubConnectionMock;


        protected FixtureBase()
        {
            HubConnectionMock = new Mock<Microsoft.AspNet.SignalR.Client.Hubs.IHubConnection>();
            HubConnectionMock.SetupGet(m => m.JsonSerializer).Returns(new Newtonsoft.Json.JsonSerializer());

            _hubProxy = new Microsoft.AspNet.SignalR.Client.Hubs.HubProxy(HubConnectionMock.Object, "whatEver");
            this.HubProxyMock = new Mock<Tests.Mocks.MockedHubProxy>(_hubProxy);
            this.HubProxyMock.SetupGet(m => m.JsonSerializer).Returns(_hubProxy.JsonSerializer);
            this.HubProxyMock.Setup(m => m.Subscribe(It.IsAny<string>())).Returns((string eventName) => _hubProxy.Subscribe(eventName));
            this.HubProxyMock.Setup(m => m.InvokeEvent(It.IsAny<System.Linq.Expressions.Expression<System.Action<Contracts.IClientContract>>>()))
                .Callback<System.Linq.Expressions.Expression<System.Action<Contracts.IClientContract>>>(call =>
                                                                                                   {
                                                                                                       ActionDetail invocation = call.GetActionDetails();
                                                                                                       _hubProxy.InvokeEvent(invocation.MethodName, invocation.Parameters.Select(Newtonsoft.Json.Linq.JToken.FromObject).ToList());
                                                                                                   });
        }

        public Mock<Tests.Mocks.MockedHubProxy> HubProxyMock { get; protected set; }
    }
}
