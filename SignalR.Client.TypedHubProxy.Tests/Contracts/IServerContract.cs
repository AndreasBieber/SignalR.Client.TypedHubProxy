using System.Threading.Tasks;

namespace SignalR.Client.TypedHubProxy.Tests.Contracts
{
    using System;

    public interface IServerContract
    {
        /// <summary>
        /// Does nothing.
        /// </summary>
        void DoNothing();

        /// <summary>
        /// Does nothing.
        /// </summary>
        /// <param name="guid">Guid.</param>
        void DoNothingWithParam(Guid guid);

        /// <summary>
        /// Returns the given guid.
        /// </summary>
        /// <param name="guid">Guid.</param>
        Guid ReturnGuid(Guid guid);

        /// <summary>
        /// Async operation.
        /// </summary>
        /// <returns></returns>
        Task ReturnTask();
    }
}
