using System;
using System.Collections.Generic;
using System.Text;
using System.DirectoryServices;
using System.Text.RegularExpressions;
using System.Collections;
using Silmoon.Security;
using System.Windows.Forms;

namespace Silmoon.Windows.Server.IIS
{
    /// <summary>
    /// 用来实现管理IIS的基本操作。
    /// </summary>
    public class IISManager : IDisposable
    {
        public IISManager()
        {

        }


        public int CreateNewWebSite(NewWebSiteInfo siteInfo)
        {
            return CreateNewWebSite(siteInfo, MakeupNewWebsiteID());
        }
        public int CreateNewWebSite(NewWebSiteInfo siteInfo, int newSiteNum)
        {
            DirectoryEntry rootEntry = GetDirectoryEntry("");
            DirectoryEntry newSiteEntry = rootEntry.Children.Add(newSiteNum.ToString(), "IIsWebServer");
            newSiteEntry.CommitChanges();

            newSiteEntry.Properties[WebSiteParameter.ServerBindings.ToString()].Value = siteInfo.Bindings;
            newSiteEntry.Properties[WebSiteParameter.ServerComment.ToString()].Value = siteInfo.SiteName;
            newSiteEntry.Properties[WebSiteParameter.AccessFlags.ToString()].Value = 512 | 1;
            newSiteEntry.Properties[WebSiteParameter.AnonymousUserName.ToString()].Value = siteInfo.AccessUser.IdentityString;
            newSiteEntry.Properties[WebSiteParameter.AnonymousUserPass.ToString()].Value = siteInfo.AccessUser.PasswordCode;
            newSiteEntry.Properties[WebSiteParameter.ScriptMaps.ToString()].Value = siteInfo.ScriptMaps;
            newSiteEntry.Properties[WebSiteParameter.LogFileLocaltimeRollover.ToString()].Value = siteInfo.LogFileLocaltimeRollover.ToString();
            newSiteEntry.Properties[WebSiteParameter.LogFileDirectory.ToString()].Value = siteInfo.LogFileDirectory;
            newSiteEntry.CommitChanges();

            DirectoryEntry vdEntry = newSiteEntry.Children.Add("root", "IIsWebVirtualDir");
            vdEntry.CommitChanges();

            //创建应用程序，并指定应用程序池为"HostPool","true"表示如果HostPool不存在，则自动创建
            vdEntry.Invoke("AppCreate3", new object[] { 2, siteInfo.AppPoolName, true });
            vdEntry.Properties[WebVirtualDirParameter.Path.ToString()].Value = siteInfo.DirectoryPath;
            //设置应用程序名称
            vdEntry.Properties[WebVirtualDirParameter.AppFriendlyName.ToString()].Value = "v_" + siteInfo.SiteName;
            vdEntry.CommitChanges();
            DisposeDirectoryEntry(rootEntry);
            return newSiteNum;
        }
        public void DeleteWebSite(string webSiteID)
        {
            DirectoryEntry siteEntry = GetDirectoryEntry("/" + webSiteID);
            DirectoryEntry rootEntry = GetDirectoryEntry("");

            rootEntry.Children.Remove(siteEntry);
            rootEntry.CommitChanges();
            DisposeDirectoryEntry(siteEntry);
            DisposeDirectoryEntry(rootEntry);
        }
        public WebSiteInfo GetWebsiteInfo(string webSiteID)
        {
            WebSiteInfo siteInfo = new WebSiteInfo();
            DirectoryEntry rootEntry = GetDirectoryEntry("");
            foreach (DirectoryEntry ent in rootEntry.Children)
            {
                if (ent.Name == webSiteID && ent.SchemaClassName == "IIsWebServer")
                {
                    siteInfo.SiteName = ent.Properties[WebSiteParameter.ServerComment.ToString()].Value.ToString();
                    siteInfo.Bindings = ent.Properties[WebSiteParameter.ServerBindings.ToString()].Value;
                    siteInfo.State = (WebSiteState)ent.Properties["ServerState"].Value;
                    siteInfo.LogFileLocaltimeRollover = SmString.StringToBool(ent.Properties[WebSiteParameter.LogFileLocaltimeRollover.ToString()].Value.ToString());
                    siteInfo.LogFileDirectory = ent.Properties[WebSiteParameter.LogFileDirectory.ToString()].Value.ToString();

                    DirectoryEntry appInfo = ent.Children.Find("root", "IIsWebVirtualDir");
                    siteInfo.AccessUser = new IdentityAuthInfo(appInfo.Properties[WebVirtualDirParameter.AnonymousUserName.ToString()].Value.ToString(), appInfo.Properties[WebVirtualDirParameter.AnonymousUserPass.ToString()].Value.ToString());
                    siteInfo.AppPoolName = appInfo.Properties[WebVirtualDirParameter.AppPoolId.ToString()].Value.ToString();
                    siteInfo.DirectoryPath = appInfo.Properties[WebVirtualDirParameter.Path.ToString()].Value.ToString();
                    siteInfo.ScriptMaps = appInfo.Properties[WebVirtualDirParameter.ScriptMaps.ToString()].Value;

                    return siteInfo;
                }
            }
            DisposeDirectoryEntry(rootEntry);
            return siteInfo;
        }

