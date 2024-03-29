﻿using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Prometheus;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NetCoreClient.Elk.Tasks
{
    public class DataService : BackgroundService
    {
        private readonly ILogger<DataService> _logger;
        private readonly Settings _settings;

        private readonly MetricServer _metricServer = new MetricServer(port: 9091);
        private readonly Random _random = new Random();

        private static readonly GaugeConfiguration configuration = new GaugeConfiguration { LabelNames = new[] { "service" }};
        private static readonly Gauge Gauge1 = Metrics.CreateGauge("myapp_gauge1", "A simple gauge 1", configuration);
        private static readonly Gauge Gauge2 = Metrics.CreateGauge("myapp_gauge2", "A simple gauge 2", configuration);
        private static readonly Gauge Gauge3 = Metrics.CreateGauge("myapp_gauge3", "A simple gauge 3", configuration);
        private static readonly Gauge Gauge4 = Metrics.CreateGauge("myapp_gauge4", "A simple gauge 4", configuration);
        private static readonly Gauge Gauge5 = Metrics.CreateGauge("myapp_gauge5", "A simple gauge 5", configuration);
        private static readonly Gauge Gauge6 = Metrics.CreateGauge("myapp_gauge6", "A simple gauge 6", configuration);
        private static readonly Gauge Gauge7 = Metrics.CreateGauge("myapp_gauge7", "A simple gauge 7", configuration);
        private static readonly Gauge Gauge8 = Metrics.CreateGauge("myapp_gauge8", "A simple gauge 8", configuration);
        private static readonly Gauge Gauge9 = Metrics.CreateGauge("myapp_gauge9", "A simple gauge 9", configuration);
  
        public DataService(IOptions<Settings> settings, ILogger<DataService> logger)
        {
            _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await base.StartAsync(cancellationToken);
            _metricServer.Start();
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await _metricServer.StopAsync();
            await base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("{Source} background task is starting.", nameof(DataService));

            cancellationToken.Register(() => _logger.LogDebug("#1 {Source} background task is stopping.", nameof(DataService)));

            while (!cancellationToken.IsCancellationRequested)
            {
                _logger.LogDebug("{Source} background task is doing background work.", nameof(DataService));

                SendData();

                await Task.Delay(_settings.CheckConfigurationUpdateDelay, cancellationToken);
            }

            _logger.LogDebug("{Source} background task is stopping.", nameof(DataService));

            await Task.CompletedTask;
        }

        private void SendData()
        {
            _logger.LogDebug("{Source} is sending data.", nameof(DataService));

            Gauge1.WithLabels("service1").Set(_random.Next(1000, 1500));
            Gauge2.WithLabels("service1").Set(_random.Next(2000, 2500));
            Gauge3.WithLabels("service1").Set(_random.Next(3000, 3500));
            Gauge4.WithLabels("service2").Set(_random.Next(4000, 4500));
            Gauge5.WithLabels("service2").Set(_random.Next(5000, 5500));
            Gauge6.WithLabels("service2").Set(_random.Next(6000, 6500));
            Gauge7.WithLabels("service3").Set(_random.Next(7000, 7500));
            Gauge8.WithLabels("service3").Set(_random.Next(8000, 8500));
            Gauge9.WithLabels("service3").Set(_random.Next(9000, 9500));

            _logger.LogInformation("{Source} has sent some data", nameof(DataService));     

            using (_logger.BeginScope(new Dictionary<string, object>{
                ["Property1"] = "Value 1",
                ["Property2"] = 2
            }))
            {
                _logger.LogInformation("Testing scope");
            }  
        }
    }
}
