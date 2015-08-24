using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Microsoft.AspNet.SignalR.Client
{
    /// <summary>
    ///     Typed Hub Proxy.
    /// </summary>
    /// <typeparam name="TServerHubInterface">Interface of the server hub.</typeparam>
    /// <typeparam name="TClientInterface">Interface which contains the client events.</typeparam>
    [PublicAPI]
    public interface IHubProxy<TServerHubInterface, TClientInterface> : IHubProxyOneWay<TServerHubInterface>,
        IDisposable
        where TServerHubInterface : class
        where TClientInterface : class
    {
        /// <summary>
        ///     Subscribes to a hub event.
        ///     <para>When the server hub send an event of the given type, the handler will be invoked.</para>
        /// </summary>
        /// <param name="eventToBind">The event method exposed by the server hub interface.</param>
        /// <param name="action">The method that you handle the event.</param>
        IDisposable SubscribeOn(Expression<Func<TClientInterface, Action>> eventToBind, Action action);

        /// <summary>
        ///     Subscribes to a hub event.
        ///     <para>When the server hub send an event of the given type, the handler will be invoked.</para>
        /// </summary>
        /// <param name="eventToBind">The event method exposed by the server hub interface.</param>
        /// <param name="callback">The method that handle the event.</param>
        IDisposable SubscribeOn<T>(Expression<Func<TClientInterface, Action<T>>> eventToBind, Action<T> callback);

        /// <summary>
        ///     Subscribes to a hub event.
        ///     <para>When the server hub send an event of the given type, the handler will be invoked.</para>
        /// </summary>
        /// <param name="eventToBind">The event method exposed by the server hub interface.</param>
        /// <param name="wherePredicate">The callback will only be called when this predicate is true.</param>
        /// <param name="callback">The method that you handle the event.</param>
        IDisposable SubscribeOn<T>(Expression<Func<TClientInterface, Action<T>>> eventToBind,
            Func<T, bool> wherePredicate, Action<T> callback);

        /// <summary>
        ///     Subscribes to a hub event.
        ///     <para>When the server hub send an event of the given type, the handler will be invoked.</para>
        /// </summary>
        /// <param name="eventToBind">The event method exposed by the server hub interface.</param>
        /// <param name="callback">The method that you handle the event.</param>
        IDisposable SubscribeOn<T1, T2>(Expression<Func<TClientInterface, Action<T1, T2>>> eventToBind,
            Action<T1, T2> callback);

        /// <summary>
        ///     Subscribes to a hub event.
        ///     <para>When the server hub send an event of the given type, the handler will be invoked.</para>
        /// </summary>
        /// <param name="eventToBind">The event method exposed by the server hub interface.</param>
        /// <param name="wherePredicate">The callback will only be called when this predicate is true.</param>
        /// <param name="callback">The method that you handle the event.</param>
        IDisposable SubscribeOn<T1, T2>(Expression<Func<TClientInterface, Action<T1, T2>>> eventToBind,
            Func<T1, T2, bool> wherePredicate, Action<T1, T2> callback);

        /// <summary>
        ///     Subscribes to a hub event.
        ///     <para>When the server hub send an event of the given type, the handler will be invoked.</para>
        /// </summary>
        /// <param name="eventToBind">The event method exposed by the server hub interface.</param>
        /// <param name="callback">The method that you handle the event.</param>
        IDisposable SubscribeOn<T1, T2, T3>(Expression<Func<TClientInterface, Action<T1, T2, T3>>> eventToBind,
            Action<T1, T2, T3> callback);

        /// <summary>
        ///     Subscribes to a hub event.
        ///     <para>When the server hub send an event of the given type, the handler will be invoked.</para>
        /// </summary>
        /// <param name="eventToBind">The event method exposed by the server hub interface.</param>
        /// <param name="wherePredicate">The callback will only be called when this predicate is true.</param>
        /// <param name="callback">The method that you handle the event.</param>
        IDisposable SubscribeOn<T1, T2, T3>(Expression<Func<TClientInterface, Action<T1, T2, T3>>> eventToBind,
            Func<T1, T2, T3, bool> wherePredicate, Action<T1, T2, T3> callback);

        /// <summary>
        ///     Subscribes to a hub event.
        ///     <para>When the server hub send an event of the given type, the handler will be invoked.</para>
        /// </summary>
        /// <param name="eventToBind">The event method exposed by the server hub interface.</param>
        /// <param name="callback">The method that you handle the event.</param>
        IDisposable SubscribeOn<T1, T2, T3, T4>(Expression<Func<TClientInterface, Action<T1, T2, T3, T4>>> eventToBind,
            Action<T1, T2, T3, T4> callback);

        /// <summary>
        ///     Subscribes to a hub event.
        ///     <para>When the server hub send an event of the given type, the handler will be invoked.</para>
        /// </summary>
        /// <param name="eventToBind">The event method exposed by the server hub interface.</param>
        /// <param name="wherePredicate">The callback will only be called when this predicate is true.</param>
        /// <param name="callback">The method that you handle the event.</param>
        IDisposable SubscribeOn<T1, T2, T3, T4>(Expression<Func<TClientInterface, Action<T1, T2, T3, T4>>> eventToBind,
            Func<T1, T2, T3, T4, bool> wherePredicate, Action<T1, T2, T3, T4> callback);

        /// <summary>
        ///     Subscribes to a hub event.
        ///     <para>When the server hub send an event of the given type, the handler will be invoked.</para>
        /// </summary>
        /// <param name="eventToBind">The event method exposed by the server hub interface.</param>
        /// <param name="callback">The method that you handle the event.</param>
        IDisposable SubscribeOn<T1, T2, T3, T4, T5>(
            Expression<Func<TClientInterface, Action<T1, T2, T3, T4, T5>>> eventToBind,
            Action<T1, T2, T3, T4, T5> callback);

        /// <summary>
        ///     Subscribes to a hub event.
        ///     <para>When the server hub send an event of the given type, the handler will be invoked.</para>
        /// </summary>
        /// <param name="eventToBind">The event method exposed by the server hub interface.</param>
        /// <param name="wherePredicate">The callback will only be called when this predicate is true.</param>
        /// <param name="callback">The method that you handle the event.</param>
        IDisposable SubscribeOn<T1, T2, T3, T4, T5>(
            Expression<Func<TClientInterface, Action<T1, T2, T3, T4, T5>>> eventToBind,
            Func<T1, T2, T3, T4, T5, bool> wherePredicate, Action<T1, T2, T3, T4, T5> callback);

        /// <summary>
        ///     Subscribes to a hub event.
        ///     <para>When the server hub send an event of the given type, the handler will be invoked.</para>
        /// </summary>
        /// <param name="eventToBind">The event method exposed by the server hub interface.</param>
        /// <param name="callback">The method that you handle the event.</param>
        IDisposable SubscribeOn<T1, T2, T3, T4, T5, T6>(
            Expression<Func<TClientInterface, Action<T1, T2, T3, T4, T5, T6>>> eventToBind,
            Action<T1, T2, T3, T4, T5, T6> callback);

        /// <summary>
        ///     Subscribes to a hub event.
        ///     <para>When the server hub send an event of the given type, the handler will be invoked.</para>
        /// </summary>
        /// <param name="eventToBind">The event method exposed by the server hub interface.</param>
        /// <param name="wherePredicate">The callback will only be called when this predicate is true.</param>
        /// <param name="callback">The method that you handle the event.</param>
        IDisposable SubscribeOn<T1, T2, T3, T4, T5, T6>(
            Expression<Func<TClientInterface, Action<T1, T2, T3, T4, T5, T6>>> eventToBind,
            Func<T1, T2, T3, T4, T5, T6, bool> wherePredicate, Action<T1, T2, T3, T4, T5, T6> callback);

        /// <summary>
        ///     Subscribes to a hub event.
        ///     <para>When the server hub send an event of the given type, the handler will be invoked.</para>
        /// </summary>
        /// <param name="eventToBind">The event method exposed by the server hub interface.</param>
        /// <param name="callback">The method that you handle the event.</param>
        IDisposable SubscribeOn<T1, T2, T3, T4, T5, T6, T7>(
            Expression<Func<TClientInterface, Action<T1, T2, T3, T4, T5, T6, T7>>> eventToBind,
            Action<T1, T2, T3, T4, T5, T6, T7> callback);

        /// <summary>
        ///     Subscribes to a hub event.
        ///     <para>When the server hub send an event of the given type, the handler will be invoked.</para>
        /// </summary>
        /// <param name="eventToBind">The event method exposed by the server hub interface.</param>
        /// <param name="wherePredicate">The callback will only be called when this predicate is true.</param>
        /// <param name="callback">The method that you handle the event.</param>
        IDisposable SubscribeOn<T1, T2, T3, T4, T5, T6, T7>(
            Expression<Func<TClientInterface, Action<T1, T2, T3, T4, T5, T6, T7>>> eventToBind,
            Func<T1, T2, T3, T4, T5, T6, T7, bool> wherePredicate, Action<T1, T2, T3, T4, T5, T6, T7> callback);

        /// <summary>
        ///     Subscribes on all events (methods) which the server can trigger (invoke).
        /// </summary>
        /// <param name="instance">The instance which implements the client interface (TClientInterface).</param>
        IEnumerable<IDisposable> SubscribeOnAll(object instance);
    }
}