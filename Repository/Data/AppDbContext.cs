using Core.Entities;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Data
{
    public class AppDbContext: IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        DbSet<info> infos { get; set; }
        DbSet<News> news { get; set; }
        DbSet<Events> events { get; set; }
        DbSet<Schedule> schedules { get; set; }
        DbSet<Team> teams { get; set; }
        DbSet<Member> members { get; set; }
        DbSet<TrainingLevel> trainingLevels { get; set; }
        DbSet<TrainingContent> trainingContents {  get; set; }
        DbSet<Achievements> achievements { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Events>()
             .HasMany(e => e.DailyPlan)
             .WithOne(s => s.Event)
             .HasForeignKey(s => s.EventId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }

    
}
