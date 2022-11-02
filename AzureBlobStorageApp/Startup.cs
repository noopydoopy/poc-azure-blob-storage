using AzureBlobStorageApp.Configs;
using AzureBlobStorageApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AzureBlobStorageApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("AADCookies").AddCookie("AADCookies", options =>
            {
                options.Cookie.Name = "AADCookies";
                options.LoginPath = "/Login";
            });

            services.AddRazorPages();
            services.Configure<AzureADBlobsConfig>(Configuration.GetSection(AzureADBlobsConfig.SECTION));

            //services.AddAzureClients(clientBuilder =>
            //{

            //    // Add a storage account client
            //    clientBuilder.AddBlobServiceClient(Configuration.GetSection("Storage"));

            //    // Use DefaultAzureCredential by default
            //    clientBuilder.UseCredential(new DefaultAzureCredential());

            //    // Set up any default settings
            //    clientBuilder.ConfigureDefaults(Configuration.GetSection("AzureDefaults"));
            //});

            // Services
            services.AddScoped<IAzureStorageService, AzureStorageService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
