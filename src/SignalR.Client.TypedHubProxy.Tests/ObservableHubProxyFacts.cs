using System;
using System.Reactive.Linq;
using FluentAssertions;
using JetBrains.Annotations;
using SignalR.Client.TypedHubProxy.Tests.TestFixtures;
using SignalR.Client.TypedHubProxy.Tests.Utils;
using Xunit;

namespace SignalR.Client.TypedHubProxy.Tests
{
    [PublicAPI]
    public class ObservableHubProxyFacts : IClassFixture<ObservableHubProxyFixture>
    {
        private readonly ObservableHubProxyFixture _fixture;

        public ObservableHubProxyFacts(ObservableHubProxyFixture data)
        {
            _fixture = data;
        }

        [Fact]
        public void TestObserveEvent()
        {
            const int inParam1 = 1;
            var notified = false;
            var responseId = _fixture.Proxy.Observe<int>(hub => hub.Passing1Param).FirstAsync();

            responseId.Subscribe(outParam1 =>
            {
                notified = true;
                outParam1.Should().Be(inParam1, TestConsts.ERR_PARAM_MISMATCH, nameof(outParam1), inParam1, outParam1);
            });

            _fixture.HubProxyMock.Object.InvokeEvent(hub => hub.Passing1Param(inParam1));
            notified.Should().BeTrue(TestConsts.ERR_PROXY_RECEIVE_EVENT);
        }
    }
}