using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Stock.Application.Common;
using Stock.Application.Interfaces.Common;
using Stock.Domain.Entities;
using Stock.Domain.Entities.Views;
using Stock.Infrastructure.Services;
using static Stock.Foundation.Common.SystemRegistry;

namespace Stock.Infrastructure.Persistence;

public partial class StockDbContext(DbContextOptions<StockDbContext> options, IAuditFactory auditService) : DbContext(options)
{
    public virtual DbSet<Box> Boxes { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<Storage> Storages { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<Label> Labels { get; set; }

    public virtual DbSet<Translation> Translations { get; set; }

    public virtual DbSet<Audit> Audits { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Language>(entity =>
        {
            entity.HasKey(e => e.LanguageCode);

            // Properties
            entity.Property(e => e.LanguageCode)
                .HasMaxLength(8);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(64);

            entity.Property(e => e.IsDefault)
                .HasDefaultValue(false, "DF_Language_IsDefault");

            // Unique index to ensure only one default language
            entity.HasIndex(e => e.IsDefault)
                .IsUnique()
                .HasFilter("[IsDefault] = 1")
                .HasDatabaseName("UQ_Language_SingleDefault");
        });

        modelBuilder.Entity<Label>(entity =>
        {
            entity.HasKey(e => e.LabelId);

            // Properties
            entity.Property(e => e.Context)
                .IsRequired()
                .HasMaxLength(32);

            entity.Property(e => e.LabelKey)
                .IsRequired()
                .HasMaxLength(128);

            // Unique index on Context + LabelKey
            entity.HasIndex(e => new { e.Context, e.LabelKey })
                .IsUnique()
                .HasDatabaseName("UQ_Label_Context_Key");
        });

        modelBuilder.Entity<Translation>(entity =>
        {
            entity.HasKey(e => e.TranslationId);

            // Properties
            entity.Property(e => e.Text)
                .IsRequired();

            entity.Property(e => e.LanguageCode)
                .IsRequired()
                .HasMaxLength(8);

            // Relationships
            entity.HasOne(d => d.Label)
                .WithMany(p => p.Translations)
                .HasForeignKey(d => d.LabelId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Translation_LabelId");

            entity.HasOne(d => d.Language)
                .WithMany(p => p.Translations)
                .HasForeignKey(d => d.LanguageCode)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Translation_LanguageCode");

            // Unique index to ensure one translation per Label + Language combination
            entity.HasIndex(e => new { e.LabelId, e.LanguageCode })
                .IsUnique()
                .HasDatabaseName("UQ_Translation_Label_Language");
        });

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
            entity.Property(e => e.ImageAt)
                .IsRequired();

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
            entity.Property(e => e.ImageAt)
                .IsRequired();

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

            entity.Property(e => e.Notes)
                .HasMaxLength(512);

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

        modelBuilder.Entity<Audit>(entity =>
        {
            entity.HasKey(e => e.AuditId);

            // Properties
            entity.Property(e => e.AuditId)
                .ValueGeneratedOnAdd();

            entity.Property(e => e.EntityId)
                .IsRequired()
                .HasConversion<byte>();

            entity.Property(e => e.EventId)
                .IsRequired()
                .HasConversion<byte>();

            entity.Property(e => e.RecordId)
                .IsRequired()
                .HasMaxLength(128);

            entity.Property(e => e.By)
                .IsRequired()
                .HasMaxLength(128);

            entity.Property(e => e.At)
                .IsRequired();

            entity.Property(e => e.Context)
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            // Indexes
            entity.HasIndex(e => new { e.EntityId, e.RecordId })
                .HasDatabaseName("IX_Audit_Entity_Record");

            entity.HasIndex(e => new { e.By, e.At })
                .HasDatabaseName("IX_Audit_By_At");
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

        // Apply common configurations for all entities
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (entityType.IsKeyless) continue;

            entityType.SetTableName(entityType.ClrType.Name);

            foreach (var property in entityType.GetProperties())
            {
                if (property.Name.EndsWith("At"))
                {
                    property.SetColumnType("datetimeoffset");
                }

                if (property.IsPrimaryKey())
                {
                    if (property.ClrType == typeof(int) || property.ClrType == typeof(long))
                    {
                        property.ValueGenerated = ValueGenerated.OnAdd;
                    }
                }
            }
        }

        OnModelCreatingPartial(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var auditList = CaptureAudits();

        var result = await base.SaveChangesAsync(cancellationToken);

        await EnforceAuditTrail(auditList);

        return result;
    }

    private List<(AuditRequest Request, EntityEntry Entry)> CaptureAudits()
    {
        var auditPairs = new List<(AuditRequest Request, EntityEntry Entry)>();

        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is Audit || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                continue;

            var request = new AuditRequest
            {
                EntityId = Enum.Parse<Entity>(entry.Metadata.ClrType.Name),
                EventId = entry.State switch
                {
                    EntityState.Added => Event.Create,
                    EntityState.Deleted => Event.Delete,
                    EntityState.Modified => Event.Update,
                    _ => Event.Read
                }
            };

            foreach (var property in entry.Properties)
            {
                if (property.IsTemporary) continue;
                string propertyName = property.Metadata.Name;

                switch (entry.State)
                {
                    case EntityState.Added:
                        request.NewValues[propertyName] = property.CurrentValue;
                        break;
                    case EntityState.Deleted:
                        request.OldValues[propertyName] = property.OriginalValue;
                        break;
                    case EntityState.Modified:
                        if (property.IsModified)
                        {
                            var original = property.OriginalValue;
                            var current = property.CurrentValue;
                            if (Equals(original, current))
                            {
                                property.IsModified = false;
                                continue;
                            }
                            request.OldValues[propertyName] = original;
                            request.NewValues[propertyName] = current;
                        }
                        break;
                }
            }            
            auditPairs.Add((request, entry));
        }
        return auditPairs;
    }

    private async Task EnforceAuditTrail(List<(AuditRequest Request, EntityEntry Entry)> auditPairs)
    {
        if (auditPairs == null || auditPairs.Count == 0) return;

        foreach (var (request, entry) in auditPairs)
        {
            var primaryKey = entry.Metadata.FindPrimaryKey();
            if (primaryKey != null)
            {
                var keyName = primaryKey.Properties[0].Name;
                var currentId = entry.Property(keyName).CurrentValue;

                request.RecordId = currentId?.ToString() ?? "-";

                if (request.EventId == Event.Create)
                {                    
                    if (currentId is int || currentId is long)
                        request.NewValues[keyName] = currentId;
                    else
                        request.NewValues[keyName] = currentId?.ToString();
                }
            }
        }
        
        var requests = auditPairs.Select(p => p.Request);
        var auditEntities = auditService.Create(requests);

        Audits.AddRange(auditEntities);
        await base.SaveChangesAsync();
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
