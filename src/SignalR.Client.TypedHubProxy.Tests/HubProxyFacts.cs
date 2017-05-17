using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using JetBrains.Annotations;
using SignalR.Client.TypedHubProxy.Tests.Contracts;
using SignalR.Client.TypedHubProxy.Tests.TestFixtures;
using SignalR.Client.TypedHubProxy.Tests.Utils;
using Xunit;

namespace SignalR.Client.TypedHubProxy.Tests
{
    [PublicAPI]
    public class TypedHubProxyTests : IClassFixture<TypedHubProxyFixture>
    {
        private readonly TypedHubProxyFixture _fixture;

        public TypedHubProxyTests(TypedHubProxyFixture data)
        {
            _fixture = data;
        }

        [Fact]
        public void TestCallAsync()
        {
            var invoked = false;

            _fixture.HubProxyMock.SetupCallback(args => invoked = true)
                .Returns(TaskAsyncHelper.Empty);

            _fixture.Proxy.CallAsync(hub => hub.DoNothing());
            invoked.Should().BeTrue(TestConsts.ERR_PROXY_INVOKED_CORRECTLY);
        }

        [Fact]
        public void TestCallWithParamAsync()
        {
            var inParam1 = Guid.NewGuid();
            var outParam1 = Guid.Empty;
            var invoked = false;

            _fixture.HubProxyMock.SetupCallback(args =>
            {
                invoked = true;
                outParam1 = (Guid) args[0];
            })
                .Returns(TaskAsyncHelper.Empty);

            _fixture.Proxy.CallAsync(hub => hub.DoNothingWithParam(inParam1));

            invoked.Should().BeTrue(TestConsts.ERR_PROXY_INVOKED_CORRECTLY);
            outParam1.Should().Be(inParam1, TestConsts.ERR_PROXY_ROUTE_CORRECTLY);
        }

        [Fact]
        public void TestCallWithResultAsync()
        {
            var inParam1 = Guid.NewGuid();
            var invoked = false;

            _fixture.HubProxyMock.SetupCallback<Guid>(args => invoked = true)
                .Returns<string, object[]>((methodName, args) => TaskAsyncHelper.FromResult((Guid) args[0]));

            var outParam1 = _fixture.Proxy.CallAsync(hub => hub.ReturnGuid(inParam1)).Result;

            invoked.Should().BeTrue(TestConsts.ERR_PROXY_INVOKED_CORRECTLY);
            outParam1.Should().Be(inParam1, TestConsts.ERR_PROXY_ROUTE_CORRECTLY);
        }

        [Fact]
        public void TestSubscribeOnEvent()
        {
            var notified = false;

            _fixture.Proxy.SubscribeOn(hub => hub.PassingNoParams, () => notified = true);
            _fixture.HubProxyMock.Object.InvokeEvent(hub => hub.PassingNoParams());

            notified.Should().BeTrue(TestConsts.ERR_PROXY_RECEIVE_EVENT);
        }

        [Fact]
        public void TestSubscribeOnEventWhereConditionIsFalse()
        {
            var notified = false;

            _fixture.Proxy.SubscribeOn<int>(
                eventToBind: hub => hub.Passing1Param,
                wherePredicate: param1 => param1 == 2,
                callback: param1 => notified = true);

            _fixture.HubProxyMock.Object.InvokeEvent(hub => hub.Passing1Param(1));

            notified.Should().BeFalse(TestConsts.ERR_PROXY_RECEIVE_EVENT_WHERE_FALSE);
        }

        [Fact]
        public void TestSubscribeOnEventWhereConditionIsTrue()
        {
            var notified = false;

            _fixture.Proxy.SubscribeOn<int>(
                eventToBind: hub => hub.Passing1Param,
                wherePredicate: param1 => param1 == 1,
                callback: param1 => notified = true);

            _fixture.HubProxyMock.Object.InvokeEvent(hub => hub.Passing1Param(1));

            notified.Should().BeTrue(TestConsts.ERR_PROXY_RECEIVE_EVENT_WHERE_TRUE);
        }

