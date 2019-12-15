using System.Threading.Tasks;

namespace MoniLogs.Core.Queries
{
    public interface IAsyncQuery<T, U>
    {

        Task<T> Query(U parameter);
    }
}