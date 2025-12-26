namespace CinemaApp.Web.Controllers
{
    using CinemaApp.Services.Core.Interfaces;
    using CinemaApp.Web.ViewModels.Watchlist;
    using Microsoft.AspNetCore.Mvc;

    public class WatchlistController : BaseController
    {
        private readonly IWatchlistService watchlistService;

        public WatchlistController(IWatchlistService watchlistService)
        {
            this.watchlistService = watchlistService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                string? userId = this.GetUserId();
                if (userId == null) { return this.Forbid(); }

                IEnumerable<WatchlistViewModel> watchlistViewModels
                    = await this.watchlistService.GetUserWatchlistAsync(userId);

                return View(watchlistViewModels);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return this.RedirectToAction(nameof(Index), "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(string? movieId)
        {
            try
            {
                string? userId = this.GetUserId();
                if (userId == null)
                {
                    return this.Forbid();
                }
                bool result = await this.watchlistService.AddMovieToUserWatchlistAsync(movieId, userId);

                if (result == false)
                {
                    //TODO: Add notifications
                    return this.RedirectToAction(nameof(Index), "Movie");
                }
                TempData["Message"] = "The movie wat added to yous watchlist.";
                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return this.RedirectToAction(nameof(Index), "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Remove(string? movieId)
        {
            try
            {

                string? userId = this.GetUserId();
                if (userId == null)
                {
                    return this.Forbid();
                }
                bool result = await this.watchlistService.RemoveMovieFromWatchlistAsync(movieId, userId);

                if (result == false)
                {
                    //TODO: Add notifications
                    return this.RedirectToAction(nameof(Index), "Movie");
                }
                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine( ex.Message);
                return this.RedirectToAction(nameof(Index), "Home");
            }
        }
    }
}
