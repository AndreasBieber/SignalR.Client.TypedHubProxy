using System.Threading.Tasks;

namespace Sample.Client.Shared
{
    public interface IChatHub
    {
        Task SendMessage(string msg);
    }
}
