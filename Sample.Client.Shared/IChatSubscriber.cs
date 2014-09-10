namespace Sample.Client.Shared
{
    public interface IChatSubscriber
    {
        void NewMessage(string msg);
    }
}
