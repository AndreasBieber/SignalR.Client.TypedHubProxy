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
            return new TypedHubProxy<TServerHubInterface, TClientInterface>(hubProxy);
        }

        /// <summary>
        ///     Creates a strongly typed observable hubproxy.
        /// </summary>
        /// <param name="hubProxy">IHubProxy.</param>
        /// <typeparam name="TServerHubInterface">The interface of the server hub.</typeparam>
        /// <typeparam name="TClientInterface">The interface of the client events.</typeparam>
        public static IObservableHubProxy<TServerHubInterface, TClientInterface> CreateObservableProxy
            <TServerHubInterface, TClientInterface>(this IHubProxy hubProxy)
            where TServerHubInterface : class
            where TClientInterface : class
        {
            return new ObservableHubProxy<TServerHubInterface, TClientInterface>(hubProxy);
        }
    }
}