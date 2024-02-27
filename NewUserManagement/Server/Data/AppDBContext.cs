using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NewUserManagement.Shared.Models;


namespace NewUserManagement.Server.Data;

public class AppDBContext : IdentityDbContext<AppUser>
{
    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public new DbSet<AppUserConfig> Users { get; set; }
    public DbSet<LogDBEntry> LogEntries { get; set; }
    public DbSet<AppUser> AppUser { get; set; }

    public IQueryable<TEntity> GetAll<TEntity>() where TEntity : class
    {
        return Set<TEntity>();
    }

    public void Create<TEntity>(TEntity entity) where TEntity : class
    {
        Add(entity);
        SaveChanges();
    }

    public void Updater<TEntity>(TEntity entity) where TEntity : class
    {
        Updater(entity);
        SaveChanges();
    }

    public void Delete<TEntity>(TEntity entity) where TEntity : class
    {
        Remove(entity);
        SaveChanges();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Apply configuration for LogEntry entity
        modelBuilder.ApplyConfiguration(new LogEntryConfiguration());
        modelBuilder.ApplyConfiguration(new AppUserConfig());
        modelBuilder.Entity<AppUserConfig>().HasNoKey();
        modelBuilder.ApplyConfiguration(new AppIdentityRoleConfig());
        // Seed the users table with dummy data
        modelBuilder.Entity<User>().HasData(new[]
        {
                new User { Id = 1, Forename = "Peter", Surname = "Loew", Email = "ploew@example.com", IsActive = true },
                new User { Id = 2, Forename = "Benjamin Franklin", Surname = "Gates", Email = "bfgates@example.com", IsActive = true },
                new User { Id = 3, Forename = "Castor", Surname = "Troy", Email = "ctroy@example.com", IsActive = false },
                new User { Id = 4, Forename = "Memphis", Surname = "Raines", Email = "mraines@example.com", IsActive = true },
                new User { Id = 5, Forename = "Stanley", Surname = "Goodspeed", Email = "sgodspeed@example.com", IsActive = true },
                new User { Id = 6, Forename = "H.I.", Surname = "McDunnough", Email = "himcdunnough@example.com", IsActive = true },
                new User { Id = 7, Forename = "Cameron", Surname = "Poe", Email = "cpoe@example.com", IsActive = false },
                new User { Id = 8, Forename = "Edward", Surname = "Malus", Email = "emalus@example.com", IsActive = false },
                new User { Id = 9, Forename = "Damon", Surname = "Macready", Email = "dmacready@example.com", IsActive = false },
                new User { Id = 10, Forename = "Johnny", Surname = "Blaze", Email = "jblaze@example.com", IsActive = true },
                new User { Id = 11, Forename = "Robin", Surname = "Feld", Email = "rfeld@example.com", IsActive = true }
            });


    }

}
