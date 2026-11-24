using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        public string ReviewerName { get; set; } = string.Empty;

        [Required]
        public string Comment { get; set; } = string.Empty;
        public Movie Movie { get; set; } = null!;
    }
}
