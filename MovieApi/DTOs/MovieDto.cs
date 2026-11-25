using System.ComponentModel.DataAnnotations;

namespace MovieApi.DTOs
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Year { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;

        [Required]
        public string Title { get; set; } = string.Empty;

        public ICollection<GenreDto> Genres { get; set; } = [];
        public MovieDetailsDto? MovieDetails { get; set; }
        public ICollection<ReviewDto> Reviews { get; set; } = [];
        public ICollection<ActorDto> Actors { get; set; } = [];
    }
}