        public void ModifyWebSiteParameter(string webSiteID, WebSiteParameter param, object value)
        {
            DirectoryEntry entry = GetDirectoryEntry("/" + webSiteID);
            entry.Properties[param.ToString()].Value = value;
            entry.CommitChanges();
            DisposeDirectoryEntry(entry);
        }
        public object GetWebSiteParameter(string webSiteID, WebSiteParameter param)
        {
            object result = null;
            DirectoryEntry entry = GetDirectoryEntry("/" + webSiteID);
            result = entry.Properties[param.ToString()].Value;
            DisposeDirectoryEntry(entry);
            return result;
        }

        public void ModifyAppPoolParameter(string AppPoolName, AppPoolParameter param, object value)
        {
            DirectoryEntry entry = GetDirectoryEntry("/AppPools/" + AppPoolName);
            entry.Properties[param.ToString()].Value = value;
            entry.CommitChanges();
            DisposeDirectoryEntry(entry);
        }
        public object GetAppPoolParameter(string AppPoolName, AppPoolParameter param)
        {
            object result = null;
            DirectoryEntry entry = GetDirectoryEntry("/AppPools/" + AppPoolName);
            result = entry.Properties[param.ToString()].Value;
            DisposeDirectoryEntry(entry);
            return result;
        }

        public void ModifyWebVirtualDir(string webSiteID, WebVirtualDirParameter param, object value)
        {
            DirectoryEntry entry = GetWebSiteIDDirectoryEntry(webSiteID);
            foreach (DirectoryEntry appent in entry.Children)
            {
                if (appent.Name == "root")
                {
                    appent.Properties[param.ToString()].Value = value;
                    appent.CommitChanges();
                    DisposeDirectoryEntry(entry);
                    return;
                }
            }
            DisposeDirectoryEntry(entry);
        }
        public object GetWebVirtualDir(string webSiteID, WebVirtualDirParameter param)
        {
            object result = null;
            DirectoryEntry entry = GetWebSiteIDDirectoryEntry(webSiteID);
            foreach (DirectoryEntry appent in entry.Children)
            {
                if (appent.Name == "root")
                {
                    result = appent.Properties[param.ToString()].Value;
                    DisposeDirectoryEntry(entry);
                    return result;
                }
            }
            return result;
        }

        public void StartWebSite(string webSiteID)
        {
            DirectoryEntry siteEntry = GetDirectoryEntry("/" + webSiteID);
            siteEntry.Invoke("Start", new object[] { });
            DisposeDirectoryEntry(siteEntry);
        }
        public void StopWebSite(string webSiteID)
        {
            DirectoryEntry siteEntry = GetDirectoryEntry("/" + webSiteID);
            siteEntry.Invoke("Stop", new object[] { });
            DisposeDirectoryEntry(siteEntry);
        }

