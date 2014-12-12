using System;
using System.Collections.Generic;
using System.Linq;
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

            _testFixture.HubProxy.SubscribeOn<Guid>(hub => hub.DelayedAnswerFromServer, responseId => responseId.Should().Be(id));
            _testFixture.HubProxy.Call(hub => hub.SendDelayedWithMs(id, 10));
        }

        [Fact]
        public void SubscribeOnEventWithPredicate()
        {
            bool gotResponse = false;
            _testFixture.HubProxy.SubscribeOn<Guid>(hub => hub.DelayedAnswerFromServer, param => param == Guid.NewGuid(), param => gotResponse = true);
            _testFixture.HubProxy.Call(hub => hub.SendDelayedWithMs(Guid.NewGuid(), 10));

            Thread.Sleep(100);
            gotResponse.Should().BeFalse("because the event should only be fired when the predicate is true.");
        }

        [Fact]
        public void TestSubscribeOnEventsWithAllParameters()
        {
            const string assertMessage = "because {0} should be {1}, but was {2}";
            const int inParam1 = 1;
            const int inParam2 = 2;
            const int inParam3 = 3;
            const int inParam4 = 4;
            const int inParam5 = 5;
            const int inParam6 = 6;
            const int inParam7 = 7;

            var responses = new List<AsyncResult>
            {
                new AsyncResult(),
                new AsyncResult(),
                new AsyncResult(),
                new AsyncResult(),
                new AsyncResult(),
                new AsyncResult(),
                new AsyncResult(),
                new AsyncResult(),
            };

            // with 0 parameters
            _testFixture.HubProxy.SubscribeOn(hub => hub.DelayedAnswer, () => 
                RememberExceptionForAsyncResponse(0, responses, () => { }));

            // with 1 parameter
            _testFixture.HubProxy.SubscribeOn<int>(hub => hub.DelayedAnswer, outParam1 => 
                RememberExceptionForAsyncResponse(1, responses, () =>
                    outParam1.Should().Be(inParam1, assertMessage, "outParam1", inParam1, outParam1)));

            // with 2 parameters
            _testFixture.HubProxy.SubscribeOn<int,int>(hub => hub.DelayedAnswer, (outParam1, outParam2) => 
                RememberExceptionForAsyncResponse(2, responses, () =>
                {
                    outParam1.Should().Be(inParam1, assertMessage, "outParam1", inParam1, outParam1);
                    outParam2.Should().Be(inParam2, assertMessage, "outParam2", inParam2, outParam2);
                }));

            // with 3 parameters
            _testFixture.HubProxy.SubscribeOn<int, int, int>(hub => hub.DelayedAnswer, (outParam1, outParam2, outParam3) => 
                RememberExceptionForAsyncResponse(3, responses, () =>
                {
                    outParam1.Should().Be(inParam1, assertMessage, "outParam1", inParam1, outParam1);
                    outParam2.Should().Be(inParam2, assertMessage, "outParam2", inParam2, outParam2);
                    outParam3.Should().Be(inParam3, assertMessage, "outParam3", inParam2, outParam3);
                }));

            // with 4 parameters
            _testFixture.HubProxy.SubscribeOn<int, int, int, int>(hub => hub.DelayedAnswer, (outParam1, outParam2, outParam3, outParam4) =>
                RememberExceptionForAsyncResponse(4, responses, () =>
                {
                    outParam1.Should().Be(inParam1, assertMessage, "outParam1", inParam1, outParam1);
                    outParam2.Should().Be(inParam2, assertMessage, "outParam2", inParam2, outParam2);
                    outParam3.Should().Be(inParam3, assertMessage, "outParam3", inParam3, outParam3);
                    outParam4.Should().Be(inParam4, assertMessage, "outParam4", inParam4, outParam4);
                }));

            // with 5 parameters
            _testFixture.HubProxy.SubscribeOn<int, int, int, int, int>(hub => hub.DelayedAnswer, (outParam1, outParam2, outParam3, outParam4, outParam5) =>
                RememberExceptionForAsyncResponse(5, responses, () =>
                {
                    outParam1.Should().Be(inParam1, assertMessage, "outParam1", inParam1, outParam1);
                    outParam2.Should().Be(inParam2, assertMessage, "outParam2", inParam2, outParam2);
                    outParam3.Should().Be(inParam3, assertMessage, "outParam3", inParam3, outParam3);
                    outParam4.Should().Be(inParam4, assertMessage, "outParam4", inParam4, outParam4);
                    outParam5.Should().Be(inParam5, assertMessage, "outParam5", inParam5, outParam5);
                }));

            // with 6 parameters
            _testFixture.HubProxy.SubscribeOn<int, int, int, int, int, int>(hub => hub.DelayedAnswer, (outParam1, outParam2, outParam3, outParam4, outParam5, outParam6) =>
                RememberExceptionForAsyncResponse(6, responses, () =>
                {
                    outParam1.Should().Be(inParam1, assertMessage, "outParam1", inParam1, outParam1);
                    outParam2.Should().Be(inParam2, assertMessage, "outParam2", inParam2, outParam2);
                    outParam3.Should().Be(inParam3, assertMessage, "outParam3", inParam3, outParam3);
                    outParam4.Should().Be(inParam4, assertMessage, "outParam4", inParam4, outParam4);
                    outParam5.Should().Be(inParam5, assertMessage, "outParam5", inParam5, outParam5);
                    outParam6.Should().Be(inParam6, assertMessage, "outParam6", inParam6, outParam6);
                }));

            // with 7 parameters
            _testFixture.HubProxy.SubscribeOn<int, int, int, int, int, int, int>(hub => hub.DelayedAnswer, (outParam1, outParam2, outParam3, outParam4, outParam5, outParam6, outParam7) =>
                RememberExceptionForAsyncResponse(7, responses, () =>
                {
                    outParam1.Should().Be(inParam1, assertMessage, "outParam1", inParam1, outParam1);
                    outParam2.Should().Be(inParam2, assertMessage, "outParam2", inParam2, outParam2);
                    outParam3.Should().Be(inParam3, assertMessage, "outParam3", inParam3, outParam3);
                    outParam4.Should().Be(inParam4, assertMessage, "outParam4", inParam4, outParam4);
                    outParam5.Should().Be(inParam5, assertMessage, "outParam5", inParam5, outParam5);
                    outParam6.Should().Be(inParam6, assertMessage, "outParam6", inParam6, outParam6);
                    outParam7.Should().Be(inParam7, assertMessage, "outParam7", inParam7, outParam7);
                }));

            _testFixture.HubProxy.Call(hub => hub.SendDelayed());
            _testFixture.HubProxy.Call(hub => hub.SendDelayed(inParam1));
            _testFixture.HubProxy.Call(hub => hub.SendDelayed(inParam1, inParam2));
            _testFixture.HubProxy.Call(hub => hub.SendDelayed(inParam1, inParam2, inParam3));
            _testFixture.HubProxy.Call(hub => hub.SendDelayed(inParam1, inParam2, inParam3, inParam4));
            _testFixture.HubProxy.Call(hub => hub.SendDelayed(inParam1, inParam2, inParam3, inParam4, inParam5));
            _testFixture.HubProxy.Call(hub => hub.SendDelayed(inParam1, inParam2, inParam3, inParam4, inParam5, inParam6));
            _testFixture.HubProxy.Call(hub => hub.SendDelayed(inParam1, inParam2, inParam3, inParam4, inParam5, inParam6, inParam7));

            Task getAllResponsesTask = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (responses.All(r => r.GotResponse))
                    {
                        return;
                    }
                }
            }, TaskCreationOptions.LongRunning);

            Task.WaitAll(new []{getAllResponsesTask}, TimeSpan.FromSeconds(15))
                .Should()
                .BeTrue("because all responses should be received witin 5 seconds (got {0} responses out of {1})", responses.Count(r => r.GotResponse), responses.Count);

            AsyncResult erroneousResult = responses.FirstOrDefault(r => r.Exception != null);

            if (erroneousResult != null)
            {
                throw erroneousResult.Exception;
            }
        }

        private void RememberExceptionForAsyncResponse(int responseCount, List<AsyncResult>persistTo,
            Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                persistTo[responseCount].Exception = ex;
            }

            persistTo[responseCount].GotResponse = true;
        }

        private class AsyncResult
        {
            public bool GotResponse { get; set; }
            public Exception Exception { get; set; }
        }
    }
}
