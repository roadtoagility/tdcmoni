using System.Threading.Tasks;

namespace MoniLogs.Core.Commands
{
    public interface IAsyncCommand<T>
    {
        Task Execute(T parameter);
    }
}