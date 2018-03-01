using JetBrains.Annotations;

namespace SignalR.Client.TypedHubProxy.Tests.Contracts
{
    [PublicAPI]
    public interface IWrongClientContract
    {
        object PassingNoParams();
    }
}
