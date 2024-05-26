namespace StockWeb.Models
{
    public class Crypto
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Change { get; set; }
        public string ChangePercent { get; set; }
        public string MarketCap { get; set; }
        public string Volume24h { get; set; }
        public string CirculatingSupply { get; set; }
        public List<double> HistoricalPrices { get; set; } // Historical price data
    }
}