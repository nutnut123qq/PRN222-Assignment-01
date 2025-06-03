using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StudentNameMVC.Helpers;
using FUNewsManagement.DataAccess.Models;

namespace StudentNameMVC.Controllers
{
    public class BaseController : Controller
    {
        protected SystemAccount? CurrentUser => SessionHelper.GetCurrentUser(HttpContext.Session);
        protected bool IsLoggedIn => SessionHelper.IsLoggedIn(HttpContext.Session);

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!IsLoggedIn)
            {
                context.Result = RedirectToAction("Login", "Account");
                return;
            }

            ViewBag.CurrentUser = CurrentUser;
            base.OnActionExecuting(context);
        }
    }

    public class AdminBaseController : BaseController
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (context.Result != null) return; // Already redirected

            var configuration = HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            if (!SessionHelper.IsAdmin(HttpContext.Session, configuration))
            {
                context.Result = RedirectToAction("AccessDenied", "Account");
            }
        }
    }

    public class StaffBaseController : BaseController
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (context.Result != null) return; // Already redirected

            if (!SessionHelper.IsStaff(HttpContext.Session))
            {
                context.Result = RedirectToAction("AccessDenied", "Account");
            }
        }
    }

    public class LecturerBaseController : BaseController
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (context.Result != null) return; // Already redirected

            if (!SessionHelper.IsLecturer(HttpContext.Session))
            {
                context.Result = RedirectToAction("AccessDenied", "Account");
            }
        }
    }
}
