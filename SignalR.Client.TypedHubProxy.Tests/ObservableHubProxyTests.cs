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

        [Fact]
        public async Task ObserveEvent()
        {
            Guid id = Guid.NewGuid();

            var responseId = _testFixture.HubProxy.Observe<Guid>(hub => hub.DelayedAnswerFromServer).FirstAsync();
            _testFixture.HubProxy.Call(hub => hub.SendDelayedWithMs(id, 10));
            (await responseId).Should().Be(id);
        }
    }
}