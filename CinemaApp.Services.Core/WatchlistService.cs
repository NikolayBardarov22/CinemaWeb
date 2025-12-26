namespace CinemaApp.Services.Core
{
    using CinemaApp.Data;
    using CinemaApp.Data.Models;
    using CinemaApp.Services.Core.Interfaces;
    using CinemaApp.Web.ViewModels.Watchlist;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using static CinemaApp.GCommon.ApplicationConstants;

    public class WatchlistService : IWatchlistService
    {
        private readonly CinemaAppDbContext _dbContext;
        public WatchlistService(CinemaAppDbContext context)
        {
            this._dbContext = context;
        }

        public async Task<IEnumerable<WatchlistViewModel>> GetUserWatchlistAsync(string id)
        {
            IEnumerable<WatchlistViewModel> userWatchlist = await this._dbContext
                .ApplicationUserMovies
                .Include(aum => aum.Movie)
                .AsNoTracking()
                .Where(aum => aum.ApplicationUserId == id)
                .Select(aum => new WatchlistViewModel()
                {
                    MovieId = aum.MovieId.ToString(),
                    Title = aum.Movie.Title,
                    Genre = aum.Movie.Genre,
                    ReleaseDate = aum.Movie.ReleaseDate.ToString(DateFormat),
                    ImageUrl = aum.Movie.ImageUrl ?? $"~/images/{NoImageUrl}",
                })
                .ToArrayAsync();

            return userWatchlist;
        }

        public async Task<bool> AddMovieToUserWatchlistAsync(string? movieId, string? userId)
        {
            bool result = false;
            if (movieId != null && userId != null)
            {
                bool isValidId = Guid.TryParse(movieId, out Guid movieIdGuid);
                if (isValidId)
                {
                    ApplicationUserMovie? userMovieEntry = await this._dbContext
                        .ApplicationUserMovies
                        .IgnoreQueryFilters()
                        .SingleOrDefaultAsync(aum => aum.ApplicationUserId.ToLower() == userId &&
                                                         aum.MovieId.ToString() == movieIdGuid.ToString());

                    if (userMovieEntry != null)
                    {
                        userMovieEntry.IsDeleted = false;
                    }
                    else
                    {
                        userMovieEntry = new ApplicationUserMovie()
                        {
                            ApplicationUserId = userId,
                            MovieId = movieIdGuid
                        };
                        await this._dbContext.ApplicationUserMovies.AddAsync(userMovieEntry);
                    }
                    await this._dbContext.SaveChangesAsync(); result = true;
                }
            }
            return result;
        }

        public async Task<bool> RemoveMovieFromWatchlistAsync(string? movieId, string? userId)
        {
            bool result = false;
            if (movieId != null && userId != null)
            {
                bool isMovieIdValid = Guid.TryParse(movieId, out Guid validMovieId);
                if (isMovieIdValid)
                {
                    ApplicationUserMovie? userMovieEntry = await this._dbContext
                        .ApplicationUserMovies
                        .SingleOrDefaultAsync(aum => aum.ApplicationUserId.ToLower() == userId &&
                        aum.MovieId.ToString() == validMovieId.ToString());

                    if (userMovieEntry != null)
                    {
                        userMovieEntry.IsDeleted = true;
                        await this._dbContext.SaveChangesAsync();
                        result = true;
                    }
                }
            }
            return result;
        }

        public async Task<bool> IsMovieAddedToTheWatchlist(string? movieId, string? userId)
        {
            bool result = false;
            if (movieId != null && userId != null)
            {
                bool isMovieIdValid = Guid.TryParse(movieId, out Guid validMovieId);
                if (isMovieIdValid)
                {
                    ApplicationUserMovie? userMovieEntry = await this._dbContext
                        .ApplicationUserMovies
                        .SingleOrDefaultAsync(aum => aum.ApplicationUserId.ToLower() == userId &&
                        aum.MovieId.ToString() == validMovieId.ToString());

                    if (userMovieEntry != null) return true;
                }
            }
            return result;
        }
    }
}
