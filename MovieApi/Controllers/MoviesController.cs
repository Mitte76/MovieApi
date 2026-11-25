using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.DTOs;
using MovieApi.Mapping;
using MovieApi.Models;

[Route("api/[controller]")]
[ApiController]
public class MoviesController : ControllerBase
{
    private readonly MovieApiContext _context;

    public MoviesController(MovieApiContext context)
    {
        _context = context;
    }

    // GET: api/Movie
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovie()
    {
        var movies = await _context.Movies
            .AsNoTracking()
            .Include(movie => movie.Details)
            .Include(movie => movie.Genres)
            .Include(movie => movie.Reviews)
            .Include(movie => movie.Actors)
            .ToListAsync();

        return movies
            .Select(DtoMapper.ToDto)
            .ToList();
    }

    // GET: api/Movie/5
    [HttpGet("{id}")]
    public async Task<ActionResult<MovieDto>> GetMovie(int id)
    {
        var movie = await _context.Movies
            .AsNoTracking()
            .Include(movie => movie.Details)
            .Include(movie => movie.Genres)
            .Include(movie => movie.Reviews)
            .Include(movie => movie.Actors)
            .FirstOrDefaultAsync(movie => movie.Id == id);

        if (movie == null)
        {
            return NotFound();
        }
        MovieDto movieDto = DtoMapper.ToDto(movie);


        return movieDto;
    }

    // PUT: api/Movie/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutMovie(int? id, Movie movie)
    {
        if (id != movie.Id)
        {
            return BadRequest();
        }

        _context.Entry(movie).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MovieExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }


    // POST: api/Movie
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<MovieDto>> PostMovie(CreateMovieDto dtoMovie)
    {
        var genreIds = dtoMovie.GenreIds.Distinct().ToArray();
        var genres = await _context.Set<Genre>()
            .Where(genre => genreIds.Contains(genre.Id))
            .ToListAsync();

        if (genres.Count != genreIds.Length)
        {
            ModelState.AddModelError(nameof(dtoMovie.GenreIds), "One or more genre IDs do not exist.");
        }

        var actorIds = dtoMovie.ActorIds.Distinct().ToArray();
        var actors = await _context.Set<Actor>()
            .Where(actor => actorIds.Contains(actor.Id))
            .ToListAsync();

        if (actors.Count != actorIds.Length)
        {
            ModelState.AddModelError(nameof(dtoMovie.ActorIds), "One or more actor IDs do not exist.");
        }

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var movie = new Movie
        {
            Year = dtoMovie.Year,
            Duration = dtoMovie.Duration,
            Title = dtoMovie.Title,
            Genres = genres,
            Actors = actors,
            Reviews = dtoMovie.Reviews
                .Select(review => new Review
                {
                    Rating = review.Rating,
                    ReviewerName = review.ReviewerName,
                    Comment = review.Comment
                })
                .ToList(),
            Details = dtoMovie.Details is null
                ? null
                : new MovieDetails
                {
                    Budget = dtoMovie.Details.Budget,
                    Synopsis = dtoMovie.Details.Synopsis,
                    Language = dtoMovie.Details.Language
                }
        };

        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetMovie),
            new { id = movie.Id },
            DtoMapper.ToDto(movie));
    }

    // DELETE: api/Movie/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovie(int? id)
    {
        var movie = await _context.Movies.FindAsync(id);
        if (movie == null)
        {
            return NotFound();
        }

        _context.Movies.Remove(movie);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool MovieExists(int? id)
    {
        return _context.Movies.Any(e => e.Id == id);
    }
}
