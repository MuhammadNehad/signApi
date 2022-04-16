using locationRecordeapi.ApiKeyAuth;
using locationRecordeapi.Controllers;
using locationRecordeapi.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace locationRecordeapi
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly locationRecordeapiContext _context;

        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger,UrlEncoder encoder,ISystemClock systemClock) :base(options,logger,encoder,systemClock)
        {
            _context = Startup.DBContextMethod();
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            Response.Headers.Add("Authenticate", "Basic");
            if(!Request.Headers.ContainsKey("Authorization") )
            {
                return Task.FromResult(AuthenticateResult.Fail("Authorization Header Missing"));
            }
            var authorizationHeader = Request.Headers["Authorization"].ToString();
            var authHeaderRegex = new Regex("Basic (.*)");
            if(!authHeaderRegex.IsMatch(authorizationHeader))
            {
                return Task.FromResult(AuthenticateResult.Fail("Authorization code not formatted correctly"));
            }

            var autheBase64 = Encoding.UTF8.GetString(Convert.FromBase64String(authHeaderRegex.Replace(authorizationHeader, "$1")));
            var authSplit = autheBase64.Split(Convert.ToChar(":"),2);
            var userName = authSplit[0];
            var password = authSplit.Length > 1 ? authSplit[1] : throw new Exception("Unable To get password");
            //if(authUsername != "roundthecode" || password != )

            var user = AuthenticateController.checkuserExisted(_context, password, userName);

            if(user == null)
            {
                return Task.FromResult(AuthenticateResult.Fail("The username or password is not correct."));

            }
            var authenticatedUser = new AuthenticatedUser("BasicAuthentication", true, "roundthecode");
           var claimIdentity= new ClaimsIdentity(authenticatedUser);
            var claimsPrincipal = new ClaimsPrincipal(claimIdentity);
            //var claim = claimsPrincipal.Claims.Where(mclaim => mclaim.Value == "roundthecode").FirstOrDefault();
            //claimIdentity.RemoveClaim(claim);
            return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
        }
    }
}
