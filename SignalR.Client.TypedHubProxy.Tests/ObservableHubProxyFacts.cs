using System;
using System.Reactive.Linq;
using FluentAssertions;
using Xunit;

namespace SignalR.Client.TypedHubProxy.Tests
{
    public class ObservableHubProxyTests : IUseFixture<FixtureObservableHubProxy>
    {
        private FixtureObservableHubProxy _fixture;

        public void SetFixture(FixtureObservableHubProxy data)
        {
            _fixture = data;
        }

        [Fact]
        public void TestObserveEvent()
        {
            const int inParam1 = 1;
            bool notified = false;
            IObservable<int> responseId = _fixture.Proxy.Observe<int>(hub => hub.Passing1Param).FirstAsync();
            
            responseId.Subscribe(outParam1 =>
                                 {
                                     notified = true;
                                     outParam1.Should().Be(inParam1, TestConsts.ERR_PARAM_MISMATCH, "outParam1", inParam1, outParam1);
                                 });

            _fixture.HubProxyMock.Object.InvokeEvent(hub => hub.Passing1Param(inParam1));
            notified.Should().BeTrue(TestConsts.ERR_PROXY_RECEIVE_EVENT);
        }
    }
}