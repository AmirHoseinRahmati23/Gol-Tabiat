using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PoshakMonix.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoshakMonix.Data
{
    public class PoshakMonixContext : IdentityDbContext
    {
        public PoshakMonixContext(DbContextOptions<PoshakMonixContext> options): base(options)
        {
        }

        public DbSet<Cart> Carts{ get; set; }
        public DbSet<CartDetail> CartDetails { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Factor> Factors { get; set; }
        public DbSet<FactorDetail> FactorDetails { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<SubGroup> SubGroups { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<SecooundCardSliderItem> SecooundCardSliderItems { get; set; }
        public DbSet<SliderImage> SliderImages { get; set; }
        public DbSet<SupportMessage> SupportMessages { get; set; }
        public DbSet<DiscountCode> DiscountCodes { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            SeedUsers(builder);

            SeedRoles(builder);

            SeedUserRoles(builder);
        }



        #region IdentitySeeds
        private void SeedUsers(ModelBuilder builder)
        {
            var firstAdmin = new User()
            {
                Id = "b74ddd14-6340-4840-95c2-db12554843e5 ",
                UserName = "AmirHosein_Rahmati_233_GodMode",
                NormalizedUserName = "AMIRHOSEIN_RAHMATI_233_GODMODE",
                Email = "www.amir233@gmail.com",
                LockoutEnabled = false,
                PhoneNumber = "1234567890",
                ConcurrencyStamp = "4f5f066d-b118-4d17-9420-37edf9cccb59",
                SecurityStamp = "3dd750e2-a940-43b3-aefc-c677f6a68ca9",
                PasswordHash = "AQAAAAEAACcQAAAAEM3HumZQwETRIbUvyBu0iN1Cg2BqrHKdfsYJvZAPOyShb1FUIzp0UpovEPrjtwvqpA=="
            };


            builder.Entity<User>().HasData(firstAdmin);
        }
        private void SeedRoles(ModelBuilder builder)
        {

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole()
                {
                    Id = "c7b013f0-5201-4317-abd8-c211f91b7330",
                    Name = "Admin",
                    ConcurrencyStamp = "2",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole()
                {
                    Id = "b74d13f0-5201-4840-95c2-a14da6895711",
                    Name = "User",
                    ConcurrencyStamp = "5",
                    NormalizedName = "USER"
                });
        }
        private void SeedUserRoles(ModelBuilder builder)
        {

            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>()
                {
                    RoleId = "c7b013f0-5201-4317-abd8-c211f91b7330",
                    UserId = "b74ddd14-6340-4840-95c2-db12554843e5"
                });
        }       
              


        #endregion

    }
}