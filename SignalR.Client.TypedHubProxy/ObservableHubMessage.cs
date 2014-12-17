using System;

namespace Microsoft.AspNet.SignalR.Client
{
    internal class ObservableHubMessage<T> : IObservable<T>
    {
        private readonly IHubProxy _proxy;
        private readonly string _eventName;

        public ObservableHubMessage(IHubProxy hubProxy, string eventName)
        {
            _proxy = hubProxy;
            _eventName = eventName;
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return _proxy.On<T>(_eventName, observer.OnNext);
        }
    }
}