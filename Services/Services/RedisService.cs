using EventZone.Services.Interface;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace EventZone.Services.Services
{
    public class RedisService : IRedisService
    {
        private readonly IDatabase _db;

        // Constructor for production
        public RedisService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("RedisCacheDB");
            var redis = ConnectionMultiplexer.Connect(connectionString);
            _db = redis.GetDatabase();
        }

        // Constructor for testing
        public RedisService(IDatabase database)
        {
            _db = database;
        }

        public async Task SetStringAsync(string key, string value, TimeSpan expiry)
        {
            await _db.StringSetAsync(key, value, expiry);
        }

        public async Task<string> GetStringAsync(string key)
        {
            return await _db.StringGetAsync(key);
        }

        public async Task<bool> DeleteKeyAsync(string key)
        {
            return await _db.KeyDeleteAsync(key);
        }
    }
}
