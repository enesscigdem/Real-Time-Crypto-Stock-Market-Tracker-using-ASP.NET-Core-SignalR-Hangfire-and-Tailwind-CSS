using Microsoft.AspNetCore.SignalR;
public class StockHub : Hub
{
    public async Task SendStockUpdate(string stock, double price, string color)
    {
        await Clients.All.SendAsync("ReceiveStockUpdate", stock, price, color);
    }
}