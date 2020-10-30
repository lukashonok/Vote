using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Vote.Services;
using Repositories;
using Entities;
using Services.VoteModelService;
using Services.TargetModelService;
using Services.VotePlaceModelService;
using Services.VoteProcessModelService;
using Services.CompromisingEvidenceFileModelService;
using Services.CompromisingEvidenceModelService;
using Services.PhoneNumberModelService;
using Vote.Hubs;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using Microsoft.Extensions.Options;

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
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new List<CultureInfo>()
                {
                    new CultureInfo("ru"),
                    new CultureInfo("en"),
                    new CultureInfo("by"),
                };
                options.DefaultRequestCulture = new RequestCulture("ru");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            services.AddSignalR();
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                //string connection = "Host=ec2-54-217-213-79.eu-west-1.compute.amazonaws.com;Port=5432;Database=dchr8ii7h7l198;Username=hvaawqcheeowwj;Password=f64ebe3496848928d2a4f6aac92145464ab2603b8b6b0738e87eb5054a97768b";
                string connection = "dbname = dchr8ii7h7l198 host = ec2 - 54 - 217 - 213 - 79.eu - west - 1.compute.amazonaws.com port = 5432 user = hvaawqcheeowwj password = f64ebe3496848928d2a4f6aac92145464ab2603b8b6b0738e87eb5054a97768b sslmode = require";
                options.UseNpgsql(connection);
            });
            services.AddTransient<IVoteModelService, VoteModelService>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<ITargetModelService, TargetModelService>();
            services.AddTransient<IVotePlaceModelService, VotePlaceModelService>();
            services.AddTransient<IVoteProcessModelService, VoteProcessModelService>();
            services.AddTransient<IPhoneNumberModelService, PhoneNumberModelService>();
            services.AddTransient<INotificationModelService, NotificationModelService>();
            services.AddTransient<ICompromisingEvidenceModelService, CompromisingEvidenceModelService>();
            services.AddTransient<ICompromisingEvidenceFileModelService, CompromisingEvidenceFileModelService>();

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

            app.UseRequestLocalization(app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>().Value);

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
                endpoints.MapHub<StatHub>("/getStat");
                endpoints.MapRazorPages();
            });

            CreateRoles(serviceProvider);
        }

        private void CreateRoles(IServiceProvider serviceProvider)
        {
            const string SuperAdmin = "SuperAdmin";
            const string ADMIN = "Admin";
            const string MANAGER = "Manager";
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            Task<IdentityResult> roleResult;
            string email = "vladlykashonok@gmail.com";

            Task<bool> hasSuperAdminRole = roleManager.RoleExistsAsync(SuperAdmin);
            hasSuperAdminRole.Wait();
            Task<bool> hasManagerRole = roleManager.RoleExistsAsync(MANAGER);
            hasManagerRole.Wait();
            Task<bool> hasAdminRole = roleManager.RoleExistsAsync(ADMIN);
            hasAdminRole.Wait();
            if (!hasSuperAdminRole.Result)
            {
                roleResult = roleManager.CreateAsync(new IdentityRole(SuperAdmin));
                roleResult.Wait();
            }
            if (!hasManagerRole.Result)
            {
                roleResult = roleManager.CreateAsync(new IdentityRole(MANAGER));
                roleResult.Wait();
            }
            if (!hasAdminRole.Result)
            {
                roleResult = roleManager.CreateAsync(new IdentityRole(ADMIN));
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
                    Task<IdentityResult> newUserRole = userManager.AddToRoleAsync(administrator, SuperAdmin);
                    newUserRole.Wait();
                }
            }
        }
    }
}