        public bool CheckBindings(string bindStr)
        {
            DirectoryEntry ent = GetDirectoryEntry("");

            foreach (DirectoryEntry child in ent.Children)
            {
                if (child.SchemaClassName == "IIsWebServer")
                {
                    if (child.Properties["ServerBindings"].Value != null)
                    {
                        if (child.Properties["ServerBindings"].Value.ToString() == bindStr)
                        {
                            DisposeDirectoryEntry(ent);
                            return false;
                        }
                    }
                }
            }
            DisposeDirectoryEntry(ent);
            return true;
        }
        public bool IsExistWebSite(string webSiteID)
        {
            DirectoryEntry rootEntry = GetDirectoryEntry("");
            bool result = false;
            foreach (DirectoryEntry ent in rootEntry.Children)
            {
                if (ent.Name == webSiteID && ent.SchemaClassName == "IIsWebServer")
                    result = true;
            }
            DisposeDirectoryEntry(rootEntry);
            return result;
        }

        public AppPoolBaseInfo[] AppPoolList
        {
            get
            {
                ArrayList _arrArr = new ArrayList();
                DirectoryEntry poolEnt = GetDirectoryEntry("/AppPools");
                foreach (DirectoryEntry pool in poolEnt.Children)
                {
                    if (pool.SchemaClassName == "IIsApplicationPool")
                    {
                        AppPoolBaseInfo baseInfo;
                        baseInfo.PoolName = pool.Name;
                        baseInfo.State = (AppPoolState)((int)pool.Properties["AppPoolState"].Value);
                        _arrArr.Add(baseInfo);
                    }
                }
                DisposeDirectoryEntry(poolEnt);
                return (AppPoolBaseInfo[])_arrArr.ToArray(typeof(AppPoolBaseInfo));
            }
        }
        public WebSiteBaseInfo[] WebSiteList
        {
            get
            {
                ArrayList _arrArr = new ArrayList();
                DirectoryEntry webEnt = GetDirectoryEntry("");
                foreach (DirectoryEntry siteEntry in webEnt.Children)
                {
                    if (siteEntry.SchemaClassName == "IIsWebServer")
                    {
                        WebSiteBaseInfo baseInfo;
                        baseInfo.SiteName = siteEntry.Properties["ServerComment"][0].ToString();
                        baseInfo.SiteID = siteEntry.Name; ;
                        baseInfo.State = (WebSiteState)GetWebSiteState(siteEntry.Name.ToString());

                        DirectoryEntry appInfo = siteEntry.Children.Find("root", "IIsWebVirtualDir");
                        baseInfo.AppPoolName = appInfo.Properties["AppPoolId"].Value.ToString();

                        _arrArr.Add(baseInfo);
                    }
                }
                DisposeDirectoryEntry(webEnt);
                return (WebSiteBaseInfo[])_arrArr.ToArray(typeof(WebSiteBaseInfo));
            }
        }

        public WebSiteState GetWebSiteState(string webSiteID)
        {
            WebSiteState state;
            DirectoryEntry entry = GetWebSiteIDDirectoryEntry(webSiteID);
            state = (WebSiteState)entry.Properties["ServerState"].Value;
            return state;
        }
        public AppPoolState GetAppPoolState(string AppPoolName)
        {
            AppPoolState state = 0;
            DirectoryEntry rootEntry = GetDirectoryEntry("/AppPools");
            foreach (DirectoryEntry ent in rootEntry.Children)
            {
                if (ent.SchemaClassName == "IIsApplicationPool" || ent.Name == AppPoolName)
                {
                    state = (AppPoolState)ent.Properties["AppPoolState"].Value;
                    break;
                }
            }
            DisposeDirectoryEntry(rootEntry);
            return state;
        }
        public WebSiteBaseInfo[] GetAppPoolWebSites(string AppPoolName)
        {
            ArrayList result = new ArrayList();
            WebSiteBaseInfo[] websites = WebSiteList;
            foreach (WebSiteBaseInfo website in websites)
            {
                if (AppPoolName.ToLower() == website.AppPoolName.ToLower())
                    result.Add(website);
            }
            return (WebSiteBaseInfo[])result.ToArray(typeof(WebSiteBaseInfo));
        }
        public string GetWebSiteID(string siteName)
        {
            Regex regex = new Regex(siteName);
            string tmpStr;
            DirectoryEntry ent = GetDirectoryEntry("");

            foreach (DirectoryEntry child in ent.Children)
            {
                if (child.SchemaClassName == "IIsWebServer")
                {
                    if (child.Properties[WebSiteParameter.ServerComment.ToString()].Value != null)
                    {
                        tmpStr = child.Properties["ServerComment"].Value.ToString();
                        if (regex.Match(tmpStr).Success)
                        {
                            DisposeDirectoryEntry(ent);
                            return child.Name;
                        }
                    }

                    if (child.Properties[WebSiteParameter.ServerBindings.ToString()].Value != null)
                    {
                        tmpStr = child.Properties["ServerBindings"].Value.ToString();
                        if (regex.Match(tmpStr).Success)
                        {
                            DisposeDirectoryEntry(ent);
                            return child.Name;
                        }
                    }
                }
            }
            DisposeDirectoryEntry(ent);
            return null;
        }

