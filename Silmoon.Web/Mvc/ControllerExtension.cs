using Silmoon.Business.Core.Types;
using Silmoon.Business.Models;
using Silmoon.Web.Controls;
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
        public static ActionResult MvcSessionChecking(this Controller controller, UserSessionController<IUser> userSession, UserRole? IsRole, bool requestUserSession = false, bool isApiRequest = false, string signInUrl = "~/User/Signin?url=$context_raw_url")
        {
            return userSession.MvcSessionChecking(controller, IsRole, requestUserSession, isApiRequest, signInUrl);
        }
    }
}
