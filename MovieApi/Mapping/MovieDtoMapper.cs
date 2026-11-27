using MovieApi.DTOs.Read;
using MovieApi.Models;

namespace MovieApi.Mapping
{
    public static class MovieDtoMapper
    {
        public static MovieDto ToDto(Movie movie)
        {
            return new MovieDto
            {
                Id = movie.Id,
                Year = movie.Year,
                Duration = movie.Duration,
                Title = movie.Title,
                Genres = movie
                    .Genres.Select(genre => new GenreDto
                    {
                        Id = genre.Id,
                        GenreName = genre.GenreName,
                    })
                    .ToList(),
            };
        }

        public static MovieDetailedDto ToDetailedDto(Movie movie)
        {
            return new MovieDetailedDto
            {
                Id = movie.Id,
                Year = movie.Year,
                Duration = movie.Duration,
                Title = movie.Title,
                Genres = movie
                    .Genres.Select(genre => new GenreDto
                    {
                        Id = genre.Id,
                        GenreName = genre.GenreName,
                    })
                    .ToList(),

                Details = movie.Details is null
                    ? null
                    : new DetailsDto
                    {
                        Budget = movie.Details.Budget,
                        Synopsis = movie.Details.Synopsis,
                        Language = movie.Details.Language,
                    },

                Reviews = movie
                    .Reviews.Select(review => new ReviewDto
                    {
                        Id = review.Id,
                        Rating = review.Rating,
                        ReviewerName = review.ReviewerName,
                        Comment = review.Comment,
                    })
                    .ToList(),

                Actors = movie
                    .Actors.Select(actor => new ActorDto
                    {
                        Id = actor.Id,
                        Name = actor.Name,
                        BirthYear = actor.BirthYear,
                    })
                    .ToList(),
            };
        }
    }
}
