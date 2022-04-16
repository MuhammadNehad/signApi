using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using locationRecordeapi.Data;
using System.Security.Cryptography;
using System.Text;
using System.Net.Mail;
using System.Net;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;
using locationRecordeapi.Filters;
using locationRecordeapi.ApiKeyAuth;

namespace locationRecordeapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[TokenAuthenticationFilter]
    //[BasicAuthorize]
    public class EmplyeesController : ControllerBase
    {
        private readonly locationRecordeapiContext _context;
        List<int?> inParentsArray = new List<int?>();
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


        /// <summary>
        /// method <c>GetEmplyeesWithattendings</c> gets Employees data with their attendings  
        /// </summary>
        /// <param name="curEmp">receive user who passed data</param>
        /// <param name="permsid">gets Permissions</param>
        /// <param name="from">from specific time to search</param>
        /// <param name="To">to specific time to search</param>
        /// <param name="empName">used name to search</param>
        /// <param name="period">specify whether per day ,month or year</param>
        /// <param name="parentId">passes parent role</param>
        /// <returns>returns attendings data in object</returns>
        [HttpGet("[action]")]
        public ActionResult<object> GetEmplyeesWithattendings([FromQuery] Emplyees curEmp, [FromQuery] int[] permsid,[FromQuery] DateTime from,[FromQuery]DateTime To,[FromQuery]string empName,[FromQuery] string period,[FromQuery] int parentId)
        {
            if (!checkValidations(_context, curEmp, permsid))
            {
                return StatusCode(400, "check you are registered or have permission");
            }
            DateTime currentdt = DateTime.UtcNow.AddHours(2);
            List<EmplyeesAttendings> attendsList = new List<EmplyeesAttendings>();
            //loop in attendings table
            foreach(EmplyeesAttendings eas in _context.EmplyeesAttendings)
            {
                if(arole(eas, curEmp.id, parentId))
                {
                    attendsList.Add(eas);
                }
            }



            //var emplyeesAttendings =  _context.attendings.Select(ea => new {
            //    ea.Id,
            //    ea.atdt,
            //    ea.empKey,
            //    ea.locationKey,
            //    ea.entering,
            //    ea.leaveAfter,
            //    aemplyee = _context.Emplyees.Select(e => new
            //    {
            //        e.id,
            //        e.email,
            //        e.empCode,
            //        e.name,
            //        e.phone,
            //        e.role,
            //        mrole = _context.roles.ToList().Where(rs =>
            //            arole(rs, e.id, e.role ?? -100, curEmp.id, parentId)
            // ).FirstOrDefault()
            //    }).AsEnumerable().Where(emp => (emp.id == ea.empKey && emp.name.Contains(empName??"") &&emp.mrole != null) ).FirstOrDefault(),
            //    location = _context.EmpsLocation.Where(loc => loc.Id == ea.locationKey)
            //             .FirstOrDefault()


            //}).AsEnumerable().Where(eas=>(eas.atdt >= from && eas.atdt <= To && eas.aemplyee != null)).ToList();
            dynamic linqq = null;

             if (period == "month")
            {
                linqq = attendsList.GroupBy(g => new { g.atdt.Month, g.empKey });
            }
            else if (period == "year")
            {
                linqq = attendsList.GroupBy(g => new { g.atdt.Year, g.empKey });
            }
            else
            {
                linqq = attendsList.GroupBy(g => new { g.atdt.Date, g.empKey });

            }
            //

            if (linqq == null)
            {
                return NotFound();
            }

            return StatusCode(200, linqq);
        }
       

        public bool arole(EmplyeesAttendings eas,int curEmpId,int parentId)
        {
            if(eas.parent_id !=null && (eas.parent_id == parentId || inParentsArray.Contains(eas.parent_id)))
            {
                inParentsArray.Add(eas.parent_id);
                return true;
            }
            if (eas.aemplyeeId == curEmpId)
            {
                return true;
            }
            return false;
        }
        
        // PUT: api/Emplyees/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmplyees(int id,[FromForm] Emplyees emplyees, [FromQuery] Emplyees curEmp, [FromQuery] int[] permsid)
        {
            if(curEmp.id != id)
            { 
                if (!checkValidations(_context, curEmp, permsid))
                {
                    return StatusCode(400, "check you are registered or have permission");
                }
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
                emplyees.password = AuthenticateController.Encrypt(emplyees.password);
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
            if (emplyees.loggedIn ==null)
            {

                _context.Entry(emplyees).Property(e => e.loggedIn).IsModified = false;

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
                    emp.password = AuthenticateController.Encrypt(resetingDataVal["resetingData"]["password"].ToString());
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
            emplyees.password = AuthenticateController.Encrypt(emplyees.password);
            
            _context.Emplyees.Add(emplyees);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmplyees", new { id = emplyees.id }, emplyees);
        }
        [HttpGet("[action]")]
        public ActionResult signout()
        {
            return SignOut();
        }
        [HttpGet("[action]/{code}/{pass}")]
        public ActionResult<object> Login(string code,string pass)
        {
            var emplyee = AuthenticateController.checkuserExisted(_context, pass, code);
            if (emplyee == null)
            {
                return NotFound();
            }
            return StatusCode(200, emplyee);
        }
        // POST: api/Emplyees/Login

        [HttpPost("[action]")]
        [AllowAnonymous]
        public ActionResult<object> Login(UserLogin userLogin)
        {
            var emplyee =AuthenticateController.checkuserExisted(_context,userLogin.password,userLogin.code);
            if(emplyee == null)
            {
                return NotFound();
            }
            return StatusCode(200, emplyee);
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

    }
}
