using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Order>Orders { get; set; }
    public DbSet<OrdersBooks> OrdersBooks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=BookStoreDb;Trusted_connection=true;MultipleActiveResultSets=True");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrdersBooks>().HasKey(sc => new { sc.OrderId, sc.BookId });
        modelBuilder
            .Entity<OrdersBooks>().HasOne(sc => sc.Book)
            .WithMany(x=>x.OrdersBook)
            .HasForeignKey(x=>x.BookId);
        
        modelBuilder
            .Entity<OrdersBooks>().HasOne(sc => sc.Order)
            .WithMany(x=>x.OrdersBook)
            .HasForeignKey(x=>x.OrderId);
        
        modelBuilder
            .Entity<Order>().HasOne(sc => sc.User)
            .WithMany(x=>x.Orders)
            .HasForeignKey(x=>x.UserId);
    }
}