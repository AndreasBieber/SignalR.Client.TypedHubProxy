using System.Threading.Tasks;

namespace SignalR.Client.TypedHubProxy.Tests.Utils
{
    internal static class TaskAsyncHelper
    {
        public static Task Empty { get; } = MakeTask<object>(null);

        public static Task<T> FromResult<T>(T value)
        {
            var tcs = new TaskCompletionSource<T>();
            tcs.SetResult(value);
            return tcs.Task;
        }

        private static Task<T> MakeTask<T>(T value)
        {
            return FromResult(value);
        }
    }
}