        [Fact]
        public void TestSubscribeOnEventsWithAllPossibleParameters()
        {
            const int inParam1 = 1;
            const int inParam2 = 2;
            const int inParam3 = 3;
            const int inParam4 = 4;
            const int inParam5 = 5;
            const int inParam6 = 6;
            const int inParam7 = 7;

            var notified = false;

            // test with 1 parameter
            _fixture.Proxy.SubscribeOn<int>(hub => hub.Passing1Param, outParam1 =>
            {
                notified = true;
                outParam1.Should().Be(inParam1, TestConsts.ERR_PARAM_MISMATCH, nameof(outParam1), inParam1, outParam1);
            });
            _fixture.HubProxyMock.Object.InvokeEvent(hub => hub.Passing1Param(inParam1));
            notified.Should().BeTrue(TestConsts.ERR_PROXY_RECEIVE_EVENT);
            notified = false;

            // test with 2 parameter
            _fixture.Proxy.SubscribeOn<int, int>(hub => hub.Passing2Params, (outParam1, outParam2) =>
            {
                notified = true;
                outParam1.Should().Be(inParam1, TestConsts.ERR_PARAM_MISMATCH, nameof(outParam1), inParam1, outParam1);
                outParam2.Should().Be(inParam2, TestConsts.ERR_PARAM_MISMATCH, nameof(outParam2), inParam2, outParam2);
            });
            _fixture.HubProxyMock.Object.InvokeEvent(hub => hub.Passing2Params(inParam1, inParam2));
            notified.Should().BeTrue(TestConsts.ERR_PROXY_RECEIVE_EVENT);
            notified = false;

            // test with 3 parameter
            _fixture.Proxy.SubscribeOn<int, int, int>(hub => hub.Passing3Params, (outParam1, outParam2, outParam3) =>
            {
                notified = true;
                outParam1.Should().Be(inParam1, TestConsts.ERR_PARAM_MISMATCH, nameof(outParam1), inParam1, outParam1);
                outParam2.Should().Be(inParam2, TestConsts.ERR_PARAM_MISMATCH, nameof(outParam2), inParam2, outParam2);
                outParam3.Should().Be(inParam3, TestConsts.ERR_PARAM_MISMATCH, nameof(outParam3), inParam3, outParam3);
            });
            _fixture.HubProxyMock.Object.InvokeEvent(hub => hub.Passing3Params(inParam1, inParam2, inParam3));
            notified.Should().BeTrue(TestConsts.ERR_PROXY_RECEIVE_EVENT);
            notified = false;

            // test with 4 parameter
            _fixture.Proxy.SubscribeOn<int, int, int, int>(hub => hub.Passing4Params,
                (outParam1, outParam2, outParam3, outParam4) =>
                {
                    notified = true;
                    outParam1.Should().Be(inParam1, TestConsts.ERR_PARAM_MISMATCH, nameof(outParam1), inParam1, outParam1);
                    outParam2.Should().Be(inParam2, TestConsts.ERR_PARAM_MISMATCH, nameof(outParam2), inParam2, outParam2);
                    outParam3.Should().Be(inParam3, TestConsts.ERR_PARAM_MISMATCH, nameof(outParam3), inParam3, outParam3);
                    outParam4.Should().Be(inParam4, TestConsts.ERR_PARAM_MISMATCH, nameof(outParam4), inParam4, outParam4);
                });
            _fixture.HubProxyMock.Object.InvokeEvent(hub => hub.Passing4Params(inParam1, inParam2, inParam3, inParam4));
            notified.Should().BeTrue(TestConsts.ERR_PROXY_RECEIVE_EVENT);
            notified = false;

            // test with 5 parameter
            _fixture.Proxy.SubscribeOn<int, int, int, int, int>(hub => hub.Passing5Params,
                (outParam1, outParam2, outParam3, outParam4, outParam5) =>
                {
                    notified = true;
                    outParam1.Should().Be(inParam1, TestConsts.ERR_PARAM_MISMATCH, nameof(outParam1), inParam1, outParam1);
                    outParam2.Should().Be(inParam2, TestConsts.ERR_PARAM_MISMATCH, nameof(outParam2), inParam2, outParam2);
                    outParam3.Should().Be(inParam3, TestConsts.ERR_PARAM_MISMATCH, nameof(outParam3), inParam3, outParam3);
                    outParam4.Should().Be(inParam4, TestConsts.ERR_PARAM_MISMATCH, nameof(outParam4), inParam4, outParam4);
                    outParam5.Should().Be(inParam5, TestConsts.ERR_PARAM_MISMATCH, nameof(outParam5), inParam5, outParam5);
                });
            _fixture.HubProxyMock.Object.InvokeEvent(
                hub => hub.Passing5Params(inParam1, inParam2, inParam3, inParam4, inParam5));
            notified.Should().BeTrue(TestConsts.ERR_PROXY_RECEIVE_EVENT);
            notified = false;

            // test with 6 parameter
            _fixture.Proxy.SubscribeOn<int, int, int, int, int, int>(hub => hub.Passing6Params,
                (outParam1, outParam2, outParam3, outParam4, outParam5, outParam6) =>
                {
                    notified = true;
                    outParam1.Should().Be(inParam1, TestConsts.ERR_PARAM_MISMATCH, nameof(outParam1), inParam1, outParam1);
                    outParam2.Should().Be(inParam2, TestConsts.ERR_PARAM_MISMATCH, nameof(outParam2), inParam2, outParam2);
                    outParam3.Should().Be(inParam3, TestConsts.ERR_PARAM_MISMATCH, nameof(outParam3), inParam3, outParam3);
                    outParam4.Should().Be(inParam4, TestConsts.ERR_PARAM_MISMATCH, nameof(outParam4), inParam4, outParam4);
                    outParam5.Should().Be(inParam5, TestConsts.ERR_PARAM_MISMATCH, nameof(outParam5), inParam5, outParam5);
                    outParam6.Should().Be(inParam6, TestConsts.ERR_PARAM_MISMATCH, nameof(outParam6), inParam6, outParam6);
                });
            _fixture.HubProxyMock.Object.InvokeEvent(
                hub => hub.Passing6Params(inParam1, inParam2, inParam3, inParam4, inParam5, inParam6));
            notified.Should().BeTrue(TestConsts.ERR_PROXY_RECEIVE_EVENT);
            notified = false;

            // test with 7 parameter
            _fixture.Proxy.SubscribeOn<int, int, int, int, int, int, int>(hub => hub.Passing7Params,
                (outParam1, outParam2, outParam3, outParam4, outParam5, outParam6, outParam7) =>
                {
                    notified = true;
                    outParam1.Should().Be(inParam1, TestConsts.ERR_PARAM_MISMATCH, nameof(outParam1), inParam1, outParam1);
                    outParam2.Should().Be(inParam2, TestConsts.ERR_PARAM_MISMATCH, nameof(outParam2), inParam2, outParam2);
                    outParam3.Should().Be(inParam3, TestConsts.ERR_PARAM_MISMATCH, nameof(outParam3), inParam3, outParam3);
                    outParam4.Should().Be(inParam4, TestConsts.ERR_PARAM_MISMATCH, nameof(outParam4), inParam4, outParam4);
                    outParam5.Should().Be(inParam5, TestConsts.ERR_PARAM_MISMATCH, nameof(outParam5), inParam5, outParam5);
                    outParam6.Should().Be(inParam6, TestConsts.ERR_PARAM_MISMATCH, nameof(outParam6), inParam6, outParam6);
                    outParam7.Should().Be(inParam7, TestConsts.ERR_PARAM_MISMATCH, nameof(outParam7), inParam7, outParam7);
                });
            _fixture.HubProxyMock.Object.InvokeEvent(
                hub => hub.Passing7Params(inParam1, inParam2, inParam3, inParam4, inParam5, inParam6, inParam7));
            notified.Should().BeTrue(TestConsts.ERR_PROXY_RECEIVE_EVENT);
        }

