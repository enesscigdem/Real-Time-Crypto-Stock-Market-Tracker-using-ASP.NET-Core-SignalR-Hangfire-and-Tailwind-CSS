using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using StockWeb.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class StockUpdateService : UpdateServiceBase<StockHub>
{
    private readonly StockService _stockService;

    private static readonly List<string> stockSymbols = new List<string>
    {
        "AAPL", "MSFT", "AMZN", "GOOGL", "META", "TSLA", "NVDA", "NFLX", "ADBE", "INTC"
    };

    public StockUpdateService(IHubContext<StockHub> hubContext, ILogger<StockUpdateService> logger, StockService stockService)
        : base(hubContext, logger)
    {
        _stockService = stockService;
    }

    protected override async Task UpdatePrices()
    {
        foreach (var symbol in stockSymbols)
        {
            var stock = _stocks.Find(s => s.Name == symbol);
            if (stock == null)
            {
                stock = new Stock { Name = symbol };
                _stocks.Add(stock);
            }

            stock.PreviousPrice = stock.Price;

            try
            {
                var (openPrice, currentPrice) = await _stockService.GetStockPriceAndChangeAsync(symbol);
                stock.OpenPrice = openPrice;
                stock.Price = currentPrice;
                await _hubContext.Clients.All.SendAsync("ReceiveStockUpdate", stock.Name, stock.Price, stock.GetPriceChangeColor());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating stock price for {stock.Name}");
            }
        }
    }
}