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
    [Route("api/filmstudio")]
    [ApiController]
    public class FilmstudioController : ControllerBase
    {
        private readonly MovieContext _context;

        public FilmstudioController(MovieContext context)
        {
            _context = context;
        }

        // GET
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Filmstudio>>> GetFilmstudios()
        {
            return await _context.Filmstudios.ToListAsync();
        }

        // GET: api/Filmstudios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Filmstudio>> GetFilmstudio(int id)
        {
            var filmstudio = await _context.Filmstudios.FindAsync(id);

            if (filmstudio == null)
            {
                return NotFound();
            }

            return filmstudio;
        }

        // PUT: api/Filmstudios/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
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
        [HttpPost]
        public async Task<ActionResult<Filmstudio>> PostFilmstudio(Filmstudio filmstudio)
        {

            bool nameExist = _context.Filmstudios.Any(f => f.Name == filmstudio.Name);



            if (filmstudio.Name != null && nameExist == false)
            {
                Movie movie = new Movie();

                _context.Filmstudios.Add(filmstudio);

                await _context.SaveChangesAsync();

                return Content("Studion har lagts till.");



            }

            else if (nameExist == true)
                return Content("Filmstudion \"" + filmstudio.Name + "\" finns redan i databasen.");




            return Content("Ingen studio inskriven.");


        }

        private bool FilmstudioExists(int id)
        {
            return _context.Filmstudios.Any(e => e.Id == id);
        }
    }
}
