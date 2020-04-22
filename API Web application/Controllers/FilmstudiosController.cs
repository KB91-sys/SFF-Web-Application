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
       
        // PUT CHANGE NAME AND LOCATION OF SPECIFIC FILMSTUDIO
        [HttpPut]
        public async Task<IActionResult> PutFilmstudio(Filmstudio filmstudio)
        {
            // CHECK ID
            bool checkIfIdExist = _context.Filmstudios.Any(x => x.Id == filmstudio.Id);


            if(checkIfIdExist == true) 
            {
                var filmstudioObj = from x in _context.Filmstudios
                                    where x.Id == filmstudio.Id
                                    select x;

                foreach (var x in filmstudioObj)
                {
                    x.StudioName = filmstudio.StudioName;
                    x.Location = filmstudio.Location;

                    
                }

                await _context.SaveChangesAsync();


                return Ok(filmstudio);


            }


            if (checkIfIdExist == false)
            {

                return Ok("Kunde inte hitta en filmstudio med id:t i databasen.");
            }

            if (filmstudio.StudioName == null)
            {

                return Ok("Du mååste skriva in ett nytt namn på filmstudion.");
            }

            if (filmstudio.Location == null)
            {
                return Ok("Filmstudions ort måste skrivas in.");

            }

            return BadRequest();





        }

        // POST: api/Filmstudios    
        [HttpPost]
        public async Task<ActionResult<Filmstudio>> PostFilmstudio(Filmstudio filmstudio)
        {

            bool nameExist = _context.Filmstudios.Any(f => f.StudioName == filmstudio.StudioName);



            if (filmstudio.StudioName != null && nameExist == false)
            {
                Movie movie = new Movie();

                _context.Filmstudios.Add(filmstudio);

                await _context.SaveChangesAsync();

                return Ok("Studion har lagts till.");



            }

            if (nameExist == true)
                return Ok("Filmstudion \"" + filmstudio.StudioName + "\" finns redan i databasen.");

            if (filmstudio.StudioName == null)
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
