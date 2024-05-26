using Microsoft.AspNetCore.SignalR;

public class CryptoHub : Hub
{
    public async Task SendCryptoUpdate(string crypto, double price, string color)
    {
        await Clients.All.SendAsync("ReceiveCryptoUpdate", crypto, price, color);
    }
}