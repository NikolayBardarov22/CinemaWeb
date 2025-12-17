using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Web.ViewModels.Movie
{
    public class DeleteMovieViewModel
    {
        [Required]
        public string Id { get; set; } = null!;
        public string? Title { get; set; }
        public string? ImageUrl { get; set; }
    }
}
