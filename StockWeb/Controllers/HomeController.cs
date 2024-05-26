using Hangfire;
using Microsoft.AspNetCore.Mvc;
using StockWeb.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public class HomeController : Controller
{
    private readonly StockService _stockService;

    private static readonly List<string> stockSymbols = new List<string>
    {
        "AAPL", "MSFT", "AMZN", "GOOGL", "META", "TSLA", "NVDA", "NFLX", "ADBE", "INTC"
    };

    public HomeController(StockService stockService)
    {
        _stockService = stockService;
    }

    public async Task<IActionResult> Index()
    {
        var stocks = new List<Stock>();

        foreach (var symbol in stockSymbols)
        {
            try
            {
                var (openPrice, currentPrice) = await _stockService.GetStockPriceAndChangeAsync(symbol);
                stocks.Add(new Stock { Name = symbol, OpenPrice = openPrice, Price = currentPrice, PreviousPrice = openPrice });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting stock price for {symbol}: {ex.Message}");
                stocks.Add(new Stock { Name = symbol, Price = 0, OpenPrice = 0 });
            }
        }

        foreach (var symbol in stockSymbols)
        {
            RecurringJob.AddOrUpdate(
                $"fetch-stocks-{symbol}",
                () => _stockService.FetchStockData(symbol),
                Cron.Minutely);
        }

        return View(stocks);
    }
}