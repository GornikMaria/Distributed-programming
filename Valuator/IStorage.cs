namespace Valudator
{
    public interface IStorage
    {
        void Store(string key, string value);
        string Load(string key);
        public string Get(string value);
        public List<string> GetKeys();
    }
}