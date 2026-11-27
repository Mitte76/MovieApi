using System.ComponentModel.DataAnnotations;

namespace MovieApi.DTOs.Create
{
    public class CreateMovieDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Year { get; set; } = string.Empty;

        [Required]
        public string Duration { get; set; } = string.Empty;

        public ICollection<int> GenreIds { get; set; } = [];
        public ICollection<int> ActorIds { get; set; } = [];
        public ICollection<CreateReviewDto> Reviews { get; set; } = [];
        public CreateMovieDetailsDto? Details { get; set; }
    }
}
