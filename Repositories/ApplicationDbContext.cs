using System;
using Entities;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Repositories
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<VoteModel> Vote { get; set; }
        public DbSet<TargetModel> Target { get; set; }
        public DbSet<VotePlaceModel> VotePlace { get; set; }
        public DbSet<VoteProcessModel> VoteProcess { get; set; }
        public DbSet<PhoneNumberModel> PhoneNumber { get; set; }
        public DbSet<NotificationModel> Notification { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<CompromisingEvidenceModel> CompromisingEvidence { get; set; }
        public DbSet<CompromisingEvidenceFileModel> CompromisingEvidenceFile { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    string connection = "Server=(localdb)\\mssqllocaldb;Database=aspnet-Vote-F2ABA109-2547-4D2D-BC44-04C6649F52E6;Trusted_Connection=True;MultipleActiveResultSets=true";
        //    optionsBuilder.UseSqlServer(connection, b => b.MigrationsAssembly("Vote"));
        //}
    }
}
