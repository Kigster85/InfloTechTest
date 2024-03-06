using Microsoft.AspNetCore.Identity;
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

        public IQueryable<TEntity> GetAll<TEntity>() where TEntity : class
        {
            return Set<TEntity>();
        }

        public async Task CreateAsync<TEntity>(TEntity entity) where TEntity : class
        {
            await AddAsync(entity);
            await SaveChangesAsync();
        }

        public async Task<EntityEntry<TEntity>> UpdateAsync<TEntity>(TEntity entity) where TEntity : class
        {
            var entry = Update(entity);
            await SaveChangesAsync();
            return entry;
        }

        public async Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class
        {
            Remove(entity);
            await SaveChangesAsync();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            #region Administrator Role Seed

            const string administratorRoleName = "Admin";

            IdentityRole administratorRoleToSeed = new()
            {
                Id = Guid.NewGuid().ToString(),
                Name = administratorRoleName,
                NormalizedName = administratorRoleName.ToUpperInvariant()
            };

            modelBuilder.Entity<IdentityRole>().HasData(administratorRoleToSeed);
            #endregion

            #region Administrator user seed

            const string administratorUserEmail = "admin@example.com";

            var passwordHasher = new PasswordHasher<AppUser>();

            AppUser administratorUserToSeed = new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = administratorUserEmail,
                NormalizedUserName = administratorUserEmail.ToUpperInvariant(),
                EmailAddress = administratorUserEmail,
                Email = administratorUserEmail,
                NormalizedEmail = administratorUserEmail.ToUpperInvariant(),
                Password = string.Empty,
                Forename = "Admin",
                Surname = "User",
                IsActive = true,

            };

            string hashedPassword = passwordHasher.HashPassword(administratorUserToSeed, "AdminPassword123!");

            administratorUserToSeed.PasswordHash = hashedPassword;

            modelBuilder.Entity<AppUser>().HasData(administratorUserToSeed);

            #endregion

            #region Add the Adminstrator user to the administrator role

            IdentityUserRole<string> identityUserRoleToSeed = new()
            {
                RoleId = administratorRoleToSeed.Id,
                UserId = administratorUserToSeed.Id,
            };

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(identityUserRoleToSeed);

            #endregion

            var usersToSeed = new List<AppUser>
            {
            new AppUser
            {
                UserName = "ploew@example.com",
                EmailAddress = "ploew@example.com",
                Email = "ploew@example.com",
                Forename = "Peter",
                Surname = "Loew",
                IsActive = true,
                Password = string.Empty,
            },

            new AppUser
            {
                UserName = "bfgates@example.com",
                EmailAddress = "bfgates@example.com",
                Email = "bfgates@example.com",
                Forename = "Benjamin Franklin",
                Surname = "Gates",
                IsActive = true,
                Password = string.Empty,
            },

            new AppUser
            {
                UserName = "ctroy@example.com",
                EmailAddress = "ctroy@example.com",
                Email = "ctroy@example.com",
                Forename = "Castor",
                Surname = "Troy",
                IsActive = false,
                Password = string.Empty,
            },

            new AppUser
            {
                UserName = "mraines@example.com",
                EmailAddress = "mraines@example.com",
                Email = "mraines@example.com",
                Forename = "Memphis",
                Surname = "Raines",
                IsActive = true,
                Password = string.Empty,
            },

            new AppUser
            {
                UserName = "sgodspeed@example.com",
                EmailAddress = "sgodspeed@example.com",
                Email = "sgodspeed@example.com",
                Forename = "Stanley",
                Surname = "Goodspeed",
                IsActive = true,
                Password = string.Empty,
            },

            new AppUser
            {
                UserName = "himcdunnough@example.com",
                EmailAddress = "himcdunnough@example.com",
                Email = "himcdunnough@example.com",
                Forename = "H.I.",
                Surname = "McDunnough",
                IsActive = true,
                Password = string.Empty,
            },

            new AppUser
            {
                UserName = "cpoe@example.com",
                EmailAddress = "cpoe@example.com",
                Email = "cpoe@example.com",
                Forename = "Cameron",
                Surname = "Poe",
                IsActive = false,
                Password = string.Empty,
            },

            new AppUser
            {
                UserName = "emalus@example.com",
                EmailAddress = "emalus@example.com",
                Email = "emalus@example.com",
                Forename = "Edward",
                Surname = "Malus",
                IsActive = false,
                Password = string.Empty,
            },

            new AppUser
            {
                UserName = "dmacready@example.com",
                EmailAddress = "dmacready@example.com",
                Email = "dmacready@example.com",
                Forename = "Damon",
                Surname = "Macready",
                IsActive = false,
                Password = string.Empty,
            },

            new AppUser
            {
                UserName = "jblaze@example.com",
                EmailAddress = "jblaze@example.com",
                Email = "jblaze@example.com",
                Forename = "Johnny",
                Surname = "Blaze",
                IsActive = true,
                Password = string.Empty,
            },

            new AppUser
            {
                UserName = "rfeld@example.com",
                EmailAddress = "rfeld@example.com",
                Email = "rfeld@example.com",
                Forename = "Robin",
                Surname = "Feld",
                IsActive = true,
                Password = string.Empty,
            },

        };
            foreach (var user in usersToSeed)
            {
                string userPassword = "Password123!"; // Set a default password for all users
                string userHashedPassword = passwordHasher.HashPassword(user, userPassword); user.Password = hashedPassword;
                user.PasswordHash = userHashedPassword;
                modelBuilder.Entity<AppUser>().HasData(user);
            }
            modelBuilder.ApplyConfiguration(new LogEntryConfiguration());

        }
    }
        
       
    

}
