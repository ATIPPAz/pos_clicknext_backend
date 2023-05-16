using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PosApi.Models;

namespace PosApi.Context;

public partial class posContext : DbContext
{
    public posContext(DbContextOptions<posContext> options)
        : base(options)
    {
    }

    public virtual DbSet<etstsetst> etstsetsts { get; set; }

    public virtual DbSet<item> items { get; set; }

    public virtual DbSet<receipt> receipts { get; set; }

    public virtual DbSet<receiptdetail> receiptdetails { get; set; }

    public virtual DbSet<unit> units { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<etstsetst>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("etstsetst");

            entity.Property(e => e.itemCode).HasColumnType("text");
            entity.Property(e => e.itemName).HasColumnType("text");
        });

        modelBuilder.Entity<item>(entity =>
        {
            entity.HasKey(e => e.itemId).HasName("PRIMARY");

            entity.ToTable("item");

            entity.HasIndex(e => e.itemCode, "itemCode_UNIQUE").IsUnique();

            entity.HasIndex(e => e.unitId, "unitId_idx");

            entity.Property(e => e.itemCode).HasMaxLength(45);
            entity.Property(e => e.itemName).HasMaxLength(45);
            entity.Property(e => e.itemPrice).HasPrecision(10);

            entity.HasOne(d => d.unit).WithMany(p => p.items)
                .HasForeignKey(d => d.unitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("unitId");
        });

        modelBuilder.Entity<receipt>(entity =>
        {
            entity.HasKey(e => e.receiptId).HasName("PRIMARY");

            entity.ToTable("receipt");

            entity.HasIndex(e => e.receiptCode, "receiptCode_UNIQUE").IsUnique();

            entity.Property(e => e.receiptCode).HasMaxLength(45);
            entity.Property(e => e.receiptDate).HasColumnType("datetime");
            entity.Property(e => e.receiptGrandTotal).HasPrecision(10);
            entity.Property(e => e.receiptSubTotal).HasPrecision(10);
            entity.Property(e => e.receiptTotalBeforeDiscount).HasPrecision(10);
            entity.Property(e => e.receiptTotalDiscount).HasPrecision(10);
            entity.Property(e => e.receiptTradeDiscount).HasPrecision(10);
        });

        modelBuilder.Entity<receiptdetail>(entity =>
        {
            entity.HasKey(e => e.receiptDetailId).HasName("PRIMARY");

            entity.ToTable("receiptdetail");

            entity.HasIndex(e => e.itemId, "itemId_idx");

            entity.HasIndex(e => e.receiptId, "receiptId_idx");

            entity.HasIndex(e => e.unitId, "unitId_idx");

            entity.Property(e => e.itemAmount).HasPrecision(10);
            entity.Property(e => e.itemDiscount).HasPrecision(10);
            entity.Property(e => e.itemDiscountPercent).HasPrecision(10);
            entity.Property(e => e.itemPrice).HasPrecision(10);

            entity.HasOne(d => d.item).WithMany(p => p.receiptdetails)
                .HasForeignKey(d => d.itemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rd_itemId");

            entity.HasOne(d => d.receipt).WithMany(p => p.receiptdetails)
                .HasForeignKey(d => d.receiptId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("receiptId");

            entity.HasOne(d => d.unit).WithMany(p => p.receiptdetails)
                .HasForeignKey(d => d.unitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rd_unitId");
        });

        modelBuilder.Entity<unit>(entity =>
        {
            entity.HasKey(e => e.unitId).HasName("PRIMARY");

            entity.ToTable("unit");

            entity.Property(e => e.unitName).HasMaxLength(45);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
