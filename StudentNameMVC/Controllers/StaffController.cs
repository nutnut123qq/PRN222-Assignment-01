using Microsoft.AspNetCore.Mvc;
using FUNewsManagement.Services;
using FUNewsManagement.DataAccess.Models;
using StudentNameMVC.Models;

namespace StudentNameMVC.Controllers
{
    public class StaffController : StaffBaseController
    {
        private readonly ICategoryService _categoryService;
        private readonly INewsArticleService _newsService;
        private readonly ITagService _tagService;
        private readonly ISystemAccountService _accountService;

        public StaffController(ICategoryService categoryService, INewsArticleService newsService, 
            ITagService tagService, ISystemAccountService accountService)
        {
            _categoryService = categoryService;
            _newsService = newsService;
            _tagService = tagService;
            _accountService = accountService;
        }

        public async Task<IActionResult> Index()
        {
            var news = await _newsService.GetNewsByAuthorAsync(CurrentUser!.AccountId);
            var newsViewModels = news.Select(n => new NewsArticleViewModel
            {
                NewsArticleId = n.NewsArticleId,
                NewsTitle = n.NewsTitle,
                Headline = n.Headline,
                CategoryName = n.Category?.CategoryName,
                CreatedDate = n.CreatedDate,
                NewsStatus = n.NewsStatus,
                Tags = n.NewsTags?.Select(nt => nt.Tag.TagName).ToList() ?? new List<string>()
            }).ToList();

            return View(newsViewModels);
        }

        // Category Management
        [HttpGet]
        public async Task<IActionResult> ManageCategories(string? search)
        {
            IEnumerable<Category> categories;
            
            if (!string.IsNullOrEmpty(search))
            {
                categories = await _categoryService.SearchCategoriesAsync(search);
            }
            else
            {
                categories = await _categoryService.GetAllCategoriesAsync();
            }

            var categoryViewModels = new List<CategoryViewModel>();
            foreach (var category in categories)
            {
                var canDelete = await _categoryService.CanDeleteCategoryAsync(category.CategoryId);
                categoryViewModels.Add(new CategoryViewModel
                {
                    CategoryId = category.CategoryId,
                    CategoryName = category.CategoryName,
                    CategoryDescription = category.CategoryDescription,
                    ParentCategoryId = category.ParentCategoryId,
                    IsActive = category.IsActive,
                    CanDelete = canDelete
                });
            }

            ViewBag.Search = search;
            ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
            return View(categoryViewModels);
        }

