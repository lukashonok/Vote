using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vote.Areas.Identity.Data;
using Vote.Data;

[assembly: HostingStartup(typeof(Vote.Areas.Identity.IdentityHostingStartup))]
namespace Vote.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddDbContext<VoteContext>(options =>
                    options.UseSqlServer(
                context.Configuration.GetConnectionString("VoteContextConnection")));

                services.AddDefaultIdentity<ApplicationUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.SignIn.RequireConfirmedEmail = true;
                }
                ).AddEntityFrameworkStores<VoteContext>();
            });
        }
    }
}