using System;
using System.Linq;
using System.Reflection;

namespace Microsoft.AspNet.SignalR.Client
{
    public static partial class TypedHubProxyExtensions
    {
        /// <summary>
        ///     Creates a strongly typed hubproxy.
        /// </summary>
        /// <param name="hubProxy">IHubProxy.</param>
        /// <typeparam name="TServerHubInterface">The interface of the server hub.</typeparam>
        /// <typeparam name="TClientInterface">The interface of the client events.</typeparam>
        public static ITypedHubProxy<TServerHubInterface, TClientInterface> CreateTypedProxy
            <TServerHubInterface, TClientInterface>(this IHubProxy hubProxy)
            where TServerHubInterface : class
            where TClientInterface : class
        {
            Type typedHubProxy = typeof(TypedHubProxy<,>).MakeGenericType(typeof(TServerHubInterface),
                typeof(TClientInterface));
            return
                (ITypedHubProxy<TServerHubInterface, TClientInterface>)
                    Activator.CreateInstance(typedHubProxy, BindingFlags.NonPublic | BindingFlags.Instance, null,
                        new object[] { hubProxy }, null);
        }

        /// <summary>
        ///     Subscribes on all events (methods) which the server can call.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hubProxy"></param>
        /// <param name="instance"></param>
        /// <exception cref="NotSupportedException"></exception>
        [Obsolete(
            "This method will be removed in the next version of SignalR.Client.TypedHubProxy. Please use hubConnection.CreateHubProxy<TServerHubInterface, TClientInterface> instead.",
            false)]
        public static void SubscribeOn<T>(this object hubProxy, object instance) where T : class
        {
            if (!(hubProxy is IHubProxy) && hubProxy.GetType().BaseType != typeof(InterfaceHubProxyBase))
            {
                throw new NotSupportedException("This method can only be called for HubProxies.");
            }

            var theRealHubProxy = hubProxy as IHubProxy;
            if (theRealHubProxy == null)
            {
                FieldInfo fieldInfo = hubProxy.GetType()
                    .GetField("_hubProxy", BindingFlags.NonPublic | BindingFlags.Instance);

                if (fieldInfo == null)
                {
                    // should never happen
                    throw new Exception("Something went wrong -.-");
                }
                theRealHubProxy = (IHubProxy)fieldInfo.GetValue(hubProxy);
            }

            Type interfaceType = typeof(T);

            if (!interfaceType.IsInterface)
            {
                throw new NotSupportedException("T is not an interface.");
            }

            MethodInfo[] methodInfos = interfaceType.GetMethods();

            foreach (MethodInfo methodInfo in methodInfos)
            {
                ParameterInfo[] parameterInfos = methodInfo.GetParameters();

                if (parameterInfos.Count() > 7)
                {
                    throw new NotSupportedException(
                        String.Format(
                            "Only interface methods with less or equal 7 parameters are supported: {0}.{1}({2})!",
                        // ReSharper disable once PossibleNullReferenceException
                            methodInfo.DeclaringType.FullName.Replace("+", "."),
                            methodInfo.Name,
                            String.Join(", ",
                                methodInfo.GetParameters()
                                    .Select(p => String.Format("{0} {1}", p.ParameterType.Name, p.Name)))));
                }

                MethodInfo onMethod;
                Type actionType;

                if (parameterInfos.Any())
                {
                    onMethod =
                        typeof(HubProxyExtensions).GetMethods(BindingFlags.Static | BindingFlags.Public)
                            .First(
                                m => m.Name.Equals("On") && m.GetGenericArguments().Length == parameterInfos.Length);

                    onMethod = onMethod.MakeGenericMethod(parameterInfos.Select(pi => pi.ParameterType).ToArray());
                    actionType = parameterInfos.Length > 1
                        ? typeof(Action<,>).MakeGenericType(parameterInfos.Select(p => p.ParameterType).ToArray())
                        : typeof(Action<>).MakeGenericType(parameterInfos.Select(p => p.ParameterType).ToArray());
                }
                else
                {
                    onMethod =
                        typeof(HubProxyExtensions).GetMethods(BindingFlags.Static | BindingFlags.Public)
                            .First(
                                m => m.Name.Equals("On") && m.GetGenericArguments().Length == 0);

                    actionType = typeof(Action);
                }

                Delegate actionDelegate = Delegate.CreateDelegate(actionType, instance, methodInfo);


                onMethod.Invoke(null, new object[] { theRealHubProxy, methodInfo.Name, actionDelegate });
            }
        }
    }
}
