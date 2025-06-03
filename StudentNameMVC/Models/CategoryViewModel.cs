using System.ComponentModel.DataAnnotations;

namespace StudentNameMVC.Models
{
    public class CategoryViewModel
    {
        public short CategoryId { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters")]
        public string CategoryName { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? CategoryDescription { get; set; }

        public short? ParentCategoryId { get; set; }

        public bool IsActive { get; set; } = true;

        public string? ParentCategoryName { get; set; }
        public bool CanDelete { get; set; } = true;
        public int NewsCount { get; set; }
        public int SubCategoryCount { get; set; }
    }
}
