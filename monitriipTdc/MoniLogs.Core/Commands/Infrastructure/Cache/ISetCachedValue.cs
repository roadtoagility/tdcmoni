using MoniLogs.Core.Commands;
using MoniLogs.Core.Commands.Infrastructure.Parameters;

namespace MoniLogs.Core.Commands.Infrastructure.Cache
{
    public interface ISetCachedValue : IAsyncCommand<SetCacheParameter>
    {
        
    }
}