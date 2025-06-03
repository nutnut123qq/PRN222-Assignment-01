using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FUNewsManagement.DataAccess.Models
{
    public class SystemAccount
    {
        [Key]
        public short AccountId { get; set; }

        [Required]
        [StringLength(100)]
        public string AccountName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string AccountEmail { get; set; } = string.Empty;

        [Required]
        public short AccountRole { get; set; } // 1=Staff, 2=Lecturer

        [Required]
        [StringLength(100)]
        public string AccountPassword { get; set; } = string.Empty;

        // Navigation properties
        public virtual ICollection<NewsArticle> CreatedArticles { get; set; } = new List<NewsArticle>();
        public virtual ICollection<NewsArticle> UpdatedArticles { get; set; } = new List<NewsArticle>();
    }
}
