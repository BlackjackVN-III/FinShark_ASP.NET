using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TeddySmith.API.Models;

namespace TeddySmith.API.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions dbContextOptions): base(dbContextOptions)
        {
            
        }

        public DbSet<Stock> Stock { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Portfollo> Portfollos { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Portfollo>(x => x.HasKey(p => new{p.AppUserId,p.StockId}));

            modelBuilder.Entity<Portfollo>()
                  .HasOne(u => u.AppUser)
                  .WithMany(u => u.Portfollos)
                  .HasForeignKey(u => u.AppUserId);

            modelBuilder.Entity<Portfollo>()
                 .HasOne(u => u.Stock)
                 .WithMany(u => u.Portfollos)
                 .HasForeignKey(u => u.StockId);



            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = "1" },
                new IdentityRole { Id = "2", Name = "User", NormalizedName = "USER", ConcurrencyStamp = "2" }
            );
        }

        /*        protected override void OnModelCreating(ModelBuilder modelBuilder)
                {
                    base.OnModelCreating(modelBuilder);

                    List<IdentityRole> roles = new List<IdentityRole>
                    {
                        new IdentityRole
                        {
                             Id = "Admin",
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        },
                        new IdentityRole
                        {
                             Id = "User",
                            Name = "User",
                            NormalizedName = "USER"
                        }

                    };
                    modelBuilder.Entity<IdentityRole>().HasData(roles);

                }*/
    }
}
 