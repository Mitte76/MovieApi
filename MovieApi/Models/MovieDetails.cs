using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models
{
    public class MovieDetails
    {
        public int Id { get; set; }
        public int Budget { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; } = null!;

        [Required]
        public string Synopsis { get; set; } = string.Empty;

        [Required]
        public string Language { get; set; } = string.Empty;
    }
}
