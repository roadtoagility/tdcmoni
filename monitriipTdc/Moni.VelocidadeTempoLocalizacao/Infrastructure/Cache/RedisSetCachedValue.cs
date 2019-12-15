using System;
using System.Threading.Tasks;
using MoniLogs.Core.Commands.Infrastructure.Cache;
using MoniLogs.Core.Commands.Infrastructure.Parameters;
using StackExchange.Redis;

namespace Moni.VelocidadeTempoLocalizacao.Infrastructure.Cache
{
    public class RedisSetCachedValue : ISetCachedValue
    {
        private IDatabase _db = null;
        
        public RedisSetCachedValue(IDatabase db)
        {
            _db = db;
        }
        
        public async Task Execute(SetCacheParameter parameter)
        {
            await _db.StringSetAsync(parameter.Key, parameter.Value);
        }
    }
}