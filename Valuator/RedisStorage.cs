using StackExchange.Redis;

namespace Valudator
{
    public class RedisStorage : IStorage
    {
        private readonly IDatabase db;
        private string _host = "localhost";
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public RedisStorage() {
            _connectionMultiplexer = ConnectionMultiplexer.Connect(_host);
            db = _connectionMultiplexer.GetDatabase();
        }

        public string Load(string key)
        {
            return db.StringGet(key);
        }

        public void Store(string key, string value)
        {
            db.StringSet(key, value);
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