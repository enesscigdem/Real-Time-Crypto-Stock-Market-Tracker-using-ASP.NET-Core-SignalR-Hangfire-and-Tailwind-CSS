namespace StockWeb.Models
{
    public class Stock
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public double PreviousPrice { get; set; }
        public double OpenPrice { get; set; }
        public bool IsCrypto { get; set; }
        public double Change => Price - PreviousPrice;

        public string GetPriceChangeColor()
        {
            return Price > PreviousPrice ? "price-up" : Price < PreviousPrice ? "price-down" : "price-neutral";
        }
    }
}