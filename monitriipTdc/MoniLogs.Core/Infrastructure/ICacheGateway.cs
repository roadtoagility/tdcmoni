using System.Threading.Tasks;

namespace MoniLogs.Core.Infrastructure
{
    public interface ICacheGateway
    {
        Task<string> GetValue(string key);
        Task SetValue(string key, string value);
    }
}