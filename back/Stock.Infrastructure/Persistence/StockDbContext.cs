using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Stock.Domain.Entities;
using Stock.Domain.Entities.Common;
using Stock.Domain.Entities.Views;

namespace Stock.Infrastructure.Persistence;

public partial class StockDbContext : DbContext
{
    public StockDbContext(DbContextOptions<StockDbContext> options)
        : base(options) { }

    public virtual DbSet<Box> Boxes { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<Storage> Storages { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Brand> Brands { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.BrandId);

            // Properties
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(64);
            entity.Property(e => e.Color)
                .IsRequired()
                .HasMaxLength(8);
            entity.Property(e => e.Background)
                .IsRequired()
                .HasMaxLength(8);

            // Unique index on Name
            entity.HasIndex(e => e.Name)
                .IsUnique()
                .HasDatabaseName("UQ_Brand_Name")
                .HasFilter(null);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId);

            // Properties
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(64);
            entity.Property(e => e.Icon)
                .IsRequired()
                .HasMaxLength(32);
            entity.Property(e => e.Color)
                .IsRequired()
                .HasMaxLength(8);

            // Unique index on Name
            entity.HasIndex(e => e.Name)
                .IsUnique()
                .HasDatabaseName("UQ_Category_Name")
                .HasFilter(null);
        });

        modelBuilder.Entity<Box>(entity =>
        {
            entity.HasKey(e => e.BoxId);

            // Properties
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(64);
            entity.Property(p => p.Height)
                .IsRequired()
                .HasColumnType("decimal(5, 2)");
            entity.Property(p => p.Width)
                .IsRequired()
                .HasColumnType("decimal(5, 2)");
            entity.Property(p => p.Depth)
                .IsRequired()
                .HasColumnType("decimal(5, 2)");
            entity.Property(p => p.Volume)
                .IsRequired()
                .HasColumnType("decimal(15, 2)")
                .HasComputedColumnSql("ISNULL(CAST(([Height] * [Width] * [Depth]) AS decimal(15, 2)), 0.0)", stored: true);
            entity.Property(e => e.Notes)
                .IsRequired()
                .HasMaxLength(512);

            // Self-referencing relationship for parent box
            entity.HasOne(d => d.ParentBox)
                .WithMany(p => p.SubBoxes)
                .HasForeignKey(d => d.ParentBoxId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Box_ParentBoxId");
            // Unique index on Name within the same ParentBox
            entity.HasIndex(e => new { e.ParentBoxId, e.Name })
                .IsUnique()
                .HasDatabaseName("UQ_Box_Name")
                .HasFilter(null);
            // Relationships
            entity.HasOne(d => d.Category).WithMany(p => p.Boxes)
               .HasForeignKey(s => s.CategoryId)
               .OnDelete(DeleteBehavior.Restrict)
               .HasConstraintName("FK_Box_CategoryId");
            entity.HasOne(d => d.Brand).WithMany(p => p.Boxes)
               .HasForeignKey(s => s.BrandId)
               .OnDelete(DeleteBehavior.Restrict)
               .HasConstraintName("FK_Box_BrandId");

            // Check constraints to ensure dimensions are positive
            entity.ToTable(t => t.HasCheckConstraint("CK_Box_Depth", "[Depth] > 0"));
            entity.ToTable(t => t.HasCheckConstraint("CK_Box_Height", "[Height] > 0"));
            entity.ToTable(t => t.HasCheckConstraint("CK_Box_Width", "[Width] > 0"));
        });
        
        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.ItemId);

            // Properties
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(64);
            entity.Property(e => e.Notes)
                .IsRequired()
                .HasMaxLength(512);

            // Unique index on Name
            entity.HasIndex(e => e.Name)
                .IsUnique()
                .HasDatabaseName("UQ_Item_Name")
                .HasFilter(null);

            // Relationships
            entity.HasOne(d => d.Category).WithMany(p => p.Items)
               .HasForeignKey(s => s.CategoryId)
               .OnDelete(DeleteBehavior.Restrict)
               .HasConstraintName("FK_Item_CategoryId");            
        });

        modelBuilder.Entity<Storage>(entity =>
        {
            entity.HasKey(e => e.StorageId);

            entity.HasOne(d => d.Box).WithMany(p => p.Storages)
                .HasForeignKey(s => s.BoxId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Storage_BoxId");

            entity.HasOne(d => d.Item).WithMany(p => p.Storages)
                .HasForeignKey(s => s.ItemId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Storage_ItemId");

            entity.HasOne(d => d.Brand).WithMany(p => p.Storages)
                .HasForeignKey(s => s.BrandId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Storage_BrandId");

            entity.HasIndex(e => new { e.BoxId, e.ItemId, e.BrandId })
                .IsUnique()
                .HasDatabaseName("UQ_Storage_Box_Item_Brand")
                .HasFilter(null);

            entity.ToTable(t => t.HasCheckConstraint("CK_Storage_Quantity", "[Quantity] > 0"));
        });

        // Views
        modelBuilder.Entity<BoxDetailed>(entity =>
        {
            entity.HasNoKey();
            entity.ToTable(nameof(BoxDetailed), t => t.ExcludeFromMigrations());
            entity.Property(p => p.Height).HasColumnType("decimal(5, 2)");
            entity.Property(p => p.Width).HasColumnType("decimal(5, 2)");
            entity.Property(p => p.Depth).HasColumnType("decimal(5, 2)");
            entity.Property(p => p.Volume).HasColumnType("decimal(15, 2)");
        });

        // Apply common configurations for all entities that inherit from AuditableEntity
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (entityType.IsKeyless) continue;

            entityType.SetTableName(entityType.ClrType.Name);

            if (typeof(AuditableEntity).IsAssignableFrom(entityType.ClrType))
            {
                var tableName = entityType.GetTableName();

                modelBuilder.Entity(entityType.ClrType)
                    .Property(nameof(AuditableEntity.CreatedAt))
                    .HasColumnType("datetimeoffset")
                    .IsRequired()
                    .HasDefaultValueSql("SYSDATETIMEOFFSET()", $"DF_{tableName}_CreatedAt")
                    .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                modelBuilder.Entity(entityType.ClrType)
                    .Property(nameof(AuditableEntity.UpdatedAt))
                    .HasColumnType("datetimeoffset")
                    .IsRequired()
                    .HasDefaultValueSql("SYSDATETIMEOFFSET()", $"DF_{tableName}_UpdatedAt");
            }
        }        

        OnModelCreatingPartial(modelBuilder);
    }

    public override int SaveChanges()
    {
        UpdateAuditFields();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateAuditFields()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is AuditableEntity &&
                        (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var entity = (AuditableEntity)entry.Entity;
            var now = DateTime.UtcNow;

            if (entry.State == EntityState.Added)
            {
                entity.CreatedAt = now;
            }
            else
            {
                entry.Property(nameof(AuditableEntity.CreatedAt)).IsModified = false;
            }

            entity.UpdatedAt = now;
        }
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
