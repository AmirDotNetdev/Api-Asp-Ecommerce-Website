using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestApi.Configurations;
using TestApi.Models.AuthModels;
using TestApi.Models.ProductModels;
using TestApi.Models.ShoppingCartModels;

namespace TestApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApiUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new RoleConfiguration());
        }

        public DbSet<RefreshToken> RefreshTokens {get; set;}
        public DbSet<BaseProduct> BaseProducts { get; set; }
        public DbSet<MainCategory> MainCategories { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<ImageBase> ImageBases { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<ProductSize> ProductSizes { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts{ get; set; }
        public DbSet<CartItem> CartItems { get; set; }
    }
}