using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;

namespace Valuator
{
    public class RedisStorage : IStorage
    {
        private readonly ILogger<RedisStorage> _logger;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly RedisKey _textIdentifiersKey = "textIdentifiers";
        private readonly string _host = "localhost";
 
        public RedisStorage(ILogger<RedisStorage> logger)
        {
            _logger = logger;
            _connectionMultiplexer = ConnectionMultiplexer.Connect(_host);
        }

        public void Store(string key, string value)
        {
            IDatabase db = _connectionMultiplexer.GetDatabase();
            db.StringSet(key, value);
        }

        public string Load(string key)
        {
            IDatabase db = _connectionMultiplexer.GetDatabase();
            return db.StringGet(key);
        }

        public void SimilarityStore(string similarityKey, string text)
        {
            IDatabase db = _connectionMultiplexer.GetDatabase();
            var keys = _connectionMultiplexer.GetServer("localhost:6379").Keys();
            int similarity = 0;
            foreach (var key in keys)
            {
                RedisValue value = db.StringGet(key);
                if (text == value.ToString())
                {
                    similarity = 1;
                    break;
                }
        }
        db.StringSet(similarityKey, similarity.ToString());
        } 
    }
}