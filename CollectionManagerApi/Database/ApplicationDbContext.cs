using CollectionManagerApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tag = CollectionManagerApi.Models.Tag;

namespace CollectionManagerApi.Database;

public class ApplicationDbContext : IdentityDbContext<User>
{

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
            
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var properties = entityType.GetProperties()
                .Where(p => p.ClrType == typeof(DateTime) || p.ClrType == typeof(DateTime?));

            foreach (var property in properties)
            {
                property.SetValueConverter(new Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<DateTime, DateTime>(
                    v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(), // Ensure UTC when writing
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc)            // Ensure UTC when reading
                ));
            }
        }
        
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Token).HasMaxLength(200);
            entity.HasIndex(r => r.Token).IsUnique();
        });
        
        modelBuilder.Entity<RefreshToken>()
            .HasOne(e => e.User)
            .WithMany(e => e.RefreshTokens)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(r => r.Id);
        });
        
        modelBuilder.Entity<CollectionGroup>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Name).HasMaxLength(100);
            entity.Property(r => r.Name).IsRequired();
            entity.HasIndex(r => r.Name).IsUnique();
            
        });
        
        modelBuilder.Entity<CollectionGroup>()
            .HasOne(r => r.User)
            .WithMany(r => r.CollectionGroups)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        
        modelBuilder.Entity<Storage>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Name).HasMaxLength(100);
            entity.Property(r => r.Name).IsRequired();
            entity.HasIndex(r => r.Name).IsUnique();
            
        });
        
        modelBuilder.Entity<Storage>()
            .HasOne(r => r.User)
            .WithMany(r => r.Storages)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        modelBuilder.Entity<CollectionItem>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Name).HasMaxLength(150);
            entity.Property(r => r.Name).IsRequired();
            entity.Property(r => r.Comments).HasMaxLength(1000);
        });
        
        modelBuilder.Entity<CollectionItem>()
            .HasOne(r => r.User)
            .WithMany(r => r.CollectionItems)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<CollectionItem>()
            .HasOne(c => c.CollectionGroup)
            .WithMany(c => c.CollectionItems)
            .HasForeignKey(c => c.CollectionGroupId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<CollectionItem>()
            .HasOne(c => c.Storage)
            .WithMany(s => s.CollectionItems)
            .HasForeignKey(c => c.StorageId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<CollectionItem>()
            .HasMany(c => c.Tags)
            .WithMany(t => t.CollectionItems);
        
        modelBuilder.Entity<CustomAttribute>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Name).HasMaxLength(150);
            entity.Property(r => r.Name).IsRequired();
        });
        
        modelBuilder.Entity<CustomAttribute>()
            .HasOne(r => r.User)
            .WithMany(r => r.CustomAttributes)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        
        modelBuilder.Entity<CollectionItemCustomAttribute>(entity =>
        {
            entity.HasKey(r => r.Id);
        });

        modelBuilder.Entity<CollectionItemCustomAttribute>()
            .HasOne(r => r.User)
            .WithMany(r => r.CollectionItemCustomAttributes)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CollectionItemCustomAttribute>()
            .HasOne(c => c.CollectionItem)
            .WithMany(c => c.CollectionItemsCustomAttributes)
            .HasForeignKey(c => c.CollectionItemId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<CollectionItemCustomAttribute>()
            .HasOne(c => c.CustomAttribute)
            .WithMany(c => c.CollectionItemsCustomAttributes)
            .HasForeignKey(c => c.CustomAttributeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(r => r.Id);
        });
        modelBuilder.Entity<Tag>()
            .HasOne(r => r.User)
            .WithMany(r => r.Tags)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        
        modelBuilder.Entity<Tag>()
            .HasMany(t => t.CollectionItems)
            .WithMany(c => c.Tags);

        modelBuilder.Entity<CollectionItemTag>(entity =>
        {
            entity.HasKey(r => new {r.CollectionItemId , r.TagId});
        });

        modelBuilder.Entity<ItemDocument>(entity =>
        {
            entity.HasKey(r => r.Id);
        });
        
        modelBuilder.Entity<ItemDocument>()
            .HasOne(r => r.User)
            .WithMany(r => r.ItemDocuments)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ItemDocument>()
            .HasOne(r => r.CollectionItem)
            .WithMany(r => r.Documents)
            .HasForeignKey(r => r.CollectionItemId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<ItemImage>(entity =>
        {
            entity.HasKey(r => r.Id);
        });
        
        modelBuilder.Entity<ItemImage>()
            .HasOne(r => r.User)
            .WithMany(r => r.ItemImages)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ItemImage>()
            .HasOne(r => r.CollectionItem)
            .WithMany(r => r.Images)
            .HasForeignKey(r => r.CollectionItemId)
            .OnDelete(DeleteBehavior.Cascade);



    }
    
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<CollectionGroup> CollectionGroups { get; set; }
    public DbSet<Storage> Storages { get; set; }
    public DbSet<CollectionItem> CollectionItems { get; set; }
    public DbSet<CustomAttribute> CustomAttributes { get; set; }
    public DbSet<CollectionItemCustomAttribute> CollectionItemCustomAttributes { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<CollectionItemTag> CollectionItemTags { get; set; }
    public DbSet<ItemDocument> ItemDocuments { get; set; }
    public DbSet<ItemImage> ItemImages { get; set; }
    
    
}