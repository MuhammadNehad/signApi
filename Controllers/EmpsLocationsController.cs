using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using locationRecordeapi;
using locationRecordeapi.Data;

namespace locationRecordeapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpsLocationsController : ControllerBase
    {
        private readonly locationRecordeapiContext _context;

        public EmpsLocationsController(locationRecordeapiContext context)
        {
            _context = context;
        }

        // GET: api/EmpsLocations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmpsLocation>>> GetEmpsLocation([FromQuery] Emplyees curEmp, [FromQuery] int[] permsid)
        {
            if (!EmplyeesController.checkValidations(_context, curEmp, permsid))
            {
                return StatusCode(400, "check you are registered or have permission");
            }
            return StatusCode(200, await _context.EmpsLocation.ToListAsync());
        }

        // GET: api/EmpsLocations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmpsLocation>> GetEmpsLocation(int id)
        {
            var empsLocation = await _context.EmpsLocation.FindAsync(id);

            if (empsLocation == null)
            {
                return NotFound();
            }

            return empsLocation;
        }
        // GET: api/EmpsLocations/GetEmpsLocationByFK/1
        //[HttpGet("[Action]/{id}")]
        //public async Task<ActionResult<EmpsLocation>> GetEmpsLocationByFK(int empKey)
        //{
        //    var empsLocation = _context.EmpsLocation.Where(l=>l.empKey ==empKey).Last();

        //    if (empsLocation == null)
        //    {
        //        return NotFound();
        //    }

        //    return empsLocation;
        //}
        // PUT: api/EmpsLocations/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmpsLocation(int id, [FromForm] EmpsLocation empsLocation, [FromQuery] Emplyees curEmp, [FromQuery] int[] permsid)
        {
            if (!EmplyeesController.checkValidations(_context, curEmp, permsid))
            {
                return StatusCode(400, "check you are registered or have permission");
            }
            if (id != empsLocation.Id)
            {
                return BadRequest();
            }

            _context.Entry(empsLocation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmpsLocationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(200,new { status= 200 });
        }

        // POST: api/EmpsLocations
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<EmpsLocation>> PostEmpsLocation([FromForm] EmpsLocation empsLocation,[FromQuery]Emplyees curEmp, [FromQuery] int[] permsid)
        {
            if (!EmplyeesController.checkValidations(_context, curEmp, permsid))
            {
                return StatusCode(400, "check you are registered or have permission");
            }
            _context.EmpsLocation.Add(empsLocation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmpsLocation", new { Id = empsLocation.Id }, empsLocation);
        }

        // DELETE: api/EmpsLocations/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<EmpsLocation>> DeleteEmpsLocation(int id)
        {
            var empsLocation = await _context.EmpsLocation.FindAsync(id);
            if (empsLocation == null)
            {
                return NotFound();
            }

            _context.EmpsLocation.Remove(empsLocation);
            await _context.SaveChangesAsync();

            return empsLocation;
        }

        private bool EmpsLocationExists(int id)
        {
            return _context.EmpsLocation.Any(e => e.Id == id);
        }
    }
}
