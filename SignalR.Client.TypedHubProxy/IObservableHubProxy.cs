using System;
using System.Linq.Expressions;

namespace Microsoft.AspNet.SignalR.Client
{
    /// <summary>
    ///     IObservableHubProxy.
    /// </summary>
    /// <typeparam name="TServerHubInterface">Interface of the server hub.</typeparam>
    /// <typeparam name="TClientInterface">Interface which contains the client events.</typeparam>
    public interface IObservableHubProxy<TServerHubInterface, TClientInterface> : ITypedHubOneWayProxy<TServerHubInterface>
        where TServerHubInterface : class
        where TClientInterface : class
    {
        /// <summary>
        ///     Subscribes to a hub event.
        ///     <para>When the server hub send an event of the given type, the handler will be invoked.</para>
        /// </summary>
        /// <param name="eventToBind">The event method exposed by the server hub interface.</param>
        IObservable<T> Observe<T>(Expression<Func<TClientInterface, Action<T>>> eventToBind);
    }
}