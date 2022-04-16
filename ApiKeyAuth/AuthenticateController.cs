using locationRecordeapi.Controllers;
using locationRecordeapi.Data;
using locationRecordeapi.TokenAuthentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace locationRecordeapi.ApiKeyAuth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly locationRecordeapiContext _context;
        ITokenManager tokenManager;
        public AuthenticateController(ITokenManager tokenManager)
        {
            this.tokenManager = tokenManager;
            _context = Startup.DBContextMethod();
        }
        [HttpPost]
        public IActionResult Authenticate(UserLogin userLogin)
        {
            var muser = checkuserExisted(_context, userLogin.password, userLogin.code);
            if (muser!= null)
            {
                return Ok(new { Token = tokenManager.NewToken() });
            }
            else
            {
                ModelState.AddModelError("Unauthorized", "You are not unauthorized.");
                return Unauthorized(ModelState);
            }
        }


        public static object checkuserExisted(locationRecordeapiContext _context, string password, string code)
        {

            string passenc = Encrypt(password);
            object e = _context.Emplyees.Where(em => em.empCode == code && em.password == passenc).Select(e => new {
                e.id,
                e.email,
                e.empCode,
                e.locationKey,
                e.name
                ,
                e.loggedIn,
                e.phone,

                mrole = _context.roles.Where(r => r.Id == e.role).Select(r => new {
                    r.Id,
                    r.name,
                    roles_perms_rel = _context.roles_perms_rel.Where(rpr => rpr.role_id == r.Id).Select(rpr => new
                    {
                        rpr.id,
                        rpr.perm_id,
                        rpr.role_id,
                        perm = _context.Permissions.Where(per => per.Id == rpr.perm_id).FirstOrDefault()
                    }).ToList()
                }).ToList(),

            }).ToList().FirstOrDefault();

            return e;
        }

        public static string Encrypt(string pass)
        {
            using (MD5 md5 = MD5.Create())
            {
                UTF8Encoding utf8 = new UTF8Encoding();
                byte[] data = md5.ComputeHash(utf8.GetBytes(pass));
                return Convert.ToBase64String(data);
            }
        }


    }
}
