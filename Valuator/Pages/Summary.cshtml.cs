﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Valuator.Pages;
public class SummaryModel : PageModel
{
    private readonly ILogger<SummaryModel> _logger;
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public SummaryModel(ILogger<SummaryModel> logger, IConnectionMultiplexer connectionMultiplexer)
    {
        _logger = logger;
        _connectionMultiplexer = connectionMultiplexer;
    }

    public double Rank { get; set; }
    public double Similarity { get; set; }

    public void OnGet(string id)
    {
        IDatabase db = _connectionMultiplexer.GetDatabase();
        _logger.LogDebug(id);

        //проинициализировать свойства Rank и Similarity значениями из БД
        Rank = Math.Round(double.Parse(db.StringGet("RANK-" + id)), 2);
        Similarity = double.Parse(db.StringGet("SIMILARITY-" + id));
    }
}