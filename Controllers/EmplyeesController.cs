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
using System.Net.Mail;
using System.Net;
using Newtonsoft.Json.Linq;

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
        public ActionResult<object> GetEmplyeesWithattendings([FromQuery] Emplyees curEmp, [FromQuery] int[] permsid,[FromQuery] DateTime from,[FromQuery]DateTime To,[FromQuery]string empName,[FromQuery] string period,[FromQuery] int parentId)
        {
            if (!checkValidations(_context, curEmp, permsid))
            {
                return StatusCode(400, "check you are registered or have permission");
            }
            DateTime currentdt = DateTime.UtcNow.AddHours(2);


            var emplyeesAttendings =  _context.attendings.Select(ea => new {
                ea.Id,
                ea.atdt,
                ea.empKey,
                ea.locationKey,
                ea.entering,
                ea.leaveAfter,
                aemplyee = _context.Emplyees.Select(e => new
                {
                    e.id,
                    e.email,
                    e.empCode,
                    e.name,
                    e.phone,
                    e.role,
                    mrole = _context.roles.Where(rs => rs.Id == e.role && rs.proleId == parentId).FirstOrDefault()
                }).AsEnumerable().Where(emp => (emp.id == ea.empKey && emp.name.Contains(empName??"") &&emp.mrole != null) ).FirstOrDefault(),
                location = _context.EmpsLocation.Where(loc => loc.Id == ea.locationKey)
                         .FirstOrDefault()


            }).AsEnumerable().Where(eas=>(eas.atdt >= from && eas.atdt <= To && eas.aemplyee != null)).ToList();
            dynamic linqq = null;

             if (period == "month")
            {
                linqq = emplyeesAttendings.GroupBy(g => new { g.atdt.Month, g.empKey });
            }
            else if (period == "year")
            {
                linqq = emplyeesAttendings.GroupBy(g => new { g.atdt.Year, g.empKey });
            }
            else
            {
                linqq = emplyeesAttendings.GroupBy(g => new { g.atdt.Date, g.empKey });

            }
            //

            if (linqq == null)
            {
                return NotFound();
            }

            return StatusCode(200, linqq);
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
            _context.Entry(emplyees).Property(e => e.id).IsModified = false;
           if(String.IsNullOrWhiteSpace(emplyees.password))
            {
                _context.Entry(emplyees).Property(e => e.password).IsModified = false;

            }
            else
            {
                emplyees.password = Encrypt(emplyees.password);
            }
            if (String.IsNullOrWhiteSpace(emplyees.email))
            { 

            _context.Entry(emplyees).Property(e => e.email).IsModified = false;

            }
            if (String.IsNullOrWhiteSpace(emplyees.phone))
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
            if (String.IsNullOrWhiteSpace(emplyees.name))
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
        [HttpGet("[action]")]
        public bool sentForgotPassEmail([FromQuery]string[] emailData,[FromQuery]string toEmail)
        {
          Emplyees emp=  _context.Emplyees.FirstOrDefault(et => et.email == toEmail);
            if(emp != null)
            {
                Random rnd = new Random();
                Byte[] random = new Byte[100];
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                rng.GetBytes(random);
                List<Byte> randomList = random.ToList();
                Byte[] empCodeByte = Encoding.UTF8.GetBytes(emp.empCode);

                randomList.InsertRange(rnd.Next(1, 100), empCodeByte);
               
                string empbase64Code = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(Convert.ToBase64String(empCodeByte)))+"&key="+Convert.ToBase64String(randomList.ToArray())+"&Exp="+Convert.ToBase64String(Encoding.UTF8.GetBytes(DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss")));
                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(emailData[0]);
                mm.To.Add(toEmail);
                mm.Subject = "Forget password";
                mm.Body = "Reset Password URL http://62.135.109.243:3232/View/resetPassword.html?code=" + empbase64Code;
                mm.IsBodyHtml = false;

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
                smtpClient.Port = 587;

                NetworkCredential nc = new NetworkCredential(emailData[0], emailData[1]);
                smtpClient.Credentials = nc;
                smtpClient.EnableSsl = true;
                smtpClient.Send(mm);
                return true;
            }
            return false;
        }


        [HttpPost("[action]")]

        // reseting data [newPassword ,curuserCode]
        public bool resetPassword([FromBody]object resetingData, [FromQuery] string code, [FromQuery] string key)
        {
            var resetingDataVal = JObject.Parse(resetingData.ToString());
            Byte[] k = Convert.FromBase64String(key.PadRight(key.Length + (4-key.Length % 4)%4,'='));
            Byte[] pass = Convert.FromBase64String(Encoding.UTF8.GetString(Convert.FromBase64String(code)));
            if(k.Intersect(pass).Any())
            {
              Emplyees emp=  _context.Emplyees.FirstOrDefault(e =>e.empCode == Encoding.UTF8.GetString(pass));
                if (String.IsNullOrWhiteSpace(resetingDataVal["resetingData"]["password"].ToString()))
                {
                    _context.Entry(emp).Property(e => e.password).IsModified = false;

                }
                else
                {
                    emp.password = Encrypt(resetingDataVal["resetingData"]["password"].ToString());
                    try
                    {
                         _context.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {

                            return false;

                    }
                }
            }

            return true;
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
                        perm = _context.Permissions.Where(per=>per.Id== rpr.perm_id).FirstOrDefault()
                    }).ToList()
                }).ToList() ,

            }).ToList().FirstOrDefault();
            
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

        public  static bool checkValidations(locationRecordeapiContext context , Emplyees curEmp ,  int[] permsid)
        {
            Emplyees emp = context.Emplyees.Where(em => em.id == curEmp.id && em.empCode == curEmp.empCode).FirstOrDefault();
            if (emp == null)
            { return false; }
            var role = context.roles.
                Select(r =>
                new 
                {
                    Id =r.Id
                ,
                    name=r.name
                ,
                    _roles_perms_rel = context.roles_perms_rel.Where(rprl=>rprl.role_id==r.Id)
                .Select(rpr =>
                new roles_perms_rel { id = rpr.id, perm_id= rpr.perm_id, role_id=rpr.role_id }
                ).ToList()
                }).Where(ro => ro.Id == emp.role).FirstOrDefault();
            //roles role = context.roles.Where(ro => ro.Id == emp.role).FirstOrDefault();
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
