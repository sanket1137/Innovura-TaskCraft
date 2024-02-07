using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data
{
    public class ApplicationDbContext: DbContext 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
                
        }
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Label> Label { get; set; }
        public DbSet<RefreshToken> Token { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(user =>
            {
                user.HasMany(u => u.Tasks)
                    .WithOne(t => t.User)
                    .HasForeignKey(t => t.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                user.HasMany(u => u.Labels)
                    .WithOne(l => l.User)
                    .HasForeignKey(l => l.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                user.HasOne(u => u.RefreshToken)
                    .WithOne(rt => rt.User)
                    .HasForeignKey<User>(u => u.Id)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Label>(label =>
            {
                label.HasOne(l => l.User)
                    .WithMany(u => u.Labels)
                    .HasForeignKey(l => l.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<RefreshToken>(refreshToken =>
            {
                refreshToken.HasOne(rt => rt.User)
                    .WithOne(u => u.RefreshToken)
                    .HasForeignKey<RefreshToken>(rt => rt.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<TaskItem>(task =>
            {
                task.HasOne(t => t.Label)
                    .WithMany(l => l.Tasks)
                    .HasForeignKey(t => t.LabelId)
                    .OnDelete(DeleteBehavior.NoAction);

                task.HasOne(t => t.User)
                    .WithMany(u => u.Tasks)
                    .HasForeignKey(t => t.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(modelBuilder);

            // Other configurations...

            // Ensure that you include the necessary configurations for other entities and relationships in your model.
        }
    }
}
