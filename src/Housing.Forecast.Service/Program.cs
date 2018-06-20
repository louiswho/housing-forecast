using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Housing.Forecast.Context;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Housing.Forecast.Service
{
  public static class Program
  {
    public static void Main(string[] args)
    {
        var host = BuildWebHost(args);
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<ForecastContext>();
            context.Database.EnsureCreated();
            Seeder.Initialize(services).Wait();
        }
        host.Run();

    }

    public static IWebHost BuildWebHost(string[] args) =>
      WebHost.CreateDefaultBuilder(args)
        .UseApplicationInsights(Environment.GetEnvironmentVariable("INSTRUMENTATION_KEY"))
        .UseStartup<Startup>()
        .Build();
  }
}
