using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Valuator.Pages;
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

        string rankKey = "RANK-" + id;
        //TODO: посчитать rank и сохранить в БД по ключу rankKey
        string rank = GetRank(text).ToString();
        _storage.Store(rankKey, rank);

        string similarityKey = "SIMILARITY-" + id;
        //TODO: посчитать similarity и сохранить в БД по ключу similarityKey
        _storage.SimilarityStore(similarityKey, text);

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
}