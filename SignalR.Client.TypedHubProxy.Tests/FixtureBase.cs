namespace SignalR.Client.TypedHubProxy.Tests
{
    public abstract class FixtureBase
    {
        protected Moq.Mock<Microsoft.AspNet.SignalR.Client.Hubs.IHubConnection> HubConnectionMock;
        

        protected FixtureBase()
        {
            HubConnectionMock = new Moq.Mock<Microsoft.AspNet.SignalR.Client.Hubs.IHubConnection>();
            HubConnectionMock.SetupGet(m => m.JsonSerializer).Returns(new Newtonsoft.Json.JsonSerializer());

            HubProxyMock = new Moq.Mock<Microsoft.AspNet.SignalR.Client.Hubs.HubProxy>(this.HubConnectionMock.Object, "whatEver");
            //HubProxyMock.Setup(m => m.Invoke(Moq.It.IsAny<string>(), Moq.It.IsAny<object[]>()))
            //    .Returns(() =>
            //    {
            //        var tcs = new System.Threading.Tasks.TaskCompletionSource<object>();
            //        tcs.SetResult(null);

            //        return tcs.Task;
            //    });
            

            //HubProxy = new Microsoft.AspNet.SignalR.Client.Hubs.HubProxy(this.HubConnectionMock.Object, "whatever");
        }

        public Moq.Mock<Microsoft.AspNet.SignalR.Client.Hubs.HubProxy> HubProxyMock { get; protected set; }
    }
}
