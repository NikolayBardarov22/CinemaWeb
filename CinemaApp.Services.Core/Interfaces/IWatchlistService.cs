using CinemaApp.Web.ViewModels.Watchlist;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Services.Core.Interfaces
{
    public interface IWatchlistService
    {
        Task<IEnumerable<WatchlistViewModel>> GetUserWatchlistAsync(string id);
        Task<bool> AddMovieToUserWatchlistAsync(string? movieId, string? userId);
        Task<bool> RemoveMovieFromWatchlistAsync(string? movieId, string? userId);
        Task<bool> IsMovieAddedToTheWatchlist(string? movieId, string? userId);
    }
}
