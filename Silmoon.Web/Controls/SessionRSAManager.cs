using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Silmoon.Web.Controls
{
    public class SessionRSAManager
    {

        public SessionRSAManager()
        {

        }

        /// <summary>
        /// 获取当前的用户会话控制器使用的RSA密钥，需要现调用LoadDefaultRSAKeyFile方法载入密钥。
        /// </summary>
        static RSACryptoServiceProvider defaultRSA = null;
        public static RSACryptoServiceProvider DefaultRSA
        {
            get
            {
                if (defaultRSA == null) LoadDefaultRSAKeyFile();
                return defaultRSA;
            }
        }

        public static RSACryptoServiceProvider LoadDefaultRSAKeyFile(string rsaXmlFileName = @"C:\rsa_private.xml")
        {
            if (File.Exists(rsaXmlFileName))
            {
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

                string xml = File.ReadAllText(rsaXmlFileName);
                try
                {
                    rsa.FromXmlString(xml);
                }
                catch (Exception ex)
                {
                    rsa.Clear();
                    throw ex;
                }
                defaultRSA = rsa;
            }
            else
                throw new FileNotFoundException("RSA密钥XML字符串文件没有找到。", rsaXmlFileName);
            return defaultRSA;
        }
        public static string GeneratorPrivateKey(int keyBytes = 1024)
        {
            string result = "";
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(keyBytes))
            {
                result = rsa.ToXmlString(true);
            }
            return result;
        }
        public static string FromPrivateGetPublicKey(string privateXml)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(privateXml);
                return rsa.ToXmlString(false);
            }
        }
        public static RSACryptoServiceProvider LoadRSA(string xml)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xml);
            return rsa;
        }
    }
}
