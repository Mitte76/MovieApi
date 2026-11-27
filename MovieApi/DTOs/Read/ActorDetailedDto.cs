namespace MovieApi.DTOs.Read
{
    public class ActorDetailedDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string BirthYear { get; set; } = string.Empty;

        public ICollection<MovieDto> Movies { get; set; } = [];
    }
}
