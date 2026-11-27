using MovieApi.DTOs.Read;
using MovieApi.Models;

namespace MovieApi.Mapping
{
    public static class DtoMapper
    {
        public static MovieDto MovieToDto(Movie movie)
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

        public static MovieDetailedDto MovieToDetailedDto(Movie movie)
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

        public static ActorDto ActorToDto(Actor actor)
        {
            return new ActorDto
            {
                Id = actor.Id,
                Name = actor.Name,
                BirthYear = actor.BirthYear,
            };
        }

        public static ActorDetailedDto ActorToDetailedDto(Actor actor)
        {
            return new ActorDetailedDto
            {
                Id = actor.Id,
                Name = actor.Name,
                BirthYear = actor.BirthYear,
                Movies = actor
                    .Movies.Select(movie => new MovieDto
                    {
                        Id = movie.Id,
                        Title = movie.Title,
                        Year = movie.Year,
                        Duration = movie.Duration,
                        Genres = movie
                            .Genres.Select(genre => new GenreDto
                            {
                                Id = genre.Id,
                                GenreName = genre.GenreName,
                            })
                            .ToList(),
                    })
                    .ToList(),
            };
        }

        public static ReviewDto ReviewToDto(Review review)
        {
            return new ReviewDto
            {
                Id = review.Id,
                Rating = review.Rating,
                ReviewerName = review.ReviewerName,
                Comment = review.Comment,
            };
        }
    }
}
