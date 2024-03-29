﻿using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCoreClient.Elk.Extensions;
using NetCoreClient.Elk.Tasks;

namespace NetCoreClient.Elk
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            // Adds all custom configurations for this service.
            services
                .Configure<Settings>(Configuration)
                .AddOptions()
                .AddLogging()
                .AddCustomHealthCheck(Configuration)
                .AddHostedService<DataService>();        
        }

        /// <summary>
        /// Configures the HTTP request pipeline. Automatically called by the runtime.
        /// </summary>
        /// <param name="app">The application builder.</param>
        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
                {
                    Predicate = r => r.Name.Contains("self")
                });
            });
        }
    }
}