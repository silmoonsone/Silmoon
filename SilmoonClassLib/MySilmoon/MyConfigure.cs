using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Silmoon.MySilmoon.Instance;
using Silmoon.Security;

namespace Silmoon.MySilmoon
{
    public class MyConfigure
    {
        static string LicenseEncryptedString = "";

        public static void ClearCache()
        {
            LicenseEncryptedString = "";
        }
        public static VersionResult GetRemoteVersion(string productString, string userIdentity = "")
        {
            VersionResult result = new VersionResult();
            try
            {
                string url = "https://encrypted.silmoon.com/apps/apis/config?appName=" + productString + "&userIdentity=" + userIdentity + "&configName=_validation&outType=text/xml";

                XmlDocument xml = new XmlDocument();
                xml.Load(url);
                result.ExpiredVersion = int.Parse(xml["validationResult"]["version"].Attributes["expiredVersion"].Value);
                result.NotificationVersion = int.Parse(xml["validationResult"]["version"].Attributes["notificationVersion"].Value);
                result.LatestVersion = int.Parse(xml["validationResult"]["version"].Attributes["latestVersion"].Value);
                result.UserIdentityStateCode = int.Parse(xml["validationResult"]["userIdentity"].Attributes["stateCode"].Value);
                result.UserIdentityStateMessage = xml["validationResult"]["userIdentity"].Attributes["stateMessage"].Value;
            }
            catch (Exception ex)
            {
                result.Error = ex;
            }
            return result;
        }
        public static LicenseResult GetRemoteLicense(string productString)
        {
            LicenseResult result = new LicenseResult();
            try
            {
                string url = "https://encrypted.silmoon.com/apps/apis/config?appName=" + productString + "&configName=_license&outType=text/xml";

                XmlDocument xml = new XmlDocument();
                xml.Load(url);
                result.unlimited_state = xml["license"]["license_config_1"]["unlimited"]["state"].InnerText;
                result.unlimited_key = xml["license"]["license_config_1"]["unlimited"]["key"].InnerText;
            }
            catch (Exception ex)
            {
                result.Error = ex;
            }
            return result;
        }

        public static string GetLicenseEncryptedString(string productString, bool force = false)
        {
            if (force) LicenseEncryptedString = "";
            if (!string.IsNullOrEmpty(LicenseEncryptedString)) return LicenseEncryptedString;

            string sysDatFile = Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\slf.dat";
            string appendKey = "";
            string keyFileContent = "";

            if (File.Exists(sysDatFile))
            {
                if (File.Exists(Application.StartupPath + "\\license.slf") && (keyFileContent = File.ReadAllText(Application.StartupPath + "\\license.slf")) != "")
                {
                    string[] lines = File.ReadAllLines(sysDatFile);
                    foreach (var item in lines)
                    {
                        string[] lineArr = item.Split('\0');
                        if (lineArr.Length == 2)
                        {
                            if (lineArr[0] == productString)
                            {
                                appendKey = lineArr[1];
                                break;
                            }
                        }
                    }

                    if (appendKey != "")
                    {
                        using (CSEncrypt enc = new CSEncrypt(appendKey))
                        {
                            try
                            {
                                string s = enc.Decrypt(keyFileContent);
                                LicenseEncryptedString = s;
                                return LicenseEncryptedString;
                            }
                            catch
                            {
                                return null;
                            }
                        }
                    }
                    else
                        return null;
                }
                else
                    return null;
            }
            else
                return null;
        }
        /// <summary>
        /// 获取加密授权配置中的一个配置项目
        /// </summary>
        /// <param name="productString">当前产品的标识名称</param>
        /// <returns></returns>
        public static NameValueCollection GetLicenseEncryptedConfigure(string productString)
        {
            NameValueCollection result = new NameValueCollection();
            string s = GetLicenseEncryptedString(productString);
            if (!string.IsNullOrEmpty(s))
            {
                result = SmString.AnalyzeNameValue(s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries), "=");
            }
            return result;
        }
        public static int GetLicenseLevelCode(string productString)
        {
            NameValueCollection values = GetLicenseEncryptedConfigure(productString);
            string levelCodeString = values["levelCode"];
            int levelCode = 0;
            int.TryParse(levelCodeString, out levelCode);
            return levelCode;
        }
        public static string GetLicenseUserIdentity(string productString)
        {
            NameValueCollection values = GetLicenseEncryptedConfigure(productString);
            string clientString = values["userIdentity"];
            return SmString.FixNullString(clientString);
        }
    }
}