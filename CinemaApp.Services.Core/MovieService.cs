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
            return allMovies;
        }
    }
}
