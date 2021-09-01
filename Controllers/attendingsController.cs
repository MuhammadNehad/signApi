using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using locationRecordeapi;

namespace locationRecordeapi.Data
{
    [Route("api/[controller]")]
    [ApiController]
    public class attendingsController : ControllerBase
    {
        private readonly locationRecordeapiContext _context;

        public attendingsController(locationRecordeapiContext context)
        {
            _context = context;
        }

        // GET: api/attendings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<attendings>>> Getattendings()
        {
            return await _context.attendings.ToListAsync();
        }

        // GET: api/attendings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<attendings>> Getattendings(int id)
        {
            var attendings = await _context.attendings.FindAsync(id);

            if (attendings == null)
            {
                return NotFound();
            }

            return attendings;
        }

        // PUT: api/attendings/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> Putattendings(int id, attendings attendings)
        {
            if (id != attendings.Id)
            {
                return BadRequest();
            }

            _context.Entry(attendings).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!attendingsExists(id))
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

        // POST: api/attendings
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        //public async Task<ActionResult<String>> Postattendings([FromBody] attendings attendings)
        //{
        //    //_context.attendings.Add(attendings);
        //    //await _context.SaveChangesAsync();
        //    Console.WriteLine( attendings.empKey.ToString() );
        //    return "success";
        //}
        public async Task<ActionResult<attendings>> Postattendings([FromBody] attendings attendings)
        {
            _context.attendings.Add(attendings);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getattendings", new { id = attendings.Id }, attendings);
        }

        // DELETE: api/attendings/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<attendings>> Deleteattendings(int id)
        {
            var attendings = await _context.attendings.FindAsync(id);
            if (attendings == null)
            {
                return NotFound();
            }

            _context.attendings.Remove(attendings);
            await _context.SaveChangesAsync();

            return attendings;
        }

        private bool attendingsExists(int id)
        {
            return _context.attendings.Any(e => e.Id == id);
        }
    }
}