        public void InvokeAppPool(string appPoolName,AppPoolOption option)
        {
            DirectoryEntry appEnt = GetDirectoryEntry("/AppPools");
            DirectoryEntry findPool = appEnt.Children.Find(appPoolName, "IIsApplicationPool");
            findPool.Invoke(option.ToString(), null);
            appEnt.CommitChanges();
            DisposeDirectoryEntry(appEnt);
        }

        private DirectoryEntry GetWebSiteIDDirectoryEntry(string webSiteID)
        {
            DirectoryEntry rootEntry = GetDirectoryEntry("");
            foreach (DirectoryEntry ent in rootEntry.Children)
            {
                if (ent.Name == webSiteID && ent.SchemaClassName == "IIsWebServer")
                {
                    DisposeDirectoryEntry(rootEntry);
                    return ent;
                }
            }
            DisposeDirectoryEntry(rootEntry);
            return null;
        }
        public int MakeupNewWebsiteID()
        {
            ArrayList list = new ArrayList();
            string tmpStr;
            DirectoryEntry rootEntry = GetDirectoryEntry("");
            foreach (DirectoryEntry child in rootEntry.Children)
            {
                if (child.SchemaClassName == "IIsWebServer")
                {
                    tmpStr = child.Name.ToString();
                    list.Add(Convert.ToInt32(tmpStr));
                }
            }
            list.Sort();
            int i = new Random().Next(1000, 60000);
            foreach (int j in list)
            {
                if (i == j) i++;
            }
            DisposeDirectoryEntry(rootEntry);
            return i;
        }
        private DirectoryEntry GetDirectoryEntry(string path)
        {
            DirectoryEntry result = new DirectoryEntry("IIS://localhost/w3svc" + path);
            return result;
        }
        private void DisposeDirectoryEntry(DirectoryEntry ent)
        {
            ent.Close();
            ent.Dispose();
            ent = null;
        }
        #region IDisposable 成员

        public void Dispose()
        {

        }

        #endregion

