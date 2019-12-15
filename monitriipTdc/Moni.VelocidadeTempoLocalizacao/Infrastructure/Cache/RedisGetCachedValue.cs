using System.Threading.Tasks;
using MoniLogs.Core.Queries.Infrastructure;
using StackExchange.Redis;

namespace Moni.VelocidadeTempoLocalizacao.Infrastructure.Cache
{
    public class RedisGetCachedValue : IGetCachedValue
    {
        private IDatabase _db = null;
        
        public RedisGetCachedValue(IDatabase db)
        {
            _db = db;
        }
        
        public async Task<string> Query(string parameter)
        {
            return await _db.StringGetAsync(parameter);
        }
    }
}