        [HttpGet]
        public async Task<IActionResult> CreateCategory()
        {
            ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
            return PartialView("_CreateCategoryModal", new CategoryViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
                return PartialView("_CreateCategoryModal", model);
            }

            try
            {
                var category = new Category
                {
                    CategoryName = model.CategoryName,
                    CategoryDescription = model.CategoryDescription,
                    ParentCategoryId = model.ParentCategoryId,
                    IsActive = model.IsActive
                };

                await _categoryService.CreateCategoryAsync(category);
                return Json(new { success = true, message = "Category created successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditCategory(short id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var model = new CategoryViewModel
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                CategoryDescription = category.CategoryDescription,
                ParentCategoryId = category.ParentCategoryId,
                IsActive = category.IsActive
            };

            ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
            return PartialView("_EditCategoryModal", model);
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory(CategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
                return PartialView("_EditCategoryModal", model);
            }

            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(model.CategoryId);
                if (category == null)
                {
                    return Json(new { success = false, message = "Category not found." });
                }

                category.CategoryName = model.CategoryName;
                category.CategoryDescription = model.CategoryDescription;
                category.ParentCategoryId = model.ParentCategoryId;
                category.IsActive = model.IsActive;

                await _categoryService.UpdateCategoryAsync(category);
                return Json(new { success = true, message = "Category updated successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCategory(short id)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(id);
                return Json(new { success = true, message = "Category deleted successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // News Management
        [HttpGet]
        public async Task<IActionResult> ManageNews(string? search)
        {
            IEnumerable<NewsArticle> news;
            
            if (!string.IsNullOrEmpty(search))
            {
                var searchResults = await _newsService.SearchNewsAsync(search);
                news = searchResults.Where(n => n.CreatedById == CurrentUser!.AccountId);
            }
            else
            {
                news = await _newsService.GetNewsByAuthorAsync(CurrentUser!.AccountId);
            }

            var newsViewModels = news.Select(n => new NewsArticleViewModel
            {
                NewsArticleId = n.NewsArticleId,
                NewsTitle = n.NewsTitle,
                Headline = n.Headline,
                CategoryName = n.Category?.CategoryName,
                CreatedDate = n.CreatedDate,
                NewsStatus = n.NewsStatus,
                Tags = n.NewsTags?.Select(nt => nt.Tag.TagName).ToList() ?? new List<string>()
            }).ToList();

            ViewBag.Search = search;
            return View(newsViewModels);
        }

        [HttpGet]
        public async Task<IActionResult> CreateNews()
        {
            ViewBag.Categories = await _categoryService.GetActiveCategoriesAsync();
            ViewBag.Tags = await _tagService.GetAllTagsAsync();
            return PartialView("_CreateNewsModal", new NewsArticleViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateNews(NewsArticleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryService.GetActiveCategoriesAsync();
                ViewBag.Tags = await _tagService.GetAllTagsAsync();
                return PartialView("_CreateNewsModal", model);
            }

            try
            {
                var news = new NewsArticle
                {
                    NewsTitle = model.NewsTitle,
                    Headline = model.Headline,
                    NewsContent = model.NewsContent,
                    NewsSource = model.NewsSource,
                    CategoryId = model.CategoryId,
                    NewsStatus = model.NewsStatus,
                    CreatedById = CurrentUser!.AccountId
                };

                // Parse tags from string
                var tagIds = new List<int>();
                if (!string.IsNullOrEmpty(model.TagsString))
                {
                    var tagNames = model.TagsString.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                                  .Select(t => t.Trim()).ToList();
                    
                    foreach (var tagName in tagNames)
                    {
                        var tag = await _tagService.GetOrCreateTagAsync(tagName);
                        tagIds.Add(tag.TagId);
                    }
                }

                await _newsService.CreateNewsAsync(news, tagIds);
                return Json(new { success = true, message = "News created successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditNews(string id)
        {
            var news = await _newsService.GetNewsWithDetailsAsync(id);
            if (news == null || news.CreatedById != CurrentUser!.AccountId)
            {
                return NotFound();
            }

            var model = new NewsArticleViewModel
            {
                NewsArticleId = news.NewsArticleId,
                NewsTitle = news.NewsTitle,
                Headline = news.Headline,
                NewsContent = news.NewsContent,
                NewsSource = news.NewsSource,
                CategoryId = news.CategoryId,
                NewsStatus = news.NewsStatus,
                TagsString = string.Join(", ", news.NewsTags?.Select(nt => nt.Tag.TagName) ?? new List<string>())
            };

            ViewBag.Categories = await _categoryService.GetActiveCategoriesAsync();
            ViewBag.Tags = await _tagService.GetAllTagsAsync();
            return PartialView("_EditNewsModal", model);
        }

        [HttpPost]
        public async Task<IActionResult> EditNews(NewsArticleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryService.GetActiveCategoriesAsync();
                ViewBag.Tags = await _tagService.GetAllTagsAsync();
                return PartialView("_EditNewsModal", model);
            }

            try
            {
                var news = await _newsService.GetNewsByIdAsync(model.NewsArticleId);
                if (news == null || news.CreatedById != CurrentUser!.AccountId)
                {
                    return Json(new { success = false, message = "News not found or access denied." });
                }

                news.NewsTitle = model.NewsTitle;
                news.Headline = model.Headline;
                news.NewsContent = model.NewsContent;
                news.NewsSource = model.NewsSource;
                news.CategoryId = model.CategoryId;
                news.NewsStatus = model.NewsStatus;
                news.UpdatedById = CurrentUser.AccountId;

                // Parse tags from string
                var tagIds = new List<int>();
                if (!string.IsNullOrEmpty(model.TagsString))
                {
                    var tagNames = model.TagsString.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                                  .Select(t => t.Trim()).ToList();
                    
                    foreach (var tagName in tagNames)
                    {
                        var tag = await _tagService.GetOrCreateTagAsync(tagName);
                        tagIds.Add(tag.TagId);
                    }
                }

                await _newsService.UpdateNewsAsync(news, tagIds);
                return Json(new { success = true, message = "News updated successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteNews(string id)
        {
            try
            {
                var news = await _newsService.GetNewsByIdAsync(id);
                if (news == null || news.CreatedById != CurrentUser!.AccountId)
                {
                    return Json(new { success = false, message = "News not found or access denied." });
                }

                await _newsService.DeleteNewsAsync(id);
                return Json(new { success = true, message = "News deleted successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // Profile Management
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var account = await _accountService.GetAccountByIdAsync(CurrentUser!.AccountId);
            if (account == null)
            {
                return NotFound();
            }

            var model = new AccountViewModel
            {
                AccountId = account.AccountId,
                AccountName = account.AccountName,
                AccountEmail = account.AccountEmail,
                AccountRole = account.AccountRole,
                AccountPassword = account.AccountPassword
            };

            // Get user statistics
            var userNews = await _newsService.GetNewsByAuthorAsync(CurrentUser.AccountId);
            ViewBag.TotalNews = userNews.Count();
            ViewBag.ActiveNews = userNews.Count(n => n.NewsStatus);
            ViewBag.InactiveNews = userNews.Count(n => !n.NewsStatus);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(AccountViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Get user statistics for view
                var userNews = await _newsService.GetNewsByAuthorAsync(CurrentUser!.AccountId);
                ViewBag.TotalNews = userNews.Count();
                ViewBag.ActiveNews = userNews.Count(n => n.NewsStatus);
                ViewBag.InactiveNews = userNews.Count(n => !n.NewsStatus);
                return View(model);
            }

            try
            {
                var account = await _accountService.GetAccountByIdAsync(CurrentUser!.AccountId);
                if (account == null)
                {
                    return NotFound();
                }

                account.AccountName = model.AccountName;
                account.AccountEmail = model.AccountEmail;

                // Only update password if provided
                if (!string.IsNullOrEmpty(model.AccountPassword))
                {
                    account.AccountPassword = model.AccountPassword;
                }

                await _accountService.UpdateAccountAsync(account);
                TempData["SuccessMessage"] = "Profile updated successfully!";
                return RedirectToAction("Profile");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                // Get user statistics for view
                var userNews = await _newsService.GetNewsByAuthorAsync(CurrentUser!.AccountId);
                ViewBag.TotalNews = userNews.Count();
                ViewBag.ActiveNews = userNews.Count(n => n.NewsStatus);
                ViewBag.InactiveNews = userNews.Count(n => !n.NewsStatus);
                return View(model);
            }
        }
    }
}
