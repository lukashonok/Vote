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
        //    string connection = "Host=ec2-54-217-213-79.eu-west-1.compute.amazonaws.com;Port=5432;Database=dchr8ii7h7l198;Username=hvaawqcheeowwj;Password=f64ebe3496848928d2a4f6aac92145464ab2603b8b6b0738e87eb5054a97768b";
        //    optionsBuilder.UseNpgsql(connection);
        //}
    }
}
