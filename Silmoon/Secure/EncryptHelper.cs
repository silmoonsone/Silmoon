using Silmoon.Extension;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace Silmoon.Secure
{
    public class EncryptHelper
    {
        public static byte[] AesEncrypt(byte[] Data, byte[] Key, CipherMode CipherMode = CipherMode.ECB, PaddingMode PaddingMode = PaddingMode.PKCS7)
        {
            if (Data is null) return null;
            if (Data.Length == 0) return new byte[0];

            using (RijndaelManaged aesProvider = new RijndaelManaged())
            {
                aesProvider.Key = Key;
                aesProvider.Mode = CipherMode;
                aesProvider.Padding = PaddingMode;
                using (ICryptoTransform cryptoTransform = aesProvider.CreateEncryptor())
                {
                    byte[] results = cryptoTransform.TransformFinalBlock(Data, 0, Data.Length);
                    aesProvider.Clear();
                    aesProvider.Dispose();
                    return results;
                }
            }
        }
        public static byte[] AesDecrypt(byte[] EncryptedData, byte[] Key, CipherMode CipherMode = CipherMode.ECB, PaddingMode PaddingMode = PaddingMode.PKCS7)
        {
            if (EncryptedData is null) return null;
            if (EncryptedData.Length == 0) return new byte[0];

            using (RijndaelManaged aesProvider = new RijndaelManaged())
            {
                aesProvider.Key = Key;
                aesProvider.Mode = CipherMode;
                aesProvider.Padding = PaddingMode;
                using (ICryptoTransform cryptoTransform = aesProvider.CreateDecryptor())
                {
                    byte[] results = cryptoTransform.TransformFinalBlock(EncryptedData, 0, EncryptedData.Length);
                    aesProvider.Clear();
                    return results;
                }
            }
        }

        public static string AesEncryptStringToBase64String(string Data, string Key, CipherMode CipherMode = CipherMode.ECB, PaddingMode PaddingMode = PaddingMode.PKCS7)
        {
            var data = Encoding.UTF8.GetBytes(Data);
            var key = Encoding.UTF8.GetBytes(Key);
            return Convert.ToBase64String(AesEncrypt(data, key, CipherMode, PaddingMode));
        }
        public static string AesDecryptBase64StringToString(string Based64String, string Key, CipherMode CipherMode = CipherMode.ECB, PaddingMode PaddingMode = PaddingMode.PKCS7)
        {
            var data = Convert.FromBase64String(Based64String);
            var key = Encoding.UTF8.GetBytes(Key);
            return Encoding.UTF8.GetString(AesDecrypt(data, key, CipherMode, PaddingMode));
        }

        public static string AesEncryptStringToHexString(string Data, byte[] Key, CipherMode CipherMode = CipherMode.ECB, PaddingMode PaddingMode = PaddingMode.PKCS7)
        {
            var data = Encoding.UTF8.GetBytes(Data);
            return AesEncrypt(data, Key, CipherMode, PaddingMode).ByteArrayToHexString();
        }
        public static string AesDecryptHexStringToString(string HexString, byte[] Key, CipherMode CipherMode = CipherMode.ECB, PaddingMode PaddingMode = PaddingMode.PKCS7)
        {
            var data = HexString.HexStringToByteArray();
            return Encoding.UTF8.GetString(AesDecrypt(data, Key, CipherMode, PaddingMode));
        }



        public static byte[] AesEncryptV2(byte[] Data, string Key)
        {
            byte[] iv = new byte[16];
            byte[] keyBytes = Encoding.UTF8.GetBytes(Key);
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.IV = iv;

                using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    array = encryptor.TransformFinalBlock(Data, 0, Data.Length);
                    return array;
                }
            }
        }
        public static byte[] AesDecryptV2(byte[] Data, string Key)
        {
            byte[] iv = new byte[16];
            byte[] keyBytes = Encoding.UTF8.GetBytes(Key);

            using (Aes aes = Aes.Create())
            {
                try
                {
                    aes.Key = keyBytes;
                    aes.IV = iv;

                    using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                    {
                        byte[] array = decryptor.TransformFinalBlock(Data, 0, Data.Length);
                        return array;
                    }
                }
                catch
                {
                    return null;
                }
            }
        }
        public static string AesEncryptStringV2(string PlainText, string Key, bool UseHexString = true)
        {
            byte[] cipherBytes = AesEncryptV2(Encoding.UTF8.GetBytes(PlainText), Key);
            if (UseHexString)
            {
                StringBuilder stringBuilder = new StringBuilder(cipherBytes.Length * 2);
                foreach (byte b in cipherBytes)
                {
                    stringBuilder.AppendFormat("{0:x2}", b);
                }
                return stringBuilder.ToString();
            }
            else
            {
                return Convert.ToBase64String(cipherBytes);
            }
        }
        public static string AesDecryptStringV2(string CipherText, string Key, bool UseHexString = true)
        {
            byte[] cipherBytes;
            if (UseHexString)
            {
                cipherBytes = new byte[CipherText.Length / 2];
                for (int i = 0; i < cipherBytes.Length; i++)
                {
                    cipherBytes[i] = Convert.ToByte(CipherText.Substring(i * 2, 2), 16);
                }
            }
            else
            {
                cipherBytes = Convert.FromBase64String(CipherText);
            }
            byte[] plainBytes = AesDecryptV2(cipherBytes, Key);
            if (plainBytes is null) return null;
            else
                return Encoding.UTF8.GetString(plainBytes);
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
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && rsaKeyXmlFile == @"C:\rsa_private.xml") rsaKeyXmlFile = "/etc/rsa_private.xml";

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
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && rsaKeyXmlFile == @"C:\rsa_private.xml") rsaKeyXmlFile = "/etc/rsa_private.xml";

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
                return provider.Encrypt(data, true);
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

        public static object AesEncryptStringToHexString(string s, object value)
        {
            throw new NotImplementedException();
        }
    }
}
