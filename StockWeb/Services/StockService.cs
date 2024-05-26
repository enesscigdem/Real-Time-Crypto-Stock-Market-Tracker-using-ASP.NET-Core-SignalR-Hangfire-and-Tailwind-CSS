using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using StockWeb.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

public class StockService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<StockService> _logger;

    public StockService(HttpClient httpClient, ILogger<StockService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<(double openPrice, double currentPrice)> GetStockPriceAndChangeAsync(string stockSymbol)
    {
        try
        {
            var apiKey = "cp93c01r01qo7b19egg0cp93c01r01qo7b19eggg";
            var response = await _httpClient.GetStringAsync($"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={apiKey}");
            var json = JObject.Parse(response);

            var openPrice = double.Parse(json["o"].ToString());
            var currentPrice = double.Parse(json["c"].ToString());

            return (openPrice, currentPrice);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching stock data for {stockSymbol}");
            return (0, 0);
        }
    }

    public async Task FetchStockData(string stockSymbol)
    {
        var (openPrice, currentPrice) = await GetStockPriceAndChangeAsync(stockSymbol);
        _logger.LogInformation("Fetched data for {stockSymbol}: Open Price = {openPrice}, Current Price = {currentPrice}", stockSymbol, openPrice, currentPrice);
    }
}