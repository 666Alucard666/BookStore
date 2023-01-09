using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class GigienaStoreDbContext : DbContext
{
    public GigienaStoreDbContext(DbContextOptions<GigienaStoreDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; } = null!;
    public virtual DbSet<HairCareProduct> HairCareProducts { get; set; } = null!;
    public virtual DbSet<NailsCareProduct> NailsCareProducts { get; set; } = null!;
    public virtual DbSet<OralCavityProduct> OralCavityProducts { get; set; } = null!;
    public virtual DbSet<Order> Orders { get; set; } = null!;
    public virtual DbSet<OrderProduct> OrderProducts { get; set; } = null!;
    public virtual DbSet<Product> Products { get; set; } = null!;
    public virtual DbSet<ProductInfo> ProductInfos { get; set; } = null!;
    public virtual DbSet<Shop> Shops { get; set; } = null!;
    public virtual DbSet<ShopProduct> ShopProducts { get; set; } = null!;
    public virtual DbSet<SkinCareProduct> SkinCareProducts { get; set; } = null!;
    public virtual DbSet<User> Users { get; set; } = null!;
    public virtual DbSet<Worker> Workers { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=GigienaStoreDb;Trusted_connection=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customer");

            entity.Property(e => e.CustomerId).ValueGeneratedNever();

            entity.Property(e => e.Address).HasMaxLength(50);

            entity.Property(e => e.City).HasMaxLength(20);

            entity.Property(e => e.Dob)
                .HasColumnType("datetime")
                .HasColumnName("DOB");

            entity.Property(e => e.Dor)
                .HasColumnType("datetime")
                .HasColumnName("DOR");

            entity.Property(e => e.MiddleName).HasMaxLength(20);

            entity.Property(e => e.Name).HasMaxLength(20);

            entity.Property(e => e.Phone).HasMaxLength(20);

            entity.Property(e => e.Sex)
                .HasMaxLength(2)
                .IsFixedLength();

            entity.Property(e => e.Surname).HasMaxLength(20);

            entity.Property(e => e.Zip).HasColumnName("ZIP");

            entity.HasOne(d => d.CustomerNavigation)
                .WithOne(p => p.Customer)
                .HasForeignKey<Customer>(d => d.CustomerId)
                .HasConstraintName("FK_Customer_User");
        });

        modelBuilder.Entity<HairCareProduct>(entity =>
        {
            entity.ToTable("HairCareProduct");

            entity.Property(e => e.HairCareProductId).ValueGeneratedNever();

            entity.Property(e => e.HairDisease).HasMaxLength(30);

            entity.Property(e => e.HairType).HasMaxLength(30);

            entity.Property(e => e.NotContains).HasMaxLength(50);

            entity.HasOne(d => d.HairCareProductNavigation)
                .WithOne(p => p.HairCareProduct)
                .HasForeignKey<HairCareProduct>(d => d.HairCareProductId)
                .HasConstraintName("FK_HairCareProduct_ProductInfo");
        });

        modelBuilder.Entity<NailsCareProduct>(entity =>
        {
            entity.ToTable("NailsCareProduct");

            entity.Property(e => e.NailsCareProductId).ValueGeneratedNever();

            entity.Property(e => e.Fragrance).HasMaxLength(30);

            entity.Property(e => e.NailsDisease).HasMaxLength(30);

            entity.Property(e => e.NailsType).HasMaxLength(30);

            entity.HasOne(d => d.NailsCareProductNavigation)
                .WithOne(p => p.NailsCareProduct)
                .HasForeignKey<NailsCareProduct>(d => d.NailsCareProductId)
                .HasConstraintName("FK_NailsCareProduct_ProductInfo");
        });

        modelBuilder.Entity<OralCavityProduct>(entity =>
        {
            entity.ToTable("OralCavityProduct");

            entity.Property(e => e.OralCavityProductId).ValueGeneratedNever();

            entity.Property(e => e.GumDiseaseType).HasMaxLength(50);

            entity.HasOne(d => d.OralCavityProductNavigation)
                .WithOne(p => p.OralCavityProduct)
                .HasForeignKey<OralCavityProduct>(d => d.OralCavityProductId)
                .HasConstraintName("FK_OralCavityProduct_ProductInfo");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order");

            entity.Property(e => e.OrderId).ValueGeneratedNever();

            entity.Property(e => e.PaymentStatus).HasMaxLength(10);

            entity.Property(e => e.PaymentType).HasMaxLength(15);

            entity.Property(e => e.ProcessedDate).HasColumnType("datetime");

            entity.Property(e => e.RecipientAddress)
                .HasMaxLength(50)
                .IsFixedLength();

            entity.Property(e => e.RecipientCity).HasMaxLength(15);

            entity.Property(e => e.RecipientName).HasMaxLength(15);

            entity.Property(e => e.RecipientPhone).HasMaxLength(20);

            entity.Property(e => e.RecipientSurname).HasMaxLength(15);

            entity.Property(e => e.Sum).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Customer)
                .WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Customer");

            entity.HasOne(d => d.Shop)
                .WithMany(p => p.Orders)
                .HasForeignKey(d => d.ShopId)
                .HasConstraintName("FK_Order_Shop");
        });

        modelBuilder.Entity<OrderProduct>(entity =>
        {
            entity.HasKey(e => new { e.ProductId, e.OrderId });

            entity.ToTable("OrderProduct");

            entity.HasOne(d => d.Order)
                .WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientCascade)
                .HasConstraintName("FK_OrderProduct_Order");

            entity.HasOne(d => d.Product)
                .WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientCascade)
                .HasConstraintName("FK_OrderProduct_Product");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.Property(e => e.ProductId).ValueGeneratedNever();

            entity.Property(e => e.Name);

            entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");

            entity.Property(e => e.ProducingCompany).HasMaxLength(25);

            entity.Property(e => e.ProducingCountry).HasMaxLength(20);

            entity.Property(e => e.ProducingDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<ProductInfo>(entity =>
        {
            entity.ToTable("ProductInfo");

            entity.Property(e => e.ProductInfoId).ValueGeneratedNever();

            entity.Property(e => e.Category).HasMaxLength(30);

            entity.Property(e => e.Gender).HasMaxLength(2);

            entity.HasOne(d => d.ProductInfoNavigation)
                .WithOne(p => p.ProductInfo)
                .HasForeignKey<ProductInfo>(d => d.ProductInfoId)
                .HasConstraintName("FK_ProductInfo_Product");
        });

        modelBuilder.Entity<Shop>(entity =>
        {
            entity.ToTable("Shop");

            entity.Property(e => e.ShopId).ValueGeneratedNever();

            entity.Property(e => e.Address).HasMaxLength(50);

            entity.Property(e => e.City).HasMaxLength(20);

            entity.Property(e => e.Name).HasMaxLength(50);

            entity.Property(e => e.Region).HasMaxLength(20);

            entity.Property(e => e.Size).HasMaxLength(10);
        });

        modelBuilder.Entity<ShopProduct>(entity =>
        {
            entity.HasKey(e => new { e.ProductId, e.ShopId });

            entity.ToTable("ShopProduct");

            entity.HasOne(d => d.Product)
                .WithMany(p => p.ShopProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientCascade)
                .HasConstraintName("FK_ShopProduct_Product");

            entity.HasOne(d => d.Shop)
                .WithMany(p => p.ShopProducts)
                .HasForeignKey(d => d.ShopId)
                .OnDelete(DeleteBehavior.ClientCascade)
                .HasConstraintName("FK_ShopProduct_Shop");
        });

        modelBuilder.Entity<SkinCareProduct>(entity =>
        {
            entity.ToTable("SkinCareProduct");

            entity.Property(e => e.SkinCareProductId).ValueGeneratedNever();

            entity.Property(e => e.SkinType).HasMaxLength(30);

            entity.Property(e => e.UsePurpose).HasMaxLength(30);

            entity.HasOne(d => d.SkinCareProductNavigation)
                .WithOne(p => p.SkinCareProduct)
                .HasForeignKey<SkinCareProduct>(d => d.SkinCareProductId)
                .HasConstraintName("FK_SkinCareProduct_ProductInfo");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.UserId).ValueGeneratedNever();

            entity.Property(e => e.Email).HasMaxLength(50);
        });

        modelBuilder.Entity<Worker>(entity =>
        {
            entity.ToTable("Worker");

            entity.Property(e => e.WorkerId).ValueGeneratedNever();

            entity.Property(e => e.Address).HasMaxLength(50);

            entity.Property(e => e.City).HasMaxLength(20);

            entity.Property(e => e.Dob)
                .HasColumnType("datetime")
                .HasColumnName("DOB");

            entity.Property(e => e.MiddleName).HasMaxLength(20);

            entity.Property(e => e.Name).HasMaxLength(20);

            entity.Property(e => e.Sex)
                .HasMaxLength(2)
                .IsFixedLength();

            entity.Property(e => e.Specialty).HasMaxLength(20);

            entity.Property(e => e.Surname).HasMaxLength(20);

            entity.HasOne(d => d.Shop)
                .WithMany(p => p.Workers)
                .HasForeignKey(d => d.ShopId)
                .HasConstraintName("FK_Worker_Shop");

            entity.HasOne(d => d.WorkerNavigation)
                .WithOne(p => p.Worker)
                .HasForeignKey<Worker>(d => d.WorkerId)
                .HasConstraintName("FK_Worker_User");
        });
    }
}