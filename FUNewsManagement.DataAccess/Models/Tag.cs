using System.ComponentModel.DataAnnotations;

namespace FUNewsManagement.DataAccess.Models
{
    public class Tag
    {
        [Key]
        public int TagId { get; set; }
        
        [Required]
        [StringLength(50)]
        public string TagName { get; set; } = string.Empty;
        
        [StringLength(200)]
        public string? Note { get; set; }
        
        // Navigation properties
        public virtual ICollection<NewsTag> NewsTags { get; set; } = new List<NewsTag>();
    }
}
