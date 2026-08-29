using Amazon.S3.Model;
using Collection_Manager_Api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tag = Collection_Manager_Api.Models.Tag;

namespace Collection_Manager_Api.Database;

public class ApplicationDbContext : IdentityDbContext<User>
{

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
            
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(r => r.Id);
        });
        
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Name).HasMaxLength(100);
            entity.Property(r => r.Name).IsRequired();
            entity.HasIndex(r => r.Name).IsUnique();
            
        });
        
        modelBuilder.Entity<Storage>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Name).HasMaxLength(100);
            entity.Property(r => r.Name).IsRequired();
            entity.HasIndex(r => r.Name).IsUnique();
            
        });

        modelBuilder.Entity<CollectionItem>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Name).HasMaxLength(150);
            entity.Property(r => r.Name).IsRequired();
            entity.Property(r => r.Comments).HasMaxLength(1000);
        });
        
        modelBuilder.Entity<CollectionItem>()
            .HasOne(c => c.Category)
            .WithMany(c => c.CollectionItems)
            .HasForeignKey(c => c.CategoryId);
        modelBuilder.Entity<CollectionItem>()
            .HasOne(c => c.Storage)
            .WithMany(s => s.CollectionItems)
            .HasForeignKey(c => c.StorageId);
        
        modelBuilder.Entity<CollectionItem>()
            .HasMany(c => c.Tags)
            .WithMany(t => t.CollectionItems);
        
        modelBuilder.Entity<CustomAttribute>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Name).HasMaxLength(150);
            entity.Property(r => r.Name).IsRequired();
        });
        
        modelBuilder.Entity<CollectionItemCustomAttribute>(entity =>
        {
            entity.HasKey(r => r.Id);
        });

        modelBuilder.Entity<CollectionItemCustomAttribute>()
            .HasOne(c => c.CollectionItem)
            .WithMany(c => c.CollectionItemsCustomAttributes)
            .HasForeignKey(c => c.CollectionItemId);
        modelBuilder.Entity<CollectionItemCustomAttribute>()
            .HasOne(c => c.CustomAttribute)
            .WithMany(c => c.CollectionItemsCustomAttributes)
            .HasForeignKey(c => c.CustomAttributeId);

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(r => r.Id);
        });
        modelBuilder.Entity<Tag>()
            .HasMany(t => t.CollectionItems)
            .WithMany(c => c.Tags);

        modelBuilder.Entity<CollectionItemTag>(entity =>
        {
            entity.HasKey(r => r.CollectionItemId);
            entity.HasKey(c => c.TagId);
            entity.HasIndex(c => c.TagId).IsUnique();
            entity.HasIndex(c => c.CollectionItemId).IsUnique();
        });

        modelBuilder.Entity<ItemDocument>(entity =>
        {
            entity.HasKey(r => r.Id);
        });

        modelBuilder.Entity<ItemDocument>()
            .HasOne(r => r.CollectionItem)
            .WithMany(r => r.Documents)
            .HasForeignKey(r => r.CollectionItemId);
        
        modelBuilder.Entity<ItemImage>(entity =>
        {
            entity.HasKey(r => r.Id);
        });

        modelBuilder.Entity<ItemImage>()
            .HasOne(r => r.CollectionItem)
            .WithMany(r => r.Images)
            .HasForeignKey(r => r.CollectionItemId);



    }
    
    public DbSet<Category> Categories { get; set; }
    public DbSet<Storage> Storages { get; set; }
    public DbSet<CollectionItem> CollectionItems { get; set; }
    public DbSet<CustomAttribute> CustomAttributes { get; set; }
    public DbSet<CollectionItemCustomAttribute> CollectionItemCustomAttributes { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<CollectionItemTag> CollectionItemTags { get; set; }
    public DbSet<ItemDocument> ItemDocuments { get; set; }
    public DbSet<ItemImage> ItemImages { get; set; }
    
    
}