using Microsoft.AspNetCore.Mvc;
using FUNewsManagement.Services;
using FUNewsManagement.DataAccess.Models;
using StudentNameMVC.Models;
using StudentNameMVC.Helpers;

namespace StudentNameMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly ISystemAccountService _accountService;
        private readonly IConfiguration _configuration;

        public AccountController(ISystemAccountService accountService, IConfiguration configuration)
        {
            _accountService = accountService;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Check admin account first
            var adminEmail = _configuration["AdminAccount:Email"];
            var adminPassword = _configuration["AdminAccount:Password"];

            if (model.Email == adminEmail && model.Password == adminPassword)
            {
                // Create admin user session
                var adminUser = new SystemAccount
                {
                    AccountId = 0,
                    AccountName = "Administrator",
                    AccountEmail = adminEmail!,
                    AccountRole = 0, // Admin role
                    AccountPassword = adminPassword!
                };

                SessionHelper.SetCurrentUser(HttpContext.Session, adminUser);
                return RedirectToAction("Index", "AdminPanel");
            }

            // Check regular users
            var user = await _accountService.AuthenticateAsync(model.Email, model.Password);
            if (user != null)
            {
                SessionHelper.SetCurrentUser(HttpContext.Session, user);

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                // Redirect based on role
                return user.AccountRole switch
                {
                    1 => RedirectToAction("Index", "Staff"), // Staff
                    2 => RedirectToAction("Index", "Lecturer"), // Lecturer
                    _ => RedirectToAction("Index", "Home")
                };
            }

            ModelState.AddModelError("", "Invalid email or password.");
            return View(model);
        }

        public IActionResult Logout()
        {
            SessionHelper.ClearCurrentUser(HttpContext.Session);
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
