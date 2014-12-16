using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Microsoft.AspNet.SignalR.Client
{
    /// <summary>
    ///     Strongly typed one way proxy for SignalR.
    ///     It supports only the calls from client to server.
    /// </summary>
    /// <typeparam name="TServerHubInterface">The interface of the server hub.</typeparam>
    public class TypedHubOneWayProxy<TServerHubInterface> : ITypedHubOneWayProxy<TServerHubInterface>
        where TServerHubInterface : class
    {
        /// <summary>
        /// Error message for incorrect type parameter which is not an interface.
        /// </summary>
        protected const string ERR_NOT_AN_INTERFACE = "\"{0}\" is not an interface.";

        /// <summary>
        /// Weakly typed hub proxy.
        /// </summary>
        protected readonly IHubProxy _hubProxy;

        internal TypedHubOneWayProxy(IHubProxy hubProxy)
        {
            _hubProxy = hubProxy;
        }

        internal TypedHubOneWayProxy(HubConnection hubConnection, string hubName)
        {
            if (!typeof (TServerHubInterface).IsInterface)
            {
                throw new ArgumentException(string.Format(ERR_NOT_AN_INTERFACE, typeof (TServerHubInterface).Name));
            }

            _hubProxy = hubConnection.CreateHubProxy(hubName);
        }

        #region ITypedHubOneWayProxy implementations

        void ITypedHubOneWayProxy<TServerHubInterface>.Call(Expression<Action<TServerHubInterface>> call)
        {
            ((ITypedHubOneWayProxy<TServerHubInterface>) this).CallAsync(call).Wait();
        }

        TResult ITypedHubOneWayProxy<TServerHubInterface>.Call<TResult>(
            Expression<Func<TServerHubInterface, TResult>> call)
        {
            return ((ITypedHubOneWayProxy<TServerHubInterface>) this).CallAsync(call).Result;
        }

        Task ITypedHubOneWayProxy<TServerHubInterface>.CallAsync(
            Expression<Action<TServerHubInterface>> call)
        {
            ActionDetail invocation = call.GetActionDetails();
            return _hubProxy.Invoke(invocation.MethodName, invocation.Parameters);
        }

        Task ITypedHubOneWayProxy<TServerHubInterface>.CallAsync(
            Expression<Func<TServerHubInterface, Task>> call)
        {
            ActionDetail invocation = call.GetActionDetails();
            return _hubProxy.Invoke(invocation.MethodName, invocation.Parameters);
        }

        Task<TResult> ITypedHubOneWayProxy<TServerHubInterface>.CallAsync<TResult>(
            Expression<Func<TServerHubInterface, TResult>> call)
        {
            ActionDetail invocation = call.GetActionDetails();
            return _hubProxy.Invoke<TResult>(invocation.MethodName, invocation.Parameters);
        }

        Task<TResult> ITypedHubOneWayProxy<TServerHubInterface>.CallAsync<TResult>(
            Expression<Func<TServerHubInterface, Task<TResult>>> call)
        {
            ActionDetail invocation = call.GetActionDetails();
            return _hubProxy.Invoke<TResult>(invocation.MethodName, invocation.Parameters);
        }

        #endregion
    }
}