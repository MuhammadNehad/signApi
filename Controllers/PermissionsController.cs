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
    public class PermissionsController : ControllerBase
    {
        private readonly locationRecordeapiContext _context;

        public PermissionsController(locationRecordeapiContext context)
        {
            _context = context;
        }

        // GET: api/Permissions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Permissions>>> GetPermissions()
        {
            return StatusCode(200,await _context.Permissions.ToListAsync());
        }

        // GET: api/Permissions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Permissions>> GetPermissions(int id)
        {
            var permissions = await _context.Permissions.FindAsync(id);

            if (permissions == null)
            {
                return NotFound();
            }

            return permissions;
        }


        [HttpGet("[action]")]
        public ActionResult<List<int>> getByName([FromQuery] string[] permsNames)
        {
            List<int> permsList = new List<int>();

            foreach (string perm in permsNames)
            {
                int? permissions = _context.Permissions.Where(per => per.name == perm).Select(per=> per.Id).FirstOrDefault();
                if (permissions == null)
                {
                    return StatusCode(400,new { status=400,message="one or more Permissions are not valid."});
                }
                permsList.Add((int)permissions);
            }


            return StatusCode(200, new { status= 200 ,permsL=permsList});
        }

        // PUT: api/Permissions/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPermissions(int id, Permissions permissions)
        {
            if (id != permissions.Id)
            {
                return BadRequest();
            }

            _context.Entry(permissions).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PermissionsExists(id))
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

        // POST: api/Permissions
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Permissions>> PostPermissions(Permissions permissions)
        {
            _context.Permissions.Add(permissions);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PermissionsExists(permissions.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPermissions", new { id = permissions.Id }, permissions);
        }

        // DELETE: api/Permissions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Permissions>> DeletePermissions(string id)
        {
            var permissions = await _context.Permissions.FindAsync(id);
            if (permissions == null)
            {
                return NotFound();
            }

            _context.Permissions.Remove(permissions);
            await _context.SaveChangesAsync();

            return permissions;
        }

        private bool PermissionsExists(int id)
        {
            return _context.Permissions.Any(e => e.Id == id);
        }
    }
}
