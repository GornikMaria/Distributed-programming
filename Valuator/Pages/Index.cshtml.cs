using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Valudator;

namespace Valuator.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IStorage _storage;

        public IndexModel(ILogger<IndexModel> logger, IStorage storage)
        {
            _logger = logger;
            _storage = storage;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost(string text)
        {
            _logger.LogDebug(text);

            string id = Guid.NewGuid().ToString();

            //TODO: посчитать rank и сохранить в БД по ключу rankKey
            string rankKey = "RANK-" + id;
            string rank = GetRank(text).ToString("0.##");
            _storage.Store(rankKey, rank);

            //TODO: посчитать similarity и сохранить в БД по ключу similarityKey
            string similarityKey = "SIMILARITY-" + id;
            string similarity = GetSimilarity(text).ToString();
            _storage.Store(similarityKey, similarity);

            //TODO: сохранить в БД text по ключу textKey
            string textKey = "TEXT-" + id;
            _storage.Store(textKey, text);

            return Redirect($"summary?id={id}");
        }

        private double GetRank(string text)
        {
            if (text == null) {
                return 0;
            }
            int notLetterCharsCount = text.Where(ch => !char.IsLetter(ch)).Count();
            return notLetterCharsCount / (double) text.Length;
        }

        private double GetSimilarity(string text)
        {
            var keys = _storage.GetKeys();
            string startText = "TEXT-";
            for (int i = 0; i < keys.Count; i++)
            {
                var value = keys[i];
                if (value.StartsWith(startText))
                {
                    return 1;
                }
            }
            return 0;
        }
    }
}
