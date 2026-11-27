using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.DTOs.Create;
using MovieApi.DTOs.Read;
using MovieApi.DTOs.Update;
using MovieApi.Mapping;
using MovieApi.Models;

namespace MovieApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController(MovieApiContext context) : ControllerBase
    {
        private readonly MovieApiContext _context = context;

        // GET: api/Movie?details=true&reviews=true&actors=true&genre=true
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDetailedDto>>> GetMovie(
            [FromQuery] bool details = false,
            [FromQuery] bool reviews = false,
            [FromQuery] bool actors = false,
            [FromQuery] bool genre = false,
            [FromQuery] bool all = false
        )
        {
            var includeDetails = details || all;
            var includeReviews = reviews || all;
            var includeActors = actors || all;
            var includeGenre = genre || all;

            IQueryable<Movie> query = _context.Movies.AsNoTracking();

            if (includeDetails)
            {
                query = query.Include(movie => movie.Details);
            }

            if (includeReviews)
            {
                query = query.Include(movie => movie.Reviews);
            }

            if (includeActors)
            {
                query = query.Include(movie => movie.Actors);
            }

            if (includeGenre)
            {
                query = query.Include(movie => movie.Genres);
            }

            var movies = await query.ToListAsync();

            return movies.Select(DtoMapper.MovieToDetailedDto).ToList();
        }

        // GET: api/Movie/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDto>> GetMovie(int id)
        {
            var movie = await _context
                .Movies.AsNoTracking()
                .Include(movie => movie.Details)
                .Include(movie => movie.Genres)
                .Include(movie => movie.Reviews)
                .Include(movie => movie.Actors)
                .FirstOrDefaultAsync(movie => movie.Id == id);

            if (movie == null)
            {
                return NotFound();
            }
            MovieDto movieDto = DtoMapper.MovieToDto(movie);

            return movieDto;
        }

        // PUT: api/Movie/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, UpdateMovieDto dtoMovie)
        {
            var movie = await _context
                .Movies.Include(movie => movie.Genres)
                .Include(movie => movie.Actors)
                .Include(movie => movie.Details)
                .FirstOrDefaultAsync(movie => movie.Id == id);

            if (movie is null)
            {
                return NotFound();
            }

            var genreIds = dtoMovie.GenreIds.Distinct().ToArray();
            var genres = await _context
                .Set<Genre>()
                .Where(genre => genreIds.Contains(genre.Id))
                .ToListAsync();

            if (genres.Count != genreIds.Length)
            {
                ModelState.AddModelError(
                    nameof(dtoMovie.GenreIds),
                    "One or more genre IDs do not exist."
                );
            }

            var actorIds = dtoMovie.ActorIds.Distinct().ToArray();
            var actors = await _context
                .Set<Actor>()
                .Where(actor => actorIds.Contains(actor.Id))
                .ToListAsync();

            if (actors.Count != actorIds.Length)
            {
                ModelState.AddModelError(
                    nameof(dtoMovie.ActorIds),
                    "One or more actor IDs do not exist."
                );
            }

            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            movie.Title = dtoMovie.Title;
            movie.Year = dtoMovie.Year;
            movie.Duration = dtoMovie.Duration;
            movie.Genres = genres;
            movie.Actors = actors;

            if (dtoMovie.Details is null)
            {
                if (movie.Details is not null)
                {
                    _context.Remove(movie.Details);
                    movie.Details = null;
                }
            }
            else if (movie.Details is null)
            {
                movie.Details = new MovieDetails
                {
                    Budget = dtoMovie.Details.Budget,
                    Synopsis = dtoMovie.Details.Synopsis,
                    Language = dtoMovie.Details.Language,
                };
            }
            else
            {
                movie.Details.Budget = dtoMovie.Details.Budget;
                movie.Details.Synopsis = dtoMovie.Details.Synopsis;
                movie.Details.Language = dtoMovie.Details.Language;
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/Movie
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MovieDto>> PostMovie(CreateMovieDto dtoMovie)
        {
            var genreIds = dtoMovie.GenreIds.Distinct().ToArray();
            var genres = await _context
                .Set<Genre>()
                .Where(genre => genreIds.Contains(genre.Id))
                .ToListAsync();

            if (genres.Count != genreIds.Length)
            {
                ModelState.AddModelError(
                    nameof(dtoMovie.GenreIds),
                    "One or more genre IDs do not exist."
                );
            }

            var actorIds = dtoMovie.ActorIds.Distinct().ToArray();
            var actors = await _context
                .Set<Actor>()
                .Where(actor => actorIds.Contains(actor.Id))
                .ToListAsync();

            if (actors.Count != actorIds.Length)
            {
                ModelState.AddModelError(
                    nameof(dtoMovie.ActorIds),
                    "One or more actor IDs do not exist."
                );
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
                Reviews = dtoMovie
                    .Reviews.Select(review => new Review
                    {
                        Rating = review.Rating,
                        ReviewerName = review.ReviewerName,
                        Comment = review.Comment,
                    })
                    .ToList(),
                Details = dtoMovie.Details is null
                    ? null
                    : new MovieDetails
                    {
                        Budget = dtoMovie.Details.Budget,
                        Synopsis = dtoMovie.Details.Synopsis,
                        Language = dtoMovie.Details.Language,
                    },
            };

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetMovie),
                new { id = movie.Id },
                DtoMapper.MovieToDto(movie)
            );
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
}
