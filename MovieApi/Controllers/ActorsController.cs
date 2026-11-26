using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.DTOs.Movies.Read;
using MovieApi.Mapping;
using MovieApi.Models;

[Route("api/[controller]")]
[ApiController]
public class ActorsController : ControllerBase
{
    private readonly MovieApiContext _context;
    public ActorsController(MovieApiContext context)
    {
        _context = context;
    }

    //GET: api/Actor
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ActorDto>>> GetActor()
    {
        var actors = await _context.Actor
            .AsNoTracking().ToListAsync();

        return actors.Select(ActorDtoMapper.ToDto).ToList();
    }

    [HttpGet("/detailed")]
    public async Task<ActionResult<IEnumerable<ActorDetailedDto>>> GetActorDetailed()
    {
        var actors = await _context.Actor
            .AsNoTracking()
            .Include(actor => actor.Movies)
                    .ThenInclude(movie => movie.Genres)
            .ToListAsync();

        return actors.Select(ActorDtoMapper.ToDetailedDto)
            .ToList();

    }


    // GET: api/Actor/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Actor>> GetActor(int id)
    {
        var actor = await _context.Actor.FindAsync(id);

        if (actor == null)
        {
            return NotFound();
        }

        return actor;
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
        _context.Actor.Add(actor);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetActor", new { id = actor.Id }, actor);
    }

    // DELETE: api/Actor/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteActor(int? id)
    {
        var actor = await _context.Actor.FindAsync(id);
        if (actor == null)
        {
            return NotFound();
        }

        _context.Actor.Remove(actor);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ActorExists(int? id)
    {
        return _context.Actor.Any(e => e.Id == id);
    }
}
