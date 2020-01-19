using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core_WebApp.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Core_WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // Registeres objects in dependency
        // 1 - Database context
        // 2 - MVC Opotions
        //      Filers
        //      Formatters
        // 3 - Security
        //      Authentication for users
        //      Authorization
        //          - Based on Roles
        //              - Role based policies
        //          - Based on JSON Web TOken
        // 4 - Cookies
        // 5 - CORS Policies
        // 6 - Custom Services
        //          Domain based services class aka Business Logic
        // 7 - Sessions

        // 1st method that willbe invoked when program.cs makes a call. It loads all dependencies
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            // Register the DbContext in the DI container
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("AppDbConnection"));
            }
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // IApplicationBuilder -> Used to manage HttpRequest using 'Middlewares'
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
