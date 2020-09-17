using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Vote.Areas.Identity.Data;
using Vote.Models;

namespace Vote.Data
{
    public class VoteContext : IdentityDbContext<ApplicationUser>
    {
        public VoteContext(DbContextOptions<VoteContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<VoteModel> Vote { get; set; }
        public DbSet<VotePlaceModel> VotePlace { get; set; }
        public DbSet<TargetModel> Target { get; set; }
        public DbSet<CompromisingEvidenceModel> CompromisingEvidence { get; set; }
        public DbSet<CompromisingEvidenceFileModel> CompromisingEvidenceFile { get; set; }

    }
}
