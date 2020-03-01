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
using Core_WebApp.Custom_Filter;
using Core_WebApp.CustomMiddleware;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Core_WebApp.Data;
using Microsoft.AspNetCore.Identity;

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
            services.AddMemoryCache();
            services.AddSession();
            services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();

            services.AddControllers(); // Only for Web API controller      

            // MVC request and WebAPI request processing
            services.AddControllersWithViews(
                //options => options.Filters.Add(typeof(MyExceptionFilter))
                );

            // Register the DbContext in the DI container
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("AppDbConnection"));
            });

            // security classes
            services.AddDbContext<AuthDbContext>(options =>
                  options.UseSqlServer(
                      Configuration.GetConnectionString("AuthDbContextConnection")));
            // resolve UserManager<IdentityUser>, SignInManager<IdentityUser>
            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddDefaultUI()
            //    .AddEntityFrameworkStores<AuthDbContext>();

            services.AddIdentity<IdentityUser, IdentityRole>()
              .AddDefaultUI()
              .AddEntityFrameworkStores<AuthDbContext>();
            // Ends here


            // adding policies
            services.AddAuthorization(options => {
                options.AddPolicy("ReadPolicy", policy =>
                {
                    policy.RequireRole("Clerk", "Admin", "Manager");
                });
                options.AddPolicy("WritePolicy", policy =>
                {
                    policy.RequireRole("Admin", "Manager");
                });
            });

            //Register repository services in the DI container
            // 1st parameter is ServiceType, 2nd param is called Implementation
            services.AddScoped<IRepository<Category, int>, CategoryRepository>();
            services.AddScoped<IRepository<Product, int>, ProductRepository>();
            services.AddScoped<IRepository<ErrorLog, int>, ErrorRepository>();

            services.AddMvc(); // security for Controllers
            //services.AddDistributedMemoryCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // IApplicationBuilder -> Used to manage HttpRequest using 'Middlewares'
        // Detect the hosting environment
        // Our actual request processing starts from this method
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // routing for API and MVC based on EndPoints
            app.UseRouting(); // use it ar first place for Identity
            app.UseSession();

            // Detect the environment            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseCustomExceptionHandlerMiddleware();

            // Uses wwwRoot to read static files like js, css, img or any other custom files 
            // to render Http Response
            app.UseStaticFiles();

            app.UseRouting(); // Common routing for MVC or API based on endpoints

            app.UseAuthorization(); // used for USerName/PWD and JWT
            app.UseAuthentication();

            // exposes Endpoint from server to accept Http Request
            // Process it using routing and generate response
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
