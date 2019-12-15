using System;
using System.Threading.Tasks;
using MoniLogs.Core.Commands.Infrastructure.Cache;
using MoniLogs.Core.Commands.Infrastructure.Parameters;
using MoniLogs.Core.Queries.Infrastructure;
using StackExchange.Redis;

namespace Moni.VelocidadeTempoLocalizacao
{
    public class CacheGateway : MoniLogs.Core.Infrastructure.ICacheGateway
    {
        private readonly ISetCachedValue _setCacheCommand;
        private readonly IGetCachedValue _getCachedValueQuery;
        
        public CacheGateway(ISetCachedValue setCacheCommand, IGetCachedValue getCachedValueQuery)
        {
            _setCacheCommand = setCacheCommand;
            _getCachedValueQuery = getCachedValueQuery;
        }
        
        public async Task<string> GetValue(string key)
        {
            return await _getCachedValueQuery.Query(key);
        }

        public async Task SetValue(string key, string value)
        {
            await _setCacheCommand.Execute(new SetCacheParameter(key, value));
        }
    }
}