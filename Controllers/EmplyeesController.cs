using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using locationRecordeapi;
using locationRecordeapi.Data;
using System.Security.Cryptography;
using System.Text;

namespace locationRecordeapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmplyeesController : ControllerBase
    {
        private readonly locationRecordeapiContext _context;

        public EmplyeesController(locationRecordeapiContext context)
        {
            _context = context;
        }

        // GET: api/Emplyees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Emplyees>>> GetEmplyees([FromQuery] Emplyees curEmp, [FromQuery] int[] permsid)
        {
            try {
                if (!checkValidations(_context, curEmp, permsid))
                {
                    return StatusCode(400, "check you are registered or have permission");
                }
                return await _context.Emplyees.AsNoTracking().ToListAsync();
            }catch(Exception e)
            {
                return new List<Emplyees>();
            }
            }

        // GET: api/Emplyees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Emplyees>> GetEmplyees(int id, [FromQuery] Emplyees curEmp, [FromQuery] int[] permsid)
        {
            if (!checkValidations(_context, curEmp, permsid))
            {
                return StatusCode(400, "check you are registered or have permission");
            }
            //var emplyees = await _context.Emplyees.SingleAsync(e => e.id == id);
            var emplyees = await _context.Emplyees.FindAsync(id);

            //var emplyees = _context.Emplyees.Where(e=>e.id == id).First();
            //_context.Entry(emplyees).State = EntityState.Detached;
            if (emplyees == null)
            {
                return NotFound();
            }

            return emplyees;
        }

        // GET: api/Emplyees/+223111
        [HttpGet("[action]/{phone}")]
        public async Task<ActionResult<Emplyees>> GetEmplyeesByPhone(string phone)
        {
            var emplyees = await _context.Emplyees.FindAsync(phone);

            if (emplyees == null)
            {
                return NotFound();
            }

            return emplyees;
        }


        // GET: api/Emplyees/+223111
        [HttpGet("[action]")]
        public ActionResult<object> GetEmplyeesWithattendings([FromQuery] Emplyees curEmp, [FromQuery] int[] permsid)
        {
            if(!checkValidations(_context,curEmp, permsid))
            {
                return StatusCode(400, "check you are registered or have permission");
            }

            object emplyees = _context.Emplyees.Select(e=>new {e.id,e.email,e.empCode,e.locationKey,e.name,e.phone,mrole= _context.roles.Where(rs=>rs.Id == e.role).First(),mAttendings
            = _context.attendings.Where(at => at.atdt.Date == DateTime.Now.Date && at.empKey == e.id)
            .Select(at=>new { at.Id,at.empKey,at.entering,at.atdt,at.locationKey,at.leaveAfter,location=_context.EmpsLocation.Where(loc=>loc.Id==at.locationKey)
            .FirstOrDefault()}).ToList()
           
            }).ToList();

            if (emplyees == null)
            {
                return NotFound();
            }

            return StatusCode(200, emplyees );
        }
        // PUT: api/Emplyees/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmplyees(int id,[FromForm] Emplyees emplyees, [FromQuery] Emplyees curEmp, [FromQuery] int[] permsid)
        {
            if (!checkValidations(_context, curEmp, permsid))
            {
                return StatusCode(400, "check you are registered or have permission");
            }

            if (id != emplyees.id)
            {
                return BadRequest();
            }

            _context.Entry(emplyees).State = EntityState.Modified;
            _context.Entry(emplyees).Property(e => e.password).IsModified = false;
            _context.Entry(emplyees).Property(e => e.id).IsModified = false;
            if(emplyees.email == null)
            { 

            _context.Entry(emplyees).Property(e => e.email).IsModified = false;

            }
            if (emplyees.phone == null)
            {

                _context.Entry(emplyees).Property(e => e.phone).IsModified = false;

            }
            if (emplyees.role == null)
            {

                _context.Entry(emplyees).Property(e => e.role).IsModified = false;

            }
            if (emplyees.locationKey == null)
            {

                _context.Entry(emplyees).Property(e => e.locationKey).IsModified = false;

            }
            if (emplyees.name == null)
            {

                _context.Entry(emplyees).Property(e => e.name).IsModified = false;

            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmplyeesExists(id))
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

        // POST: api/Emplyees
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Emplyees>> PostEmplyees([FromForm]Emplyees emplyees, [FromQuery] Emplyees curEmp, [FromQuery] int[] permsid)
        {
            if (!checkValidations(_context, curEmp, permsid))
            {
                return StatusCode(400, "check you are registered or have permission");
            }

            if (string.IsNullOrEmpty(emplyees.password.Trim()))
            {
                return NoContent();
            }
            emplyees.password = Encrypt(emplyees.password);
            
            _context.Emplyees.Add(emplyees);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmplyees", new { id = emplyees.id }, emplyees);
        }

        // POST: api/Emplyees/Login

        [HttpGet("[action]/{empCode}/{password}")]
        public ActionResult<object> Login(string empCode,string password)
        {
            string passenc = Encrypt(password);
            object e = _context.Emplyees.Where(em => em.empCode == empCode && em.password == passenc).Select(e=>new { e.id,e.email,e.empCode,e.locationKey,e.name
                ,e.phone,
                mrole= _context.roles.Where(r=>r.Id ==e.role).Select(r=> new {
                    r.Id,r.name,
                    roles_perms_rel = _context.roles_perms_rel.Where(rpr => rpr.role_id == r.Id).Select(rpr => new
                    {
                        rpr.id,
                        rpr.perm_id,
                        rpr.role_id,
                        rpr.perm
                    }).ToList()
                }).ToList() ,

            }).ToList().First();
            
            return StatusCode(200, e);
        }
        // DELETE: api/Emplyees/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Emplyees>> DeleteEmplyees(int id, [FromQuery] Emplyees curEmp, [FromQuery] int[] permsid)
        {
            if (!checkValidations(_context, curEmp, permsid))
            {
                return StatusCode(400, "check you are registered or have permission");
            }

            var emplyees = await _context.Emplyees.FindAsync(id);
            if (emplyees == null)
            {
                return NotFound();
            }

            _context.Emplyees.Remove(emplyees);
            await _context.SaveChangesAsync();

            return emplyees;
        }


        private bool EmplyeesExists(int id)
        {
            return _context.Emplyees.Any(e => e.id == id);
        }

        public static bool checkValidations(locationRecordeapiContext context , Emplyees curEmp ,  int[] permsid)
        {
            Emplyees emp = context.Emplyees.Where(em => em.id == curEmp.id && em.empCode == curEmp.empCode).FirstOrDefault();
            if (emp == null)
            { return false; }
            roles role = context.roles.Where(ro => ro.Id == emp.role).FirstOrDefault();
            if (role == null)
            {
                return false;
            }

            List<roles_perms_rel> perm = context.roles_perms_rel.Where(rpr => rpr.role_id == role.Id).ToList();
            if (perm.Count() <= 0)
            {
                return false;

            }
            List<int> permsValidIds = perm.Select(rpr => rpr.perm_id).ToList();

            if (permsid.Except(permsValidIds).Any())
            {
                return false;
            }

            return true;
        }
        static string Encrypt(string pass)
        {
            using(MD5 md5 = MD5.Create())
            {
                UTF8Encoding utf8 = new UTF8Encoding();
                byte[] data = md5.ComputeHash(utf8.GetBytes(pass));
                return Convert.ToBase64String(data);
            }
        }
    }
}
