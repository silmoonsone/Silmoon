using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Silmoon.Secure
{
    public class SecureHelper
    {
        public static string RsaEncrypt(string source, string rsaKeyXmlFile = @"C:\rsa_private.xml")
        {
            if (string.IsNullOrEmpty(source)) return "";

            return RsaEncrypt(source, SecureProvider.GetDefaultRSA(rsaKeyXmlFile));
        }
        public static string RsaDecrypt(string secert, string rsaKeyXmlFile = @"C:\rsa_private.xml")
        {
            if (string.IsNullOrEmpty(secert)) return "";

            return RsaDecrypt(secert, SecureProvider.GetDefaultRSA(rsaKeyXmlFile));
        }

        public static string RsaEncrypt(string source, RSACryptoServiceProvider rsa)
        {
            byte[] data = Encoding.UTF8.GetBytes(source);

            data = rsa.Encrypt(data, true);
            return Convert.ToBase64String(data);
        }
        public static string RsaDecrypt(string secert, RSACryptoServiceProvider rsa)
        {
            byte[] data = Convert.FromBase64String(secert);

            data = rsa.Decrypt(data, true);
            return Encoding.UTF8.GetString(data);
        }


        public static string DsaEncrypt(string data, string KEY_64, string IV_64)
        {
            byte[] byKey = Encoding.ASCII.GetBytes(KEY_64);
            byte[] byIV = Encoding.ASCII.GetBytes(IV_64);

            using (DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider())
            {
                //int i = cryptoProvider.KeySize;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateEncryptor(byKey, byIV), CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(data);
                            streamWriter.Flush();
                            cryptoStream.FlushFinalBlock();
                            streamWriter.Flush();
                            return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
                        }
                    }
                }
            }
        }
        public static string DsaDecrypt(string data, string KEY_64, string IV_64)
        {
            byte[] byKey = Encoding.ASCII.GetBytes(KEY_64);
            byte[] byIV = Encoding.ASCII.GetBytes(IV_64);

            byte[] base64;
            try
            {
                base64 = Convert.FromBase64String(data);
            }
            catch
            {
                return null;
            }

            using (DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider())
            {
                using (MemoryStream memoryStream = new MemoryStream(base64))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateDecryptor(byKey, byIV), CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
