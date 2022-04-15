using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Silmoon.Secure
{
    public class EncryptHelper
    {
        public static byte[] AesEncrypt(string source, string key, CipherMode cipherMode = CipherMode.ECB, PaddingMode paddingMode = PaddingMode.PKCS7)
        {
            var osource = Encoding.UTF8.GetBytes(source);
            var okey = Encoding.UTF8.GetBytes(key);
            return AesEncrypt(osource, okey, cipherMode, paddingMode);
        }
        public static byte[] AesEncrypt(byte[] source, byte[] key, CipherMode cipherMode = CipherMode.ECB, PaddingMode paddingMode = PaddingMode.PKCS7)
        {
            using (RijndaelManaged aesProvider = new RijndaelManaged())
            {
                aesProvider.Key = key;
                aesProvider.Mode = cipherMode;
                aesProvider.Padding = paddingMode;
                using (ICryptoTransform cryptoTransform = aesProvider.CreateEncryptor())
                {
                    byte[] inputBuffers = source;
                    byte[] results = cryptoTransform.TransformFinalBlock(inputBuffers, 0, inputBuffers.Length);
                    aesProvider.Clear();
                    aesProvider.Dispose();
                    return results;
                }
            }
        }
        public static byte[] AesDecrypt(string source, string key, CipherMode cipherMode = CipherMode.ECB, PaddingMode paddingMode = PaddingMode.PKCS7)
        {
            var osource = Encoding.UTF8.GetBytes(source);
            var okey = Encoding.UTF8.GetBytes(key);
            return AesDecrypt(osource, okey, cipherMode, paddingMode);
        }
        public static byte[] AesDecrypt(byte[] source, byte[] key, CipherMode cipherMode = CipherMode.ECB, PaddingMode paddingMode = PaddingMode.PKCS7)
        {
            using (RijndaelManaged aesProvider = new RijndaelManaged())
            {
                aesProvider.Key = key;
                aesProvider.Mode = cipherMode;
                aesProvider.Padding = paddingMode;
                using (ICryptoTransform cryptoTransform = aesProvider.CreateDecryptor())
                {
                    byte[] inputBuffers = source;
                    byte[] results = cryptoTransform.TransformFinalBlock(inputBuffers, 0, inputBuffers.Length);
                    aesProvider.Clear();
                    return results;
                }
            }
        }


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


        public static string DesEncrypt(string data, string KEY_64, string IV_64)
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
        public static string DesDecrypt(string data, string KEY_64, string IV_64)
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



        public static string RsaEncrypt(string xmlpublicKey, string data, string encoding = "UTF-8")
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            byte[] cipherbytes;
            provider.FromXmlString(xmlpublicKey);//加载公钥  

            cipherbytes = provider.Encrypt(Encoding.GetEncoding(encoding).GetBytes(data), false);

            return Convert.ToBase64String(cipherbytes);
        }
        public static string RsaDecrypt(string xmlPrivateKey, string data, string encoding = "UTF-8")
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            byte[] cipherbytes;
            provider.FromXmlString(xmlPrivateKey);//加载私钥  

            cipherbytes = provider.Decrypt(Convert.FromBase64String(data), false);

            return Encoding.GetEncoding(encoding).GetString(cipherbytes);
        }
        public static string RsaSignData(string data, string xmlPrivateKey, string encoding = "UTF-8")
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            provider.FromXmlString(xmlPrivateKey);//加载私钥  
            var dataBytes = Encoding.GetEncoding(encoding).GetBytes(data);
            byte[] signatureBytes = provider.SignData(dataBytes, new SHA1CryptoServiceProvider());
            return Convert.ToBase64String(signatureBytes);
        }
        public static bool RsaVerifySign(string data, string xmlpublicKey, string signature, string encoding = "UTF-8")
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            //导入公钥，准备验证签名  
            provider.FromXmlString(xmlpublicKey);//加载公钥  
                                                 //返回数据验证结果  
            byte[] Data = Encoding.GetEncoding(encoding).GetBytes(data);
            byte[] rgbSignature = Convert.FromBase64String(signature);

            return provider.VerifyData(Data, new SHA1CryptoServiceProvider(), rgbSignature);

        }

    }
}
