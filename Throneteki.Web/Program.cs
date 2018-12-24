namespace CrimsonDev.Throneteki
{
    using System;
    using System.Threading.Tasks;
    using CrimsonDev.Throneteki.Data;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using NLog.Web;
    using SeedData = CrimsonDev.Gameteki.Data.SeedData;
    using ThronetekiSeedData = CrimsonDev.Throneteki.Data.SeedData;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

            try
            {
                var host = CreateWebHostBuilder(args).Build();

                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;

                    try
                    {
                        var context = services.GetRequiredService<ThronetekiDbContext>();
                        context.Database.Migrate();
                        await SeedData.Initialize(scope, context);
                        await ThronetekiSeedData.Initialize(services);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, "An error occurred seeding the database.");
                    }
                }

                host.Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occurred starting the website");
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
                .UseNLog();
    }
}
