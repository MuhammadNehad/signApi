using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using locationRecordeapi;
using locationRecordeapi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace locationRecordeapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class myKeysController : ControllerBase
    {
        private readonly locationRecordeapiContext _context;
        
        public myKeysController(locationRecordeapiContext context)
        {
            _context = context;
        }

        // GET: api/myKeys
        [HttpGet]
        public async Task<ActionResult<IEnumerable<myKeys>>> GetmyKeys()
        {

            return await _context.myKeys.ToListAsync();
        }


        // GET: api/myKeys
        [HttpGet("IPAddress")]

        public  ActionResult<String> GetIP()
        {


                myKeys keys = _context.myKeys.FirstOrDefault();
                return Ok(keys.IPAddress);
       
        }



 


        // GET: api/myKeys/5
        [HttpGet("{id}")]
        public async Task<ActionResult<myKeys>> GetmyKeys(int id)
        {
            var myKeys = await _context.myKeys.FindAsync(id);

            if (myKeys == null)
            {
                return NotFound();
            }

            return myKeys;
        }

        // PUT: api/myKeys/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [AllowAnonymous]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutmyKeys(int id,[FromForm] myKeys myKeys)
        {
            if (id != myKeys.id)
            {
                return BadRequest();
            }

            _context.Entry(myKeys).State = EntityState.Modified;
            _context.Entry(myKeys).Property(e => e.id).IsModified = false;
            if (String.IsNullOrWhiteSpace(myKeys.googleMap_apiKey))
            {
                _context.Entry(myKeys).Property(e => e.googleMap_apiKey).IsModified = false;

            }
            if (String.IsNullOrWhiteSpace(myKeys.googleApp_MailAddress))
            {
                _context.Entry(myKeys).Property(e => e.googleApp_MailAddress).IsModified = false;

            }
            if (String.IsNullOrWhiteSpace(myKeys.googleApp_Password))
            {
                _context.Entry(myKeys).Property(e => e.googleApp_Password).IsModified = false;

            }
            if (String.IsNullOrWhiteSpace(myKeys.IPAddress))
            {
                _context.Entry(myKeys).Property(e => e.IPAddress).IsModified = false;

            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!myKeysExists(id))
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

        // POST: api/myKeys
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<myKeys>> PostmyKeys(myKeys myKeys)
        {
            _context.myKeys.Add(myKeys);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetmyKeys", new { id = myKeys.id }, myKeys);
        }

        // DELETE: api/myKeys/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletemyKeys(int id)
        {
            var myKeys = await _context.myKeys.FindAsync(id);
            if (myKeys == null)
            {
                return NotFound();
            }

            _context.myKeys.Remove(myKeys);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool myKeysExists(int id)
        {
            return _context.myKeys.Any(e => e.id == id);
        }
    }
}
