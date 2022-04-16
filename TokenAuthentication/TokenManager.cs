using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace locationRecordeapi.TokenAuthentication
{
    public class TokenManager :ITokenManager
    {
        private List<Token> listTokens;
      public TokenManager()
        {
            listTokens = new List<Token>();
        }
        public bool Authenticate(string UserName, string password)
        {
            if(!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(UserName) && UserName.ToLower()=="admin" && password== "Password")
            {
                return true;

            }
            else
            {
                return false;
            }

        }
        public bool checkPermission(string[] permissions, int? role)
        {
            if (permissions.Length > 0)
            {
              var dbcon=  Startup.DBContextMethod();

                var mpermissions = dbcon.roles_perms_rel.Where(rpr => rpr.role_id == role).Select(rpr => new
                {
                    rpr.id,
                    rpr.perm_id,
                    rpr.role_id,
                    perm = dbcon.Permissions.Where(per => per.Id == rpr.perm_id).FirstOrDefault()
                });

                foreach (var mperm in mpermissions)
                {
                    if (!permissions.Contains(mperm.perm.name))
                    {
                        return false;
                    }
                }
            }
            return true;
        }


        public Token NewToken()
        {
            var token = new Token
            {
                Value = Guid.NewGuid().ToString(),
                ExpiryDate = DateTime.UtcNow.AddHours(2).AddMinutes(8)
            };
            listTokens.Add(token);
            return token;
        }


        public Token RenewToken(string token)
        {
           Token mtoken =   listTokens.Where(x => token != null && x.Value == token && x.ExpiryDate > DateTime.UtcNow.AddHours(2).AddMinutes(8)).FirstOrDefault();

            if (mtoken != null)
            {
                mtoken.ExpiryDate = DateTime.UtcNow.AddHours(4);
            }
            return mtoken;
        }
        public bool VerifyToken(string token)
        {
            if(listTokens.Any(x=> token!=null && x.Value == token && x.ExpiryDate> DateTime.UtcNow.AddHours(2).AddMinutes(8)))
                {
                return true;
            }
            return false;
        }
    }
}
