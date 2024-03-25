using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NATS.Client;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
        
        //TODO: сохранить в БД text по ключу textKey
        string textKey = "TEXT-" + id;
        _storage.Store(textKey, text);

        CancellationTokenSource cts = new CancellationTokenSource();
        ConnectionFactory cf = new ConnectionFactory();

        using (IConnection c = cf.CreateConnection())
        {
            byte[] data = Encoding.UTF8.GetBytes(id);
            c.Publish("valuator.processing.rank", data);
            c.Drain();
            c.Close();
        }
        cts.Cancel();

        string similarityKey = "SIMILARITY-" + id;
        //TODO: посчитать similarity и сохранить в БД по ключу similarityKey
        _storage.SimilarityStore(similarityKey, text);

        return Redirect($"summary?id={id}");
    }
}