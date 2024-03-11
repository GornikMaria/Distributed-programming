namespace Valuator
{
    public interface IStorage
    {
        void Store(string key, string value);
        string Load(string key);  
        void SimilarityStore(string similarityKey, string text);
    }
}