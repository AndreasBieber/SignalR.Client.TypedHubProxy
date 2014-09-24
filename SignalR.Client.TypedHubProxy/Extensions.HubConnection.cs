using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using Microsoft.AspNet.SignalR.Client.Hubs;

namespace Microsoft.AspNet.SignalR.Client
{
    public static partial class TypedHubProxyExtensions
    {
        internal static IHubProxy GetHubProxy(this HubConnection hubConnection, string hubName)
        {
            FieldInfo hubsField = hubConnection.GetType()
                .GetField("_hubs", BindingFlags.NonPublic | BindingFlags.Instance);

            if (hubsField == null)
            {
                throw new ConstraintException("Couldn't find \"_hubs\" field inside of the HubConnection.");
            }

            var hubs = (Dictionary<string, HubProxy>) hubsField.GetValue(hubConnection);

            if (hubs.ContainsKey(hubName))
            {
                return hubs[hubName];
            }

            return null;
        }

        /// <summary>
        ///     Creates a strongly typed proxy for the hub with the specified name.
        /// </summary>
        /// <param name="connection">The <see cref="T:Microsoft.AspNet.SignalR.Client.HubConnection" />HubConnection.</param>
        /// <param name="hubName">The name of the hub.</param>
        /// <typeparam name="TServerHubInterface"></typeparam>
        /// <typeparam name="TClientInterface"></typeparam>
        public static ITypedHubProxy<TServerHubInterface, TClientInterface> CreateHubProxy
            <TServerHubInterface, TClientInterface>(this HubConnection connection,
                string hubName)
            where TServerHubInterface : class
            where TClientInterface : class
        {
            Type typedHubProxy = typeof (TypedHubProxy<,>).MakeGenericType(typeof (TServerHubInterface),
                typeof (TClientInterface));
            return
                (ITypedHubProxy<TServerHubInterface, TClientInterface>)
                    Activator.CreateInstance(typedHubProxy, BindingFlags.NonPublic | BindingFlags.Instance, null,
                        new object[] {connection, hubName}, null);
        }
    }
}