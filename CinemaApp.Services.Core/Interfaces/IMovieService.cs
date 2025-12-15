using CinemaApp.Web.ViewModels.Movie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Services.Core.Interfaces
{
    public interface IMovieService
    {
        Task<IEnumerable<AllMoviesIndexViewModel>> GetAllMoviesAsync();
        Task AddMovieAsync(MovieFormInputModel model);
        Task<MovieDetailsViewModel> GetMovieDatilsByIdAsync(string? id);
        Task<MovieFormInputModel?> GetEditableMovieByIdAsync(string? id);
        Task<bool> EditMovieAsync(MovieFormInputModel inputModel);
    }
}
