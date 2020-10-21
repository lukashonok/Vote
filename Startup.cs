using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Vote.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Vote.Areas.Identity.Data;
using Vote.Services;
using System.Security.Policy;

namespace Vote
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

            services.AddDbContext<VoteContext>(options =>
            {
                options.UseSqlServer(Config.ConnectionString);
            });

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    IConfiguration googleAuthentication =
                        Configuration.GetSection("Authentication:Google");
                    options.ClientId     = googleAuthentication["ClientId"];
                    options.ClientSecret = googleAuthentication["ClientSecret"];
                });


            services.AddTransient<IViewRenderService, ViewRenderService>();

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Vote/Error");
                app.UseHsts();
            }

            Configuration.Bind("Project", new Config());

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Vote}/{action=Index}/{id?}");
                endpoints.MapAreaControllerRoute(
                    name: "AdminPanel",
                    areaName: "AdminPanel",
                    pattern: "{controller=Admin}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            CreateRoles(serviceProvider);
        }

        private void CreateRoles(IServiceProvider serviceProvider)
        {
            const string ADMIN = "Admin";
            const string MANAGER = "Manager";
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            Task<IdentityResult> roleResult;
            string email = "vladlykashonok@gmail.com";

            Task<bool> hasAdminRole = roleManager.RoleExistsAsync(ADMIN);
            hasAdminRole.Wait();
            Task<bool> hasManagerRole = roleManager.RoleExistsAsync(MANAGER);
            hasManagerRole.Wait();
            if (!hasAdminRole.Result)
            {
                roleResult = roleManager.CreateAsync(new IdentityRole(ADMIN));
                roleResult.Wait();
            }
            if (!hasManagerRole.Result)
            {
                roleResult = roleManager.CreateAsync(new IdentityRole(MANAGER));
                roleResult.Wait();
            }

            Task<ApplicationUser> adminUser = userManager.FindByEmailAsync(email);
            adminUser.Wait();

            if (adminUser.Result == null)
            {
                ApplicationUser administrator = new ApplicationUser
                {
                    Email = email,
                    UserName = email
                };

                Task<IdentityResult> newUser = userManager.CreateAsync(administrator, "147896325xXx.");
                newUser.Wait();

                if (newUser.Result.Succeeded)
                {
                    Task<IdentityResult> newUserRole = userManager.AddToRoleAsync(administrator, ADMIN);
                    newUserRole.Wait();
                }
            }
        }
    }
}
