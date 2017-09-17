using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Silmoon.Security
{
    public class EncryptHelper
    {
        public string AesEncrypt(string source, string key)
        {
            using (AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider())
            {
                aesProvider.Key = UTF8Encoding.UTF8.GetBytes(key); //GetAesKey(key);固定32位秘钥方法
                aesProvider.Mode = CipherMode.ECB;
                aesProvider.Padding = PaddingMode.PKCS7;
                using (ICryptoTransform cryptoTransform = aesProvider.CreateEncryptor())
                {
                    byte[] inputBuffers = Encoding.UTF8.GetBytes(source);
                    byte[] results = cryptoTransform.TransformFinalBlock(inputBuffers, 0, inputBuffers.Length);
                    aesProvider.Clear();
                    aesProvider.Dispose();
                    return Convert.ToBase64String(results, 0, results.Length);
                }
            }
        }
        public string AesDecrypt(string source, string key)
        {
            using (AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider())
            {
                aesProvider.Key = UTF8Encoding.UTF8.GetBytes(key); //GetAesKey(key);固定32位秘钥方法
                aesProvider.Mode = CipherMode.ECB;
                aesProvider.Padding = PaddingMode.PKCS7;
                using (ICryptoTransform cryptoTransform = aesProvider.CreateDecryptor())
                {
                    byte[] inputBuffers = Convert.FromBase64String(source);
                    byte[] results = cryptoTransform.TransformFinalBlock(inputBuffers, 0, inputBuffers.Length);
                    aesProvider.Clear();
                    return Encoding.UTF8.GetString(results);
                }
            }
        }


        public string SignData(string data, string xmlPrivateKey, string encoding = "UTF-8")
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
    }
}
