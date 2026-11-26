namespace MovieApi.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public string GenreName { get; set; } = string.Empty;
        public ICollection<Movie> Movies { get; set; } = [];
    }
}
