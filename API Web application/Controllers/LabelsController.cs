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
        private readonly ProjectContext _context;

        public LabelsController(ProjectContext context)
        {
            _context = context;
        }

        // GET
        [HttpGet]
        [Produces("application/xml")]
        public async Task<ActionResult<IEnumerable<Label>>> GetPostLabels()
        {

            var xml = await _context.PostLabels.FirstOrDefaultAsync();                      

            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(xml.GetType());

            return await _context.PostLabels.ToListAsync();
        }

        // GET: api/Labels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Label>> GetLabel(int id)
        {
            var label = await _context.PostLabels.FindAsync(id);

            if (label == null)
            {
                return NotFound();
            }

            return label;
        }

        // PUT: api/Labels/5
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
        [HttpPost]        
        public async Task<ActionResult<Label>> PostLabel(Label label)
        {

            DateTime date = DateTime.Now;

            label.Date = date;
            
            _context.PostLabels.Add(label);
            await _context.SaveChangesAsync();

        
            
            
            
            return CreatedAtAction("GetLabel", new { id = label.Id }, label);
        
        
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
