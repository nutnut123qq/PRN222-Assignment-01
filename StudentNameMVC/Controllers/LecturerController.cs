using Microsoft.AspNetCore.Mvc;
using FUNewsManagement.Services;
using StudentNameMVC.Models;

namespace StudentNameMVC.Controllers
{
    public class LecturerController : LecturerBaseController
    {
        private readonly INewsArticleService _newsService;
        private readonly ICategoryService _categoryService;

        public LecturerController(INewsArticleService newsService, ICategoryService categoryService)
        {
            _newsService = newsService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(string? search, short? categoryId)
        {
            var activeNews = await _newsService.GetActiveNewsAsync();
            
            if (!string.IsNullOrEmpty(search))
            {
                activeNews = await _newsService.SearchNewsAsync(search);
                activeNews = activeNews.Where(n => n.NewsStatus); // Ensure only active news
            }
            
            if (categoryId.HasValue)
            {
                activeNews = activeNews.Where(n => n.CategoryId == categoryId.Value);
            }

            var newsViewModels = activeNews.Select(n => new NewsArticleViewModel
            {
                NewsArticleId = n.NewsArticleId,
                NewsTitle = n.NewsTitle,
                Headline = n.Headline,
                NewsContent = n.NewsContent,
                NewsSource = n.NewsSource,
                CategoryId = n.CategoryId,
                CategoryName = n.Category?.CategoryName,
                CreatedDate = n.CreatedDate,
                CreatedByName = n.CreatedBy?.AccountName,
                Tags = n.NewsTags?.Select(nt => nt.Tag.TagName).ToList() ?? new List<string>()
            }).ToList();

            ViewBag.Categories = await _categoryService.GetActiveCategoriesAsync();
            ViewBag.Search = search;
            ViewBag.CategoryId = categoryId;

            return View(newsViewModels);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var news = await _newsService.GetNewsWithDetailsAsync(id);
            if (news == null || !news.NewsStatus)
            {
                return NotFound();
            }

            var viewModel = new NewsArticleViewModel
            {
                NewsArticleId = news.NewsArticleId,
                NewsTitle = news.NewsTitle,
                Headline = news.Headline,
                NewsContent = news.NewsContent,
                NewsSource = news.NewsSource,
                CategoryId = news.CategoryId,
                CategoryName = news.Category?.CategoryName,
                CreatedDate = news.CreatedDate,
                CreatedByName = news.CreatedBy?.AccountName,
                Tags = news.NewsTags?.Select(nt => nt.Tag.TagName).ToList() ?? new List<string>()
            };

            return View(viewModel);
        }
    }
}
