namespace Microsoft.AspNet.SignalR.Client
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Typed HubProxy for SignalR.
    /// </summary>
    /// <typeparam name="TServerHubInterface">The interface of the server hub.</typeparam>
    /// <typeparam name="TClientInterface">The interface of the client events.</typeparam>
    public class HubProxy<TServerHubInterface, TClientInterface> : IHubProxy<TServerHubInterface, TClientInterface>,
        IObservableHubProxy<TServerHubInterface, TClientInterface>
        where TServerHubInterface : class
        where TClientInterface : class
    {
        private const string ERR_NOT_AN_INTERFACE = "\"{0}\" is not an interface.";

        private readonly IHubProxy _hubProxy;
        private readonly System.Reflection.MethodInfo _convertStub =
            typeof(HubProxy<TServerHubInterface, TClientInterface>).GetMethod("Convert",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

        private readonly Dictionary<Hubs.Subscription, Action<IList<Newtonsoft.Json.Linq.JToken>>> _subscriptions =
            new Dictionary<Hubs.Subscription, Action<IList<Newtonsoft.Json.Linq.JToken>>>();

        /// <summary>
        /// Ctor of HubProxy.
        /// </summary>
        /// <param name="hubProxy">IHubProxy.</param>
        /// <exception cref="ArgumentException"></exception>
        public HubProxy(IHubProxy hubProxy)
        {
            if (!typeof(TServerHubInterface).IsInterface)
            {
                throw new ArgumentException(string.Format(ERR_NOT_AN_INTERFACE, typeof(TServerHubInterface).Name));
            }

            if (!typeof(TClientInterface).IsInterface)
            {
                throw new ArgumentException(string.Format(ERR_NOT_AN_INTERFACE, typeof(TClientInterface).Name));
            }


            _hubProxy = hubProxy;
        }

        /// <summary>
        /// Ctor of HubProxy.
        /// </summary>
        /// <param name="hubConnection">HubConnection.</param>
        /// <param name="hubName">Name of the hub.</param>
        public HubProxy(HubConnection hubConnection, string hubName)
            : this(hubConnection.CreateHubProxy(hubName))
        {
        }

        #region ITypedHubOneWayProxy implementations

        void IHubProxyOneWay<TServerHubInterface>.Call(Expression<Action<TServerHubInterface>> call)
        {
            ((IHubProxyOneWay<TServerHubInterface>)this).CallAsync(call).Wait();
        }

        TResult IHubProxyOneWay<TServerHubInterface>.Call<TResult>(
            Expression<Func<TServerHubInterface, TResult>> call)
        {
            return ((IHubProxyOneWay<TServerHubInterface>)this).CallAsync(call).Result;
        }

        Task IHubProxyOneWay<TServerHubInterface>.CallAsync(
            Expression<Action<TServerHubInterface>> call)
        {
            ActionDetail invocation = call.GetActionDetails();
            return _hubProxy.Invoke(invocation.MethodName, invocation.Parameters);
        }

        Task IHubProxyOneWay<TServerHubInterface>.CallAsync(
            Expression<Func<TServerHubInterface, Task>> call)
        {
            ActionDetail invocation = call.GetActionDetails();
            return _hubProxy.Invoke(invocation.MethodName, invocation.Parameters);
        }

        Task<TResult> IHubProxyOneWay<TServerHubInterface>.CallAsync<TResult>(
            Expression<Func<TServerHubInterface, TResult>> call)
        {
            ActionDetail invocation = call.GetActionDetails();
            return _hubProxy.Invoke<TResult>(invocation.MethodName, invocation.Parameters);
        }

        Task<TResult> IHubProxyOneWay<TServerHubInterface>.CallAsync<TResult>(
            Expression<Func<TServerHubInterface, Task<TResult>>> call)
        {
            ActionDetail invocation = call.GetActionDetails();
            return _hubProxy.Invoke<TResult>(invocation.MethodName, invocation.Parameters);
        }

        #endregion

        #region IHubProxy implementations

        void IHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn(
            Expression<Func<TClientInterface, Action>> eventToBind, Action callback)
        {
            CreateSubscription(eventToBind.GetMethodName(), callback);
        }

        void IHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T>(
            Expression<Func<TClientInterface, Action<T>>> eventToBind, Action<T> callback)
        {
            CreateSubscription(eventToBind.GetMethodName(), callback);
        }

        void IHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T>(
            Expression<Func<TClientInterface, Action<T>>> eventToBind, Func<T, bool> wherePredicate, Action<T> callback)
        {
            CreateSubscription(eventToBind.GetMethodName(), callback, wherePredicate);
        }

        void IHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T1, T2>(
            Expression<Func<TClientInterface, Action<T1, T2>>> eventToBind,
            Action<T1, T2> callback)
        {
            CreateSubscription(eventToBind.GetMethodName(), callback);
        }

        void IHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T1, T2>(
            Expression<Func<TClientInterface, Action<T1, T2>>> eventToBind, Func<T1, T2, bool> wherePredicate,
            Action<T1, T2> callback)
        {
            CreateSubscription(eventToBind.GetMethodName(), callback, wherePredicate);
        }

        void IHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T1, T2, T3>(
            Expression<Func<TClientInterface, Action<T1, T2, T3>>> eventToBind,
            Action<T1, T2, T3> callback)
        {
            CreateSubscription(eventToBind.GetMethodName(), callback);
        }

        void IHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T1, T2, T3>(
            Expression<Func<TClientInterface, Action<T1, T2, T3>>> eventToBind, Func<T1, T2, T3, bool> wherePredicate,
            Action<T1, T2, T3> callback)
        {
            CreateSubscription(eventToBind.GetMethodName(), callback, wherePredicate);
        }

        void IHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T1, T2, T3, T4>(
            Expression<Func<TClientInterface, Action<T1, T2, T3, T4>>> eventToBind,
            Action<T1, T2, T3, T4> callback)
        {
            CreateSubscription(eventToBind.GetMethodName(), callback);
        }

        void IHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T1, T2, T3, T4>(
            Expression<Func<TClientInterface, Action<T1, T2, T3, T4>>> eventToBind,
            Func<T1, T2, T3, T4, bool> wherePredicate, Action<T1, T2, T3, T4> callback)
        {
            CreateSubscription(eventToBind.GetMethodName(), callback, wherePredicate);
        }

        void IHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T1, T2, T3, T4, T5>(
            Expression<Func<TClientInterface, Action<T1, T2, T3, T4, T5>>> eventToBind,
            Action<T1, T2, T3, T4, T5> callback)
        {
            CreateSubscription(eventToBind.GetMethodName(), callback);
        }

        void IHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T1, T2, T3, T4, T5>(
            Expression<Func<TClientInterface, Action<T1, T2, T3, T4, T5>>> eventToBind,
            Func<T1, T2, T3, T4, T5, bool> wherePredicate,
            Action<T1, T2, T3, T4, T5> callback)
        {
            CreateSubscription(eventToBind.GetMethodName(), callback, wherePredicate);
        }

        void IHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T1, T2, T3, T4, T5, T6>(
            Expression<Func<TClientInterface, Action<T1, T2, T3, T4, T5, T6>>> eventToBind,
            Action<T1, T2, T3, T4, T5, T6> callback)
        {
            CreateSubscription(eventToBind.GetMethodName(), callback);
        }

        void IHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T1, T2, T3, T4, T5, T6>(
            Expression<Func<TClientInterface, Action<T1, T2, T3, T4, T5, T6>>> eventToBind,
            Func<T1, T2, T3, T4, T5, T6, bool> wherePredicate,
            Action<T1, T2, T3, T4, T5, T6> callback)
        {
            CreateSubscription(eventToBind.GetMethodName(), callback, wherePredicate);
        }

        void IHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T1, T2, T3, T4, T5, T6, T7>(
            Expression<Func<TClientInterface, Action<T1, T2, T3, T4, T5, T6, T7>>> eventToBind,
            Action<T1, T2, T3, T4, T5, T6, T7> callback)
        {
            CreateSubscription(eventToBind.GetMethodName(), callback);
        }

        void IHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T1, T2, T3, T4, T5, T6, T7>(
            Expression<Func<TClientInterface, Action<T1, T2, T3, T4, T5, T6, T7>>> eventToBind,
            Func<T1, T2, T3, T4, T5, T6, T7, bool> wherePredicate,
            Action<T1, T2, T3, T4, T5, T6, T7> callback)
        {
            CreateSubscription(eventToBind.GetMethodName(), callback, wherePredicate);
        }

        void IHubProxy<TServerHubInterface, TClientInterface>.SubscribeOnAll(object instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            if (!(instance is TClientInterface))
            {
                throw new System.Data.ConstraintException(string.Format("{0} doesn't implements the interface {1}.",
                    instance.GetType().Name, typeof(TClientInterface).Name));
            }

            System.Reflection.MethodInfo[] methodInfos = typeof(TClientInterface).GetMethods();

            foreach (System.Reflection.MethodInfo methodInfo in methodInfos)
            {
                System.Reflection.ParameterInfo[] parameterInfos = methodInfo.GetParameters();

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
                        ? typeof(Action<,>).MakeGenericType(parameterInfos.Select(p => p.ParameterType).ToArray())
                        : typeof(Action<>).MakeGenericType(parameterInfos.Select(p => p.ParameterType).ToArray());
                }
                else
                {
                    actionType = typeof(Action);
                }

                Delegate actionDelegate = Delegate.CreateDelegate(actionType, instance, methodInfo);


                CreateSubscription(methodInfo.Name, actionDelegate);
            }
        }

        #endregion

        #region IObservableHubProxy implementations

        IObservable<T> IObservableHubProxy<TServerHubInterface, TClientInterface>.Observe<T>(
            Expression<Func<TClientInterface, Action<T>>> eventToBind)
        {
            return new ObservableHubMessage<T>(_hubProxy, eventToBind.GetMethodName());
        }

        #endregion

        #region IDisposable implementation

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

        #endregion

        /// <summary>
        ///     Deserialization of incoming data object.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="serializer"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected static T Convert<T>(Newtonsoft.Json.Linq.JToken obj, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (obj == null)
            {
                return default(T);
            }

            return obj.ToObject<T>(serializer);
        }

        private void CreateSubscription(string eventName, Delegate callback, Delegate wherePredicate = null)
        {
            Hubs.Subscription subscription = _hubProxy.Subscribe(eventName);

            Type[] genericArguments = callback.GetType().GetGenericArguments();

            Action<IList<Newtonsoft.Json.Linq.JToken>> handler = args =>
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
                                convertMethod.Invoke(null, new object[] { args[i], _hubProxy.JsonSerializer }))
                        .ToArray();

                    if (wherePredicate != null)
                    {
                        if ((bool)wherePredicate.DynamicInvoke(genericArgs))
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
