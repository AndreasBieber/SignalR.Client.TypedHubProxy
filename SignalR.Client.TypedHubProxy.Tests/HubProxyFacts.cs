using SignalR.Client.TypedHubProxy.Tests.Contracts;

namespace SignalR.Client.TypedHubProxy.Tests
{
    using System;
    using FluentAssertions;
    using Xunit;
    using Utils;

    public class TypedHubProxyTests : IClassFixture<TestFixtures.TypedHubProxyFixture>
    {
        private readonly TestFixtures.TypedHubProxyFixture _fixture;

        public TypedHubProxyTests(TestFixtures.TypedHubProxyFixture data)
        {
            _fixture = data;
        }

        [Fact]
        public void TestCall()
        {
            bool invoked = false;

            _fixture.HubProxyMock.SetupCallback(args => invoked = true)
                .Returns(TaskAsyncHelper.Empty);

            _fixture.Proxy.Call(hub => hub.DoNothing());
            invoked.Should().BeTrue(TestConsts.ERR_PROXY_INVOKED_CORRECTLY);
        }

        [Fact]
        public void TestCallAsync()
        {
            bool invoked = false;

            _fixture.HubProxyMock.SetupCallback(args => invoked = true)
                .Returns(TaskAsyncHelper.Empty);

            _fixture.Proxy.CallAsync(hub => hub.DoNothing());
            invoked.Should().BeTrue(TestConsts.ERR_PROXY_INVOKED_CORRECTLY);
        }

        [Fact]
        public void TestCallWithParam()
        {
            Guid inParam1 = Guid.NewGuid();
            Guid outParam1 = Guid.Empty;
            bool invoked = false;

            _fixture.HubProxyMock.SetupCallback(args =>
            {
                invoked = true;
                outParam1 = (Guid) args[0];
            })
            .Returns(TaskAsyncHelper.Empty);

            _fixture.Proxy.Call(hub => hub.DoNothingWithParam(inParam1));

            invoked.Should().BeTrue(TestConsts.ERR_PROXY_INVOKED_CORRECTLY);
            outParam1.Should().Be(inParam1, TestConsts.ERR_PROXY_ROUTE_CORRECTLY);
        }