        public static string[] GetIISConfigObjectArray(object obj)
        {
            ArrayList _arr = new ArrayList();
            if (obj.GetType() == typeof(string))
                _arr.Add(obj);
            else
            {
                object[] bindArr = (object[])obj;
                foreach (object o in bindArr)
                    _arr.Add(o);
            }
            return (string[])_arr.ToArray(typeof(string));
        }
    }
    public class ScriptsMapsInfo
    {
        public string[] _script1;
        public string[] _script2;
        public string[] _script3;
        public string[] _script4;
        public ScriptsMapsInfo()
        {
            //支持 asp/asp.net/php
            _script1 = new string[27]{".asa,"+Environment.SystemDirectory+@"\system32\inetsrv\asp.dll,5,GET,HEAD,POST,TRACE",
   ".asax,"+Environment.SystemDirectory+@"\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,5,GET,HEAD,POST,DEBUG",
   ".ascx,"+Environment.SystemDirectory+@"\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,5,GET,HEAD,POST,DEBUG",
   ".ashx,"+Environment.SystemDirectory+@"\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,1,GET,HEAD,POST,DEBUG",
   ".asmx,"+Environment.SystemDirectory+@"\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,1,GET,HEAD,POST,DEBUG",
   ".asp,"+Environment.SystemDirectory+@"\system32\inetsrv\asp.dll,5,GET,HEAD,POST,TRACE",
   ".aspx,"+Environment.SystemDirectory+@"\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,1,GET,HEAD,POST,DEBUG",
   ".axd,"+Environment.SystemDirectory+@"\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,1,GET,HEAD,POST,DEBUG",
   ".cdx,"+Environment.SystemDirectory+@"\system32\inetsrv\asp.dll,5,GET,HEAD,POST,TRACE",
   ".cer,"+Environment.SystemDirectory+@"\system32\inetsrv\asp.dll,5,GET,HEAD,POST,TRACE",
   ".config,"+Environment.SystemDirectory+@"\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,5,GET,HEAD,POST,DEBUG",
   ".cs,"+Environment.SystemDirectory+@"\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,5,GET,HEAD,POST,DEBUG",
   ".csproj,"+Environment.SystemDirectory+@"\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,5,GET,HEAD,POST,DEBUG",
   ".idc,"+Environment.SystemDirectory+@"\system32\inetsrv\httpodbc.dll,5,GET,POST",
   ".licx,"+Environment.SystemDirectory+@"\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,5,GET,HEAD,POST,DEBUG",
   ".php,"+Environment.SystemDirectory+@"\system32\php5isapi.dll,5",
   ".rem,"+Environment.SystemDirectory+@"\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,1,GET,HEAD,POST,DEBUG",
   ".resources,"+Environment.SystemDirectory+@"\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,5,GET,HEAD,POST,DEBUG",
   ".resx,"+Environment.SystemDirectory+@"\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,5,GET,HEAD,POST,DEBUG",
   ".shtm,"+Environment.SystemDirectory+@"\system32\inetsrv\ssinc.dll,5,GET,POST",
   ".shtml,"+Environment.SystemDirectory+@"\system32\inetsrv\ssinc.dll,5,GET,POST",
   ".soap,"+Environment.SystemDirectory+@"\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,1,GET,HEAD,POST,DEBUG",
   ".stm,"+Environment.SystemDirectory+@"\system32\inetsrv\ssinc.dll,5,GET,POST",
   ".vb,"+Environment.SystemDirectory+@"\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,5,GET,HEAD,POST,DEBUG",
   ".vbproj,"+Environment.SystemDirectory+@"\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,5,GET,HEAD,POST,DEBUG",
   ".vsdisco,"+Environment.SystemDirectory+@"\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,1,GET,HEAD,POST,DEBUG",
   ".webinfo,"+Environment.SystemDirectory+@"\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,5,GET,HEAD,POST,DEBUG",
         };

            //支持 asp
            _script2 = new string[8]{".asa,"+Environment.SystemDirectory+@"\system32\inetsrv\asp.dll,5,GET,HEAD,POST,TRACE",
                                     ".asp,"+Environment.SystemDirectory+@"\system32\inetsrv\asp.dll,5,GET,HEAD,POST,TRACE",
          ".cdx,"+Environment.SystemDirectory+@"\system32\inetsrv\asp.dll,5,GET,HEAD,POST,TRACE",
          ".cer,"+Environment.SystemDirectory+@"\system32\inetsrv\asp.dll,5,GET,HEAD,POST,TRACE",
          ".idc,"+Environment.SystemDirectory+@"\system32\inetsrv\httpodbc.dll,5,GET,POST",
          ".shtm,"+Environment.SystemDirectory+@"\system32\inetsrv\ssinc.dll,5,GET,POST",
          ".shtml,"+Environment.SystemDirectory+@"\system32\inetsrv\ssinc.dll,5,GET,POST",
          ".stm,"+Environment.SystemDirectory+@"\system32\inetsrv\ssinc.dll,5,GET,POST"
           };
            //支持 asp/asp.net
            _script3 = new string[26]{".asa,"+Environment.SystemDirectory+@"\system32\inetsrv\asp.dll,5,GET,HEAD,POST,TRACE",
           ".asax,"+Environment.SystemDirectory+@"\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,5,GET,HEAD,POST,DEBUG",
           ".ascx,"+Environment.SystemDirectory+@"\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,5,GET,HEAD,POST,DEBUG",
           ".ashx,"+Environment.SystemDirectory+@"\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,1,GET,HEAD,POST,DEBUG",
           ".asmx,"+Environment.SystemDirectory+@"\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,1,GET,HEAD,POST,DEBUG",
           ".asp,"+Environment.SystemDirectory+@"\system32\inetsrv\asp.dll,5,GET,HEAD,POST,TRACE",
           ".aspx,"+Environment.SystemDirectory+@"\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,1,GET,HEAD,POST,DEBUG",
           ".axd,"+Environment.SystemDirectory+@"\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,1,GET,HEAD,POST,DEBUG",
           ".cdx,"+Environment.SystemDirectory+@"\system32\inetsrv\asp.dll,5,GET,HEAD,POST,TRACE",
           ".cer,"+Environment.SystemDirectory+@"\system32\inetsrv\asp.dll,5,GET,HEAD,POST,TRACE",
           ".config,"+Environment.SystemDirectory+@"\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,5,GET,HEAD,POST,DEBUG",
           ".cs,"+Environment.SystemDirectory+@"\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,5,GET,HEAD,POST,DEBUG",
           ".csproj,"+Environment.SystemDirectory+@"\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,5,GET,HEAD,POST,DEBUG",
           ".idc,"+Environment.SystemDirectory+@"\system32\inetsrv\httpodbc.dll,5,GET,POST",
           ".licx,"+Environment.SystemDirectory+@"\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,5,GET,HEAD,POST,DEBUG",
           //".php,"+Environment.SystemDirectory+@"\system32\php5isapi.dll,5",
           ".rem,"+Environment.SystemDirectory+@"\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,1,GET,HEAD,POST,DEBUG",
           ".resources,"+Environment.SystemDirectory+@"\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,5,GET,HEAD,POST,DEBUG",
           ".resx,"+Environment.SystemDirectory+@"\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,5,GET,HEAD,POST,DEBUG",
           ".shtm,"+Environment.SystemDirectory+@"\system32\inetsrv\ssinc.dll,5,GET,POST",
           ".shtml,"+Environment.SystemDirectory+@"\system32\inetsrv\ssinc.dll,5,GET,POST",
           ".soap,"+Environment.SystemDirectory+@"\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,1,GET,HEAD,POST,DEBUG",
           ".stm,"+Environment.SystemDirectory+@"\system32\inetsrv\ssinc.dll,5,GET,POST",
           ".vb,"+Environment.SystemDirectory+@"\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,5,GET,HEAD,POST,DEBUG",
           ".vbproj,"+Environment.SystemDirectory+@"\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,5,GET,HEAD,POST,DEBUG",
           ".vsdisco,"+Environment.SystemDirectory+@"\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,1,GET,HEAD,POST,DEBUG",
           ".webinfo,"+Environment.SystemDirectory+@"\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,5,GET,HEAD,POST,DEBUG"
   };

            //支持 asp/php
            _script4 = new string[9]{".asa,"+Environment.SystemDirectory+@"\system32\inetsrv\asp.dll,5,GET,HEAD,POST,TRACE",
          ".asp,"+Environment.SystemDirectory+@"\system32\inetsrv\asp.dll,5,GET,HEAD,POST,TRACE",
          ".cdx,"+Environment.SystemDirectory+@"\system32\inetsrv\asp.dll,5,GET,HEAD,POST,TRACE",
          ".cer,"+Environment.SystemDirectory+@"\system32\inetsrv\asp.dll,5,GET,HEAD,POST,TRACE",
          ".idc,"+Environment.SystemDirectory+@"\system32\inetsrv\httpodbc.dll,5,GET,POST",
          ".php,"+Environment.SystemDirectory+@"\system32\php5isapi.dll,5",
          ".shtm,"+Environment.SystemDirectory+@"\system32\inetsrv\ssinc.dll,5,GET,POST",
          ".shtml,"+Environment.SystemDirectory+@"\system32\inetsrv\ssinc.dll,5,GET,POST",
          ".stm,"+Environment.SystemDirectory+@"\system32\inetsrv\ssinc.dll,5,GET,POST"
         };
        }
    }

