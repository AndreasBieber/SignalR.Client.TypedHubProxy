using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Microsoft.AspNet.SignalR.Client
{
    /// <summary>
    ///     Typed HubProxy for SignalR.
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

        /// <summary>
        ///     Ctor of HubProxy.
        /// </summary>
        /// <param name="hubProxy">IHubProxy.</param>
        /// <exception cref="ArgumentException"></exception>
        public HubProxy(IHubProxy hubProxy)
        {
            if (!typeof (TServerHubInterface).IsInterface)
            {
                throw new ArgumentException(string.Format(ERR_NOT_AN_INTERFACE, typeof (TServerHubInterface).Name));
            }

            if (!typeof (TClientInterface).IsInterface)
            {
                throw new ArgumentException(string.Format(ERR_NOT_AN_INTERFACE, typeof (TClientInterface).Name));
            }


            _hubProxy = hubProxy;
        }

        /// <summary>
        ///     Ctor of HubProxy.
        /// </summary>
        /// <param name="hubConnection">HubConnection.</param>
        /// <param name="hubName">Name of the hub.</param>
        public HubProxy(HubConnection hubConnection, string hubName)
            : this(hubConnection.CreateHubProxy(hubName))
        {
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // nothing to dispose
        }

        #region IObservableHubProxy implementations

        IObservable<T> IObservableHubProxy<TServerHubInterface, TClientInterface>.Observe<T>(
            Expression<Func<TClientInterface, Action<T>>> eventToBind)
        {
            return new ObservableHubMessage<T>(_hubProxy, eventToBind.GetMethodName());
        }

        #endregion

        private IDisposable CreateSubscription(string eventName, Delegate callback, Delegate wherePredicate = null)
        {
            var callbackType = callback.GetType();
            var callbackGenericArguments = callbackType.GetGenericArguments();

            var methodInfos = typeof (HubProxyExtensions)
                .GetMethods(BindingFlags.Static | BindingFlags.Public)
                .Where(m =>
                    m.Name.Equals(nameof(HubProxyExtensions.On)));

            if (callbackGenericArguments.Any())
            {
                methodInfos = methodInfos.Where(
                    m =>
                        m.GetParameters()
                            .Last()
                            .ParameterType
                            .GenericTypeArguments
                            .Count()
                            .Equals(callbackGenericArguments.Count()));
            }

            var relatedOnMethodStub = methodInfos.First();

            var relatedOnMethod = relatedOnMethodStub.IsGenericMethodDefinition
                ? relatedOnMethodStub.MakeGenericMethod(callbackGenericArguments)
                : relatedOnMethodStub;


            var callbackToInvoke = callback;

            if (wherePredicate != null)
            {
                // Create parameterexpressions for all callback-args
                IEnumerable<ParameterExpression> parameterExpressions =
                    callbackGenericArguments.Select(Expression.Parameter).ToList();

                // Create invoke-expression to evaluate the where-predicate
                var wherePredicateInvocation = Expression.Invoke(Expression.Constant(wherePredicate),
                    parameterExpressions);

                // Create invoke-expression for the original callback
                var callbackInvocation = Expression.Invoke(Expression.Constant(callback),
                    parameterExpressions);

                // Create conditional-expression to evaluate the where-predicate. If the result is true, the original callback will be invoked
                var invokeCallbackIfWherePredicateIsTrue = Expression.IfThen(
                    wherePredicateInvocation, callbackInvocation);

                // Compile the expression to create the callbackWrapper-delegate
                callbackToInvoke =
                    Expression.Lambda(invokeCallbackIfWherePredicateIsTrue, parameterExpressions).Compile();
            }

            // Invoke HubProxyExtensions.On
            var invokeResult = relatedOnMethod.Invoke(null, new object[] {_hubProxy, eventName, callbackToInvoke});
            return (IDisposable) invokeResult;
        }

        #region ITypedHubOneWayProxy implementations

        Task IHubProxyOneWay<TServerHubInterface>.CallAsync(
            Expression<Action<TServerHubInterface>> call)
        {
            var invocation = call.GetActionDetails();
            return _hubProxy.Invoke(invocation.MethodName, invocation.Parameters);
        }

        Task IHubProxyOneWay<TServerHubInterface>.CallAsync(
            Expression<Func<TServerHubInterface, Task>> call)
        {
            var invocation = call.GetActionDetails();
            return _hubProxy.Invoke(invocation.MethodName, invocation.Parameters);
        }

        Task<TResult> IHubProxyOneWay<TServerHubInterface>.CallAsync<TResult>(
            Expression<Func<TServerHubInterface, TResult>> call)
        {
            var invocation = call.GetActionDetails();
            return _hubProxy.Invoke<TResult>(invocation.MethodName, invocation.Parameters);
        }

        Task<TResult> IHubProxyOneWay<TServerHubInterface>.CallAsync<TResult>(
            Expression<Func<TServerHubInterface, Task<TResult>>> call)
        {
            var invocation = call.GetActionDetails();
            return _hubProxy.Invoke<TResult>(invocation.MethodName, invocation.Parameters);
        }

        #endregion

        #region IHubProxy implementations

        IDisposable IHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn(
            Expression<Func<TClientInterface, Action>> eventToBind, Action callback)
        {
            return CreateSubscription(eventToBind.GetMethodName(), callback);
        }

        IDisposable IHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T>(
            Expression<Func<TClientInterface, Action<T>>> eventToBind, Action<T> callback)
        {
            return CreateSubscription(eventToBind.GetMethodName(), callback);
        }

        IDisposable IHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T>(
            Expression<Func<TClientInterface, Action<T>>> eventToBind, Func<T, bool> wherePredicate, Action<T> callback)
        {
            return CreateSubscription(eventToBind.GetMethodName(), callback, wherePredicate);
        }

        IDisposable IHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T1, T2>(
            Expression<Func<TClientInterface, Action<T1, T2>>> eventToBind,
            Action<T1, T2> callback)
        {
            return CreateSubscription(eventToBind.GetMethodName(), callback);
        }

        IDisposable IHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T1, T2>(
            Expression<Func<TClientInterface, Action<T1, T2>>> eventToBind, Func<T1, T2, bool> wherePredicate,
            Action<T1, T2> callback)
        {
            return CreateSubscription(eventToBind.GetMethodName(), callback, wherePredicate);
        }

        IDisposable IHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T1, T2, T3>(
            Expression<Func<TClientInterface, Action<T1, T2, T3>>> eventToBind,
            Action<T1, T2, T3> callback)
        {
            return CreateSubscription(eventToBind.GetMethodName(), callback);
        }

        IDisposable IHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T1, T2, T3>(
            Expression<Func<TClientInterface, Action<T1, T2, T3>>> eventToBind, Func<T1, T2, T3, bool> wherePredicate,
            Action<T1, T2, T3> callback)
        {
            return CreateSubscription(eventToBind.GetMethodName(), callback, wherePredicate);
        }

        IDisposable IHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T1, T2, T3, T4>(
            Expression<Func<TClientInterface, Action<T1, T2, T3, T4>>> eventToBind,
            Action<T1, T2, T3, T4> callback)
        {
            return CreateSubscription(eventToBind.GetMethodName(), callback);
        }

        IDisposable IHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T1, T2, T3, T4>(
            Expression<Func<TClientInterface, Action<T1, T2, T3, T4>>> eventToBind,
            Func<T1, T2, T3, T4, bool> wherePredicate, Action<T1, T2, T3, T4> callback)
        {
            return CreateSubscription(eventToBind.GetMethodName(), callback, wherePredicate);
        }

        IDisposable IHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T1, T2, T3, T4, T5>(
            Expression<Func<TClientInterface, Action<T1, T2, T3, T4, T5>>> eventToBind,
            Action<T1, T2, T3, T4, T5> callback)
        {
            return CreateSubscription(eventToBind.GetMethodName(), callback);
        }

        IDisposable IHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T1, T2, T3, T4, T5>(
            Expression<Func<TClientInterface, Action<T1, T2, T3, T4, T5>>> eventToBind,
            Func<T1, T2, T3, T4, T5, bool> wherePredicate,
            Action<T1, T2, T3, T4, T5> callback)
        {
            return CreateSubscription(eventToBind.GetMethodName(), callback, wherePredicate);
        }

        IDisposable IHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T1, T2, T3, T4, T5, T6>(
            Expression<Func<TClientInterface, Action<T1, T2, T3, T4, T5, T6>>> eventToBind,
            Action<T1, T2, T3, T4, T5, T6> callback)
        {
            return CreateSubscription(eventToBind.GetMethodName(), callback);
        }

        IDisposable IHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T1, T2, T3, T4, T5, T6>(
            Expression<Func<TClientInterface, Action<T1, T2, T3, T4, T5, T6>>> eventToBind,
            Func<T1, T2, T3, T4, T5, T6, bool> wherePredicate,
            Action<T1, T2, T3, T4, T5, T6> callback)
        {
            return CreateSubscription(eventToBind.GetMethodName(), callback, wherePredicate);
        }

        IDisposable IHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T1, T2, T3, T4, T5, T6, T7>(
            Expression<Func<TClientInterface, Action<T1, T2, T3, T4, T5, T6, T7>>> eventToBind,
            Action<T1, T2, T3, T4, T5, T6, T7> callback)
        {
            return CreateSubscription(eventToBind.GetMethodName(), callback);
        }

        IDisposable IHubProxy<TServerHubInterface, TClientInterface>.SubscribeOn<T1, T2, T3, T4, T5, T6, T7>(
            Expression<Func<TClientInterface, Action<T1, T2, T3, T4, T5, T6, T7>>> eventToBind,
            Func<T1, T2, T3, T4, T5, T6, T7, bool> wherePredicate,
            Action<T1, T2, T3, T4, T5, T6, T7> callback)
        {
            return CreateSubscription(eventToBind.GetMethodName(), callback, wherePredicate);
        }

        IEnumerable<IDisposable> IHubProxy<TServerHubInterface, TClientInterface>.SubscribeOnAll(object instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            if (!(instance is TClientInterface))
            {
                throw new ConstraintException(
                    $"{instance.GetType().Name} doesn't implements the interface {typeof (TClientInterface).Name}.");
            }

            var methodInfos = typeof (TClientInterface).GetMethods();

            foreach (var methodInfo in methodInfos)
            {
                var parameterInfos = methodInfo.GetParameters();

                if (parameterInfos.Count() > 7)
                {
                    var declaringType = methodInfo.DeclaringType?.FullName.Replace("+", ".");
                    var methodParameters = string.Join(", ",
                        methodInfo.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}"));
                    throw new NotSupportedException(
                        $"Only interface methods with less or equal 7 parameters are supported: {declaringType}.{methodInfo.Name}({methodParameters})!");
                }

                var actionType = Expression.GetActionType(parameterInfos.Select(p => p.ParameterType).ToArray());
                var actionDelegate = Delegate.CreateDelegate(actionType, instance, methodInfo);

                yield return CreateSubscription(methodInfo.Name, actionDelegate);
            }
        }

        #endregion
    }
}