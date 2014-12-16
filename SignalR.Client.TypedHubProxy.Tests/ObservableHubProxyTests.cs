using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace SignalR.Client.TypedHubProxy.Tests
{
    public class ObservableHubProxyTests : IUseFixture<ObservableHubProxyFixture>
    {
        private ObservableHubProxyFixture _testFixture;

        public void SetFixture(ObservableHubProxyFixture data)
        {
            _testFixture = data;
        }

        [Fact(Timeout = 200, Skip = "Doesn't work on appveyor")]
        public void ObserveEvent()
        {
            var ticks = _testFixture.HubProxy.Observe<long>(hub => hub.Timer);
            ticks.ToEnumerable().Should().NotBeEmpty();
        }

        [Fact]
        public async Task ObserveResponseOnRequest()
        {
            Guid id = Guid.NewGuid();

            var responseId = _testFixture.HubProxy.Observe<Guid>(hub => hub.DelayedAnswerFromServer).FirstAsync();
            _testFixture.HubProxy.Call(hub => hub.SendDelayedWithMs(id, 10));
            (await responseId).Should().Be(id);
        }

        [Fact]
        public void ObserveNoResponseWithoutRequest()
        {
            var responseId = _testFixture.HubProxy.Observe<Guid>(hub => hub.DelayedAnswerFromServer)
                .Timeout(TimeSpan.FromMilliseconds(100), Observable.Empty<Guid>());
            responseId.ToEnumerable().Should().BeEmpty();
        }
    }
}