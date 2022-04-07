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
    public class trafficLogsController : ControllerBase
    {
        private readonly locationRecordeapiContext _context;

        public trafficLogsController(locationRecordeapiContext context)
        {
            _context = context;
        }

        // GET: api/trafficLogs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<trafficLog>>> GettrafficLog()
        {
            return await _context.trafficLog.ToListAsync();
        }

        // GET: api/trafficLogs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<trafficLog>> GettrafficLog(int id)
        {
            var trafficLog = await _context.trafficLog.FindAsync(id);

            if (trafficLog == null)
            {
                return NotFound();
            }

            return trafficLog;
        }

        // PUT: api/trafficLogs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PuttrafficLog(int id, trafficLog trafficLog)
        {
            if (id != trafficLog.Id)
            {
                return BadRequest();
            }

            _context.Entry(trafficLog).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!trafficLogExists(id))
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

        // POST: api/trafficLogs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<trafficLog>> PosttrafficLog([FromForm]trafficLog trafficLog)
        {
            _context.trafficLog.Add(trafficLog);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GettrafficLog", new { id = trafficLog.Id }, trafficLog);
        }

        // DELETE: api/trafficLogs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletetrafficLog(int id)
        {
            var trafficLog = await _context.trafficLog.FindAsync(id);
            if (trafficLog == null)
            {
                return NotFound();
            }

            _context.trafficLog.Remove(trafficLog);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool trafficLogExists(int id)
        {
            return _context.trafficLog.Any(e => e.Id == id);
        }
    }
}
