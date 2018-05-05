using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElectronicJournal.Models;

namespace ElectronicJournal.Controllers
{
    [Produces("application/json")]
    [Route("api/Missings")]
    public class MissingsController : Controller
    {
        private readonly ElectronicJournalContext _context;

        public MissingsController(ElectronicJournalContext context)
        {
            _context = context;
        }

        // GET: api/Missings
        [HttpGet]
        public IEnumerable<Missing> GetMissing()
        {
            return _context.Missing;
        }

        // GET: api/Missings/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMissing([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var missing = await _context.Missing.SingleOrDefaultAsync(m => m.ID == id);

            if (missing == null)
            {
                return NotFound();
            }

            return Ok(missing);
        }

        // PUT: api/Missings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMissing([FromRoute] int id, [FromBody] Missing missing)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != missing.ID)
            {
                return BadRequest();
            }

            _context.Entry(missing).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MissingExists(id))
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

        // POST: api/Missings
        [HttpPost]
        public async Task<IActionResult> PostMissing([FromBody] Missing missing)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Missing.Add(missing);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMissing", new { id = missing.ID }, missing);
        }

        // DELETE: api/Missings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMissing([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var missing = await _context.Missing.SingleOrDefaultAsync(m => m.ID == id);
            if (missing == null)
            {
                return NotFound();
            }

            _context.Missing.Remove(missing);
            await _context.SaveChangesAsync();

            return Ok(missing);
        }

        private bool MissingExists(int id)
        {
            return _context.Missing.Any(e => e.ID == id);
        }
    }
}