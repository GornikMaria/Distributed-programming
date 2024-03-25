﻿﻿using System.Globalization;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace Valuator.Pages;

public class SummaryModel : PageModel
{
    private readonly ILogger<SummaryModel> _logger;
    private readonly IStorage _storage;

    public SummaryModel(ILogger<SummaryModel> logger, IStorage storage)
    {
        _logger = logger;
        _storage = storage;
    }

    public double Rank { get; set; }
    public double Similarity { get; set; }

    public void OnGet(string id)
    {           
        _logger.LogDebug(id);

        //TODO: проинициализировать свойства Rank и Similarity значениями из БД
        string rankKey = "RANK-" + id;
        Rank = double.Parse(_storage.Load(rankKey), CultureInfo.InvariantCulture);

        string similarityKey = "SIMILARITY-" + id;
        Similarity = Convert.ToDouble(_storage.Load(similarityKey));
    }
}
