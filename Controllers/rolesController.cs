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
        public async Task<ActionResult<IEnumerable<object>>> Getroles()
        {
            return  await _context.roles.
                Select(r=>
                new { r.Id
                ,r.name
                ,r.proleId
                ,
                    roles_perms = _context.roles_perms_rel.Where(ropre=>ropre.role_id==r.Id)
                .Select(rpr =>
                new {rpr.id,rpr.perm_id,rpr.role_id}
                ).ToList()}).ToListAsync();
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
        public async Task<IActionResult> Putroles(int id,[FromForm] roles roles, [FromQuery] Emplyees curEmp, [FromQuery] int[] permsid)
        {
            if (!EmplyeesController.checkValidations(_context, curEmp, permsid))
            {
                return StatusCode(400, "check you are registered or have permission");
            }
            if (id != roles.Id)
            {
                return BadRequest();
            }

            _context.Entry(roles).State = EntityState.Modified;
            _context.Entry(roles).Property(e => e.Id).IsModified = false;
            if (String.IsNullOrWhiteSpace(roles.name))
            {
                _context.Entry(roles).Property(e => e.name).IsModified = false;

            }

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
        public async Task<ActionResult<roles>> Postroles([FromForm] roles roles)
        {
            _context.roles.Add(roles);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getroles", new { id = roles.Id }, roles);
        }

        // DELETE: api/roles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<roles>> Deleteroles(int id, [FromQuery] Emplyees curEmp, [FromQuery] int[] permsid)
        {
            if (!EmplyeesController.checkValidations(_context, curEmp, permsid))
            {
                return StatusCode(400, "check you are registered or have permission");
            }
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
