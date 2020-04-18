﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using API_Web_application.Models;


namespace SFF_API.Controllers

{

    [Route("api/movies")]

    [ApiController]
    public class MoviesController : ControllerBase
    {

        private readonly MovieContext _context;

        public MoviesController(MovieContext context)
        {
            _context = context;
        }



        // GET

        [HttpGet]

        public async Task<ActionResult<IEnumerable<Movie>>> GetMovie()
        {
            var movieDb = from x in _context.Movies
                          select x;

            if( !movieDb.Any())
            {
                return Ok("Finns inga filmer registrerade.");
            }


            return await _context.Movies.ToListAsync();

        }

        // GET
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovies(int id)
        {

            var movies = await _context.Movies.FindAsync(id);

            bool existInDatabase = _context.Movies.Any(t => t.Id == id);


            if (movies == null)
            {
                return Content("Kunde ej hitta id:t " + movies.Id + " i databasen");

            }
            else
            {

                return movies;
            }

        }



        // POST
        [HttpPost("add")]
        public async Task<ActionResult<Movie>> PostMovies(Movie movies)
        {

            // Check if title exist in database 
            bool exists = _context.Movies.Any(t => t.MovieTitle == movies.MovieTitle);




            // If movies doesnt already exist in database and movietitle isnt null => Add movie
            if (movies.MovieTitle != null && exists == false && movies.MaxLoanAmount != 0)
            {

                
               
                var message = CreatedAtAction(nameof(GetMovies), new { id = movies.Id }, movies);
                _context.Add(movies);
                await _context.SaveChangesAsync();
                  
                
                return message;
            
            
            }
                       
            if (movies.MovieTitle == null)
                return Ok("Titlen på filmen får ej vara tom.");


            if (exists == true)
                return Ok("Filmtiteln " + movies.MovieTitle + " finns redan i databasen.");

            if (movies.MaxLoanAmount == 0)
                return Ok("Du måste skriva in hur många gånger filmen får lånas ut.");
         
                return BadRequest();
           
        }
        // DELETE

        [HttpDelete("{id}")]
        public async Task<ActionResult<Movie>> DeleteMovies(int id)
        {

            var movies = await _context.Movies.FindAsync(id);

            if (movies == null)

            {

                return NotFound();

            }



            _context.Movies.Remove(movies);

            await _context.SaveChangesAsync();



            return movies;

        }



        private bool MoviesExists(int id)

        {

            return _context.Movies.Any(e => e.Id == id);

        }




    }

}