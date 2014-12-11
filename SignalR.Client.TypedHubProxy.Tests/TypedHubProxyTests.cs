using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace SignalR.Client.TypedHubProxy.Tests
{
    public class TypedHubProxyTests: IUseFixture<TestFixture>
    {
        private TestFixture _testFixture;

        public void SetFixture(TestFixture data)
        {
            _testFixture = data;
        }

        [Fact]
        public void CallServerSync()
        {
            _testFixture.HubProxy.Call(hub => hub.ThrowAway(Guid.NewGuid()));
        }

        [Fact]
        public void CallServerWithResultSync()
        {
            Guid id = Guid.NewGuid();
            _testFixture.HubProxy.Call(hub => hub.PingBackGuid(id))
                .Should()
                .Be(id, "because the server should return the sent guid");
        }

        [Fact]
        public void CallServerAsync()
        {
            _testFixture.HubProxy.CallAsync(hub => hub.ThrowAway(Guid.NewGuid()));
        }

        [Fact]
        public void CallServerAsyncWithResult()
        {
            Guid id = Guid.NewGuid();
            _testFixture.HubProxy.CallAsync(hub => hub.PingBackGuid(id))
                .Result
                .Should()
                .Be(id, "because the server should return the sent guid");
        }

        [Fact]
        public void SubscribeOnEvent()
        {
            Guid id = Guid.NewGuid();

            _testFixture.HubProxy.SubscribeOn<Guid>(hub => hub.DelayedAnswer, (responseId) => responseId.Should().Be(id));
            _testFixture.HubProxy.Call(hub => hub.SendDelayed(id, 10));
        }

        [Fact]
        public void SubscribeOnEventWithPredicate()
        {
            bool gotResponse = false;
            _testFixture.HubProxy.SubscribeOn<Guid>(hub => hub.DelayedAnswer, param => param == Guid.NewGuid(), param => gotResponse = true);
            _testFixture.HubProxy.Call(hub => hub.SendDelayed(Guid.NewGuid(), 10));

            Thread.Sleep(100);
            gotResponse.Should().BeFalse("because the event should only be fired when the predicate is true.");
        }

 
    }
}
