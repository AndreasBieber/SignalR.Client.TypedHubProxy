using System;

namespace SignalR.Client.TypedHubProxy.Tests
{
    public interface ITestHub
    {
        /// <summary>
        /// Does nothing.
        /// </summary>
        /// <param name="guid">Guid.</param>
        void ThrowAway(Guid guid);

        /// <summary>
        /// Returns the given guid.
        /// </summary>
        /// <param name="guid">Guid.</param>
        Guid PingBackGuid(Guid guid);
        
        /// <summary>
        /// Sends the given guid after x ms.
        /// </summary>
        /// <param name="guid">Guid.</param>
        /// <param name="ms">Milliseconds.</param>
        void SendDelayedWithMs(Guid guid, int ms);

        void SendDelayed();
        void SendDelayed(int param1);
        void SendDelayed(int param1, int param2);
        void SendDelayed(int param1, int param2, int param3);
        void SendDelayed(int param1, int param2, int param3, int param4);
        void SendDelayed(int param1, int param2, int param3, int param4, int param5);
        void SendDelayed(int param1, int param2, int param3, int param4, int param5, int param6);
        void SendDelayed(int param1, int param2, int param3, int param4, int param5, int param6, int param7);
    }
}
