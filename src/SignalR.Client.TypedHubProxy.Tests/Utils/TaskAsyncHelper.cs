namespace SignalR.Client.TypedHubProxy.Tests.Utils
{
    internal static class TaskAsyncHelper
    {
        private static readonly System.Threading.Tasks.Task _emptyTask = MakeTask<object>(null);

        public static System.Threading.Tasks.Task Empty
        {
            get
            {
                return _emptyTask;
            }
        }

        public static System.Threading.Tasks.Task<T> FromResult<T>(T value)
        {
            var tcs = new System.Threading.Tasks.TaskCompletionSource<T>();
            tcs.SetResult(value);
            return tcs.Task;
        }

        private static System.Threading.Tasks.Task<T> MakeTask<T>(T value)
        {
            return FromResult(value);
        }

    }
}
