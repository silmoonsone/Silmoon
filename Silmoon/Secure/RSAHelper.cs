using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Silmoon.Secure
{
    public class RSAHelper
    {

        public RSAHelper()
        {

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
                return rsa;
            }
            else
                throw new FileNotFoundException("RSA密钥XML字符串文件没有找到。", rsaXmlFileName);
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
        public static string GetPublicKey(string privateKeyXml)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(privateKeyXml);
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
