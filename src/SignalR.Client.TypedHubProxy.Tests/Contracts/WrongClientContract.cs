namespace SignalR.Client.TypedHubProxy.Tests.Contracts
{
    class WrongClientContract : IWrongClientContract
    {
        public object PassingNoParams()
        {
            return new { };
        }
    }
}
