using Silmoon.Business.Core.Types;
using Silmoon.Business.Models;
using Silmoon.Extension;
using Silmoon.Models;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Silmoon.Web.Controls
{
    public abstract class UserSessionController<TUser> : System.Web.SessionState.IRequiresSessionState where TUser : IUser
    {
        RSACryptoServiceProvider rsa = null;
        string cookieDomain = null;
        DateTime cookieExpires = default;
        int sessionTimeout = 30;

        public int SessionTimeout
        {
            get
            {
                if (HttpContext.Current.Session != null)
                    sessionTimeout = HttpContext.Current.Session.Timeout;
                return sessionTimeout;
            }
            set
            {
                sessionTimeout = value;
                if (HttpContext.Current.Session != null)
                    HttpContext.Current.Session.Timeout = value;
            }
        }
        public event EventHandler UserLogin;
        public event EventHandler UserLogout;
        public event UserSessionHanlder OnRequestRefreshUserSession;
        public event UserTokenHanlder OnRequestUserToken;

        public string Username
        {
            get
            {
                return User.Username;
            }
        }
        public UserRole? Role
        {
            get
            {
                var user = User;
                if (user == null) return null;
                else return user.Role;
            }
        }
        public LoginState State
        {
            get
            {
                if (HttpContext.Current.Session["___silmoon_state"] != null)
                {
                    int result = (int)LoginState.None;
                    int.TryParse(HttpContext.Current.Session["___silmoon_state"].ToString(), out result);
                    return (LoginState)result;
                }
                else return LoginState.None;
            }
            set
            {
                HttpContext.Current.Session["___silmoon_state"] = (int)value;
            }
        }
        public TUser User
        {
            get
            {
                if (HttpContext.Current.Session["___silmoon_user"] != null)
                    return (TUser)HttpContext.Current.Session["___silmoon_user"];
                else return default;
            }
            set { HttpContext.Current.Session["___silmoon_user"] = value; }
        }

        public RSACryptoServiceProvider RSACookiesCrypto
        {
            get { return rsa; }
            set { rsa = value; }
        }
        public string CookieDomain
        {
            get { return cookieDomain; }
            set { cookieDomain = value; }
        }
        public DateTime CookieExpires
        {
            get { return cookieExpires; }
            set { cookieExpires = value; }
        }

        public UserSessionController() : this((string)null) { }
        public UserSessionController(string cookieDomain)
        {
            this.cookieDomain = cookieDomain;
        }

        public bool GteRole(UserRole role)
        {
            var srule = Role;
            if (srule.HasValue)
                return srule >= role;
            else return false;
        }


        public object ReadSession(string key)
        {
            return HttpContext.Current.Session[key];
        }
        /// <summary>
        /// 检查UserSession的登录状态。
        /// 如果用户登录，会将实例赋值到ViewBag.UserSession，用户的数据赋值到ViewBag.User。
        /// </summary>
        /// <param name="controller">controller传入null，将不会自动转跳，并且在登录状态下不会，将用户会话实例赋值到ViewBag.UserSession，用户的数据不会赋值到ViewBag.User。</param>
        /// <param name="signInUrl">若传入controller，会使用转跳到本参数指定的URL。</param>
        /// <returns></returns>
        public ActionResult MvcSessionChecking(Controller controller, UserRole? IsRole, bool refreshUrlSession = false, string signInUrl = "~/User/Signin?url=$SigninUrl")
        {
            signInUrl = signInUrl?.Replace("$SigninUrl", controller.Server.UrlEncode(controller.Request.RawUrl));
            var username = controller.Request.QueryString["Username"];
            var userToken = controller.Request.QueryString["UserToken"] ?? controller.Request.QueryString["AppUserToken"];


            if (State != LoginState.Login)
            {
                if (userToken.IsNullOrEmpty())
                {
                    if (controller.Request.IsAjaxRequest())
                        return new JsonResult { Data = SimpleStateFlag.Create(false, -9999, "no signin."), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    else return new RedirectResult(signInUrl);
                }
                else
                {
                    var userInfo = OnRequestUserToken(username, userToken);
                    if (userInfo != null)
                    {
                        User = userInfo;
                        DoLogin(User);
                    }
                    else
                    {
                        if (controller.Request.IsAjaxRequest())
                            return new JsonResult { Data = SimpleStateFlag.Create(false, -9999, "signin by usertoken fail."), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                        else return new RedirectResult(signInUrl);
                    }

                    ///使用UserToken登录后处理
                    if (IsRole.HasValue)
                    {
                        if (Role < IsRole)
                        {
                            if (controller.Request.IsAjaxRequest())
                                return new JsonResult { Data = SimpleStateFlag.Create(false, -9999, "access denied."), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                            else return new ContentResult() { Content = "access denied", ContentType = "text/plain" };
                        }
                    }
                    controller.ViewBag.User = User;
                    controller.ViewBag.UserSession = this;
                    return null;
                }
            }
            else
            {
                if (refreshUrlSession)
                {
                    var userInfo = onRequestRefreshUserSession();
                    if (userInfo != null)
                        User = userInfo;
                    else
                    {
                        if (controller.Request.IsAjaxRequest())
                            return new JsonResult { Data = SimpleStateFlag.Create(false, -9999, "user object refresh fail."), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                        else return new RedirectResult(signInUrl);
                    }
                }

                if (IsRole.HasValue)
                {
                    if (Role < IsRole)
                    {
                        if (controller.Request.IsAjaxRequest())
                            return new JsonResult { Data = SimpleStateFlag.Create(false, -9999, "access denied."), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                        else return new ContentResult() { Content = "access denied", ContentType = "text/plain" };
                    }
                }

                controller.ViewBag.User = User;
                controller.ViewBag.UserSession = this;
                return null;
            }
        }

        public void WriteSession(string key, string value)
        {
            HttpContext.Current.Session.Timeout = sessionTimeout;
            HttpContext.Current.Session[key] = value;
        }

        public bool LoginFromCookie()
        {
            if (rsa == null) return false;
            if (HttpContext.Current.Request.Cookies["___silmoon_user_session"] != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Cookies["___silmoon_user_session"].Value))
            {
                byte[] data = Convert.FromBase64String(HttpContext.Current.Request.Cookies["___silmoon_user_session"].Value);
                data = rsa.Decrypt(data, true);
                string username = Encoding.Default.GetString(data, 2, BitConverter.ToInt16(data, 0));
                string password = Encoding.Default.GetString(data, BitConverter.ToInt16(data, 0) + 4, data[BitConverter.ToInt16(data, 0) + 2]);
                return CookieRelogin(username, password);
            }
            else
            {
                return false;
            }
        }
        public bool LoginCrossLoginCookie()
        {
            if (rsa == null) return false;
            if (HttpContext.Current.Request.Cookies["___silmoon_cross_session"] != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Cookies["___silmoon_cross_session"].Value))
            {
                byte[] data = Convert.FromBase64String(HttpContext.Current.Request.Cookies["___silmoon_cross_session"].Value);
                data = rsa.Decrypt(data, true);
                string username = Encoding.Default.GetString(data, 2, BitConverter.ToInt16(data, 0));
                string password = Encoding.Default.GetString(data, BitConverter.ToInt16(data, 0) + 4, data[BitConverter.ToInt16(data, 0) + 2]);
                bool result = CrossLogin(username, password);
                HttpContext.Current.Response.Cookies.Remove("___silmoon_cross_session");
                return result;
            }
            else
            {
                return false;
            }
        }
        public bool LoginFormToken(string token)
        {
            if (rsa == null) return false;
            byte[] data = Convert.FromBase64String(token);
            data = rsa.Decrypt(data, true);
            string username = Encoding.Default.GetString(data, 2, BitConverter.ToInt16(data, 0));
            string password = Encoding.Default.GetString(data, BitConverter.ToInt16(data, 0) + 4, data[BitConverter.ToInt16(data, 0) + 2]);
            bool result = TokenLogin(username, password);
            return result;
        }

        public virtual bool CookieRelogin(string username, string password)
        {
            return false;
        }
        public virtual bool CrossLogin(string username, string password)
        {
            return false;
        }
        public virtual bool TokenLogin(string username, string password)
        {
            return false;
        }

        public string GetUserToken()
        {
            if (rsa == null) return "";

            byte[] usernameData = Encoding.Default.GetBytes(User.Username);
            byte[] passwordData = Encoding.Default.GetBytes(User.Password);
            byte[] data = new byte[4 + usernameData.Length + passwordData.Length];

            Array.Copy(BitConverter.GetBytes((short)usernameData.Length), 0, data, 0, 2);
            Array.Copy(usernameData, 0, data, 2, usernameData.Length);
            Array.Copy(BitConverter.GetBytes((short)passwordData.Length), 0, data, usernameData.Length + 2, 2);
            Array.Copy(passwordData, 0, data, usernameData.Length + 4, passwordData.Length);

            data = rsa.Encrypt(data, true);
            return Convert.ToBase64String(data);
        }
        /// <summary>
        /// 写入一个在指定时间过期的【___silmoon_user_session】的Cookie，以便下次自动登录。
        /// </summary>
        /// <param name="Expires">过期时间</param>
        public void WriteCookie(DateTime Expires = default(DateTime))
        {
            if (rsa != null)
            {
                if (Expires != default) cookieExpires = Expires;

                if (cookieDomain != null)
                    HttpContext.Current.Response.Cookies["___silmoon_user_session"].Domain = cookieDomain;

                if (CookieExpires != default(DateTime))
                    HttpContext.Current.Response.Cookies["___silmoon_user_session"].Expires = cookieExpires;

                HttpContext.Current.Response.Cookies["___silmoon_user_session"].Value = GetUserToken();
            }
        }
        public void WriteCrossLoginCookie(string domain = null)
        {
            if (rsa != null)
            {
                HttpContext.Current.Response.Cookies["___silmoon_cross_session"].Value = GetUserToken();
                if (!string.IsNullOrEmpty(domain))
                    HttpContext.Current.Response.Cookies["___silmoon_cross_session"].Domain = domain;
                HttpContext.Current.Response.Cookies["___silmoon_cross_session"].Expires = DateTime.Now.AddDays(1);
            }
        }

        public virtual void DoLogin(TUser user)
        {
            DoLogin(user.Username, user.Password, user.Role, user);
        }
        public virtual void DoLogin(string username, string password, TUser user)
        {
            DoLogin(username, password, 0, user);
        }
        public virtual void DoLogin(string username, string password, UserRole role)
        {
            DoLogin(username, password, role, default(TUser));
        }
        public virtual void DoLogin(string username, string password, UserRole role, TUser user)
        {
            HttpContext.Current.Session.Timeout = sessionTimeout;
            User = user;
            State = LoginState.Login;
            UserLogin?.Invoke(this, EventArgs.Empty);
        }
        public virtual void DoLogout()
        {
            State = LoginState.Logout;
            HttpContext.Current.Session.Remove("___silmoon_username");
            HttpContext.Current.Session.Remove("___silmoon_password");
            HttpContext.Current.Session.Remove("___silmoon_level");
            HttpContext.Current.Session.Remove("___silmoon_userflag");
            HttpContext.Current.Session.Remove("___silmoon_object");
            HttpContext.Current.Session.Remove("___silmoon_user");

            if (UserLogout != null) UserLogout(this, EventArgs.Empty);
        }

        public void ClearCrossCookie()
        {
            if (HttpContext.Current.Request.Cookies["___silmoon_cross_session"] != null)
            {
                if (cookieDomain != null)
                    HttpContext.Current.Response.Cookies["___silmoon_cross_session"].Domain = cookieDomain;
                HttpContext.Current.Response.Cookies["___silmoon_cross_session"].Expires = DateTime.Now.AddYears(-10);
            }
        }
        public void ClearUserCookie()
        {
            if (cookieDomain != null)
                HttpContext.Current.Response.Cookies["___silmoon_user_session"].Domain = cookieDomain;
            HttpContext.Current.Response.Cookies["___silmoon_user_session"].Expires = DateTime.Now.AddYears(-10);
        }
        public virtual void Clear()
        {
            ClearUserCookie();
            ClearCrossCookie();
            DoLogout();
        }

        TUser onRequestRefreshUserSession()
        {
            if (OnRequestRefreshUserSession == null)
                return default(TUser);
            else
            {
                return OnRequestRefreshUserSession(User);
            }
        }
        public delegate TUser UserSessionHanlder(TUser User);
        public delegate TUser UserTokenHanlder(string Username, string UserToken);
    }


    [Serializable]
    public enum LoginState
    {
        None = 0,
        Login = 1,
        Logout = -1,
    }
}