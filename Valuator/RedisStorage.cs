using StackExchange.Redis;

namespace Valudator
{
    public class RedisStorage : IStorage
    {
        private readonly IDatabase db;
        private readonly IConnectionMultiplexer _connection;

        public RedisStorage() {
            _connection = ConnectionMultiplexer.Connect("localhost");
            db = _connection.GetDatabase();
        }

        public string Load(string key)
        {
            return db.StringGet(key);
        }

        public void Store(string key, string value)
        {
            db.StringSet(key, value);
        }

        public List<string> GetKeys()
        {
            var keys = _connection.GetServer("localhost:6379").Keys();
            return keys.Select(key => key.ToString()).ToList();
        }
    }
}