using Microsoft.AspNetCore.Http;
using Silmoon.Web.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Silmoon.Web.Core.Controls
{
    public abstract class AspNetCoreUserSessionController<T>
    {
        RSACryptoServiceProvider rsa = null;
        string cookieDomain = null;
        DateTime cookieExpires = default(DateTime);
        int sessionTimeout = 30;
        ISession session = null;
        IRequestCookieCollection cookie = null;
        public event EventHandler UserLogin;
        public event EventHandler UserLogout;

        public string Username
        {
            get
            {
                if (session.GetString("___silmoon_username") != null)
                    return session.GetString("___silmoon_username").ToString();
                else return null;
            }
            set
            {
                session.SetString("___silmoon_username", value);
            }
        }
        public string Password
        {
            get
            {
                if (session.GetString("___silmoon_password") != null)
                    return session.GetString("___silmoon_password").ToString();
                else return null;
            }
            set
            {
                session.SetString("___silmoon_password", value);
            }
        }
        public int UserLevel
        {
            get
            {
                if (session.GetInt32("___silmoon_level").HasValue)
                {
                    return session.GetInt32("___silmoon_level").Value;
                }
                else return -1;
            }
            set
            {
                session.SetInt32("___silmoon_level", value);
            }
        }
        public LoginState State
        {
            get
            {
                if (session.GetString("___silmoon_state") != null)
                {
                    int result = (int)LoginState.None;
                    int.TryParse(session.GetString("___silmoon_state").ToString(), out result);
                    return (LoginState)result;
                }
                else return LoginState.None;
            }
            set
            {
                session.SetInt32("___silmoon_state", (int)value);
            }
        }
        public StateFlag UserFlag
        {
            get
            {
                return session.Get<StateFlag>("___silmoon_userflag");
            }
            set { session.Set("___silmoon_userflag", value); }
        }
        public object UserObject
        {
            get
            {
                return session.Get<object>("___silmoon_object");
            }
            set { session.Set("___silmoon_object", value); }
        }
        public T User
        {
            get
            {
                return session.Get<T>("___silmoon_user");
            }
            set { session.Set("___silmoon_user", value); }
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

        public AspNetCoreUserSessionController(ISession session) : this(null, session) { }
        public AspNetCoreUserSessionController(string cookieDomain, ISession session)
        {
            this.cookieDomain = cookieDomain;
        }



        private void check_sessionOfLogin()
        {
            if (State != LoginState.Login)
            {
                throw new Exception("获取会话信息错误，用户没有登陆，或者会话已经无效！");
            }
        }

        public object ReadSession(string field)
        {
            return session.Get<object>(field);
        }
        public void ReadSession(bool readCookies = true)
        {
            if (State != LoginState.Login)
            {
                if (readCookies && LoginFromCookie())
                {
                    State = LoginState.Login;
                }
            }
            else
            {

            }
        }
        public void WriteSession(string field, string value)
        {
            HttpContext.Current.Session.Timeout = sessionTimeout;
            HttpContext.Current.Session[field] = value;
        }

        public bool LoginFromCookie()
        {
            if (rsa == null) return false;
            if (cookie["___silmoon_user_session"] != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Cookies["___silmoon_user_session"].Value))
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

            byte[] usernameData = Encoding.Default.GetBytes(Username);
            byte[] passwordData = Encoding.Default.GetBytes(Password);
            byte[] data = new byte[4 + usernameData.Length + passwordData.Length];

            Array.Copy(BitConverter.GetBytes((short)usernameData.Length), 0, data, 0, 2);
            Array.Copy(usernameData, 0, data, 2, usernameData.Length);
            Array.Copy(BitConverter.GetBytes((short)passwordData.Length), 0, data, usernameData.Length + 2, 2);
            Array.Copy(passwordData, 0, data, usernameData.Length + 4, passwordData.Length);

            data = rsa.Encrypt(data, true);
            return Convert.ToBase64String(data);
        }

        public void WriteCookie(DateTime Expires = default(DateTime))
        {
            if (rsa != null)
            {
                if (Expires != default(DateTime)) cookieExpires = Expires;

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

        public virtual void DoLogin(string username, string password, T user)
        {
            DoLogin(username, password, 0, user);
        }
        public virtual void DoLogin(string username, string password, int userLevel)
        {
            DoLogin(username, password, userLevel, default(T));
        }
        public virtual void DoLogin(string username, string password, int userLevel, T user)
        {
            Username = username;
            Password = password;
            UserLevel = userLevel;
            User = user;
            State = LoginState.Login;

            UserLogin?.Invoke(this, EventArgs.Empty);
        }
        public virtual void DoLogout()
        {
            State = LoginState.Logout;
            session.Remove("___silmoon_username");
            session.Remove("___silmoon_password");
            session.Remove("___silmoon_level");
            session.Remove("___silmoon_userflag");
            session.Remove("___silmoon_object");
            session.Remove("___silmoon_user");

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
    }
}
