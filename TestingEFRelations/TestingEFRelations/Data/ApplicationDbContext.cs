using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TestingEFRelations.Models;

namespace TestingEFRelations.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        
        //public DbSet<TestingEFRelations.Models.Address> Address { get; set; }
        public DbSet<TestingEFRelations.Models.Wishlist> Wishlist { get; set; }
        public DbSet<TestingEFRelations.Models.Cart> Cart { get; set; }
        public DbSet<TestingEFRelations.Models.Product> Product { get; set; }
        public DbSet<TestingEFRelations.Models.SmlImage> SmlImage { get; set; }
        public DbSet<TestingEFRelations.Models.Image> Image { get; set; }
        public DbSet<TestingEFRelations.Models.Receipt> Receipt { get; set; }
        public DbSet<TestingEFRelations.Models.Size> Size { get; set; }
    }
}
