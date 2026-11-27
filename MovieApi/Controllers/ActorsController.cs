using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.DTOs.Read;
using MovieApi.Mapping;
using MovieApi.Models;

namespace MovieApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorsController(MovieApiContext context) : ControllerBase
    {
        private readonly MovieApiContext _context = context;

        //GET: api/Actor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActorDetailedDto>>> GetActor(
            [FromQuery] bool includeMovies = false
        )
        {
            IQueryable<Actor> query = _context.Actors.AsNoTracking();

            if (includeMovies)
            {
                query = query.Include(actor => actor.Movies).ThenInclude(movie => movie.Genres);
            }
            var actors = await query.ToListAsync();

            return actors.Select(DtoMapper.ActorToDetailedDto).ToList();
        }

        [HttpGet("/detailed")]
        public async Task<ActionResult<IEnumerable<ActorDetailedDto>>> GetActorDetailed()
        {
            var actors = await _context
                .Actors.AsNoTracking()
                .Include(actor => actor.Movies)
                    .ThenInclude(movie => movie.Genres)
                .ToListAsync();

            return actors.Select(DtoMapper.ActorToDetailedDto).ToList();
        }

        // GET: api/Actor/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ActorDto>> GetActor(int id)
        {
            var actor = await _context.Actors.FindAsync(id);

            if (actor == null)
            {
                return NotFound();
            }
            ActorDto actorDto = DtoMapper.ActorToDto(actor);

            return actorDto;
        }

        // PUT: api/Actor/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActor(int? id, Actor actor)
        {
            if (id != actor.Id)
            {
                return BadRequest();
            }

            _context.Entry(actor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActorExists(id))
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

        // POST: api/Actor
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Actor>> PostActor(Actor actor)
        {
            _context.Actors.Add(actor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetActor", new { id = actor.Id }, actor);
        }

        // DELETE: api/Actor/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActor(int? id)
        {
            var actor = await _context.Actors.FindAsync(id);
            if (actor == null)
            {
                return NotFound();
            }

            _context.Actors.Remove(actor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ActorExists(int? id)
        {
            return _context.Actors.Any(e => e.Id == id);
        }
    }
}
