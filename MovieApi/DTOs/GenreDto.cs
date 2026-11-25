using System.ComponentModel.DataAnnotations;

namespace MovieApi.DTOs
{
    public class GenreDto
    {
        public int Id { get; set; }

        [Required]
        public string GenreName { get; set; } = string.Empty;
    }
}
