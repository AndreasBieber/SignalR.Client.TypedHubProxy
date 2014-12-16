namespace SignalR.Client.TypedHubProxy.Tests
{
    using Microsoft.AspNet.SignalR.Client;
    using Moq;
    using System.Linq;

    public abstract class FixtureBase
    {
        private readonly Microsoft.AspNet.SignalR.Client.Hubs.HubProxy _hubProxy;
        protected Mock<Microsoft.AspNet.SignalR.Client.Hubs.IHubConnection> HubConnectionMock;


        protected FixtureBase()
        {
            HubConnectionMock = new Mock<Microsoft.AspNet.SignalR.Client.Hubs.IHubConnection>();
            HubConnectionMock.SetupGet(m => m.JsonSerializer).Returns(new Newtonsoft.Json.JsonSerializer());

            _hubProxy = new Microsoft.AspNet.SignalR.Client.Hubs.HubProxy(HubConnectionMock.Object, "whatEver");
            this.HubProxyMock = new Mock<MockedHubProxy>(_hubProxy);
            this.HubProxyMock.SetupGet(m => m.JsonSerializer).Returns(_hubProxy.JsonSerializer);
            this.HubProxyMock.Setup(m => m.Subscribe(It.IsAny<string>())).Returns((string eventName) => _hubProxy.Subscribe(eventName));
            this.HubProxyMock.Setup(m => m.InvokeEvent(It.IsAny<System.Linq.Expressions.Expression<System.Action<IClientContract>>>()))
                .Callback<System.Linq.Expressions.Expression<System.Action<IClientContract>>>(call =>
                                                                                                   {
                                                                                                       ActionDetail invocation = call.GetActionDetails();
                                                                                                       _hubProxy.InvokeEvent(invocation.MethodName, invocation.Parameters.Select(Newtonsoft.Json.Linq.JToken.FromObject).ToList());
                                                                                                   });
        }

        public Mock<MockedHubProxy> HubProxyMock { get; protected set; }
    }
}