        [Fact]
        public void TestCallWithParamAsync()
        {
            Guid inParam1 = Guid.NewGuid();
            Guid outParam1 = Guid.Empty;
            bool invoked = false;

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
        public void TestCallWithResult()
        {
            Guid inParam1 = Guid.NewGuid();
            bool invoked = false;

            _fixture.HubProxyMock.SetupCallback<Guid>(args => invoked = true)
                .Returns<string,object[]>((methodName, args) => TaskAsyncHelper.FromResult((Guid)args[0]));

            Guid outParam1 = _fixture.Proxy.Call(hub => hub.ReturnGuid(inParam1));

            invoked.Should().BeTrue(TestConsts.ERR_PROXY_INVOKED_CORRECTLY);
            outParam1.Should().Be(inParam1, TestConsts.ERR_PROXY_ROUTE_CORRECTLY);
        }

        [Fact]
        public void TestCallWithResultAsync()
        {
            Guid inParam1 = Guid.NewGuid();
            bool invoked = false;

            _fixture.HubProxyMock.SetupCallback<Guid>(args => invoked = true)
                .Returns<string, object[]>((methodName, args) => TaskAsyncHelper.FromResult((Guid)args[0]));

            Guid outParam1 = _fixture.Proxy.CallAsync(hub => hub.ReturnGuid(inParam1)).Result;

            invoked.Should().BeTrue(TestConsts.ERR_PROXY_INVOKED_CORRECTLY);
            outParam1.Should().Be(inParam1, TestConsts.ERR_PROXY_ROUTE_CORRECTLY);
        }


        [Fact]
        public void TestSubscribeOnEvent()
        {
            bool notified = false;

            _fixture.Proxy.SubscribeOn(hub => hub.PassingNoParams, () => notified = true);
            _fixture.HubProxyMock.Object.InvokeEvent(hub => hub.PassingNoParams());

            notified.Should().BeTrue(TestConsts.ERR_PROXY_RECEIVE_EVENT);
        }

        [Fact]
        public void TestSubscribeOnEventWhereConditionIsFalse()
        {
            bool notified = false;

            _fixture.Proxy.SubscribeOn<int>(
                eventToBind: hub => hub.Passing1Param, 
                wherePredicate: param1 => param1 == 2, 
                callback: param1 => notified = true);

            _fixture.HubProxyMock.Object.InvokeEvent(hub => hub.Passing1Param(1));

            notified.Should().BeFalse(TestConsts.ERR_PROXY_RECEIVE_EVENT_WHERE_TRUE);
        }

        [Fact]
        public void TestSubscribeOnEventWhereConditionIsTrue()
        {
            bool notified = false;

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

            bool notified = false;

            // test with 1 parameter
            _fixture.Proxy.SubscribeOn<int>(hub => hub.Passing1Param, outParam1 =>
                                                                      {
                                                                          notified = true;
                                                                          outParam1.Should().Be(inParam1, TestConsts.ERR_PARAM_MISMATCH, "outParam1", inParam1, outParam1);
                                                                      });
            _fixture.HubProxyMock.Object.InvokeEvent(hub => hub.Passing1Param(inParam1));
            notified.Should().BeTrue(TestConsts.ERR_PROXY_RECEIVE_EVENT);
            notified = false;

            // test with 2 parameter
            _fixture.Proxy.SubscribeOn<int,int>(hub => hub.Passing2Params, (outParam1, outParam2) =>
            {
                notified = true;
                outParam1.Should().Be(inParam1, TestConsts.ERR_PARAM_MISMATCH, "outParam1", inParam1, outParam1);
                outParam2.Should().Be(inParam2, TestConsts.ERR_PARAM_MISMATCH, "outParam2", inParam2, outParam2);
            });
            _fixture.HubProxyMock.Object.InvokeEvent(hub => hub.Passing2Params(inParam1, inParam2));
            notified.Should().BeTrue(TestConsts.ERR_PROXY_RECEIVE_EVENT);
            notified = false;

            // test with 3 parameter
            _fixture.Proxy.SubscribeOn<int, int, int>(hub => hub.Passing3Params, (outParam1, outParam2, outParam3) =>
            {
                notified = true;
                outParam1.Should().Be(inParam1, TestConsts.ERR_PARAM_MISMATCH, "outParam1", inParam1, outParam1);
                outParam2.Should().Be(inParam2, TestConsts.ERR_PARAM_MISMATCH, "outParam2", inParam2, outParam2);
                outParam3.Should().Be(inParam3, TestConsts.ERR_PARAM_MISMATCH, "outParam3", inParam3, outParam3);
            });
            _fixture.HubProxyMock.Object.InvokeEvent(hub => hub.Passing3Params(inParam1, inParam2, inParam3));
            notified.Should().BeTrue(TestConsts.ERR_PROXY_RECEIVE_EVENT);
            notified = false;

            // test with 4 parameter
            _fixture.Proxy.SubscribeOn<int,int, int, int>(hub => hub.Passing4Params, (outParam1, outParam2, outParam3, outParam4) =>
            {
                notified = true;
                outParam1.Should().Be(inParam1, TestConsts.ERR_PARAM_MISMATCH, "outParam1", inParam1, outParam1);
                outParam2.Should().Be(inParam2, TestConsts.ERR_PARAM_MISMATCH, "outParam2", inParam2, outParam2);
                outParam3.Should().Be(inParam3, TestConsts.ERR_PARAM_MISMATCH, "outParam3", inParam3, outParam3);
                outParam4.Should().Be(inParam4, TestConsts.ERR_PARAM_MISMATCH, "outParam4", inParam4, outParam4);
            });
            _fixture.HubProxyMock.Object.InvokeEvent(hub => hub.Passing4Params(inParam1, inParam2, inParam3, inParam4));
            notified.Should().BeTrue(TestConsts.ERR_PROXY_RECEIVE_EVENT);
            notified = false;

            // test with 5 parameter
            _fixture.Proxy.SubscribeOn<int,int, int, int, int>(hub => hub.Passing5Params, (outParam1, outParam2, outParam3, outParam4, outParam5) =>
            {
                notified = true;
                outParam1.Should().Be(inParam1, TestConsts.ERR_PARAM_MISMATCH, "outParam1", inParam1, outParam1);
                outParam2.Should().Be(inParam2, TestConsts.ERR_PARAM_MISMATCH, "outParam2", inParam2, outParam2);
                outParam3.Should().Be(inParam3, TestConsts.ERR_PARAM_MISMATCH, "outParam3", inParam3, outParam3);
                outParam4.Should().Be(inParam4, TestConsts.ERR_PARAM_MISMATCH, "outParam4", inParam4, outParam4);
                outParam5.Should().Be(inParam5, TestConsts.ERR_PARAM_MISMATCH, "outParam5", inParam5, outParam5);
            });
            _fixture.HubProxyMock.Object.InvokeEvent(hub => hub.Passing5Params(inParam1, inParam2, inParam3, inParam4, inParam5));
            notified.Should().BeTrue(TestConsts.ERR_PROXY_RECEIVE_EVENT);
            notified = false;

            // test with 6 parameter
            _fixture.Proxy.SubscribeOn<int,int, int, int, int, int>(hub => hub.Passing6Params, (outParam1, outParam2, outParam3, outParam4, outParam5, outParam6) =>
            {
                notified = true;
                outParam1.Should().Be(inParam1, TestConsts.ERR_PARAM_MISMATCH, "outParam1", inParam1, outParam1);
                outParam2.Should().Be(inParam2, TestConsts.ERR_PARAM_MISMATCH, "outParam2", inParam2, outParam2);
                outParam3.Should().Be(inParam3, TestConsts.ERR_PARAM_MISMATCH, "outParam3", inParam3, outParam3);
                outParam4.Should().Be(inParam4, TestConsts.ERR_PARAM_MISMATCH, "outParam4", inParam4, outParam4);
                outParam5.Should().Be(inParam5, TestConsts.ERR_PARAM_MISMATCH, "outParam5", inParam5, outParam5);
                outParam6.Should().Be(inParam6, TestConsts.ERR_PARAM_MISMATCH, "outParam6", inParam6, outParam6);
            });
            _fixture.HubProxyMock.Object.InvokeEvent(hub => hub.Passing6Params(inParam1, inParam2, inParam3, inParam4, inParam5, inParam6));
            notified.Should().BeTrue(TestConsts.ERR_PROXY_RECEIVE_EVENT);
            notified = false;

            // test with 7 parameter
            _fixture.Proxy.SubscribeOn<int,int, int, int, int, int, int>(hub => hub.Passing7Params, (outParam1, outParam2, outParam3, outParam4, outParam5, outParam6, outParam7) =>
            {
                notified = true;
                outParam1.Should().Be(inParam1, TestConsts.ERR_PARAM_MISMATCH, "outParam1", inParam1, outParam1);
                outParam2.Should().Be(inParam2, TestConsts.ERR_PARAM_MISMATCH, "outParam2", inParam2, outParam2);
                outParam3.Should().Be(inParam3, TestConsts.ERR_PARAM_MISMATCH, "outParam3", inParam3, outParam3);
                outParam4.Should().Be(inParam4, TestConsts.ERR_PARAM_MISMATCH, "outParam4", inParam4, outParam4);
                outParam5.Should().Be(inParam5, TestConsts.ERR_PARAM_MISMATCH, "outParam5", inParam5, outParam5);
                outParam6.Should().Be(inParam6, TestConsts.ERR_PARAM_MISMATCH, "outParam6", inParam6, outParam6);
                outParam7.Should().Be(inParam7, TestConsts.ERR_PARAM_MISMATCH, "outParam7", inParam7, outParam7);
            });
            _fixture.HubProxyMock.Object.InvokeEvent(hub => hub.Passing7Params(inParam1, inParam2, inParam3, inParam4, inParam5, inParam6, inParam7));
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
            _fixture.Proxy.SubscribeOnAll(clientContract);
            _fixture.HubProxyMock.Object.InvokeEvent(hub => hub.Passing2Params(1, 2));
        }
    }
}