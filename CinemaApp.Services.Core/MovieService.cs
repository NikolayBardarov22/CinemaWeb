namespace CinemaApp.Services.Core
{
    using CinemaApp.Data;
    using CinemaApp.Services.Core.Interfaces;
    using CinemaApp.Web.ViewModels.Movie;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;
    using static CinemaApp.GCommon.ApplicationConstants;
    using CinemaApp.Data.Models;
    using System.Globalization;

    public class MovieService : IMovieService
    {
        private readonly CinemaAppDbContext _dbContext;
        public MovieService(CinemaAppDbContext context)
        {
            this._dbContext = context;
        }
        public async Task AddMovieAsync(MovieFormInputModel model)
        {
            Movie movie = new Movie()
            {
                Title = model.Title,
                Genre = model.Genre,
                Duration = model.Duration,
                Description = model.Description,
                Director = model.Director,
                ImageUrl = model.ImageUrl,
                ReleaseDate
                = DateOnly.ParseExact(model.ReleaseDate, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None)
            };
            await this._dbContext.Movies.AddAsync(movie);
            await this._dbContext.SaveChangesAsync();
        }
        public async Task<bool> EditMovieAsync(MovieFormInputModel inputModel)
        {
            Movie? editableMovie = await this.FindMovieByStringId(inputModel.Id);

            if (editableMovie == null) { return false; }

            DateOnly releaseDate = DateOnly.ParseExact(inputModel.ReleaseDate
                , DateFormat, CultureInfo.InvariantCulture
                , DateTimeStyles.None);

            editableMovie.Title = inputModel.Title;
            editableMovie.Description = inputModel.Description;
            editableMovie.Director = inputModel.Director;
            editableMovie.Duration = inputModel.Duration;
            editableMovie.Genre = inputModel.Genre;
            editableMovie.ImageUrl = inputModel.ImageUrl ?? $"~/images/{NoImageUrl}";
            editableMovie.ReleaseDate = releaseDate;

            await this._dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<AllMoviesIndexViewModel>> GetAllMoviesAsync()
        {
            IEnumerable<AllMoviesIndexViewModel> allMovies = await _dbContext
                .Movies
                .AsNoTracking()
                .Select(m => new AllMoviesIndexViewModel
                {
                    Id = m.Id.ToString(),
                    Title = m.Title,
                    Genre = m.Genre,
                    ReleaseDate = m.ReleaseDate.ToString(DateFormat),
                    Director = m.Director,
                    ImageUrl = m.ImageUrl
                })
                .ToArrayAsync();
            foreach (AllMoviesIndexViewModel movie in allMovies)
            {
                if (String.IsNullOrEmpty(movie.ImageUrl))
                    movie.ImageUrl = $"~/images/{NoImageUrl}.jpg";
            }
            return allMovies;
        }

        public async Task<MovieFormInputModel?> GetEditableMovieByIdAsync(String? id)
        {
            MovieFormInputModel? editableMovie = null;

            bool isValidGuid = Guid.TryParse(id, out Guid movieId);

            if (isValidGuid)
            {
                editableMovie = await this._dbContext
                    .Movies
                    .AsNoTracking()
                    .Where(m => m.Id == movieId)
                    .Select(m => new MovieFormInputModel
                    {
                        Description = m.Description,
                        Director = m.Director,
                        Duration = m.Duration,
                        ImageUrl = m.ImageUrl != null ? m.ImageUrl : $"~/images/{NoImageUrl}.jpg",
                        Genre = m.Genre,
                        ReleaseDate = m.ReleaseDate.ToString(DateFormat),
                        Title = m.Title
                    })
                    .SingleOrDefaultAsync();
            }
            return editableMovie;
        }

        public async Task<MovieDetailsViewModel> GetMovieDatilsByIdAsync(String? id)
        {
            MovieDetailsViewModel? movieDetails = null;

            bool isValidGuid = Guid.TryParse(id, out Guid movieId);

            if (isValidGuid)
            {
                movieDetails = await this._dbContext
                    .Movies
                    .AsNoTracking()
                    .Where(m => m.Id == movieId)
                    .Select(m => new MovieDetailsViewModel
                    {
                        Id = m.Id.ToString(),
                        Description = m.Description,
                        Director = m.Director,
                        Duration = m.Duration,
                        ImageUrl = m.ImageUrl != null ? m.ImageUrl : $"~/images/{NoImageUrl}.jpg",
                        Genre = m.Genre,
                        ReleaseDate = m.ReleaseDate.ToString(DateFormat),
                        Title = m.Title
                    })
                    .SingleOrDefaultAsync();
            }
            return movieDetails;
        }
        //TODO To be investigated when relations to Movie entity are introduced
        public async Task<bool> DeleteMovieAsync(string? id)
        {
            Movie? movieToDelete = await this.FindMovieByStringId(id);

            if (movieToDelete == null) { return false; }

            this._dbContext.Movies.Remove(movieToDelete);
            await this._dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<DeleteMovieViewModel> GetMovieDeleteDetailsByIdAsync(string? id)
        {
            DeleteMovieViewModel? deleteMovieViewModel = null;
            Movie? movieToDelete = await this.FindMovieByStringId(id);

            if (movieToDelete != null)
            {
                deleteMovieViewModel = new DeleteMovieViewModel()
                {
                    Id = movieToDelete.Id.ToString(),
                    Title = movieToDelete.Title,
                    ImageUrl = movieToDelete.ImageUrl ?? $"~/images/{NoImageUrl}.jpg"
                };
            }
            return deleteMovieViewModel;
        }
        public async Task<bool> SoftDeleteMovieAsync(string? id)
        {
            Movie? movieToDelete = await this.FindMovieByStringId(id);

            if (movieToDelete == null) { return false; }

            movieToDelete.IsDeleted = true;
            await this._dbContext.SaveChangesAsync();
            return true;
        }
        //TODO: Implement as generic methood in BaseService
        private async Task<Movie?> FindMovieByStringId(string? id)
        {
            Movie? movie = null;

            if (!String.IsNullOrWhiteSpace(id))
            {
                bool isGuidValid = Guid.TryParse(id, out Guid movieGuid);

                if (isGuidValid)
                {
                    movie = await this._dbContext.Movies.FindAsync(movieGuid);
                }
            }
            return movie;
        }
    }
}
