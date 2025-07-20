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
        public DbSet<info> infos { get; set; }
        public DbSet<News> news { get; set; }
        public DbSet<Events> events { get; set; }
        public DbSet<Schedule> schedules { get; set; }
        public DbSet<Team> teams { get; set; }
        public DbSet<Member> members { get; set; }
        public DbSet<TrainingLevel> trainingLevels { get; set; }
        public DbSet<TrainingContent> trainingContents {  get; set; }
        public DbSet<AnotherLink> anotherLinks { get; set; }
        public DbSet<Achievements> achievements { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<ContentCategory> contentCategories { get; set; }
        public DbSet<StickyNotes> StickyNotes { get; set; }
        public DbSet<QAndA> qAndAs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Events>()
             .HasMany(e => e.DailyPlan)
             .WithOne(s => s.Event)
             .HasForeignKey(s => s.EventId)
             .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<TrainingContent>()
            .Property(t => t.CreatedAt)
            .HasDefaultValueSql("GETDATE()");

            //builder.Entity<News>()
            //   .HasOne(n => n.ApplicationUser)  // News has one ApplicationUser
            //   .WithMany(u => u.NewsArticles)  // ApplicationUser can have many News articles
            //   .HasForeignKey(n => n.ApplicationUserId);
        }
    }

    
}
