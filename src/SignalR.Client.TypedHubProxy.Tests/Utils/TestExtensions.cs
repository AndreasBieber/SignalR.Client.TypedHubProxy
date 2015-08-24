using System;
using System.Threading.Tasks;
using Moq;
using Moq.Language.Flow;
using SignalR.Client.TypedHubProxy.Tests.Mocks;

namespace SignalR.Client.TypedHubProxy.Tests.Utils
{
    internal static class TestExtensions
    {
        public static IReturnsThrows<MockedHubProxy, Task> SetupCallback(
            this Mock<MockedHubProxy> mockedHubProxy,
            Action<object[]> callback)
        {
            return mockedHubProxy.Setup(m => m.Invoke(It.IsAny<string>(), It.IsAny<object[]>()))
                .Callback<string, object[]>((methodName, args) => callback(args));
        }

        public static IReturnsThrows<MockedHubProxy, Task<TResult>> SetupCallback<TResult>(
            this Mock<MockedHubProxy> mockedHubProxy,
            Action<object[]> callback)
        {
            return mockedHubProxy.Setup(m => m.Invoke<TResult>(It.IsAny<string>(), It.IsAny<object[]>()))
                .Callback<string, object[]>((methodName, args) => callback(args));
        }
    }
}