    public struct NewWebSiteInfo
    {
        public object Bindings;
        public string SiteName;
        public string DirectoryPath;
        public IdentityAuthInfo AccessUser;
        public string[] ScriptMaps;
        public string AppPoolName;

        public bool LogFileLocaltimeRollover;
        public string LogFileDirectory;
    }
    public struct WebSiteInfo
    {
        public object Bindings;
        public string SiteName;
        public string DirectoryPath;
        public IdentityAuthInfo AccessUser;
        public object ScriptMaps;
        public string AppPoolName;
        public WebSiteState State;
        public bool LogFileLocaltimeRollover;
        public string LogFileDirectory;
    }
    public struct WebSiteBaseInfo
    {
        public string SiteName;
        public string SiteID;
        public WebSiteState State;
        public string AppPoolName;
    }
    public enum WebSiteParameter
    {
        /// <summary>
        /// 站点的访问标记和权限|int
        /// </summary>
        AccessFlags,
        /// <summary>
        /// 是否使用本地时间创建日志|bool
        /// </summary>
        LogFileLocaltimeRollover,
        /// <summary>
        /// 站点是否自动启动|bool
        /// </summary>
        ServerAutoStart,
        /// <summary>
        /// 站点运行使用的匿名账户|string
        /// </summary>
        AnonymousUserName,
        /// <summary>
        /// 站点运行使用的匿名账户密码|string
        /// </summary>
        AnonymousUserPass,
        /// <summary>
        /// 站点类型|string
        /// </summary>
        KeyType,
        /// <summary>
        /// 站点标记，也是站点的名称|string
        /// </summary>
        ServerComment,
        /// <summary>
        /// 日志保存的目录|string
        /// </summary>
        LogFileDirectory,
        /// <summary>
        /// 服务器绑定|string[]
        /// </summary>
        ServerBindings,
        AllowKeepAlive,
        AppAllowClientDebug,
        /// <summary>
        /// 调试ASP|bool
        /// </summary>
        AppAllowDebugging,
        AspAllowOutOfProcComponents,
        /// <summary>
        /// ASP的会话状态|bool
        /// </summary>
        AspAllowSessionState,
        AspAppServiceFlags,
        AspBufferingLimit,
        AspBufferingOn,
        AspCalcLineNumber,
        AspCodepage,
        AspEnableApplicationRestart,
        AspEnableAspHtmlFallback,
        AspEnableChunkedEncoding,
        AspEnableParentPaths,
        AspEnableTypelibCache,
        /// <summary>
        /// 将ASP错误记录到系统日志|bool
        /// </summary>
        AspErrorsToNTLog,
        AspExceptionCatchEnable,
        AspExecuteInMTA,
        AspKeepSessionIDSecure,
        AspLCID,
        AspLogErrorRequests,
        AspMaxDiskTemplateCacheFiles,
        AspMaxRequestEntityAllowed,
        AspProcessorThreadMax,
        AspQueueConnectionTestTime,
        AspQueueTimeout,
        AspRequestQueueMax,
        AspRunOnEndAnonymously,
        AspScriptEngineCacheMax,
        AspScriptErrorSentToBrowser,
        AspScriptFileCacheSize,
        /// <summary>
        /// ASP执行超时时间|int
        /// </summary>
        AspScriptTimeout,
        AspSessionMax,
        AspSessionTimeout,
        AspTrackThreadingModel,
        AuthFlags,
        CacheISAPI,
        CGITimeout,
        ConnectionTimeout,
        ContentIndexed,
        DirBrowseFlags,
        LogExtFileFlags,
        LogFilePeriod,
        LogFileTruncateSize,
        LogType,
        MaxConnections,
        PasswordChangeFlags,
        /// <summary>
        /// 网站所在的进程池|string
        /// </summary>
        AppPoolId,
        /// <summary>
        /// ASP错误信息|string
        /// </summary>
        AspScriptErrorMessage,
        AspScriptLanguage,
        /// <summary>
        /// 默认文档|string[]
        /// </summary>
        DefaultDoc,
        LogOdbcDataSource,
        LogOdbcPassword,
        LogOdbcTableName,
        LogOdbcUserName,
        LogPluginClsid,
        AspDiskTemplateCacheDirectory,
        /// <summary>
        /// 自定义的HTTP头|string[]
        /// </summary>
        HttpCustomHeaders,
        HttpErrors,
        /// <summary>
        /// 站点校本映射|string
        /// </summary>
        ScriptMaps,
        /// <summary>
        /// 站点最大使用带宽|string[]
        /// </summary>
        MaxBandwidth,
        AdminACL,
    }

