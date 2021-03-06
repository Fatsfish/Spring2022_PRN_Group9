using EMS.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EMS
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
            //services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                //.AddMicrosoftIdentityWebApp(Configuration.GetSection("AzureAd"));
            services.AddDbContext<EventMSContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("EventMS")));
            services.AddSession(options => { options.IdleTimeout = TimeSpan.FromMinutes(30); });
            services.AddMvc().AddSessionStateTempDataProvider();
            services.AddHttpContextAccessor();
            services.AddDirectoryBrowser();
            services.AddAuthorization(options =>
            {
                // By default, all incoming requests will be authorized according to the default policy
                options.FallbackPolicy = options.DefaultPolicy;
            });
            services.AddRazorPages()
                .AddMvcOptions(options => { });
                //.AddMicrosoftIdentityUI();
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
            app.UseSession();
            app.UseRouting();
            var fileProvider = new PhysicalFileProvider(Path.Combine(env.WebRootPath, "img"));
            var requestPath = "/Images";

            // Enable displaying browser links.
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = fileProvider,
                RequestPath = requestPath
            });
            //app.UseAuthentication();
            //app.UseAuthorization();
            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = fileProvider,
                RequestPath = requestPath
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}
