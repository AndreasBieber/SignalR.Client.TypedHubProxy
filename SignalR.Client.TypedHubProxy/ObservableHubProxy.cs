using System;
using System.Linq.Expressions;
using System.Reactive.Linq;

namespace Microsoft.AspNet.SignalR.Client
{
    /// <summary>
    ///     Observable strongly typed proxy for SignalR.
    /// </summary>
    /// <typeparam name="TServerHubInterface">The interface of the server hub.</typeparam>
    /// <typeparam name="TClientInterface">The interface of the client events.</typeparam>
    public class ObservableHubProxy<TServerHubInterface, TClientInterface> :
        TypedHubOneWayProxy<TServerHubInterface>, IObservableHubProxy<TServerHubInterface, TClientInterface>
        where TServerHubInterface : class
        where TClientInterface : class
    {
        internal ObservableHubProxy(IHubProxy hubProxy) : base(hubProxy)
        {
        }

        internal ObservableHubProxy(HubConnection hubConnection, string hubName) : base(hubConnection, hubName)
        {
            if (!typeof (TClientInterface).IsInterface)
            {
                throw new ArgumentException(string.Format(ERR_NOT_AN_INTERFACE, typeof (TClientInterface).Name));
            }
        }

        #region IObservableHubProxy implementations

        IObservable<T> IObservableHubProxy<TServerHubInterface, TClientInterface>.Observe<T>(
            Expression<Func<TClientInterface, Action<T>>> eventToBind)
        {
            return Observable.Create<T>(observer => _hubProxy.On<T>(eventToBind.GetMethodName(), observer.OnNext));
        }

        #endregion
    }
}