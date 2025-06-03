using Microsoft.EntityFrameworkCore;
using FUNewsManagement.DataAccess.Models;

namespace FUNewsManagement.DataAccess
{
    public class FUNewsManagementDbContext : DbContext
    {
        public FUNewsManagementDbContext(DbContextOptions<FUNewsManagementDbContext> options) : base(options)
        {
        }

        public DbSet<SystemAccount> SystemAccounts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<NewsArticle> NewsArticles { get; set; }
        public DbSet<NewsTag> NewsTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure SystemAccount
            modelBuilder.Entity<SystemAccount>(entity =>
            {
                entity.HasKey(e => e.AccountId);
                entity.Property(e => e.AccountId).ValueGeneratedOnAdd();
                entity.HasIndex(e => e.AccountEmail).IsUnique();
            });

            // Configure Category
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategoryId);
                entity.Property(e => e.CategoryId).ValueGeneratedOnAdd();
                
                entity.HasOne(e => e.ParentCategory)
                    .WithMany(e => e.SubCategories)
                    .HasForeignKey(e => e.ParentCategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Tag
            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasKey(e => e.TagId);
                entity.Property(e => e.TagId).ValueGeneratedOnAdd();
            });

            // Configure NewsArticle
            modelBuilder.Entity<NewsArticle>(entity =>
            {
                entity.HasKey(e => e.NewsArticleId);
                
                entity.HasOne(e => e.Category)
                    .WithMany(e => e.NewsArticles)
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.CreatedBy)
                    .WithMany(e => e.CreatedArticles)
                    .HasForeignKey(e => e.CreatedById)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.UpdatedBy)
                    .WithMany(e => e.UpdatedArticles)
                    .HasForeignKey(e => e.UpdatedById)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure NewsTag (Many-to-Many)
            modelBuilder.Entity<NewsTag>(entity =>
            {
                entity.HasKey(e => new { e.NewsArticleId, e.TagId });
                
                entity.HasOne(e => e.NewsArticle)
                    .WithMany(e => e.NewsTags)
                    .HasForeignKey(e => e.NewsArticleId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Tag)
                    .WithMany(e => e.NewsTags)
                    .HasForeignKey(e => e.TagId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
