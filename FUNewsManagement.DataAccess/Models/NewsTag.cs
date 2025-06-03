using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FUNewsManagement.DataAccess.Models
{
    public class NewsTag
    {
        [Required]
        public string NewsArticleId { get; set; } = string.Empty;
        
        [Required]
        public int TagId { get; set; }
        
        // Navigation properties
        [ForeignKey("NewsArticleId")]
        public virtual NewsArticle NewsArticle { get; set; } = null!;
        
        [ForeignKey("TagId")]
        public virtual Tag Tag { get; set; } = null!;
    }
}
