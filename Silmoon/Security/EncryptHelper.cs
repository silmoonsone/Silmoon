using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Silmoon.Security
{
    public class EncryptHelper
    {
        public static byte[] AesEncrypt(string source, string key, CipherMode cipherMode = CipherMode.ECB, PaddingMode paddingMode = PaddingMode.PKCS7)
        {
            var okey = Encoding.UTF8.GetBytes(key);
            return AesEncrypt(source, okey, cipherMode, paddingMode);
        }
        public static byte[] AesEncrypt(string source, byte[] key, CipherMode cipherMode = CipherMode.ECB, PaddingMode paddingMode = PaddingMode.PKCS7)
        {
            using (RijndaelManaged aesProvider = new RijndaelManaged())
            {
                aesProvider.Key = key; //GetAesKey(key);固定32位秘钥方法
                aesProvider.Mode = cipherMode;
                aesProvider.Padding = paddingMode;
                using (ICryptoTransform cryptoTransform = aesProvider.CreateEncryptor())
                {
                    byte[] inputBuffers = Encoding.UTF8.GetBytes(source);
                    byte[] results = cryptoTransform.TransformFinalBlock(inputBuffers, 0, inputBuffers.Length);
                    aesProvider.Clear();
                    aesProvider.Dispose();
                    return results;
                }
            }
        }
        public static byte[] AesDecrypt(string source, string key, CipherMode cipherMode = CipherMode.ECB, PaddingMode paddingMode = PaddingMode.PKCS7)
        {
            var okey = Encoding.UTF8.GetBytes(key);
            return AesDecrypt(source, okey, cipherMode, paddingMode);
        }
        public static byte[] AesDecrypt(string source, byte[] key, CipherMode cipherMode = CipherMode.ECB, PaddingMode paddingMode = PaddingMode.PKCS7)
        {
            using (RijndaelManaged aesProvider = new RijndaelManaged())
            {
                aesProvider.Key = key; //GetAesKey(key);固定32位秘钥方法
                aesProvider.Mode = cipherMode;
                aesProvider.Padding = paddingMode;
                using (ICryptoTransform cryptoTransform = aesProvider.CreateDecryptor())
                {
                    byte[] inputBuffers = Convert.FromBase64String(source);
                    byte[] results = cryptoTransform.TransformFinalBlock(inputBuffers, 0, inputBuffers.Length);
                    aesProvider.Clear();
                    return results;
                }
            }
        }


        public static string SignData(string data, string xmlPrivateKey, string encoding = "UTF-8")
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            provider.FromXmlString(xmlPrivateKey);//加载私钥  
            var dataBytes = Encoding.GetEncoding(encoding).GetBytes(data);
            byte[] signatureBytes = provider.SignData(dataBytes, new SHA1CryptoServiceProvider());
            return Convert.ToBase64String(signatureBytes);
        }
        public static bool VerifySign(string data, string xmlpublicKey, string signature, string encoding = "UTF-8")
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            //导入公钥，准备验证签名  
            provider.FromXmlString(xmlpublicKey);//加载公钥  
                                                 //返回数据验证结果  
            byte[] Data = Encoding.GetEncoding(encoding).GetBytes(data);
            byte[] rgbSignature = Convert.FromBase64String(signature);

            return provider.VerifyData(Data, new SHA1CryptoServiceProvider(), rgbSignature);

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

        public static string SHA1(string s)
        {
            using (var c = new SHA1CryptoServiceProvider())
            {
                byte[] bresult = c.ComputeHash(Encoding.UTF8.GetBytes(s));
                return BitConverter.ToString(bresult).Replace("-", "");
            }
        }
        public static string MD5(string s)
        {
            using (var c = new MD5CryptoServiceProvider())
            {
                byte[] bresult = c.ComputeHash(Encoding.UTF8.GetBytes(s));
                return BitConverter.ToString(bresult).Replace("-", "");
            }
        }

    }
}
