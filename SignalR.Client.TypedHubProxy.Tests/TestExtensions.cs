namespace SignalR.Client.TypedHubProxy.Tests
{
    using Microsoft.AspNet.SignalR.Client;

    internal static class TestExtensions
    {
        public static void SetupInvoke<T>(this Moq.Mock<Microsoft.AspNet.SignalR.Client.Hubs.HubProxy> mock,
            System.Linq.Expressions.Expression<System.Action<T>> expression, System.Action callback) where T : class
        {
            ActionDetail actionDetails = expression.GetActionDetails();

            mock.Setup(m => m.Invoke(actionDetails.MethodName, actionDetails.Parameters))
                .Callback(callback);
        }
    }
}
