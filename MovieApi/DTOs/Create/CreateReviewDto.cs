using System.ComponentModel.DataAnnotations;

namespace MovieApi.DTOs.Create
{
    public class CreateReviewDto
    {
        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        public string ReviewerName { get; set; } = string.Empty;

        [Required]
        public string Comment { get; set; } = string.Empty;
    }
}