    public struct AppPoolBaseInfo
    {
        public string PoolName;
        public AppPoolState State;
    }
    public enum WebSiteState
    {
        Running = 2,
        Stoped = 4,
    }
    public enum AppPoolState
    {
        Running = 2,
        Stoped = 4,
    }
    public enum AppPoolOption
    {
        Start=1,
        Stop=2,
        Recycle=3,
    }
    public enum AppPoolParameter
    {
        AppPoolIdentityType,
        MaxProcesses,
        Win32Error,
        AppPoolCommand,
        AppPoolAutoStart,
        AppPoolState,
        KeyType,
        AppPoolQueueLength,
        CPULimit,
        CPUResetInterval,
        DisallowOverlappingRotation,
        DisallowRotationOnConfigChange,
        IdleTimeout,
        LoadBalancerCapabilities,
        LogEventOnRecycle,
        OrphanWorkerProcess,
        PeriodicRestartMemory,
        PeriodicRestartPrivateMemory,
        PeriodicRestartRequests,
        PeriodicRestartTime,
        PingingEnabled,
        PingInterval,
        PingResponseTime,
        RapidFailProtection,
        RapidFailProtectionInterval,
        RapidFailProtectionMaxCrashes,
        ShutdownTimeLimit,
        SMPAffinitized,
        SMPProcessorAffinityMask,
        StartupTimeLimit,
        AdminACL,
        WAMUserName,
        WAMUserPass,
    }

