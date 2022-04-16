using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace locationRecordeapi
{
    public class AuthenticatedUser : IIdentity
    {
        public AuthenticatedUser(string authenticationType ,bool isAuthenticated , string name)
        {
            this.AuthenticationType = authenticationType;
            this.IsAuthenticated = isAuthenticated;
            this.Name = name;
        }
        public string AuthenticationType { get; set; }

        public bool IsAuthenticated { get; set; }

        public string Name { get; set; }
    }
}
