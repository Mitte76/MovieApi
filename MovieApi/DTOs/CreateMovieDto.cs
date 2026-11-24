using System.ComponentModel.DataAnnotations;
using MovieApi.Models;

namespace MovieApi.DTOs
{
    public class CreateMovieDto
    {
        public int Year { get; set; }
        public int Duration { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Genre { get; set; } = string.Empty;
        public MovieDetails? MovieDetails { get; set; }
        //public ICollection<Review> Reviews { get; set; } = [];
        //public ICollection<Actor> Actors { get; set; } = [];
    }
}
