using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PoshakMonix.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PoshakMonix.Models.Entities;
using PoshakMonix.Core.UnitOfWork.Repositories;
using PoshakMonix.Core.UnitOfWork.Services;
using UnitOfWork.Repositories;
using UnitOfWork.Services;

namespace PoshakMonix.Web
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
            services.AddDbContext<PoshakMonixContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Monix"));
            });
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<PoshakMonixContext>()
                .AddDefaultTokenProviders();

            
            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = "131174477822-ui2ges2pqaa0l86h5chtjhntl20mptp6.apps.googleusercontent.com";
                    options.ClientSecret = "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";
                });


            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddTransient<IGroupRepository, GroupService>();

            services.AddControllersWithViews();

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
                endpoints.MapControllerRoute(
                    name: "area",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
