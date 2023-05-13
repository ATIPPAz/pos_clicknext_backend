using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PosApi.Models;

namespace PosApi.Context;

public partial class posContext : DbContext
{

  
    public posContext()
    {
    }

    public posContext(DbContextOptions<posContext> options)
        : base(options)
    {
    }

    public virtual DbSet<item> items { get; set; }

    public virtual DbSet<receipt> receipts { get; set; }

    public virtual DbSet<receiptdetail> receiptdetails { get; set; }

    public virtual DbSet<unit> units { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("Server=0.tcp.ap.ngrok.io;Port=16110;Database=pos;User Id=root;Password=Pan28060.", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.33-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

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
