using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Moni.VelocidadeTempoLocalizacao
{
    public class CacheGateway : MoniLogs.Core.Infrastructure.ICacheGateway
    {
        private IDatabase _db = null;
        
        public CacheGateway()
        {
            
        }
        
        public async Task<string> GetValue(string key)
        {
            if (_db == null)
                Connect();
            return await _db.StringGetAsync(key);
        }

        public async Task SetValue(string key, string value)
        {
            if (_db == null)
                Connect();
            await _db.StringSetAsync(key, value);
        }

        private void Connect()
        {
            var redisIp = Environment.GetEnvironmentVariable("CLIENT_REDIS");
            var configString = $"{redisIp}:6379,connectRetry=5";
            var redis = ConnectionMultiplexer.Connect(configString);
            _db = redis.GetDatabase();
        }
    }
}