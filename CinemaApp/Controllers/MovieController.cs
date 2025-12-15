namespace CinemaApp.Web.Controllers
{
    using CinemaApp.Services.Core.Interfaces;
    using CinemaApp.Web.ViewModels.Movie;
    using Microsoft.AspNetCore.Mvc;
    using static ViewModels.ValidationMessages.Movie;
    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;
        public MovieController(IMovieService movieService)
        {
            this._movieService = movieService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<AllMoviesIndexViewModel> allMovies
                = await _movieService.GetAllMoviesAsync();

            return View(allMovies);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(MovieFormInputModel inputModel)
        {
            if (!this.ModelState.IsValid) return this.View(inputModel);
            try
            {
                await this._movieService.AddMovieAsync(inputModel);
                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                this.ModelState.AddModelError(string.Empty, ServiceCreateError);
                return this.View(inputModel);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Details(String? id)
        {
            try
            {
                MovieDetailsViewModel? movieDetails = await this._movieService.GetMovieDatilsByIdAsync(id);
                if (movieDetails == null)
                {
                    //TODO Custom 404 page
                    return this.RedirectToAction(nameof(Index));
                }
                return this.View(movieDetails);
            }
            catch (Exception e)
            {
                //TODO Implement it with ILogger
                //Add JS bars to indicate such errors
                Console.WriteLine(e.Message);
                return this.RedirectToAction(nameof(Index));
            }
        }
    }
}
