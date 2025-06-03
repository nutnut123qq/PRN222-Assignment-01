using System.ComponentModel.DataAnnotations;

namespace FUNewsManagement.DataAccess.Models
{
    public class Category
    {
        [Key]
        public short CategoryId { get; set; }
        
        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? CategoryDescription { get; set; }
        
        public short? ParentCategoryId { get; set; }
        
        [Required]
        public bool IsActive { get; set; } = true;
        
        // Navigation properties
        public virtual Category? ParentCategory { get; set; }
        public virtual ICollection<Category> SubCategories { get; set; } = new List<Category>();
        public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
    }
}
