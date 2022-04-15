using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Silmoon.Secure
{
    public class EncryptHelper
    {
        public static string AesEncrypt(string data, string key, CipherMode cipherMode = CipherMode.ECB, PaddingMode paddingMode = PaddingMode.PKCS7)
        {
            if (data is null) return null;
            if (data == "") return "";

            var osource = Encoding.UTF8.GetBytes(data);
            var okey = Encoding.UTF8.GetBytes(key);
            return Convert.ToBase64String(AesEncrypt(osource, okey, cipherMode, paddingMode));
        }
        public static byte[] AesEncrypt(byte[] data, byte[] key, CipherMode cipherMode = CipherMode.ECB, PaddingMode paddingMode = PaddingMode.PKCS7)
        {
            using (RijndaelManaged aesProvider = new RijndaelManaged())
            {
                aesProvider.Key = key;
                aesProvider.Mode = cipherMode;
                aesProvider.Padding = paddingMode;
                using (ICryptoTransform cryptoTransform = aesProvider.CreateEncryptor())
                {
                    byte[] inputBuffers = data;
                    byte[] results = cryptoTransform.TransformFinalBlock(inputBuffers, 0, inputBuffers.Length);
                    aesProvider.Clear();
                    aesProvider.Dispose();
                    return results;
                }
            }
        }
        public static string AesDecrypt(string data, string key, CipherMode cipherMode = CipherMode.ECB, PaddingMode paddingMode = PaddingMode.PKCS7)
        {
            if (data is null) return null;
            if (data == "") return "";

            var osource = Convert.FromBase64String(data);
            var okey = Encoding.UTF8.GetBytes(key);
            return Encoding.UTF8.GetString(AesDecrypt(osource, okey, cipherMode, paddingMode));
        }
        public static byte[] AesDecrypt(byte[] data, byte[] key, CipherMode cipherMode = CipherMode.ECB, PaddingMode paddingMode = PaddingMode.PKCS7)
        {
            using (RijndaelManaged aesProvider = new RijndaelManaged())
            {
                aesProvider.Key = key;
                aesProvider.Mode = cipherMode;
                aesProvider.Padding = paddingMode;
                using (ICryptoTransform cryptoTransform = aesProvider.CreateDecryptor())
                {
                    byte[] inputBuffers = data;
                    byte[] results = cryptoTransform.TransformFinalBlock(inputBuffers, 0, inputBuffers.Length);
                    aesProvider.Clear();
                    return results;
                }
            }
        }




        public static string DesEncrypt(string data, string KEY_64, string IV_64)
        {
            if (data is null) return null;
            if (data == "") return "";

            byte[] odata = Encoding.UTF8.GetBytes(data);
            byte[] byKey = Encoding.ASCII.GetBytes(KEY_64);
            byte[] byIV = Encoding.ASCII.GetBytes(IV_64);

            return Convert.ToBase64String(DesEncrypt(odata, byKey, byIV));
        }
        public static byte[] DesEncrypt(byte[] data, byte[] KEY_64, byte[] IV_64)
        {
            using (DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider())
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateEncryptor(KEY_64, IV_64), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(data, 0, data.Length);
                        cryptoStream.Flush();
                        cryptoStream.FlushFinalBlock();
                        cryptoStream.Flush();
                        byte[] result = new byte[memoryStream.Length];
                        memoryStream.Position = 0;
                        memoryStream.Read(result, 0, result.Length);
                        return result;
                    }
                }
            }
        }
        public static string DesDecrypt(string data, string KEY_64, string IV_64)
        {
            if (data is null) return null;
            if (data == "") return "";

            byte[] odata = Convert.FromBase64String(data);
            byte[] byKey = Encoding.ASCII.GetBytes(KEY_64);
            byte[] byIV = Encoding.ASCII.GetBytes(IV_64);

            return Encoding.UTF8.GetString(DesDecrypt(odata, byKey, byIV));
        }
        public static byte[] DesDecrypt(byte[] data, byte[] KEY_64, byte[] IV_64)
        {
            using (DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider())
            {
                using (MemoryStream memoryStream = new MemoryStream(data))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateDecryptor(KEY_64, IV_64), CryptoStreamMode.Read))
                    {
                        List<byte> result = new List<byte>();
                        byte[] buff = new byte[2048];
                        var readLen = 0;
                        do
                        {
                            readLen = cryptoStream.Read(buff, 0, buff.Length);
                            for (int i = 0; i < readLen; i++)
                            {
                                result.Add(buff[i]);
                            }

                        } while (readLen > 0);
                        return result.ToArray();
                    }
                }
            }
        }



        public static string RsaEncrypt(string data, string rsaKeyXmlFile = @"C:\rsa_private.xml")
        {
            if (File.Exists(rsaKeyXmlFile))
            {
                string rsaXml = File.ReadAllText(rsaKeyXmlFile);
                return RsaEncrypt(rsaXml, data, "UTF-8");
            }
            else
            {
                throw new FileNotFoundException("RSA密钥XML字符串文件没有找到。", rsaKeyXmlFile);
            }
        }
        public static string RsaDecrypt(string data, string rsaKeyXmlFile = @"C:\rsa_private.xml")
        {
            if (File.Exists(rsaKeyXmlFile))
            {
                string rsaXml = File.ReadAllText(rsaKeyXmlFile);
                return RsaDecrypt(rsaXml, data, "UTF-8");
            }
            else
            {
                throw new FileNotFoundException("RSA密钥XML字符串文件没有找到。", rsaKeyXmlFile);
            }
        }
        public static string RsaEncrypt(string data, RSACryptoServiceProvider rsa)
        {
            if (data is null) return null;
            if (data == "") return "";

            byte[] bytes = Encoding.UTF8.GetBytes(data);

            bytes = rsa.Encrypt(bytes, true);
            return Convert.ToBase64String(bytes);
        }
        public static string RsaDecrypt(string data, RSACryptoServiceProvider rsa)
        {
            if (data is null) return null;
            if (data == "") return "";

            byte[] bytes = Convert.FromBase64String(data);

            bytes = rsa.Decrypt(bytes, true);
            return Encoding.UTF8.GetString(bytes);
        }


        public static string RsaEncrypt(string xmlpublicKey, string data, string encoding = "UTF-8")
        {
            var encode = Encoding.GetEncoding(encoding);
            return Convert.ToBase64String(RsaEncrypt(xmlpublicKey, encode.GetBytes(data)));
        }
        public static byte[] RsaEncrypt(string xmlpublicKey, byte[] data)
        {
            using (RSACryptoServiceProvider provider = new RSACryptoServiceProvider())
            {
                provider.FromXmlString(xmlpublicKey);
                return provider.Encrypt(data, false);
            }
        }
        public static string RsaDecrypt(string xmlPrivateKey, string data, string encoding = "UTF-8")
        {
            var encode = Encoding.GetEncoding(encoding);
            return encode.GetString(RsaDecrypt(xmlPrivateKey, Convert.FromBase64String(data)));
        }
        public static byte[] RsaDecrypt(string xmlPrivateKey, byte[] data)
        {
            using (RSACryptoServiceProvider provider = new RSACryptoServiceProvider())
            {
                provider.FromXmlString(xmlPrivateKey);
                return provider.Decrypt(data, true);
            }
        }


        public static string RsaSignData(string data, string xmlPrivateKey, string encoding = "UTF-8")
        {
            var encode = Encoding.GetEncoding(encoding);
            return Convert.ToBase64String(RsaSignData(encode.GetBytes(data), xmlPrivateKey));
        }
        public static byte[] RsaSignData(byte[] data, string xmlPrivateKey)
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            provider.FromXmlString(xmlPrivateKey);
            return provider.SignData(data, new SHA1CryptoServiceProvider());
        }
        public static bool RsaVerifySign(string data, string xmlpublicKey, string signature)
        {
            return RsaVerifySign(Encoding.UTF8.GetBytes(data), xmlpublicKey, Convert.FromBase64String(signature));
        }
        public static bool RsaVerifySign(byte[] data, string xmlpublicKey, byte[] signature)
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            provider.FromXmlString(xmlpublicKey);

            return provider.VerifyData(data, new SHA1CryptoServiceProvider(), signature);
        }

    }
}
