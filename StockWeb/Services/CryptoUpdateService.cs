using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StockWeb.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class CryptoUpdateService : BackgroundService
{
    private readonly IHubContext<CryptoHub> _hubContext;
    private readonly CryptoService _cryptoService;
    private readonly ILogger<CryptoUpdateService> _logger;

    public CryptoUpdateService(IHubContext<CryptoHub> hubContext, CryptoService cryptoService, ILogger<CryptoUpdateService> logger)
    {
        _hubContext = hubContext;
        _cryptoService = cryptoService;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var cryptoPrices = await _cryptoService.GetCryptoPricesAsync();
                foreach (var crypto in cryptoPrices)
                {
                    await _hubContext.Clients.All.SendAsync("ReceiveCryptoUpdate", crypto.Symbol, crypto.Price, crypto.Change);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating crypto prices.");
            }
            await Task.Delay(60000, stoppingToken); // 1 dakika arayla güncelleme
        }
    }
}