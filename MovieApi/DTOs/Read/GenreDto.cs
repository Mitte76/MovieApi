using System.ComponentModel.DataAnnotations;

namespace MovieApi.DTOs.Read
{
    public class GenreDto
    {
        public int Id { get; set; }

        [Required]
        public string GenreName { get; set; } = string.Empty;
    }
}
