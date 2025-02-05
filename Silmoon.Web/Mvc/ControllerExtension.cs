using Silmoon.Extension.Enums;
using Silmoon.Extension.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Silmoon.Web.Mvc
{
    public static class ControllerExtension
    {
        public static ActionResult MvcSessionChecking(this Controller controller, UserSessionManager<IDefaultUserIdentity> userSession, IdentityRole? IsRole, bool requestUserSession = false, bool isApiRequest = false, string signInUrl = "~/User/Signin?url=$context_raw_url")
        {
            return userSession.MvcSessionChecking(controller, IsRole, requestUserSession, isApiRequest, signInUrl);
        }
    }
}
