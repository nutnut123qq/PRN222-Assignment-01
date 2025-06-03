using System.ComponentModel.DataAnnotations;

namespace StudentNameMVC.Models
{
    public class NewsArticleViewModel
    {
        public string NewsArticleId { get; set; } = string.Empty;

        [Required(ErrorMessage = "News title is required")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string NewsTitle { get; set; } = string.Empty;

        [Required(ErrorMessage = "Headline is required")]
        [StringLength(300, ErrorMessage = "Headline cannot exceed 300 characters")]
        public string Headline { get; set; } = string.Empty;

        [Required(ErrorMessage = "Content is required")]
        public string NewsContent { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Source cannot exceed 100 characters")]
        public string? NewsSource { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public short CategoryId { get; set; }

        public bool NewsStatus { get; set; } = true;

        public DateTime CreatedDate { get; set; }
        public short CreatedById { get; set; }
        public short? UpdatedById { get; set; }
        public DateTime? ModifiedDate { get; set; }

        // Display properties
        public string? CategoryName { get; set; }
        public string? CreatedByName { get; set; }
        public string? UpdatedByName { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public List<int> TagIds { get; set; } = new List<int>();
        public string TagsString { get; set; } = string.Empty; // For form input

        public string StatusText => NewsStatus ? "Active" : "Inactive";
        public string CreatedDateFormatted => CreatedDate.ToString("dd/MM/yyyy HH:mm");
        public string ModifiedDateFormatted => ModifiedDate?.ToString("dd/MM/yyyy HH:mm") ?? "";
    }
}
