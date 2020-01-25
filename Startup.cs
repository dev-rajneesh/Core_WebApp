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
using Core_WebApp.Services;

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
            services.AddControllers(); // Only for Web API controller        
            services.AddControllersWithViews(); // MVC request and WebAPI request processing        

            // Register the DbContext in the DI container
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("AppDbConnection"));
            });

            //Register repository services in the DI container
            // 1st parameter is ServiceType, 2nd param is called Implementation
            services.AddScoped<IRepository<Category, int>, CategoryRepository>();
            services.AddScoped<IRepository<Product, int>, ProductRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // IApplicationBuilder -> Used to manage HttpRequest using 'Middlewares'
        // Detect the hosting environment
        // Our actual request processing starts from this method
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Detect the environment            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            // Uses wwwRoot to read static files like js, css, img or any other custom files 
            // to render Http Response
            app.UseStaticFiles();

            app.UseRouting(); // Common routing for MVC or API based on endpoints

            app.UseAuthorization(); // Used for user-pass 

            // exposes Endpoint from server to accept Http Request
            // Process it using routing and generate response
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
