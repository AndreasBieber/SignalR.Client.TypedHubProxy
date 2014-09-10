using System.Threading.Tasks;

namespace Sample.Shared
{
    public interface IChatHub
    {
        Task SendMessage(string msg);
    }
}