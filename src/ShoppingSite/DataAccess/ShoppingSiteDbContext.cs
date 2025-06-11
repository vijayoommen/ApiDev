using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ShoppingSite.DataAccess;

public interface IShoppingSiteDbContext
{
    DbSet<Cart> Carts { get; set; }
    DbSet<CartItem> CartItems { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

public partial class ShoppingSiteDbContext : DbContext, IShoppingSiteDbContext
{
    public ShoppingSiteDbContext() { }

    public ShoppingSiteDbContext(DbContextOptions<ShoppingSiteDbContext> options)
        : base(options) { }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.ToTable("Cart");

            entity.HasIndex(e => e.CustomerId, "IX_Cart_CustomerId");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.CustomerId).IsRequired(false);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OrderId).IsRequired(false);
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UpdatedOn).HasDefaultValueSql("(getdate())");

            entity.HasMany(e => e.CartItems)
                .WithOne()
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.ToTable("CartItem");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.CartId).IsRequired();
            entity.Property(e => e.ProductId).IsRequired();
            entity.Property(e => e.Quantity).IsRequired();
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedOn).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
