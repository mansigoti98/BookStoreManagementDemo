using BookStoreManagement_1.Models;
using BookStoreManagement_1.Models.Admin;
using Microsoft.EntityFrameworkCore;

namespace BookStoreManagement_1.Data
{
    public class BookStoreDbContext : DbContext
    {
        public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options) : base(options) { }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Country> Countries { get; set; }

        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> Orderdetails { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<UserLog> UserLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .Property(b => b.Price)
                .HasPrecision(18, 2);
            base.OnModelCreating(modelBuilder);
        }
    }
}
