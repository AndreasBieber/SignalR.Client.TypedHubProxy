namespace SignalR.Client.TypedHubProxy.Tests
{
    using Moq;
    internal static class TestExtensions
    {
        public static Moq.Language.Flow.IReturnsThrows<MockedHubProxy, System.Threading.Tasks.Task> SetupCallback(this Mock<MockedHubProxy> mockedHubProxy,
            System.Action<object[]> callback)
        {
            return mockedHubProxy.Setup(m => m.Invoke(It.IsAny<string>(), It.IsAny<object[]>()))
                .Callback<string,object[]>((methodName, args) => callback(args));
        }

        public static Moq.Language.Flow.IReturnsThrows<MockedHubProxy, System.Threading.Tasks.Task<TResult>> SetupCallback<TResult>(this Mock<MockedHubProxy> mockedHubProxy,
            System.Action<object[]> callback)
        {
            return mockedHubProxy.Setup(m => m.Invoke<TResult>(It.IsAny<string>(), It.IsAny<object[]>()))
                .Callback<string, object[]>((methodName, args) => callback(args));

        }
    }
}
