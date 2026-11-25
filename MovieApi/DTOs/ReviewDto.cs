using System.ComponentModel.DataAnnotations;

namespace MovieApi.DTOs
{
    public class ReviewDto
    {
        public int Id { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        public string ReviewerName { get; set; } = string.Empty;

        [Required]
        public string Comment { get; set; } = string.Empty;

    }
}
