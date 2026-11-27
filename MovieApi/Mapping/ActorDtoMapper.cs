using MovieApi.DTOs.Read;
using MovieApi.Models;

namespace MovieApi.Mapping
{
    public static class ActorDtoMapper
    {
        public static ActorDto ToDto(Actor actor)
        {
            return new ActorDto
            {
                Id = actor.Id,
                Name = actor.Name,
                BirthYear = actor.BirthYear,
            };
        }

        public static ActorDetailedDto ToDetailedDto(Actor actor)
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
    }
}
