using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.DTOs.Create;
using MovieApi.DTOs.Read;
using MovieApi.Mapping;
using MovieApi.Models;

namespace MovieApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController(MovieApiContext context) : ControllerBase
    {
        private readonly MovieApiContext _context = context;

        // GET: api/Review
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReview()
        {
            var reviews = await _context.Reviews.AsNoTracking().ToListAsync();

            return reviews.Select(DtoMapper.ReviewToDto).ToList();
        }

        // GET: api/Review/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReviewDto>> GetReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);

            if (review == null)
            {
                return NotFound();
            }

            ReviewDto reviewDto = DtoMapper.ReviewToDto(review);
            return reviewDto;
        }

        // PUT: api/Review/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReview(int? id, Review review)
        {
            if (id != review.Id)
            {
                return BadRequest();
            }

            _context.Entry(review).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(id))
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

        [HttpPost]
        public async Task<ActionResult<ReviewDto>> PostReview(CreateReviewDto reviewDto)
        {
            var movie = await _context.Movies.FindAsync(reviewDto.MovieId);

            if (movie is null)
            {
                return NotFound($"Ingen film hittades med id {reviewDto.MovieId}.");
            }

            var review = new Review
            {
                Rating = reviewDto.Rating,
                ReviewerName = reviewDto.ReviewerName,
                Comment = reviewDto.Comment,
                Movie = movie,
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                "GetReview",
                new { id = review.Id },
                DtoMapper.ReviewToDto(review)
            );
        }

        // DELETE: api/Review/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int? id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReviewExists(int? id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }
    }
}
