using System.ComponentModel.DataAnnotations;

namespace StudentNameMVC.Models
{
    public class ReportViewModel
    {
        [Required(ErrorMessage = "Start date is required")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; } = DateTime.Today.AddDays(-30);

        [Required(ErrorMessage = "End date is required")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; } = DateTime.Today;

        public Dictionary<DateTime, int> NewsStatistics { get; set; } = new Dictionary<DateTime, int>();
        public List<NewsArticleViewModel> NewsArticles { get; set; } = new List<NewsArticleViewModel>();
        public int TotalNews { get; set; }
    }
}
