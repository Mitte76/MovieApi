using System.ComponentModel.DataAnnotations;

namespace MovieApi.DTOs.Movies.Read
{
    public class GenreDto
    {
        public int Id { get; set; }

        [Required]
        public string GenreName { get; set; } = string.Empty;
    }
}
