using FUNewsManagement.DataAccess;
using FUNewsManagement.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace StudentNameMVC.Services
{
    public class DatabaseSeeder
    {
        private readonly FUNewsManagementDbContext _context;

        public DatabaseSeeder(FUNewsManagementDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            // Ensure database is created
            await _context.Database.EnsureCreatedAsync();

            // Check if data already exists
            if (await _context.SystemAccounts.AnyAsync())
            {
                return; // Database has been seeded
            }

            // Seed Categories
            var categories = new List<Category>
            {
                new Category { CategoryName = "Technology", CategoryDescription = "Technology news and updates", IsActive = true },
                new Category { CategoryName = "Education", CategoryDescription = "Educational news and announcements", IsActive = true },
                new Category { CategoryName = "Sports", CategoryDescription = "Sports news and events", IsActive = true },
                new Category { CategoryName = "Entertainment", CategoryDescription = "Entertainment and cultural news", IsActive = true }
            };

            await _context.Categories.AddRangeAsync(categories);
            await _context.SaveChangesAsync();

            // Seed System Accounts
            var accounts = new List<SystemAccount>
            {
                new SystemAccount 
                { 
                    AccountName = "John Staff", 
                    AccountEmail = "staff@funews.com", 
                    AccountRole = 1, // Staff
                    AccountPassword = "staff123" 
                },
                new SystemAccount 
                { 
                    AccountName = "Jane Lecturer", 
                    AccountEmail = "lecturer@funews.com", 
                    AccountRole = 2, // Lecturer
                    AccountPassword = "lecturer123" 
                }
            };

            await _context.SystemAccounts.AddRangeAsync(accounts);
            await _context.SaveChangesAsync();

            // Seed Tags
            var tags = new List<Tag>
            {
                new Tag { TagName = "Breaking", Note = "Breaking news tag" },
                new Tag { TagName = "Important", Note = "Important announcements" },
                new Tag { TagName = "Event", Note = "Event related news" },
                new Tag { TagName = "Update", Note = "Updates and changes" }
            };

            await _context.Tags.AddRangeAsync(tags);
            await _context.SaveChangesAsync();

            // Seed News Articles
            var staffAccount = accounts.First(a => a.AccountRole == 1);
            var techCategory = categories.First(c => c.CategoryName == "Technology");
            var eduCategory = categories.First(c => c.CategoryName == "Education");

            var newsArticles = new List<NewsArticle>
            {
                new NewsArticle
                {
                    NewsArticleId = "NEWS20250603001",
                    NewsTitle = "New Learning Management System Launched",
                    Headline = "University introduces advanced LMS for better online learning experience",
                    NewsContent = "The university has successfully launched a new Learning Management System (LMS) that provides enhanced features for both students and faculty. The system includes improved user interface, better communication tools, and advanced analytics for tracking student progress.",
                    NewsSource = "University IT Department",
                    CategoryId = eduCategory.CategoryId,
                    NewsStatus = true,
                    CreatedById = staffAccount.AccountId,
                    CreatedDate = DateTime.Now.AddDays(-5)
                },
                new NewsArticle
                {
                    NewsArticleId = "NEWS20250603002",
                    NewsTitle = "Campus WiFi Infrastructure Upgrade",
                    Headline = "High-speed internet connectivity now available across all campus buildings",
                    NewsContent = "The university has completed a major upgrade to its WiFi infrastructure, providing faster and more reliable internet access to all students and staff. The new system supports the latest WiFi 6 technology and covers 100% of the campus area.",
                    NewsSource = "Campus Network Team",
                    CategoryId = techCategory.CategoryId,
                    NewsStatus = true,
                    CreatedById = staffAccount.AccountId,
                    CreatedDate = DateTime.Now.AddDays(-3)
                },
                new NewsArticle
                {
                    NewsArticleId = "NEWS20250603003",
                    NewsTitle = "Student Registration System Maintenance",
                    Headline = "Scheduled maintenance for student registration system this weekend",
                    NewsContent = "The student registration system will undergo scheduled maintenance this weekend from Saturday 6 PM to Sunday 6 AM. During this time, students will not be able to access course registration features. All other services will remain available.",
                    NewsSource = "Student Services",
                    CategoryId = eduCategory.CategoryId,
                    NewsStatus = true,
                    CreatedById = staffAccount.AccountId,
                    CreatedDate = DateTime.Now.AddDays(-1)
                }
            };

            await _context.NewsArticles.AddRangeAsync(newsArticles);
            await _context.SaveChangesAsync();

            // Seed News Tags
            var breakingTag = tags.First(t => t.TagName == "Breaking");
            var importantTag = tags.First(t => t.TagName == "Important");
            var updateTag = tags.First(t => t.TagName == "Update");

            var newsTags = new List<NewsTag>
            {
                new NewsTag { NewsArticleId = newsArticles[0].NewsArticleId, TagId = importantTag.TagId },
                new NewsTag { NewsArticleId = newsArticles[0].NewsArticleId, TagId = updateTag.TagId },
                new NewsTag { NewsArticleId = newsArticles[1].NewsArticleId, TagId = breakingTag.TagId },
                new NewsTag { NewsArticleId = newsArticles[1].NewsArticleId, TagId = updateTag.TagId },
                new NewsTag { NewsArticleId = newsArticles[2].NewsArticleId, TagId = importantTag.TagId }
            };

            await _context.NewsTags.AddRangeAsync(newsTags);
            await _context.SaveChangesAsync();
        }
    }
}
