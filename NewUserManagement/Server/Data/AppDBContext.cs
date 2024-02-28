using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NewUserManagement.Shared.Models;

namespace NewUserManagement.Server.Data
{
    public class AppDBContext : IdentityDbContext<AppUser>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<LogDBEntry> LogEntries { get; set; }

        // Optionally, you can remove this DbSet if not needed
        // public DbSet<AppUser> AppUser { get; set; }

        public IQueryable<TEntity> GetAll<TEntity>() where TEntity : class
        {
            return Set<TEntity>();
        }

        public void Create<TEntity>(TEntity entity) where TEntity : class
        {
            Add(entity);
            SaveChanges();
        }

        public EntityEntry<TEntity> Updater<TEntity>(TEntity entity) where TEntity : class
        {
            var entry = Update(entity);
            SaveChanges();
            return entry;
        }


        public void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            Remove(entity);
            SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new LogEntryConfiguration());
            modelBuilder.ApplyConfiguration(new AppIdentityRoleConfig());

            // You don't need to seed users here anymore
        }
    }
    
}

