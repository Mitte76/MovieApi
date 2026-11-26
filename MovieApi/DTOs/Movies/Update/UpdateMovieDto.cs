using System.ComponentModel.DataAnnotations;

namespace MovieApi.DTOs.Movies.Update
{
    public class UpdateMovieDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Year { get; set; } = string.Empty;

        [Required]
        public string Duration { get; set; } = string.Empty;

        public ICollection<int> GenreIds { get; set; } = [];
        public ICollection<int> ActorIds { get; set; } = [];
        public UpdateMovieDetailsDto? Details { get; set; }
    }
}