        [Fact]
        public void TestSubscribeOnAll()
        {
            Action<object[]> actCallback = args =>
            {
                args[0].Should().Be(1);
                args[1].Should().Be(2);
            };

            var clientContract = new TestClientContract(actCallback);
            var subscriptions = _fixture.Proxy.SubscribeOnAll(clientContract);
            _fixture.HubProxyMock.Object.InvokeEvent(hub => hub.Passing2Params(1, 2));

            subscriptions?.ToList().ForEach(s => s.Dispose());
        }

        [Fact]
        public void TestSubscribeOnAllShouldFailWhenTheClientInterfaceHasFunc()
        {
            var clientContract = new WrongClientContract();
            IEnumerable<IDisposable> subscriptions = null;
            Action act = () => subscriptions = _fixture.WrongProxy.SubscribeOnAll(clientContract)?.ToList();

            act.ShouldThrow<NotSupportedException>();
        }

        /// <summary>
        ///     Test bugfix for issue #9.
        /// </summary>
        [Fact]
        public void TestSubscribeOnAllWithMoreThan2ParametersShouldNotFail()
        {
            Action<object[]> actCallback = args =>
            {
                args[0].Should().Be(1);
                args[1].Should().Be(2);
                args[2].Should().Be(3);
            };

            var clientContract = new TestClientContract(actCallback);
            IEnumerable<IDisposable> subscriptions = null;
            Action act = () => subscriptions = _fixture.Proxy.SubscribeOnAll(clientContract);
            act.ShouldNotThrow(TestConsts.ERR_SHOULD_FIX_ISSUE_9);

            _fixture.HubProxyMock.Object.InvokeEvent(hub => hub.Passing3Params(1, 2, 3));

            subscriptions?.ToList().ForEach(s => s.Dispose());
        }
    }
}