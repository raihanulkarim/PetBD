using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PetBD.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetBD.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        // public DbSet<ProductCategory> ProductCategory { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var passHash = new PasswordHasher<IdentityUser>();
            builder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = "1",
                    UserName = "admin@petBD.com",
                    Email= "admin@petBD.com",
                    NormalizedUserName = "ADMIN@petBD.COM",
                    NormalizedEmail = "ADMIN@petBD.COM",
                    EmailConfirmed = true,
                    PasswordHash = passHash.HashPassword(null, "admin123!"),
                    SecurityStamp = Guid.NewGuid().ToString()
                }
                );
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "1",
                    Name = "admin",
                    NormalizedName = "admin",
                    ConcurrencyStamp = "1"
                }
                );
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "1",
                    UserId = "1"
                }
                );
        }
    }
}
