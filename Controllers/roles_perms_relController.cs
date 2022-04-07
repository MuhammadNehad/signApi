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
    public class roles_perms_relController : ControllerBase
    {
        private readonly locationRecordeapiContext _context;

        public roles_perms_relController(locationRecordeapiContext context)
        {
            _context = context;
        }

        // GET: api/roles_perms_rel
        [HttpGet]
        public async Task<ActionResult<IEnumerable<roles_perms_rel>>> Getroles_perms_rel()
        {
            return await _context.roles_perms_rel.ToListAsync();
        }

        // GET: api/roles_perms_rel/5
        [HttpGet("{id}")]
        public async Task<ActionResult<roles_perms_rel>> Getroles_perms_rel(int id)
        {
            var roles_perms_rel = await _context.roles_perms_rel.FindAsync(id);

            if (roles_perms_rel == null)
            {
                return NotFound();
            }

            return roles_perms_rel;
        }

        // PUT: api/roles_perms_rel/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> Putroles_perms_rel(int id, roles_perms_rel roles_perms_rel)
        {
            if (id != roles_perms_rel.id)
            {
                return BadRequest();
            }

            _context.Entry(roles_perms_rel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!roles_perms_relExists(id))
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

        // POST: api/roles_perms_rel
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<roles_perms_rel>> Postroles_perms_rel([FromForm] roles_perms_rel roles_perms_rel)
        {
            var existScalar= _context.roles_perms_rel.Where(rpr => rpr.perm_id == roles_perms_rel.perm_id && rpr.role_id == roles_perms_rel.role_id);
            if(existScalar.Count() > 0)
            {
                return StatusCode(500, new { failed = "can't Save Permission to same user twice" });                     
            }
            if(!ModelState.IsValid)
            {
                throw new Exception("Not Valid");
            }
            //roles_perms_rel.role = await _context.roles.FindAsync(roles_perms_rel.role_id);
            try {

                    var resultcheck = _context.roles.Select(ri=>new { ri.Id,ri.name,ri.proleId }).Where(r => r.Id == roles_perms_rel.role_id).FirstOrDefault();
                    
                    if (resultcheck != null) { 
                        await _context.roles_perms_rel.AddAsync(roles_perms_rel);
                        await _context.SaveChangesAsync();
                    }
                
            }
            catch (Exception ex)
            {

                    throw;
            }
            return CreatedAtAction("Getroles_perms_rel", new { id = roles_perms_rel.id }, roles_perms_rel);
        }

        // DELETE: api/roles_perms_rel/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<roles_perms_rel>> Deleteroles_perms_rel(int id)
        {
            var roles_perms_rel = await _context.roles_perms_rel.FindAsync(id);
            if (roles_perms_rel == null)
            {
                return NotFound();
            }

            _context.roles_perms_rel.Remove(roles_perms_rel);
            await _context.SaveChangesAsync();

            return roles_perms_rel;
        }

        private bool roles_perms_relExists(int id)
        {
            return _context.roles_perms_rel.Any(e => e.id == id);
        }
    }
}
