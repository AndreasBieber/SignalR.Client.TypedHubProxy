using System;

namespace SignalR.Client.TypedHubProxy.Tests
{
    public interface ITestHubClientEvents
    {
        void DelayedAnswerFromServer(Guid guid);

        void DelayedAnswer();
        void DelayedAnswer(int param1);
        void DelayedAnswer(int param1, int param2);
        void DelayedAnswer(int param1, int param2, int param3);
        void DelayedAnswer(int param1, int param2, int param3, int param4);
        void DelayedAnswer(int param1, int param2, int param3, int param4, int param5);
        void DelayedAnswer(int param1, int param2, int param3, int param4, int param5, int param6);
        void DelayedAnswer(int param1, int param2, int param3, int param4, int param5, int param6, int param7);
    }
}
