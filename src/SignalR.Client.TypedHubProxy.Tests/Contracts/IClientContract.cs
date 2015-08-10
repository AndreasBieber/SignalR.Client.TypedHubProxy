namespace SignalR.Client.TypedHubProxy.Tests.Contracts
{
    using System;

    public interface IClientContract
    {
        void DelayedAnswerFromServer(Guid guid);

        void PassingNoParams();
        void Passing1Param(int param1);
        void Passing2Params(int param1, int param2);
        void Passing3Params(int param1, int param2, int param3);
        void Passing4Params(int param1, int param2, int param3, int param4);
        void Passing5Params(int param1, int param2, int param3, int param4, int param5);
        void Passing6Params(int param1, int param2, int param3, int param4, int param5, int param6);
        void Passing7Params(int param1, int param2, int param3, int param4, int param5, int param6, int param7);

        void Timer(long tick);
    }
}
