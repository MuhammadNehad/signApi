using locationRecordeapi.ApiKeyAuth;
using locationRecordeapi.Data;
using locationRecordeapi.TokenAuthentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace locationRecordeapi.Filters
{
    public class TokenAuthenticationFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var tokenManager = (ITokenManager)context.HttpContext.RequestServices.GetService(typeof(ITokenManager));
            var dbcontext = (locationRecordeapiContext)context.HttpContext.RequestServices.GetService(typeof(locationRecordeapiContext));
            var result = true;
            var req = context.HttpContext.Request.Method;
            // has "controller" data and "action" data
            var controller = context.RouteData.Values["controller"];
            var headers =context.HttpContext.Request.Headers;
            var body =context.HttpContext.Request.Body;

            if (!headers.ContainsKey("Authorization"))
                result = false;

            string tokenHeader = string.Empty;
                if (result)
                {
                var role = 0;
                    tokenHeader = headers.FirstOrDefault(x => x.Key == "Authorization").Value;
                    
                if(tokenHeader != null)
                    {
                    var encode64 = Convert.ToBase64String(Encoding.UTF8.GetBytes("1131q:31231"));
                    
                      var base64Decode = Encoding.UTF8.GetString(Convert.FromBase64String(tokenHeader.Split(" ")[1]));
                    var token = base64Decode.Split(':')[0];
                    var Code = base64Decode.Split(':')[1];
                    var password = base64Decode.Split(':')[2];
                  var emp= dbcontext.Emplyees.FirstOrDefault(emp => emp.empCode == Code && emp.password == AuthenticateController.Encrypt(password));



                    if (emp != null)
                    { 

                    if (!tokenManager.VerifyToken(token))
                        {
                            result = false;
                        }
                        else { 
                
                            if(controller =="Permissions")
                            {
                                if(req =="GET")
                                {
                                     result =   tokenManager.checkPermission(new string[] { "permissions view" },emp.role);
                                }else if(req =="POST")
                                {
                                     result = tokenManager.checkPermission(new string[] { "permissions add" }, emp.role);

                                }
                            }else if(controller == "Emplyees")
                            {

                            }
                        }
                    }
                    else
                    {
                        result = false;
                    }
                }
            }

               if (!result)
                {
                    context.ModelState.AddModelError("Unauthorized", "You are not authorized");
                    context.Result = new UnauthorizedObjectResult(context.ModelState);
                }
           
            
        }
    }
}
