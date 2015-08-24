namespace SignalR.Client.TypedHubProxy.Tests.Utils
{
    internal static class TestConsts
    {
        public const string ERR_PROXY_INVOKED_CORRECTLY = "because the TypedHubProxy should invoke the method";
        public const string ERR_PROXY_ROUTE_CORRECTLY = "because the TypedHubProxy should route this call and all parameters correctly";
        public const string ERR_PROXY_RECEIVE_EVENT = "because the TypedHubProxy should receive the event";
        public const string ERR_PROXY_RECEIVE_EVENT_WHERE_TRUE = "because the TypedHubProxy should receive the event where the predicate is true";
        public const string ERR_PROXY_RECEIVE_EVENT_WHERE_FALSE = "because the TypedHubProxy should receive the event, but should not dispatch the message to the caller, because the where-predicate is false";
        public const string ERR_PARAM_MISMATCH = "because {0} should be {1}, but was {2}";
        public const string ERR_SHOULD_FIX_ISSUE_9 = "because should be fixed, see issue #9";
    }
}
