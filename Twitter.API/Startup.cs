using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Twitter.Data.Models.Config;
using Twitter.Manager.Managers;
using Twitter.Manager.Services;

namespace Twitter.API
{
    public class Startup
    {
        private static readonly HttpClient _client = new HttpClient() { Timeout = TimeSpan.FromMinutes(240) };

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.Configure<TwitterConfig>(options => Configuration.GetSection(nameof(TwitterConfig)).Bind(options));
            services.AddScoped<IAnalyticsManager, AnalyticsManager>();
            services.AddScoped<IHealthManager, HealthManager>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMemoryCache cache)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            ProcessTwitterStream(cache);
        }

        private void ProcessTwitterStream(IMemoryCache cache)
        {
            TwitterConfig twitterConfig = new TwitterConfig();
            Configuration.GetSection(nameof(TwitterConfig)).Bind(twitterConfig);

            TwitterStreamService.InstantiateCache(cache);
            Task.Run(() => TwitterStreamService.ProcessTwitterStream(twitterConfig, cache));
        }
    }
}
