using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace Silmoon.Secure
{
    public class SecureProvider
    {
        private static RSACryptoServiceProvider defaultRSA { get; set; } = null;
        public static RSACryptoServiceProvider LoadDefaultRSAKeyFile(string rsaKeyXmlFile = @"C:\rsa_private.xml")
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && rsaKeyXmlFile == @"C:\rsa_private.xml") rsaKeyXmlFile = "/etc/rsa_private.xml";
            if (File.Exists(rsaKeyXmlFile))
            {
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

                string xml = File.ReadAllText(rsaKeyXmlFile);
                try
                {
                    rsa.FromXmlString(xml);
                }
                catch (Exception ex)
                {
                    rsa.Dispose();
                    throw ex;
                }
                defaultRSA = rsa;
                return rsa;
            }
            else
                throw new FileNotFoundException("RSA密钥XML字符串文件没有找到。", rsaKeyXmlFile);
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
