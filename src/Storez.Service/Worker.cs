using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Storez.Service
{
    public class Worker : BackgroundService
    {
        private readonly IBusControl _bus;

        public Worker(IBusControl bus)
        {
            _bus = bus;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {

            await  _bus.StartAsync(cancellationToken).ConfigureAwait(false);
            Log.Information("BUS started !");
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await _bus.StopAsync(cancellationToken);
            Log.Information("***  BUS STOPPED ***");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Log.Information("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
