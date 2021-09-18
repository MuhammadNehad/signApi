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
    public class rolesController : ControllerBase
    {
        private readonly locationRecordeapiContext _context;

        public rolesController(locationRecordeapiContext context)
        {
            _context = context;
        }

        // GET: api/roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<roles>>> Getroles()
        {
            return await _context.roles.ToListAsync();
        }

        // GET: api/roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<roles>> Getroles(int id)
        {
            var roles = await _context.roles.FindAsync(id);

            if (roles == null)
            {
                return NotFound();
            }

            return roles;
        }

        // PUT: api/roles/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> Putroles(int id, roles roles)
        {
            if (id != roles.Id)
            {
                return BadRequest();
            }

            _context.Entry(roles).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!rolesExists(id))
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

        // POST: api/roles
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<roles>> Postroles(roles roles)
        {
            _context.roles.Add(roles);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getroles", new { id = roles.Id }, roles);
        }

        // DELETE: api/roles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<roles>> Deleteroles(int id)
        {
            var roles = await _context.roles.FindAsync(id);
            if (roles == null)
            {
                return NotFound();
            }

            _context.roles.Remove(roles);
            await _context.SaveChangesAsync();

            return roles;
        }

        private bool rolesExists(int id)
        {
            return _context.roles.Any(e => e.Id == id);
        }
    }
}
