using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ProductCatalogApi.DataAccess.Entities;

namespace ProductCatalogApi.DataAccess;

public interface IProductCatalogDbContext
{
    DbSet<Product> Products { get; set; }
    DbSet<ProductCategory> ProductCategories { get; set; }
}

public partial class ProductCatalogDbContext : DbContext, IProductCatalogDbContext
{
    public ProductCatalogDbContext()
    {
    }

    public ProductCatalogDbContext(DbContextOptions<ProductCatalogDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    //     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //         => optionsBuilder.UseSqlServer("Server=localhost;Database=apiDev;User Id=apideveloper;Password=SecureDev;Encrypt=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductCatalogApi.DataAccess.Entities.Product>(entity =>
        {
            entity.ToTable("Product", "dbo");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(250);
            entity.Property(e => e.ImageUrl).HasMaxLength(250);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IsActive).HasColumnName("Active").HasDefaultValue(true);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime2").HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime2");

            entity.HasOne(d => d.Category).WithMany().HasForeignKey(d => d.CategoryId);
        });

        modelBuilder.Entity<ProductCatalogApi.DataAccess.Entities.Product>()
            .HasOne(e => e.Category)
            .WithMany()
            .HasForeignKey(e => e.CategoryId)
            .HasPrincipalKey(e => e.Id);

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.ToTable("ProductCategory", "dbo");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(250);
            entity.Property(e => e.IsActive).HasColumnName("Active").HasDefaultValue(true);
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
