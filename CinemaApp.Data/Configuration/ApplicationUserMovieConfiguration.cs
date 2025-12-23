namespace CinemaApp.Data.Configuration
{
    using CinemaApp.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ApplicationUserMovieConfiguration : IEntityTypeConfiguration<ApplicationUserMovie>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserMovie> entity)
        {
            entity
                  .Property(aum => aum.ApplicationUserId)
                  .IsRequired(true);

            entity
                .Property(aum => aum.IsDeleted)
                .HasDefaultValue(false);

            entity
                .HasKey(aum => new { aum.ApplicationUserId, aum.MovieId });

            entity.HasOne(aum => aum.ApplicationUser)
                  .WithMany()
                  .HasForeignKey(aum => aum.ApplicationUserId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(aum => aum.Movie)
                  .WithMany(m => m.MovieWatchlists)
                  .HasForeignKey(aum => aum.MovieId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasQueryFilter(aum => aum.Movie.IsDeleted == false);

            entity
                .HasQueryFilter(aum => aum.IsDeleted == false);
        }
    }
}
