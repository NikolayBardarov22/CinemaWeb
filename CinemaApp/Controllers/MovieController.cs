namespace CinemaApp.Web.Controllers
{
    using CinemaApp.Services.Core.Interfaces;
    using CinemaApp.Web.ViewModels.Movie;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using static ViewModels.ValidationMessages.Movie;
    public class MovieController : BaseController
    {
        private readonly IMovieService _movieService;
        private readonly IWatchlistService _watchlistService;
        public MovieController(IMovieService movieService, IWatchlistService watchlistService)
        {
            this._movieService = movieService;
            this._watchlistService = watchlistService;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<AllMoviesIndexViewModel> allMovies = await _movieService.GetAllMoviesAsync();

            if (this.IsUserAutenticated())
            {
                foreach (AllMoviesIndexViewModel moviesIndexViewModel in allMovies)
                { 
                    moviesIndexViewModel.IsAddedToUserWatchlist
                        = await this._watchlistService
                        .IsMovieAddedToTheWatchlist(moviesIndexViewModel.Id, this.GetUserId());
                }
            }
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

        [AllowAnonymous]
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
        [HttpGet]
        public async Task<IActionResult> Edit(String? id)
        {
            try
            {
                MovieFormInputModel? editableMovie = await this._movieService.GetEditableMovieByIdAsync(id);

                if (editableMovie == null)
                {
                    //TODO Custom 404 page
                    return this.RedirectToAction(nameof(Index));
                }
                return this.View(editableMovie);
            }
            catch (Exception e)
            {
                //TODO Implement it with ILogger
                //Add JS bars to indicate such errors
                Console.WriteLine(e.Message);
                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MovieFormInputModel inputModel)
        {
            if (!this.ModelState.IsValid) return this.View(inputModel);

            try
            {
                bool isValidEdit = await this._movieService.EditMovieAsync(inputModel);
                if (!isValidEdit) return this.RedirectToAction(nameof(Index));//TODO Custom404

                return this.RedirectToAction(nameof(Details), new { id = inputModel.Id });
            }
            catch (Exception e)
            {
                //TODO Implement it with ILogger
                //Add JS bars to indicate such errors
                Console.WriteLine(e.Message);
                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(String? id)
        {
            try
            {
                DeleteMovieViewModel? movieDetails = await this._movieService.GetMovieDeleteDetailsByIdAsync(id);
                if (movieDetails == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                return this.View(movieDetails);
            }
            catch (Exception ex)
            {
                //TODO Implement it with ILogger
                //Add JS bars to indicate such errors
                Console.WriteLine(ex.Message);
                return this.RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        public async Task<IActionResult> Delete(DeleteMovieViewModel inputModel)
        {
            try
            {
                if (!this.ModelState.IsValid) return this.View(inputModel);

                bool deleteResult = await this._movieService.SoftDeleteMovieAsync(inputModel.Id);
                if (deleteResult == false)
                {
                    //TODO: Implement JS notification
                    return this.RedirectToAction(nameof(Index));
                }
                //TODO: Success notification
                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                //TODO Implement it with ILogger
                //Add JS bars to indicate such errors
                Console.WriteLine(ex.Message);
                return this.RedirectToAction(nameof(Index));
            }
        }
    }
}
