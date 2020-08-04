using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Silmoon.Web.Core.Authentication
{
    public class MainIdentity : IIdentity
    {
        public MainIdentity()
        {

        }
        //public MainIdentity(List<Claim> claims, string authenticationType = "Cookies") : base(claims, authenticationType)
        //{

        //}
        public string Name { get; set; }

        public string AuthenticationType { get; set; }

        public bool IsAuthenticated { get; set; }
    }
}
