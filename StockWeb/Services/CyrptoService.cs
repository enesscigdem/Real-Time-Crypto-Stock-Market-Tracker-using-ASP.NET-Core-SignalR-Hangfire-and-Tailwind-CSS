using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using StockWeb.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

public class CryptoService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CryptoService> _logger;

    public CryptoService(HttpClient httpClient, ILogger<CryptoService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<List<Crypto>> GetCryptoPricesAsync()
    {
        var url = "https://finance.yahoo.com/crypto/?offset=0&count=100";
        var response = await _httpClient.GetStringAsync(url);

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(response);

        var cryptos = new List<Crypto>();
        var rows = htmlDoc.DocumentNode.SelectNodes("//table//tbody//tr");

        foreach (var row in rows)
        {
            var symbol = row.SelectSingleNode("td[1]").InnerText.Trim();
            var name = row.SelectSingleNode("td[2]").InnerText.Trim();
            var price = double.Parse(row.SelectSingleNode("td[3]").InnerText.Trim(), System.Globalization.CultureInfo.InvariantCulture);
            var change = row.SelectSingleNode("td[4]").InnerText.Trim();
            var changePercent = row.SelectSingleNode("td[5]").InnerText.Trim();
            var marketCap = row.SelectSingleNode("td[6]").InnerText.Trim();
            var volume24h = row.SelectSingleNode("td[7]").InnerText.Trim();
            var circulatingSupply = row.SelectSingleNode("td[8]").InnerText.Trim();

            // Simulate historical prices for example
            var historicalPrices = new List<double> { 10, 20, 30, 40, 50, 60, 70 };

            cryptos.Add(new Crypto
            {
                Symbol = symbol,
                Name = name,
                Price = price,
                Change = change,
                ChangePercent = changePercent,
                MarketCap = marketCap,
                Volume24h = volume24h,
                CirculatingSupply = circulatingSupply,
                HistoricalPrices = historicalPrices // Add historical prices
            });
        }

        return cryptos;
    }

    public async Task FetchCryptoData(string cryptoSymbol)
    {
        var cryptos = await GetCryptoPricesAsync();
        // Additional logic for specific cryptocurrency data fetching can be added here
    }
}
