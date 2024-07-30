using Microsoft.EntityFrameworkCore;
using OrderManagement.Models;

namespace OrderManagement.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Order>()
            //.HasOne(o => o.Customer)
            //.WithMany()
            //.HasForeignKey(o => o.CustomerId);

            //modelBuilder.Entity<OrderItem>()
            //    .HasOne(oi => oi.Order)
            //    .WithMany(o => o.OrderItems)
            //    .HasForeignKey(oi => oi.OrderId);

            //modelBuilder.Entity<OrderItem>()
            //    .HasOne(oi => oi.Product)
            //    .WithMany()
            //    .HasForeignKey(oi => oi.ProductId);

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Book", Price = 100 },
                new Product { Id = 2, Name = "Pen", Price = 20 },
                new Product { Id = 3, Name = "Calculator", Price = 1000 }
            );

            
            modelBuilder.Entity<Customer>().HasData(
                new Customer { Id = 1, Name = "Mr. Rahat Ahmed", Phone = "1234567890", Address = "Chittagong" },
                new Customer { Id = 2, Name = "Mr. Aowal Azad", Phone = "0987654321", Address = "Dhaka" }
            );

            
            modelBuilder.Entity<Order>().HasData(
                new Order { Id = 1, CustomerId = 1 },
                new Order { Id = 2, CustomerId = 2 }
            );


            modelBuilder.Entity<OrderItem>().HasData(

                new OrderItem { Id = 1, OrderId = 1, ProductId = 1, Quantity = 5, UnitPrice = 100, TotalPrice = 500 },
                new OrderItem { Id = 2, OrderId = 1, ProductId = 2, Quantity = 10, UnitPrice = 20, TotalPrice = 200 },
                new OrderItem { Id = 3, OrderId = 1, ProductId = 3, Quantity = 3, UnitPrice = 1000, TotalPrice = 3000 },


                new OrderItem { Id = 4, OrderId = 2, ProductId = 1, Quantity = 5, UnitPrice = 100, TotalPrice = 500 },
                new OrderItem { Id = 5, OrderId = 2, ProductId = 2, Quantity = 10, UnitPrice = 20, TotalPrice = 200 },
                new OrderItem { Id = 6, OrderId = 2, ProductId = 3, Quantity = 1, UnitPrice = 1000, TotalPrice = 1000 });
        }


    }
}
