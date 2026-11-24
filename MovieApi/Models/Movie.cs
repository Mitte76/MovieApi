using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int Duration { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;
        public required string Genre { get; set; }
        public MovieDetails? MovieDetails { get; set; }
    }
}
