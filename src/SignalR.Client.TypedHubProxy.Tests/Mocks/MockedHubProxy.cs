using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SignalR.Client.TypedHubProxy.Tests.Contracts;

namespace SignalR.Client.TypedHubProxy.Tests.Mocks
{
    [PublicAPI]
    public abstract class MockedHubProxy : IHubProxy
    {
        private readonly HubProxy _hubProxy;

        protected MockedHubProxy(HubProxy hubProxy)
        {
            _hubProxy = hubProxy;
        }

        public virtual Task Invoke(string method, params object[] args)
        {
            return _hubProxy.Invoke(method, args);
        }

        public virtual Task<T> Invoke<T>(string method, params object[] args)
        {
            return _hubProxy.Invoke<T>(method, args);
        }

        public virtual Task Invoke<T>(string method, Action<T> onProgress,
            params object[] args)
        {
            return _hubProxy.Invoke(method, onProgress, args);
        }

        public virtual Task<TResult> Invoke<TResult, TProgress>(string method,
            Action<TProgress> onProgress, params object[] args)
        {
            return _hubProxy.Invoke<TResult, TProgress>(method, onProgress, args);
        }

        public virtual Subscription Subscribe(string eventName)
        {
            return _hubProxy.Subscribe(eventName);
        }

        public JToken this[string name]
        {
            get { return _hubProxy[name]; }
            set { _hubProxy[name] = value; }
        }

        public virtual JsonSerializer JsonSerializer => _hubProxy.JsonSerializer;

        public virtual void InvokeEvent(
            Expression<Action<IClientContract>> call)
        {
            var invocation = call.GetActionDetails();

            _hubProxy.InvokeEvent(invocation.MethodName,
                invocation.Parameters.Select(JToken.FromObject).ToList());
        }
    }
}