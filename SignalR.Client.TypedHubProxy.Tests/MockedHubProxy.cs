
using System.Linq;

namespace SignalR.Client.TypedHubProxy.Tests
{
    using Microsoft.AspNet.SignalR.Client;
    public class MockedHubProxy : IHubProxy
    {
        private readonly Microsoft.AspNet.SignalR.Client.Hubs.HubProxy _hubProxy;

        public MockedHubProxy(Microsoft.AspNet.SignalR.Client.Hubs.HubProxy hubProxy)
        {
            _hubProxy = hubProxy;
        }

        public virtual System.Threading.Tasks.Task Invoke(string method, params object[] args)
        {
            return _hubProxy.Invoke(method, args);
        }

        public virtual System.Threading.Tasks.Task<T> Invoke<T>(string method, params object[] args)
        {
            return _hubProxy.Invoke<T>(method, args);
        }

        public virtual System.Threading.Tasks.Task Invoke<T>(string method, System.Action<T> onProgress, params object[] args)
        {
            return _hubProxy.Invoke(method, onProgress, args);
        }

        public virtual System.Threading.Tasks.Task<TResult> Invoke<TResult, TProgress>(string method, System.Action<TProgress> onProgress, params object[] args)
        {
            return _hubProxy.Invoke<TResult, TProgress>(method, onProgress, args);
        }

        public virtual Microsoft.AspNet.SignalR.Client.Hubs.Subscription Subscribe(string eventName)
        {
            return _hubProxy.Subscribe(eventName);
        }

        public virtual void InvokeEvent(System.Linq.Expressions.Expression<System.Action<IClientContract>> call)
        {
            ActionDetail invocation = call.GetActionDetails();

            _hubProxy.InvokeEvent(invocation.MethodName, invocation.Parameters.Select(Newtonsoft.Json.Linq.JToken.FromObject).ToList());
        }

        public Newtonsoft.Json.Linq.JToken this[string name]
        {
            get { return _hubProxy[name]; }
            set { _hubProxy[name] = value; }
        }

        public virtual Newtonsoft.Json.JsonSerializer JsonSerializer
        {
            get { return _hubProxy.JsonSerializer; }
        }
    }
}
