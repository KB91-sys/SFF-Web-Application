using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Web_application.Models;

namespace API_Web_application.Controllers
{
    [Route("api/loan")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly MovieContext _context;

        //private readonly MovieContext _movieContext;
        public LoansController(MovieContext context)
        {
            _context = context;


        }

        // GET
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Loan>>> GetFilmstudioLoans()
        {
            var loanDb = from x in _context.FilmstudioLoans
                               select x;
            

            if(!loanDb.Any()){
                return Ok("Inga filmer är utlånade för tillfället.");            
            }



            return await _context.FilmstudioLoans.ToListAsync();
        
        
        
        }


        // PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFilmstudioLoan(int id, Loan loan)
        {
            if (id != loan.Id)
            {
                return BadRequest();
            }

            _context.Entry(loan).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FilmstudioLoanExists(id))
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

        // POST BORROW MOVIE    
        [HttpPost("borrow")]
        public async Task<ActionResult<Loan>> BorrowMovies(Loan loan)
        {

            bool filmExist = _context.Movies.Any(x => x.MovieTitle == loan.MovieBorrowed);
            bool studioExist = _context.Filmstudios.Any(x => x.Name == loan.StudioName);
            bool maxAmount = _context.Movies.Any(x => x.MaxLoanAmount == 0);


            // If movies doesnt already exist in database and movietitle isnt null
            if (studioExist == true && filmExist == true)
            {
                // Change the value of maxAmount in Movie database where movietitle == movieborrowed
                var changeValue = from x in _context.Movies
                                  where x.MovieTitle == loan.MovieBorrowed
                                  select x;
                
                // Reduce value by one
                foreach (var x in changeValue) 
                {
                    if(x.MaxLoanAmount != 0) 
                    {
                        x.MaxLoanAmount--;
                        x.LentUnits++;
                    }
                    else if(x.MaxLoanAmount == 0)
                    {
                        return Ok("Filmen får ej lånas ut fler gånger.");
                    
                    }
                }


                _context.FilmstudioLoans.Add(loan);
            

                    await _context.SaveChangesAsync();
                    return Ok(changeValue);


            }

        
            if (loan.StudioName == null)
                return Ok("Du måste skriva in vilken filmsudio som ska låna filmen");

            /*if (exists == false)
                return Ok("Filmen finns ej i databasen.");*/


            if (loan.MovieBorrowed == null)
                return Ok("Titlen på filmen får ej vara tom.");



            return BadRequest();

        }

        [HttpPost("returnmovie")]
        public async Task<ActionResult<Loan>> ReturnMovie(Loan filmstudios)
        {
            bool movieExist = _context.Movies.Any(m => m.MovieTitle == filmstudios.MovieBorrowed);
            bool studioExist = _context.Filmstudios.Any(n => n.Name == filmstudios.StudioName);

            if(movieExist == true && studioExist == true)
            {
                var loanObject = (from x in _context.FilmstudioLoans
                                  where x.StudioName == filmstudios.StudioName && x.MovieBorrowed == filmstudios.MovieBorrowed
                                  select x).FirstOrDefault();

                _context.FilmstudioLoans.Remove(loanObject);

                var filmObj = from x in _context.Movies
                              where x.MovieTitle == filmstudios.MovieBorrowed
                              select x;

                foreach(var x in filmObj) 
                {
                    x.MaxLoanAmount++;
                    x.LentUnits--;                               
                }

                await _context.SaveChangesAsync();

                return Ok("Filmen \"" + filmstudios.MovieBorrowed + "\" är återlämnad.");
            }

            if (filmstudios.MovieBorrowed == null)
            {
                return Ok("Du måste skriva in en film.");
            }

            if(filmstudios.StudioName == null)
            {
                return Ok("Du måste skriva in en filmstudio.");
            }

            if(movieExist == false){
                return Ok("Filmen finns ej registrerad som utlånad.");
            }
            
            if(studioExist == false){
                return Ok("Studion har ej lånat denna film.");
            }


            return BadRequest();
        
        }




        // DELETE: api/FilmstudioLoans/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Loan>> DeleteFilmstudioLoan(int id)
        {
            var filmstudioLoan = await _context.FilmstudioLoans.FindAsync(id);
            if (filmstudioLoan == null)
            {
                return NotFound();
            }

            _context.FilmstudioLoans.Remove(filmstudioLoan);
            await _context.SaveChangesAsync();

            return filmstudioLoan;
        }

        private bool FilmstudioLoanExists(int id)
        {
            return _context.FilmstudioLoans.Any(e => e.Id == id);
        }
    }
}
