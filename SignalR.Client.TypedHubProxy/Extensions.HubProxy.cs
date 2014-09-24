using System;
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
    }
}
