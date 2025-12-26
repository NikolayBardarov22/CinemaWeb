namespace CinemaApp.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    [Authorize]
    public abstract class BaseController : Controller
    {
        protected bool IsUserAutenticated()
        {
            bool retRes = false;
            if (this.User.Identity != null)
            {
                retRes = this.User.Identity.IsAuthenticated;
            }
            return retRes;
        }
        protected string? GetUserId()
        {
            string? userId = null;
            if (this.IsUserAutenticated())
            {
                userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
            return userId;
        }
    }
}
