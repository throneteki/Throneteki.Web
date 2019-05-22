namespace CrimsonDev.Throneteki
{
    using System.Diagnostics.CodeAnalysis;
    using CrimsonDev.Gameteki.Api.Helpers;
    using CrimsonDev.Gameteki.Api.Services;
    using CrimsonDev.Gameteki.Data.Models.Config;
    using CrimsonDev.Throneteki.Data;
    using CrimsonDev.Throneteki.Services;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var general = Configuration.GetSection("General").Get<GametekiApiOptions>();

            if (general.DatabaseProvider == "mssql")
            {
                services.AddDbContext<ThronetekiDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            }
            else
            {
                services.AddDbContext<ThronetekiDbContext>(options =>
                    options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            }

            services.AddGameteki(Configuration);

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            services.AddTransient<ICardService, CardService>();
            services.AddTransient<IThronetekiDbContext, ThronetekiDbContext>();
            services.AddTransient<IUserService, ThronetekiUserService>();
            services.AddTransient<IDeckValidationService, DeckValidationService>();
            services.AddScoped<RedisManager>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseSpaStaticFiles();

            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "default", template: "{controller}/{action=Index}/{id?}");
            });

            app.UseGameteki();

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("https://localhost:8080");
                }
            });

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var redisManager = scope.ServiceProvider.GetService<RedisManager>();
                redisManager.PopulateCards().Wait();
            }
        }
    }
}
