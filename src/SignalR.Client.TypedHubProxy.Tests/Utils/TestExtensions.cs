namespace SignalR.Client.TypedHubProxy.Tests.Utils
{
    using Moq;
    using Moq.Language.Flow;

    internal static class TestExtensions
    {
        public static IReturnsThrows<Tests.Mocks.MockedHubProxy, System.Threading.Tasks.Task> SetupCallback(
            this Mock<Tests.Mocks.MockedHubProxy> mockedHubProxy,
            System.Action<object[]> callback)
        {
            return mockedHubProxy.Setup(m => m.Invoke(It.IsAny<string>(), It.IsAny<object[]>()))
                .Callback<string,object[]>((methodName, args) => callback(args));
        }

        public static IReturnsThrows<Tests.Mocks.MockedHubProxy, System.Threading.Tasks.Task<TResult>> SetupCallback<TResult>(
            this Mock<Tests.Mocks.MockedHubProxy> mockedHubProxy,
            System.Action<object[]> callback)
        {
            return mockedHubProxy.Setup(m => m.Invoke<TResult>(It.IsAny<string>(), It.IsAny<object[]>()))
                .Callback<string, object[]>((methodName, args) => callback(args));

        }
    }
}
