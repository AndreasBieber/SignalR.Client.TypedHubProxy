using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Microsoft.AspNet.SignalR.Client
{
    /// <summary>
    ///     ITypedHubProxy.
    /// </summary>
    /// <typeparam name="TServerHubInterface">Interface of the server hub.</typeparam>
    public interface IHubProxyOneWay<TServerHubInterface>
        where TServerHubInterface : class
    {
        /// <summary>
        ///     Calls a method on the server hub.
        ///     <para>This call will be executed asynchronously.</para>
        /// </summary>
        /// <param name="call">The method to call. Use like: <code>hub => hub.MyMethod("param1", "param2")</code></param>
        Task CallAsync(Expression<Action<TServerHubInterface>> call);

        /// <summary>
        ///     Calls an asynchronous method on the server hub.
        ///     <para>This call will be executed asynchronously.</para>
        /// </summary>
        /// <param name="call">The asynchronous method to call. Use like: <code>hub => hub.MyMethod("param1", "param2")</code></param>
        Task CallAsync(Expression<Func<TServerHubInterface, Task>> call);

        /// <summary>
        ///     Calls a method on the server hub.
        ///     <para>This call will be executed asynchronously. A task will be returned which contains the response.</para>
        /// </summary>
        /// <param name="call">The method to call. Use like: <code>hub => hub.MyMethod("param1", "param2")</code></param>
        Task<TResult> CallAsync<TResult>(Expression<Func<TServerHubInterface, TResult>> call);

        /// <summary>
        ///     Calls an asynchronous method on the server hub.
        ///     <para>This call will be executed asynchronously. A task will be returned which contains the response.</para>
        /// </summary>
        /// <param name="call">The asynchronous method to call. Use like: <code>hub => hub.MyMethod("param1", "param2")</code></param>
        Task<TResult> CallAsync<TResult>(Expression<Func<TServerHubInterface, Task<TResult>>> call);
    }
}