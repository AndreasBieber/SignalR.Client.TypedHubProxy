using System;

namespace SignalR.Client.TypedHubProxy.Tests.Contracts
{
    public class TestClientContract : IClientContract
    {
        private readonly Action<object[]> _callback;

        public TestClientContract(Action<object[]> callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            _callback = callback;
        }

        public void DelayedAnswerFromServer(Guid guid)
        {
            DoCallback(guid);
        }

        public void Passing1Param(int param1)
        {
            DoCallback(param1);
        }

        public void Passing2Params(int param1, int param2)
        {
            DoCallback(param1, param2);
        }

        public void Passing3Params(int param1, int param2, int param3)
        {
            DoCallback(param1, param2, param3);
        }

        public void Passing4Params(int param1, int param2, int param3, int param4)
        {
            DoCallback(param1, param2, param3, param4);
        }

        public void Passing5Params(int param1, int param2, int param3, int param4, int param5)
        {
            DoCallback(param1, param2, param3, param4, param5);
        }

        public void Passing6Params(int param1, int param2, int param3, int param4, int param5, int param6)
        {
            DoCallback(param1, param2, param3, param4, param5, param6);
        }

        public void Passing7Params(int param1, int param2, int param3, int param4, int param5, int param6, int param7)
        {
            DoCallback(param1, param2, param3, param4, param5, param6, param7);
        }

        public void PassingNoParams()
        {
            DoCallback();
        }

        public void Timer(long tick)
        {
            DoCallback(tick);
        }

        private void DoCallback(params object[] args)
        {
            _callback?.Invoke(args);
        }
    }
}