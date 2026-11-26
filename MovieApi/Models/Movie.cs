using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        public string Year { get; set; } = string.Empty;
        [Required]
        public string Duration { get; set; } = string.Empty;

        [Required]
        public string Title { get; set; } = string.Empty;
        public ICollection<Genre> Genres { get; set; } = [];
        public MovieDetails? Details { get; set; }
        public ICollection<Review> Reviews { get; set; } = [];
        public ICollection<Actor> Actors { get; set; } = [];
    }
}
