using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StockWeb.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public abstract class UpdateServiceBase<T> : BackgroundService where T : Hub
{
    protected readonly IHubContext<T> _hubContext;
    protected readonly ILogger _logger;
    protected readonly List<Stock> _stocks;

    protected UpdateServiceBase(IHubContext<T> hubContext, ILogger logger)
    {
        _hubContext = hubContext;
        _logger = logger;
        _stocks = new List<Stock>();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await UpdatePrices();
            await Task.Delay(1000, stoppingToken); // Adjust the frequency as needed
        }
    }

    protected abstract Task UpdatePrices();
}