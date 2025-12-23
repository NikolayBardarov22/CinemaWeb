using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
namespace CinemaApp.Data.Models
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    [Comment("Movie in the system")]
    public class ApplicationUserMovie
    {
        [Comment("Foreign key to the referenced AspNetUser")]
        [Required]
        public String ApplicationUserId { get; set; } = null!;
        public virtual IdentityUser ApplicationUser { get; set; } = null!;

        [Comment("Foreignkey to the refernece Movie")]
        public Guid MovieId { get; set; }
        public virtual Movie Movie { get; set; } = null!;

        [Comment("Shows if ApplicationUserMovie entity movie is deleted")]
        public bool IsDeleted { get; set; }
    }
}
