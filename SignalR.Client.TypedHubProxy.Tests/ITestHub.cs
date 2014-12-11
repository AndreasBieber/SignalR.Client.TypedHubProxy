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
        void SendDelayed(Guid guid, int ms);
    }
}
