using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Web_application.Models;

namespace API_Web_application.Controllers
{
    [Route("api/reviews")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly ProjectContext _context;

        public ReviewsController(ProjectContext context)
        {
            _context = context;
        }

        // GET: api/Reviews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetTrivia()
        {
            return await _context.Reviews.ToListAsync();
        }

        // GET
        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);

            if (review == null)
            {
                return NotFound();
            }

            return review;
        }

        // PUT        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReview(int id, Review review)
        {
         

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

        // POST
        [HttpPost]
        public async Task<ActionResult<Review>> PostReview(Review review)
        {
            bool filmExist = _context.Movies.Any(x => x.MovieTitle == review.MovieTitle);
            bool studioExist = _context.Filmstudios.Any(x => x.StudioName == review.StudioName);
            
            if(filmExist == true && studioExist == true) 
            {                                
                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();
            
                return CreatedAtAction("GetReview", new { id = review.Id }, review);                        
            }

            if(filmExist == false)
            {
                return Ok("Filmen finns inte registrerad i databasen.");
            }

            if(studioExist == false)
            {
                return Ok("Filmstudion finns inte registrerad i databasen.");
            }

            return BadRequest();
                        
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<ActionResult<Review>> DeleteReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return review;
        }

        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }
    }
}
