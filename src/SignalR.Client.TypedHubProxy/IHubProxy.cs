using System;
using System.Collections.Generic;

namespace Microsoft.AspNet.SignalR.Client
{
    /// <summary>
    /// Typed Hub Proxy.
    /// </summary>
    /// <typeparam name="TServerHubInterface">Interface of the server hub.</typeparam>
    /// <typeparam name="TClientInterface">Interface which contains the client events.</typeparam>
    public interface IHubProxy<TServerHubInterface, TClientInterface> : IHubProxyOneWay<TServerHubInterface>, System.IDisposable
        where TServerHubInterface : class
        where TClientInterface : class
    {
        /// <summary>
        ///     Subscribes to a hub event.
        ///     <para>When the server hub send an event of the given type, the handler will be invoked.</para>
        /// </summary>
        /// <param name="eventToBind">The event method exposed by the server hub interface.</param>
        /// <param name="action">The method that you handle the event.</param>
        IDisposable SubscribeOn(System.Linq.Expressions.Expression<System.Func<TClientInterface, System.Action>> eventToBind, System.Action action);

        /// <summary>
        ///     Subscribes to a hub event.
        ///     <para>When the server hub send an event of the given type, the handler will be invoked.</para>
        /// </summary>
        /// <param name="eventToBind">The event method exposed by the server hub interface.</param>
        /// <param name="callback">The method that handle the event.</param>
        IDisposable SubscribeOn<T>(System.Linq.Expressions.Expression<System.Func<TClientInterface, System.Action<T>>> eventToBind, System.Action<T> callback);

        /// <summary>
        ///     Subscribes to a hub event.
        ///     <para>When the server hub send an event of the given type, the handler will be invoked.</para>
        /// </summary>
        /// <param name="eventToBind">The event method exposed by the server hub interface.</param>
        /// <param name="wherePredicate">The callback will only be called when this predicate is true.</param>
        /// <param name="callback">The method that you handle the event.</param>
        IDisposable SubscribeOn<T>(System.Linq.Expressions.Expression<System.Func<TClientInterface, System.Action<T>>> eventToBind, System.Func<T, bool> wherePredicate, System.Action<T> callback);

        /// <summary>
        ///     Subscribes to a hub event.
        ///     <para>When the server hub send an event of the given type, the handler will be invoked.</para>
        /// </summary>
        /// <param name="eventToBind">The event method exposed by the server hub interface.</param>
        /// <param name="callback">The method that you handle the event.</param>
        IDisposable SubscribeOn<T1, T2>(System.Linq.Expressions.Expression<System.Func<TClientInterface, System.Action<T1, T2>>> eventToBind, System.Action<T1, T2> callback);

        /// <summary>
        ///     Subscribes to a hub event.
        ///     <para>When the server hub send an event of the given type, the handler will be invoked.</para>
        /// </summary>
        /// <param name="eventToBind">The event method exposed by the server hub interface.</param>
        /// <param name="wherePredicate">The callback will only be called when this predicate is true.</param>
        /// <param name="callback">The method that you handle the event.</param>
        IDisposable SubscribeOn<T1, T2>(System.Linq.Expressions.Expression<System.Func<TClientInterface, System.Action<T1, T2>>> eventToBind, System.Func<T1, T2, bool> wherePredicate, System.Action<T1, T2> callback);


        /// <summary>
        ///     Subscribes to a hub event.
        ///     <para>When the server hub send an event of the given type, the handler will be invoked.</para>
        /// </summary>
        /// <param name="eventToBind">The event method exposed by the server hub interface.</param>
        /// <param name="callback">The method that you handle the event.</param>
        IDisposable SubscribeOn<T1, T2, T3>(System.Linq.Expressions.Expression<System.Func<TClientInterface, System.Action<T1, T2, T3>>> eventToBind, System.Action<T1, T2, T3> callback);

        /// <summary>
        ///     Subscribes to a hub event.
        ///     <para>When the server hub send an event of the given type, the handler will be invoked.</para>
        /// </summary>
        /// <param name="eventToBind">The event method exposed by the server hub interface.</param>
        /// <param name="wherePredicate">The callback will only be called when this predicate is true.</param>
        /// <param name="callback">The method that you handle the event.</param>
        IDisposable SubscribeOn<T1, T2, T3>(System.Linq.Expressions.Expression<System.Func<TClientInterface, System.Action<T1, T2, T3>>> eventToBind, System.Func<T1, T2, T3, bool> wherePredicate, System.Action<T1, T2, T3> callback);

        /// <summary>
        ///     Subscribes to a hub event.
        ///     <para>When the server hub send an event of the given type, the handler will be invoked.</para>
        /// </summary>
        /// <param name="eventToBind">The event method exposed by the server hub interface.</param>
        /// <param name="callback">The method that you handle the event.</param>
        IDisposable SubscribeOn<T1, T2, T3, T4>(System.Linq.Expressions.Expression<System.Func<TClientInterface, System.Action<T1, T2, T3, T4>>> eventToBind, System.Action<T1, T2, T3, T4> callback);

        /// <summary>
        ///     Subscribes to a hub event.
        ///     <para>When the server hub send an event of the given type, the handler will be invoked.</para>
        /// </summary>
        /// <param name="eventToBind">The event method exposed by the server hub interface.</param>
        /// <param name="wherePredicate">The callback will only be called when this predicate is true.</param>
        /// <param name="callback">The method that you handle the event.</param>
        IDisposable SubscribeOn<T1, T2, T3, T4>(System.Linq.Expressions.Expression<System.Func<TClientInterface, System.Action<T1, T2, T3, T4>>> eventToBind, System.Func<T1, T2, T3, T4, bool> wherePredicate, System.Action<T1, T2, T3, T4> callback);

        /// <summary>
        ///     Subscribes to a hub event.
        ///     <para>When the server hub send an event of the given type, the handler will be invoked.</para>
        /// </summary>
        /// <param name="eventToBind">The event method exposed by the server hub interface.</param>
        /// <param name="callback">The method that you handle the event.</param>
        IDisposable SubscribeOn<T1, T2, T3, T4, T5>(System.Linq.Expressions.Expression<System.Func<TClientInterface, System.Action<T1, T2, T3, T4, T5>>> eventToBind, System.Action<T1, T2, T3, T4, T5> callback);

        /// <summary>
        ///     Subscribes to a hub event.
        ///     <para>When the server hub send an event of the given type, the handler will be invoked.</para>
        /// </summary>
        /// <param name="eventToBind">The event method exposed by the server hub interface.</param>
        /// <param name="wherePredicate">The callback will only be called when this predicate is true.</param>
        /// <param name="callback">The method that you handle the event.</param>
        IDisposable SubscribeOn<T1, T2, T3, T4, T5>(System.Linq.Expressions.Expression<System.Func<TClientInterface, System.Action<T1, T2, T3, T4, T5>>> eventToBind, System.Func<T1, T2, T3, T4, T5, bool> wherePredicate, System.Action<T1, T2, T3, T4, T5> callback);

        /// <summary>
        ///     Subscribes to a hub event.
        ///     <para>When the server hub send an event of the given type, the handler will be invoked.</para>
        /// </summary>
        /// <param name="eventToBind">The event method exposed by the server hub interface.</param>
        /// <param name="callback">The method that you handle the event.</param>
        IDisposable SubscribeOn<T1, T2, T3, T4, T5, T6>(System.Linq.Expressions.Expression<System.Func<TClientInterface, System.Action<T1, T2, T3, T4, T5, T6>>> eventToBind, System.Action<T1, T2, T3, T4, T5, T6> callback);

        /// <summary>
        ///     Subscribes to a hub event.
        ///     <para>When the server hub send an event of the given type, the handler will be invoked.</para>
        /// </summary>
        /// <param name="eventToBind">The event method exposed by the server hub interface.</param>
        /// <param name="wherePredicate">The callback will only be called when this predicate is true.</param>
        /// <param name="callback">The method that you handle the event.</param>
        IDisposable SubscribeOn<T1, T2, T3, T4, T5, T6>(System.Linq.Expressions.Expression<System.Func<TClientInterface, System.Action<T1, T2, T3, T4, T5, T6>>> eventToBind, System.Func<T1, T2, T3, T4, T5, T6, bool> wherePredicate, System.Action<T1, T2, T3, T4, T5, T6> callback);

        /// <summary>
        ///     Subscribes to a hub event.
        ///     <para>When the server hub send an event of the given type, the handler will be invoked.</para>
        /// </summary>
        /// <param name="eventToBind">The event method exposed by the server hub interface.</param>
        /// <param name="callback">The method that you handle the event.</param>
        IDisposable SubscribeOn<T1, T2, T3, T4, T5, T6, T7>(System.Linq.Expressions.Expression<System.Func<TClientInterface, System.Action<T1, T2, T3, T4, T5, T6, T7>>> eventToBind, System.Action<T1, T2, T3, T4, T5, T6, T7> callback);

        /// <summary>
        ///     Subscribes to a hub event.
        ///     <para>When the server hub send an event of the given type, the handler will be invoked.</para>
        /// </summary>
        /// <param name="eventToBind">The event method exposed by the server hub interface.</param>
        /// <param name="wherePredicate">The callback will only be called when this predicate is true.</param>
        /// <param name="callback">The method that you handle the event.</param>
        IDisposable SubscribeOn<T1, T2, T3, T4, T5, T6, T7>(System.Linq.Expressions.Expression<System.Func<TClientInterface, System.Action<T1, T2, T3, T4, T5, T6, T7>>> eventToBind, System.Func<T1, T2, T3, T4, T5, T6, T7, bool> wherePredicate, System.Action<T1, T2, T3, T4, T5, T6, T7> callback);

        /// <summary>
        ///     Subscribes on all events (methods) which the server can trigger (invoke).
        /// </summary>
        /// <param name="instance">The instance which implements the client interface (TClientInterface).</param>
        IEnumerable<IDisposable> SubscribeOnAll(object instance);
    }
}
