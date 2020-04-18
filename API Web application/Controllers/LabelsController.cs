using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using API_Web_application.Models;

namespace API_Web_application.Controllers
{
    [Route("api/labels")]
    [ApiController]
    public class LabelsController : ControllerBase
    {
        private readonly MovieContext _context;

        public LabelsController(MovieContext context)
        {
            _context = context;
        }

        // GET
        [HttpGet]
        [Produces("application/xml")]
        public async Task<ActionResult<IEnumerable<Label>>> GetPostLabels()
        {

            var xml = await _context.PostLabels.FirstOrDefaultAsync();
                      
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(xml.GetType());

            await _context.SaveChangesAsync();

            return await _context.PostLabels.ToListAsync();
        }

        // PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLabel(int id, Label label)
        {
            if (id != label.Id)
            {
                return BadRequest();
            }

            _context.Entry(label).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LabelExists(id))
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
        [HttpPost("add")]        
        public async Task<ActionResult<Label>> PostLabel(Label label)
        {

            


            DateTime date = DateTime.UtcNow;

            label.Date = date;
            
            _context.PostLabels.Add(label);
            await _context.SaveChangesAsync();

                    
            
            
            return CreatedAtAction(nameof(label), new { id = label.Id }, label);
        
        
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<ActionResult<Label>> DeleteLabel(int id)
        {
            var label = await _context.PostLabels.FindAsync(id);
            if (label == null)
            {
                return NotFound();
            }

            _context.PostLabels.Remove(label);
            await _context.SaveChangesAsync();

            return label;
        }

        private bool LabelExists(int id)
        {
            return _context.PostLabels.Any(e => e.Id == id);
        }
    }
}
