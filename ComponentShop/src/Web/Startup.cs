using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Common.Identity;
using Web.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        private IServiceCollection _services;
        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<InventoryContext>(options =>
            {
                try
                {
                    //options.UseInMemoryDatabase("ComponentShopPostgreSql");
                    options.UseNpgsql(Configuration.GetConnectionString("ComponentShopConnection"), b => b.MigrationsAssembly("Web"));
                }
                catch (Exception ex )
                {
                    var message = ex.Message;
                }                
            });

            services.AddDbContext<AppIdentityDbContext>(options =>
                //options.UseInMemoryDatabase("Identity"));
                options.UseNpgsql(Configuration.GetConnectionString("IdentityConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddMemoryCache();

            //services.AddScoped<ICatalogService, CachedCatalogService>();
            //services.AddScoped<CatalogService>();
            //services.Configure<CatalogSettings>(Configuration);
            //services.AddSingleton<IImageService, LocalFileImageService>();
            //services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
            services.AddMvc();

            _services = services;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Inventory/Error");
            }

            app.UseStaticFiles();
            app.UseIdentity();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Inventory}/{action=Index}/{id?}");
            });

            //Seed Data
            InventoryContextSeed.SeedAsync(app, loggerFactory)
            .Wait();
        }
    }
}
