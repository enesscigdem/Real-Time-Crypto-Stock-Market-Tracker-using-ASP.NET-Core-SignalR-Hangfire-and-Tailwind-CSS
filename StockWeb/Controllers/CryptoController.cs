using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using StockWeb.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CryptoController : Controller
{
    private readonly CryptoService _cryptoService;
    private readonly IHubContext<CryptoHub> _hubContext;

    public CryptoController(CryptoService cryptoService, IHubContext<CryptoHub> hubContext)
    {
        _cryptoService = cryptoService;
        _hubContext = hubContext;
    }

    public async Task<IActionResult> Index()
    {
        var cryptoPrices = await _cryptoService.GetCryptoPricesAsync();
        return View(cryptoPrices);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateCryptoPrices()
    {
        var cryptos = await _cryptoService.GetCryptoPricesAsync();
        foreach (var crypto in cryptos)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveCryptoUpdate", crypto.Symbol, crypto.Price, crypto.Change);
        }
        return Ok();
    }
}