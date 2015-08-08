namespace SignalR.Client.TypedHubProxy.Tests.Utils
{
    internal class TestConsts
    {
        // ReSharper disable InconsistentNaming
        public const string ERR_PROXY_INVOKED_CORRECTLY = "because the TypedHubProxy should invoke the method";
        public const string ERR_PROXY_ROUTE_CORRECTLY = "because the TypedHubProxy should route this call and all parameters correctly";
        public const string ERR_PROXY_RECEIVE_EVENT = "because the TypedHubProxy should receive the event";
        public const string ERR_PROXY_RECEIVE_EVENT_WHERE_TRUE = "because the TypedHubProxy should receive the event where the predicate is true";
        public const string ERR_PARAM_MISMATCH = "because {0} should be {1}, but was {2}";
        public const string ERR_SHOULD_FIX_ISSUE_9 = "because should be fixed, see issue #9";
        // ReSharper restore InconsistentNaming
    }
}
