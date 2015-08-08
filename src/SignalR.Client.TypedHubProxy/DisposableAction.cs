using System;
using System.Threading;

namespace Microsoft.AspNet.SignalR.Client
{
    internal class DisposableAction : IDisposable
    {
        public static readonly DisposableAction Empty = new DisposableAction(() => { });

        private Action<object> _action;
        private readonly object _state;

        public DisposableAction(Action action)
            : this(state => ((Action)state).Invoke(), state: action)
        {

        }

        public DisposableAction(Action<object> action, object state)
        {
            _action = action;
            _state = state;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Interlocked.Exchange(ref _action, state => { }).Invoke(_state);
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
