using MovieApi.DTOs;
using MovieApi.Models;

namespace MovieApi.Mapping
{
    public static class DtoMapper
    {

        public static MovieDto ToDto(Movie movie)
        {
            return new MovieDto
            {
                Id = movie.Id,
                Year = movie.Year,
                Duration = movie.Duration,
                Title = movie.Title,
                Genres = movie.Genres
                    .Select(genre => new GenreDto
                    {
                        Id = genre.Id,
                        GenreName = genre.GenreName
                    })
                    .ToList(),

                MovieDetails = movie.Details is null
                    ? null
                    : new MovieDetailsDto
                    {
                        Budget = movie.Details.Budget,
                        Synopsis = movie.Details.Synopsis,
                        Language = movie.Details.Language
                    },

                Reviews = movie.Reviews
                    .Select(review => new ReviewDto
                    {
                        Rating = review.Rating,
                        ReviewerName = review.ReviewerName,
                        Comment = review.Comment
                    })
                    .ToList(),

                Actors = movie.Actors
                    .Select(actor => new ActorDto
                    {
                        Id = actor.Id,
                        Name = actor.Name,
                        BirthYear = actor.BirthYear
                    })
                    .ToList()
            };
        }


    }
}
