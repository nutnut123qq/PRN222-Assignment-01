using Microsoft.AspNetCore.Mvc;
using FUNewsManagement.Services;
using FUNewsManagement.DataAccess.Models;
using StudentNameMVC.Models;

namespace StudentNameMVC.Controllers
{
    public class AdminPanelController : AdminBaseController
    {
        private readonly ISystemAccountService _accountService;
        private readonly INewsArticleService _newsService;

        public AdminPanelController(ISystemAccountService accountService, INewsArticleService newsService)
        {
            _accountService = accountService;
            _newsService = newsService;
        }

        public async Task<IActionResult> Index()
        {
            var accounts = await _accountService.GetAllAccountsAsync();
            var accountViewModels = accounts.Select(a => new AccountViewModel
            {
                AccountId = a.AccountId,
                AccountName = a.AccountName,
                AccountEmail = a.AccountEmail,
                AccountRole = a.AccountRole
            }).ToList();

            return View(accountViewModels);
        }

        [HttpGet]
        public async Task<IActionResult> ManageAccounts(string? search)
        {
            IEnumerable<SystemAccount> accounts;
            
            if (!string.IsNullOrEmpty(search))
            {
                accounts = await _accountService.SearchAccountsAsync(search);
            }
            else
            {
                accounts = await _accountService.GetAllAccountsAsync();
            }

            var accountViewModels = accounts.Select(a => new AccountViewModel
            {
                AccountId = a.AccountId,
                AccountName = a.AccountName,
                AccountEmail = a.AccountEmail,
                AccountRole = a.AccountRole
            }).ToList();

            ViewBag.Search = search;
            return View(accountViewModels);
        }

        [HttpGet]
        public IActionResult CreateAccount()
        {
            return PartialView("_CreateAccountModal", new AccountViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount(AccountViewModel model)
        {
            // For create operation, password is required
            if (string.IsNullOrEmpty(model.AccountPassword))
            {
                ModelState.AddModelError("AccountPassword", "Password is required for new accounts.");
            }

            if (!ModelState.IsValid)
            {
                return PartialView("_CreateAccountModal", model);
            }

            try
            {
                var account = new SystemAccount
                {
                    AccountName = model.AccountName,
                    AccountEmail = model.AccountEmail,
                    AccountRole = model.AccountRole,
                    AccountPassword = model.AccountPassword
                };

                await _accountService.CreateAccountAsync(account);
                return Json(new { success = true, message = "Account created successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditAccount(short id)
        {
            var account = await _accountService.GetAccountByIdAsync(id);
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

            return PartialView("_EditAccountModal", model);
        }

        [HttpPost]
        public async Task<IActionResult> EditAccount(AccountViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_EditAccountModal", model);
            }

            try
            {
                var account = await _accountService.GetAccountByIdAsync(model.AccountId);
                if (account == null)
                {
                    return Json(new { success = false, message = "Account not found." });
                }

                account.AccountName = model.AccountName;
                account.AccountEmail = model.AccountEmail;
                account.AccountRole = model.AccountRole;

                // Only update password if provided
                if (!string.IsNullOrEmpty(model.AccountPassword))
                {
                    account.AccountPassword = model.AccountPassword;
                }

                await _accountService.UpdateAccountAsync(account);
                return Json(new { success = true, message = "Account updated successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccount(short id)
        {
            try
            {
                await _accountService.DeleteAccountAsync(id);
                return Json(new { success = true, message = "Account deleted successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult Reports()
        {
            var model = new ReportViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Reports(ReportViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                model.NewsStatistics = await _newsService.GetNewsStatisticsByDateRangeAsync(model.StartDate, model.EndDate);
                var newsArticles = await _newsService.GetNewsByDateRangeAsync(model.StartDate, model.EndDate);
                
                model.NewsArticles = newsArticles.Select(n => new NewsArticleViewModel
                {
                    NewsArticleId = n.NewsArticleId,
                    NewsTitle = n.NewsTitle,
                    Headline = n.Headline,
                    CategoryName = n.Category?.CategoryName,
                    CreatedDate = n.CreatedDate,
                    CreatedByName = n.CreatedBy?.AccountName,
                    NewsStatus = n.NewsStatus
                }).ToList();

                model.TotalNews = model.NewsArticles.Count;

                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }
    }
}
