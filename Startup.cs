using AngelusTBlog.Data;
using AngelusTBlog.Models;
using AngelusTBlog.Services;
using AngelusTBlog.ViewModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngelusTBlog
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
            // Useing UseNpgsql to Build and connect the Backend - using the ConnectionService to connect to the DB
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(ConnectionService.GetConnectionString(Configuration)));
            //options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            // Adding the BlogUser Identity Role
            services.AddIdentity<BlogUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)

                // Adding: the Default UI and Default Token Providers.
                .AddDefaultUI()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();

            // Adding: Razor Pages
            services.AddRazorPages();

            // Regester my custom Data service class
            services.AddScoped<DataService>();

            // Regester my custom Blog Search service
            services.AddScoped<BlogSearchService>();

            //Regester a pre - configured instance of the mail Settings class - from the Jason file
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            // Regester my I Blog Email Sender interface from class Email Service
            services.AddScoped<IBlogEmailSender, EmailService>();

            // Regester my IImage Service interface and basic image service class.
            services.AddScoped<IImageService, BasicImageService>();

            // Regester my Slug Service interface and basic slug service class.
            services.AddScoped<ISlugService, BasicSlugService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
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
                // Custom Route Map
                endpoints.MapControllerRoute(
                    name: "SlugRoute",
                    pattern: "blogPosts/UrlFriendly/{slug}",
                    defaults: new { controller = "Posts", action = "Details" });

                // Default Route Map
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
