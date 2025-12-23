namespace CinemaApp.Web.Controllers
{
    using CinemaApp.Web.ViewModels.Watchlist;
    using Microsoft.AspNetCore.Mvc;

    public class WatchlistController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable< WatchlistViewModel> watchlistViewModels = new List< WatchlistViewModel>();
            return View(watchlistViewModels);
        }
    }
}
