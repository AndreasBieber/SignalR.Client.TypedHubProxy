namespace Sample.Shared
{
    public interface IChatHub
    {
        void SendMessage(string msg);
        int GetConnectedClients();
    }
}