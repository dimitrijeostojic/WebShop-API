using Microsoft.EntityFrameworkCore;
using WebShop.API.Models.Domain;

namespace WebShop.API.Data
{
    public class WebShopDbContext : DbContext
    {
        public WebShopDbContext(DbContextOptions<WebShopDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<Cart> Cart { get; set; }
        public DbSet<CartItem> CartItem { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<Product> Product { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Cart -> CartItem (1:N)
            modelBuilder.Entity<Cart>()
                .HasMany(c => c.CartItems)
                .WithOne(ci => ci.Cart)
                .HasForeignKey(ci => ci.CartId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // CartItem -> Product (N:1)
            //modelBuilder.Entity<CartItem>()
            //    .HasOne(ci => ci.Product)
            //    .WithMany(p => p.CartItems)
            //    .HasForeignKey(ci => ci.ProductId)
            //    .IsRequired()
            //    .OnDelete(DeleteBehavior.Restrict);

            // Order -> OrderItem (1:N)
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // OrderItem -> Product (N:1)
            //modelBuilder.Entity<OrderItem>()
            //    .HasOne(oi => oi.Product)
            //    .WithMany(p => p.OrderItems)
            //    .HasForeignKey(oi => oi.ProductId)
            //    .IsRequired()
            //    .OnDelete(DeleteBehavior.Restrict);

            // Product -> Category (N:1)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // Composite key for CartItem: (CartItemId + CartId)
            modelBuilder.Entity<CartItem>()
                .HasKey(ci => new { ci.CartItemId, ci.CartId });

            // Composite key for OrderItem: (OrderItemId + OrderId)
            modelBuilder.Entity<OrderItem>()
                .HasKey(oi => new { oi.OrderItemId, oi.OrderId });


            modelBuilder.Entity<Product>().Property(p => p.Price).HasPrecision(18, 2);

            modelBuilder.Entity<OrderItem>().Property(oi => oi.Price).HasPrecision(18, 2);


            //#region Seedovanje kategorija i proizvoda
            //// Fixirani ID-jevi radi veze
            //var categoryFoodId = Guid.Parse("400cdda7-eb01-4207-be91-f2bb2c4a75c3");
            //var categoryGearId = Guid.Parse("c01542a0-7c26-495c-a15b-6365442aa50b");
            //var categoryToysId = Guid.Parse("D145FD9A-FD96-477B-AE49-24936CCB00DD");
            //var categoryCareId = Guid.Parse("31954536-64DF-467E-BF49-02E5CA4B3BB1");
            //var product1Id = Guid.Parse("2f7dd3d3-9097-49de-b750-119d10fe483a");
            //var product2Id = Guid.Parse("55acbafe-f9fc-469c-bcac-66955609b9ea");

            //// Seed kategorije
            //List<Category> categories = new List<Category>()
            //{
            //     new Category
            //{
            //    CategoryId = categoryFoodId,
            //    CategoryName = "Hrana",

            //},
            //new Category
            //{
            //    CategoryId = categoryGearId,
            //    CategoryName = "Oprema"
            //},
            //new Category
            //{
            //    CategoryId = categoryToysId,
            //    CategoryName = "Igracke"
            //},
            //new Category
            //{
            //    CategoryId = categoryCareId,
            //    CategoryName = "Nega"
            //}
            //};

            //modelBuilder.Entity<Category>().HasData(categories);

            //// Seed proizvodi
            //List<Product> products = new List<Product>()
            //{
            //    new Product
            //    {
            //        ProductId = product1Id,
            //        Name = "Granule za pse",
            //        Description = "Premium hrana za odrasle pse.",
            //        Price = 29.99m,
            //        Stock = 50,
            //        ImageUrl = "https://www.pet-centar.rs/cdn/shop/files/Obrok_u_kesici_2.png?v=1700562347&width=360",
            //        CategoryId = categoryFoodId
            //    },
            //    new Product
            //    {
            //        ProductId = product2Id,
            //        Name = "Povodac",
            //        Description = "Izdržljivi povodac za šetnju.",
            //        Price = 15.50m,
            //        Stock = 100,
            //        ImageUrl = "https://www.petbox.rs/sites/default/files/styles/product_teaser/public/product/images/crve.jpg?itok=rutNMX-W",
            //        CategoryId = categoryGearId
            //    }
            //};
            //modelBuilder.Entity<Product>().HasData(products);
            //#endregion


        }
    }
}