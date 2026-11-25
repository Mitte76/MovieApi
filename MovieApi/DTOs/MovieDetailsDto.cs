using System.ComponentModel.DataAnnotations;

namespace MovieApi.DTOs
{
    public class MovieDetailsDto
    {
        public string Budget { get; set; } = string.Empty;

        [Required]
        public string Synopsis { get; set; } = string.Empty;

        [Required]
        public string Language { get; set; } = string.Empty;
    }
}
