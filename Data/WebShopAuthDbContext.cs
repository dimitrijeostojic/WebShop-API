﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebShop.API.Data
{
    public class WebShopAuthDbContext : IdentityDbContext
    {
        public WebShopAuthDbContext(DbContextOptions<WebShopAuthDbContext> dbContextOptions) : base(dbContextOptions)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var RegularUserRoleId = "b4f150d0-f023-4de5-b9fe-247ff6b6ef87";
            var ManagerRoleId = "83d0736a-f899-4b65-8b43-73be201e670e";
            var AdminRoleId = "8ec281a0-c9bf-4c16-9346-062afbc90466";

            var roles = new List<IdentityRole>
            {
                new IdentityRole()
                {
                    Id = RegularUserRoleId,
                    ConcurrencyStamp = RegularUserRoleId,
                    Name="RegularUser",
                    NormalizedName="RegularUser".ToUpper()
                },new IdentityRole()
                {
                    Id = ManagerRoleId,
                    ConcurrencyStamp = ManagerRoleId,
                    Name="Manager",
                    NormalizedName="Manager".ToUpper()
                },new IdentityRole()
                {
                    Id = AdminRoleId,
                    ConcurrencyStamp = AdminRoleId,
                    Name="Admin",
                    NormalizedName="Admin".ToUpper()
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }

    }
}
