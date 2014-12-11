using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNet.SignalR.Client.Hubs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.AspNet.SignalR.Client
{
    /// <summary>
    ///     Proxy for SignalR.
    /// </summary>
    /// <typeparam name="TServerHubInterface">The interface of the server hub.</typeparam>
    /// <typeparam name="TClientInterface">The interface of the client events.</typeparam>
    public class TypedHubProxy<TServerHubInterface, TClientInterface> :
        TypedHubOneWayProxy<TServerHubInterface>, ITypedHubProxy<TServerHubInterface, TClientInterface>
        where TServerHubInterface : class
        where TClientInterface : class
    {
        private readonly MethodInfo _convertStub =
            typeof (TypedHubProxy<TServerHubInterface, TClientInterface>).GetMethod("Convert",
                BindingFlags.NonPublic | BindingFlags.Static);

        private readonly Dictionary<Subscription, Action<IList<JToken>>> _subscriptions =
            new Dictionary<Subscription, Action<IList<JToken>>>();

        internal TypedHubProxy(IHubProxy hubProxy) : base(hubProxy)
        {
        }

        internal TypedHubProxy(HubConnection hubConnection, string hubName) : base(hubConnection, hubName)
        {
            if (!typeof (TClientInterface).IsInterface)
            {
                throw new ArgumentException(string.Format(ERR_NOT_AN_INTERFACE, typeof (TClientInterface).Name));
            }
        }

        #region ITypedHubProxy implementations

        void ITypedHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn(
            Expression<Func<TClientInterface, Action>> eventToBind, Action callback)
        {
            CreateSubscription(eventToBind.GetMethodName(), callback);
        }

        void ITypedHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T>(
            Expression<Func<TClientInterface, Action<T>>> eventToBind, Action<T> callback)
        {
            CreateSubscription(eventToBind.GetMethodName(), callback);
        }

        void ITypedHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T>(
            Expression<Func<TClientInterface, Action<T>>> eventToBind, Func<T, bool> wherePredicate, Action<T> callback)
        {
            CreateSubscription(eventToBind.GetMethodName(), callback, wherePredicate);
        }

        void ITypedHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T1, T2>(
            Expression<Func<TClientInterface, Action<T1, T2>>> eventToBind,
            Action<T1, T2> callback)
        {
            CreateSubscription(eventToBind.GetMethodName(), callback);
        }

        void ITypedHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T1, T2>(
            Expression<Func<TClientInterface, Action<T1, T2>>> eventToBind, Func<T1, T2, bool> wherePredicate,
            Action<T1, T2> callback)
        {
            CreateSubscription(eventToBind.GetMethodName(), callback, wherePredicate);
        }

        void ITypedHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T1, T2, T3>(
            Expression<Func<TClientInterface, Action<T1, T2, T3>>> eventToBind,
            Action<T1, T2, T3> callback)
        {
            CreateSubscription(eventToBind.GetMethodName(), callback);
        }

        void ITypedHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T1, T2, T3>(
            Expression<Func<TClientInterface, Action<T1, T2, T3>>> eventToBind, Func<T1, T2, T3, bool> wherePredicate,
            Action<T1, T2, T3> callback)
        {
            CreateSubscription(eventToBind.GetMethodName(), callback, wherePredicate);
        }

        void ITypedHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T1, T2, T3, T4>(
            Expression<Func<TClientInterface, Action<T1, T2, T3, T4>>> eventToBind,
            Action<T1, T2, T3, T4> callback)
        {
            CreateSubscription(eventToBind.GetMethodName(), callback);
        }

        void ITypedHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T1, T2, T3, T4>(
            Expression<Func<TClientInterface, Action<T1, T2, T3, T4>>> eventToBind,
            Func<T1, T2, T3, T4, bool> wherePredicate, Action<T1, T2, T3, T4> callback)
        {
            CreateSubscription(eventToBind.GetMethodName(), callback, wherePredicate);
        }

        void ITypedHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T1, T2, T3, T4, T5>(
            Expression<Func<TClientInterface, Action<T1, T2, T3, T4, T5>>> eventToBind,
            Action<T1, T2, T3, T4, T5> callback)
        {
            CreateSubscription(eventToBind.GetMethodName(), callback);
        }

        void ITypedHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T1, T2, T3, T4, T5>(
            Expression<Func<TClientInterface, Action<T1, T2, T3, T4, T5>>> eventToBind,
            Func<T1, T2, T3, T4, T5, bool> wherePredicate,
            Action<T1, T2, T3, T4, T5> callback)
        {
            CreateSubscription(eventToBind.GetMethodName(), callback, wherePredicate);
        }

        void ITypedHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T1, T2, T3, T4, T5, T6>(
            Expression<Func<TClientInterface, Action<T1, T2, T3, T4, T5, T6>>> eventToBind,
            Action<T1, T2, T3, T4, T5, T6> callback)
        {
            CreateSubscription(eventToBind.GetMethodName(), callback);
        }

        void ITypedHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T1, T2, T3, T4, T5, T6>(
            Expression<Func<TClientInterface, Action<T1, T2, T3, T4, T5, T6>>> eventToBind,
            Func<T1, T2, T3, T4, T5, T6, bool> wherePredicate,
            Action<T1, T2, T3, T4, T5, T6> callback)
        {
            CreateSubscription(eventToBind.GetMethodName(), callback, wherePredicate);
        }

        void ITypedHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T1, T2, T3, T4, T5, T6, T7>(
            Expression<Func<TClientInterface, Action<T1, T2, T3, T4, T5, T6, T7>>> eventToBind,
            Action<T1, T2, T3, T4, T5, T6, T7> callback)
        {
            CreateSubscription(eventToBind.GetMethodName(), callback);
        }

        void ITypedHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T1, T2, T3, T4, T5, T6, T7>(
            Expression<Func<TClientInterface, Action<T1, T2, T3, T4, T5, T6, T7>>> eventToBind,
            Func<T1, T2, T3, T4, T5, T6, T7, bool> wherePredicate,
            Action<T1, T2, T3, T4, T5, T6, T7> callback)
        {
            CreateSubscription(eventToBind.GetMethodName(), callback, wherePredicate);
        }

        void ITypedHubProxy<TServerHubInterface, TClientInterface>.SubscribeOnAll(object instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            if (!(instance is TClientInterface))
            {
                throw new ConstraintException(string.Format("{0} doesn't implements the interface {1}.",
                    instance.GetType().Name, typeof (TClientInterface).Name));
            }

            MethodInfo[] methodInfos = typeof (TClientInterface).GetMethods();

            foreach (MethodInfo methodInfo in methodInfos)
            {
                ParameterInfo[] parameterInfos = methodInfo.GetParameters();

                if (parameterInfos.Count() > 7)
                {
                    throw new NotSupportedException(
                        string.Format(
                            "Only interface methods with less or equal 7 parameters are supported: {0}.{1}({2})!",
                            // ReSharper disable once PossibleNullReferenceException
                            methodInfo.DeclaringType.FullName.Replace("+", "."),
                            methodInfo.Name,
                            string.Join(", ",
                                methodInfo.GetParameters()
                                    .Select(p => string.Format("{0} {1}", p.ParameterType.Name, p.Name)))));
                }

                Type actionType;

                if (parameterInfos.Any())
                {
                    actionType = parameterInfos.Length > 1
                        ? typeof (Action<,>).MakeGenericType(parameterInfos.Select(p => p.ParameterType).ToArray())
                        : typeof (Action<>).MakeGenericType(parameterInfos.Select(p => p.ParameterType).ToArray());
                }
                else
                {
                    actionType = typeof (Action);
                }

                Delegate actionDelegate = Delegate.CreateDelegate(actionType, instance, methodInfo);


                CreateSubscription(methodInfo.Name, actionDelegate);
            }
        }

        #endregion

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            foreach (var kvp in _subscriptions)
            {
                kvp.Key.Received -= kvp.Value;
            }

            _subscriptions.Clear();
        }

        /// <summary>
        ///     Deserialization of incoming data object.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="serializer"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected static T Convert<T>(JToken obj, JsonSerializer serializer)
        {
            if (obj == null)
            {
                return default(T);
            }

            return obj.ToObject<T>(serializer);
        }

        private void CreateSubscription(string eventName, Delegate callback, Delegate wherePredicate = null)
        {
            Subscription subscription = _hubProxy.Subscribe(eventName);

            Type[] genericArguments = callback.GetType().GetGenericArguments();

            Action<IList<JToken>> handler = args =>
            {
                if (genericArguments.Length == 0)
                {
                    callback.DynamicInvoke();
                }
                else
                {
                    object[] genericArgs = genericArguments
                        .Select(t => _convertStub.MakeGenericMethod(t))
                        .Select(
                            (convertMethod, i) =>
                                convertMethod.Invoke(null, new object[] {args[i], _hubProxy.JsonSerializer}))
                        .ToArray();

                    if (wherePredicate != null)
                    {
                        if ((bool) wherePredicate.DynamicInvoke(genericArgs))
                        {
                            callback.DynamicInvoke(genericArgs);
                        }
                    }
                    else
                    {
                        callback.DynamicInvoke(genericArgs);
                    }
                }
            };

            subscription.Received += handler;

            if (!_subscriptions.ContainsKey(subscription))
            {
                _subscriptions.Add(subscription, handler);
            }
        }
    }
}