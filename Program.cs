using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using NetCoreClient.Elk.Extensions;
using NLog.Web.AspNetCore;
using NLog.Extensions.Hosting;
using NLog.Extensions.Logging;
using System;

namespace NetCoreClient.Elk
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureLogging((hostContext, logBuilder) =>
                {
                    logBuilder.ClearProviders();
                    logBuilder.AddNLog(new NLogLoggingConfiguration(hostContext.Configuration.GetSection("NLog"))).SetMinimumLevel(LogLevel.Trace);
                })
                .UseNLog()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory());
    }
}