using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Microsoft.AspNet.SignalR.Client
{
    /// <summary>
    ///     ITypedHubProxy.
    /// </summary>
    /// <typeparam name="TServerHubInterface">Interface of the server hub.</typeparam>
    /// <typeparam name="TClientInterface">Interface which contains the client events.</typeparam>
    public interface ITypedHubProxy<TServerHubInterface, TClientInterface> : IDisposable
    {
        /// <summary>
        ///     Calls a method on the server hub.
        ///     <para>This call will be executed synchronously.</para>
        /// </summary>
        /// <param name="call">The method to call. Use like: <code>hub => hub.MyMethod("param1", "param2")</code></param>
        void Call(Expression<Action<TServerHubInterface>> call);

        /// <summary>
        ///     Calls a method on the server hub.
        ///     <para>This call will be executed synchronously and will wait till the server has sent a response.</para>
        /// </summary>
        /// <param name="call">The method to call. Use like: <code>hub => hub.MyMethod("param1", "param2")</code></param>
        TResult Call<TResult>(Expression<Func<TServerHubInterface, TResult>> call);

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

        /// <summary>
        ///     Subscribes to a hub event.
        ///     <para>When the server hub send an event of the given type, the handler will be invoked.</para>
        /// </summary>
        /// <param name="eventToBind">The event method exposed by the server hub interface.</param>
        /// <param name="action">The method that you handle the event.</param>
        void SubscribeOn(Expression<Func<TClientInterface, Action>> eventToBind, Action action);

        /// <summary>
        ///     Subscribes to a hub event.
        ///     <para>When the server hub send an event of the given type, the handler will be invoked.</para>
        /// </summary>
        /// <param name="eventToBind">The event method exposed by the server hub interface.</param>
        /// <param name="callback">The method that handle the event.</param>
        void SubscribeOn<T>(Expression<Func<TClientInterface, Action<T>>> eventToBind, Action<T> callback);

        /// <summary>
        ///     Subscribes to a hub event.
        ///     <para>When the server hub send an event of the given type, the handler will be invoked.</para>
        /// </summary>
        /// <param name="eventToBind">The event method exposed by the server hub interface.</param>
        /// <param name="wherePredicate">The callback will only be called when this predicate is true.</param>
        /// <param name="callback">The method that you handle the event.</param>
        void SubscribeOn<T>(Expression<Func<TClientInterface, Action<T>>> eventToBind, Func<T, bool> wherePredicate, Action<T> callback);

        /// <summary>
        ///     Subscribes to a hub event.
        ///     <para>When the server hub send an event of the given type, the handler will be invoked.</para>
        /// </summary>
        /// <param name="eventToBind">The event method exposed by the server hub interface.</param>
        /// <param name="callback">The method that you handle the event.</param>
        void SubscribeOn<T1, T2>(Expression<Func<TClientInterface, Action<T1, T2>>> eventToBind, Action<T1, T2> callback);

        /// <summary>
        ///     Subscribes to a hub event.
        ///     <para>When the server hub send an event of the given type, the handler will be invoked.</para>
        /// </summary>
        /// <param name="eventToBind">The event method exposed by the server hub interface.</param>
        /// <param name="wherePredicate">The callback will only be called when this predicate is true.</param>
        /// <param name="callback">The method that you handle the event.</param>
        void SubscribeOn<T1, T2>(Expression<Func<TClientInterface, Action<T1, T2>>> eventToBind, Func<T1, T2, bool> wherePredicate, Action<T1, T2> callback);


        /// <summary>
        ///     Subscribes to a hub event.
        ///     <para>When the server hub send an event of the given type, the handler will be invoked.</para>
        /// </summary>
        /// <param name="eventToBind">The event method exposed by the server hub interface.</param>
        /// <param name="callback">The method that you handle the event.</param>
        void SubscribeOn<T1, T2, T3>(Expression<Func<TClientInterface, Action<T1, T2, T3>>> eventToBind,
            Action<T1, T2, T3> callback);

        /// <summary>
        ///     Subscribes to a hub event.
        ///     <para>When the server hub send an event of the given type, the handler will be invoked.</para>
        /// </summary>
        /// <param name="eventToBind">The event method exposed by the server hub interface.</param>
        /// <param name="wherePredicate">The callback will only be called when this predicate is true.</param>
        /// <param name="callback">The method that you handle the event.</param>
        void SubscribeOn<T1, T2, T3>(Expression<Func<TClientInterface, Action<T1, T2, T3>>> eventToBind, Func<T1, T2, T3, bool> wherePredicate,
            Action<T1, T2, T3> callback);

        /// <summary>
        ///     Subscribes to a hub event.
        ///     <para>When the server hub send an event of the given type, the handler will be invoked.</para>
        /// </summary>
        /// <param name="eventToBind">The event method exposed by the server hub interface.</param>
        /// <param name="callback">The method that you handle the event.</param>
        void SubscribeOn<T1, T2, T3, T4>(Expression<Func<TClientInterface, Action<T1, T2, T3, T4>>> eventToBind,
            Action<T1, T2, T3, T4> callback);

        /// <summary>
        ///     Subscribes to a hub event.
        ///     <para>When the server hub send an event of the given type, the handler will be invoked.</para>
        /// </summary>
        /// <param name="eventToBind">The event method exposed by the server hub interface.</param>
        /// <param name="wherePredicate">The callback will only be called when this predicate is true.</param>
        /// <param name="callback">The method that you handle the event.</param>
        void SubscribeOn<T1, T2, T3, T4>(Expression<Func<TClientInterface, Action<T1, T2, T3, T4>>> eventToBind, Func<T1, T2, T3, T4, bool> wherePredicate,
            Action<T1, T2, T3, T4> callback);

        /// <summary>
        ///     Subscribes to a hub event.
        ///     <para>When the server hub send an event of the given type, the handler will be invoked.</para>
        /// </summary>
        /// <param name="eventToBind">The event method exposed by the server hub interface.</param>
        /// <param name="callback">The method that you handle the event.</param>
        void SubscribeOn<T1, T2, T3, T4, T5>(
            Expression<Func<TClientInterface, Action<T1, T2, T3, T4, T5>>> eventToBind,
            Action<T1, T2, T3, T4, T5> callback);

        /// <summary>
        ///     Subscribes to a hub event.
        ///     <para>When the server hub send an event of the given type, the handler will be invoked.</para>
        /// </summary>
        /// <param name="eventToBind">The event method exposed by the server hub interface.</param>
        /// <param name="wherePredicate">The callback will only be called when this predicate is true.</param>
        /// <param name="callback">The method that you handle the event.</param>
        void SubscribeOn<T1, T2, T3, T4, T5>(
            Expression<Func<TClientInterface, Action<T1, T2, T3, T4, T5>>> eventToBind, Func<T1, T2, T3, T4, T5, bool> wherePredicate,
            Action<T1, T2, T3, T4, T5> callback);

        /// <summary>
        ///     Subscribes to a hub event.
        ///     <para>When the server hub send an event of the given type, the handler will be invoked.</para>
        /// </summary>
        /// <param name="eventToBind">The event method exposed by the server hub interface.</param>
        /// <param name="callback">The method that you handle the event.</param>
        void SubscribeOn<T1, T2, T3, T4, T5, T6>(
            Expression<Func<TClientInterface, Action<T1, T2, T3, T4, T5, T6>>> eventToBind,
            Action<T1, T2, T3, T4, T5, T6> callback);

        /// <summary>
        ///     Subscribes to a hub event.
        ///     <para>When the server hub send an event of the given type, the handler will be invoked.</para>
        /// </summary>
        /// <param name="eventToBind">The event method exposed by the server hub interface.</param>
        /// <param name="wherePredicate">The callback will only be called when this predicate is true.</param>
        /// <param name="callback">The method that you handle the event.</param>
        void SubscribeOn<T1, T2, T3, T4, T5, T6>(
            Expression<Func<TClientInterface, Action<T1, T2, T3, T4, T5, T6>>> eventToBind, Func<T1, T2, T3, T4, T5, T6, bool> wherePredicate,
            Action<T1, T2, T3, T4, T5, T6> callback);

        /// <summary>
        ///     Subscribes to a hub event.
        ///     <para>When the server hub send an event of the given type, the handler will be invoked.</para>
        /// </summary>
        /// <param name="eventToBind">The event method exposed by the server hub interface.</param>
        /// <param name="callback">The method that you handle the event.</param>
        void SubscribeOn<T1, T2, T3, T4, T5, T6, T7>(
            Expression<Func<TClientInterface, Action<T1, T2, T3, T4, T5, T6, T7>>> eventToBind,
            Action<T1, T2, T3, T4, T5, T6, T7> callback);

        /// <summary>
        ///     Subscribes to a hub event.
        ///     <para>When the server hub send an event of the given type, the handler will be invoked.</para>
        /// </summary>
        /// <param name="eventToBind">The event method exposed by the server hub interface.</param>
        /// <param name="wherePredicate">The callback will only be called when this predicate is true.</param>
        /// <param name="callback">The method that you handle the event.</param>
        void SubscribeOn<T1, T2, T3, T4, T5, T6, T7>(
            Expression<Func<TClientInterface, Action<T1, T2, T3, T4, T5, T6, T7>>> eventToBind, Func<T1, T2, T3, T4, T5, T6, T7, bool> wherePredicate,
            Action<T1, T2, T3, T4, T5, T6, T7> callback);

        /// <summary>
        ///     Subscribes on all events (methods) which the server can trigger (invoke).
        /// </summary>
        /// <param name="instance">The instance which implements the client interface (TClientInterface).</param>
        void SubscribeOnAll(object instance);
    }
}