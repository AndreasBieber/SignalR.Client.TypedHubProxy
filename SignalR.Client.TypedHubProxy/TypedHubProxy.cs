using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Microsoft.AspNet.SignalR.Client
{
    /// <summary>
    ///     Proxy for SignalR.
    /// </summary>
    /// <typeparam name="TServerHubInterface">The interface of the server hub.</typeparam>
    /// <typeparam name="TClientInterface">The interface of the client events.</typeparam>
    public sealed class TypedHubProxy<TServerHubInterface, TClientInterface> :
        ITypedHubProxy<TServerHubInterface, TClientInterface>
    {
        private const string ERR_NOT_AN_INTERFACE = "\"{0}\" is not an interface.";

        private readonly IHubProxy _hubProxy;

        internal TypedHubProxy(IHubProxy hubProxy)
        {
            _hubProxy = hubProxy;
        }

        internal TypedHubProxy(HubConnection hubConnection, string hubName)
        {
            if (!typeof (TServerHubInterface).IsInterface)
            {
                throw new ArgumentException(string.Format(ERR_NOT_AN_INTERFACE, typeof (TServerHubInterface).Name));
            }

            if (!typeof (TClientInterface).IsInterface)
            {
                throw new ArgumentException(string.Format(ERR_NOT_AN_INTERFACE, typeof (TClientInterface).Name));
            }

            _hubProxy = hubConnection.GetHubProxy(hubName) ?? hubConnection.CreateHubProxy(hubName);
        }

        #region ITypedHubProxy implementations

        void ITypedHubProxy<TServerHubInterface, TClientInterface>.Call(Expression<Action<TServerHubInterface>> call)
        {
            ((ITypedHubProxy<TServerHubInterface, TClientInterface>) this).CallAsync(call).Wait();
        }

        TResult ITypedHubProxy<TServerHubInterface, TClientInterface>.Call<TResult>(
            Expression<Func<TServerHubInterface, TResult>> call)
        {
            return ((ITypedHubProxy<TServerHubInterface, TClientInterface>) this).CallAsync(call).Result;
        }

        Task ITypedHubProxy<TServerHubInterface, TClientInterface>.CallAsync(
            Expression<Action<TServerHubInterface>> call)
        {
            ActionDetail invocation = call.GetActionDetails();
            return _hubProxy.Invoke(invocation.MethodName, invocation.Parameters);
        }

        Task<TResult> ITypedHubProxy<TServerHubInterface, TClientInterface>.CallAsync<TResult>(
            Expression<Func<TServerHubInterface, TResult>> call)
        {
            ActionDetail invocation = call.GetActionDetails();
            return _hubProxy.Invoke<TResult>(invocation.MethodName, invocation.Parameters);
        }

        void ITypedHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn(
            Expression<Func<TClientInterface, Action>> eventToBind, Action callback)
        {
            _hubProxy.On(eventToBind.GetMethodName(), callback);
        }

        void ITypedHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T>(
            Expression<Func<TClientInterface, Action<T>>> eventToBind, Action<T> callback)
        {
            _hubProxy.On(eventToBind.GetMethodName(), callback);
        }

        void ITypedHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T1, T2>(
            Expression<Func<TClientInterface, Action<T1, T2>>> eventToBind,
            Action<T1, T2> callback)
        {
            _hubProxy.On(eventToBind.GetMethodName(), callback);
        }

        void ITypedHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T1, T2, T3>(
            Expression<Func<TClientInterface, Action<T1, T2, T3>>> eventToBind,
            Action<T1, T2, T3> callback)
        {
            _hubProxy.On(eventToBind.GetMethodName(), callback);
        }

        void ITypedHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T1, T2, T3, T4>(
            Expression<Func<TClientInterface, Action<T1, T2, T3, T4>>> eventToBind,
            Action<T1, T2, T3, T4> callback)
        {
            _hubProxy.On(eventToBind.GetMethodName(), callback);
        }

        void ITypedHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T1, T2, T3, T4, T5>(
            Expression<Func<TClientInterface, Action<T1, T2, T3, T4, T5>>> eventToBind,
            Action<T1, T2, T3, T4, T5> callback)
        {
            _hubProxy.On(eventToBind.GetMethodName(), callback);
        }

        void ITypedHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T1, T2, T3, T4, T5, T6>(
            Expression<Func<TClientInterface, Action<T1, T2, T3, T4, T5, T6>>> eventToBind,
            Action<T1, T2, T3, T4, T5, T6> callback)
        {
            _hubProxy.On(eventToBind.GetMethodName(), callback);
        }

        void ITypedHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T1, T2, T3, T4, T5, T6, T7>(
            Expression<Func<TClientInterface, Action<T1, T2, T3, T4, T5, T6, T7>>> eventToBind,
            Action<T1, T2, T3, T4, T5, T6, T7> callback)
        {
            _hubProxy.On(eventToBind.GetMethodName(), callback);
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

                MethodInfo onMethod;
                Type actionType;

                if (parameterInfos.Any())
                {
                    onMethod =
                        typeof (HubProxyExtensions).GetMethods(BindingFlags.Static | BindingFlags.Public)
                            .First(
                                m => m.Name.Equals("On") && m.GetGenericArguments().Length == parameterInfos.Length);

                    onMethod = onMethod.MakeGenericMethod(parameterInfos.Select(pi => pi.ParameterType).ToArray());
                    actionType = parameterInfos.Length > 1
                        ? typeof (Action<,>).MakeGenericType(parameterInfos.Select(p => p.ParameterType).ToArray())
                        : typeof (Action<>).MakeGenericType(parameterInfos.Select(p => p.ParameterType).ToArray());
                }
                else
                {
                    onMethod =
                        typeof (HubProxyExtensions).GetMethods(BindingFlags.Static | BindingFlags.Public)
                            .First(
                                m => m.Name.Equals("On") && m.GetGenericArguments().Length == 0);

                    actionType = typeof (Action);
                }

                Delegate actionDelegate = Delegate.CreateDelegate(actionType, instance, methodInfo);


                onMethod.Invoke(null, new object[] {_hubProxy, methodInfo.Name, actionDelegate});
            }
        }

        #endregion

        void IDisposable.Dispose()
        {
        }
    }
}