    public enum WebVirtualDirParameter
    {
        AppIsolated,
        AnonymousUserPass,
        AppFriendlyName,
        AppPoolId,
        AppRoot,
        KeyType,
        Path,
        UNCPassword,
        AccessFlags,
        AnonymousUserName,
        AppAllowClientDebug,
        AppAllowDebugging,
        AspAllowOutOfProcComponents,
        AspAllowSessionState,
        AspAppServiceFlags,
        AspBufferingLimit,
        AspBufferingOn,
        AspCalcLineNumber,
        AspCodepage,
        AspEnableApplicationRestart,
        AspEnableAspHtmlFallback,
        AspEnableChunkedEncoding,
        AspEnableParentPaths,
        AspEnableTypelibCache,
        AspErrorsToNTLog,
        AspExceptionCatchEnable,
        AspExecuteInMTA,
        AspKeepSessionIDSecure,
        AspLCID,
        AspLogErrorRequests,
        AspMaxDiskTemplateCacheFiles,
        AspMaxRequestEntityAllowed,
        AspProcessorThreadMax,
        AspQueueConnectionTestTime,
        AspQueueTimeout,
        AspRequestQueueMax,
        AspRunOnEndAnonymously,
        AspScriptEngineCacheMax,
        AspScriptErrorSentToBrowser,
        AspScriptFileCacheSize,
        AspScriptTimeout,
        AspSessionMax,
        AspSessionTimeout,
        AspTrackThreadingModel,
        AuthFlags,
        CacheISAPI,
        CGITimeout,
        ContentIndexed,
        DirBrowseFlags,
        PasswordChangeFlags,
        AspScriptErrorMessage,
        AspScriptLanguage,
        AuthChangeURL,
        AuthExpiredUnsecureURL,
        AuthExpiredURL,
        AuthNotifyPwdExpUnsecureURL,
        AuthNotifyPwdExpURL,
        DefaultDoc,
        AspDiskTemplateCacheDirectory,
        HttpCustomHeaders,
        HttpErrors,
        ScriptMaps,
        AdminACL,
    }
}