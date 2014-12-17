namespace SignalR.Client.TypedHubProxy.Tests
{
    using System;
    using System.Reactive.Linq;
    using FluentAssertions;
    using Xunit;

    public class ObservableHubProxyFacts : IUseFixture<TestFixtures.ObservableHubProxyFixture>
    {
        private TestFixtures.ObservableHubProxyFixture _fixture;

        public void SetFixture(TestFixtures.ObservableHubProxyFixture data)
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
                                     outParam1.Should().Be(inParam1, Utils.TestConsts.ERR_PARAM_MISMATCH, "outParam1", inParam1, outParam1);
                                 });

            _fixture.HubProxyMock.Object.InvokeEvent(hub => hub.Passing1Param(inParam1));
            notified.Should().BeTrue(Utils.TestConsts.ERR_PROXY_RECEIVE_EVENT);
        }
    }
}