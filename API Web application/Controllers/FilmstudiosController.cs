using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Web_application.Models;

namespace API_Web_application.Controllers
{
    [Route("api/filmstudios")]
    [ApiController]
    public class FilmstudioController : ControllerBase
    {
        private readonly ProjectContext _context;

        public FilmstudioController(ProjectContext context)
        {
            _context = context;
        }

        // GET
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Filmstudio>>> GetFilmstudios()
        {
            var filmstudioDb = from x in _context.Filmstudios
                               select x;

            if( !filmstudioDb.Any()) {
                return Ok("Finns inga biografer registerade.");
            }




            return await _context.Filmstudios.ToListAsync();
        }
       
        // PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFilmstudio(int id, Filmstudio filmstudio)
        {
            if (id != filmstudio.Id)
            {
                return BadRequest();
            }

            _context.Entry(filmstudio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FilmstudioExists(id))
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

        // POST: api/Filmstudios    
        [HttpPost("add")]
        public async Task<ActionResult<Filmstudio>> PostFilmstudio(Filmstudio filmstudio)
        {

            bool nameExist = _context.Filmstudios.Any(f => f.Name == filmstudio.Name);



            if (filmstudio.Name != null && nameExist == false)
            {
                Movie movie = new Movie();

                _context.Filmstudios.Add(filmstudio);

                await _context.SaveChangesAsync();

                return Ok("Studion har lagts till.");



            }

            if (nameExist == true)
                return Ok("Filmstudion \"" + filmstudio.Name + "\" finns redan i databasen.");

            if (filmstudio.Name == null)
                return Ok("Du måste skriva in namnet på studion.");

            if (filmstudio.Location == null)
                return Ok("Du måste skriva in filmstudions plats.");



            return Ok("Ingen studio inskriven.");


        }

        private bool FilmstudioExists(int id)
        {
            return _context.Filmstudios.Any(e => e.Id == id);
        }
    }
}
