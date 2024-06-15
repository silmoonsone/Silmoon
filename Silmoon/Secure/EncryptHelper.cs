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
            var data = Data.GetBytes();
            var key = Key.GetBytes();
            return Convert.ToBase64String(AesEncrypt(data, key, CipherMode, PaddingMode));
        }
        public static string AesDecryptBase64StringToString(string Based64String, string Key, CipherMode CipherMode = CipherMode.ECB, PaddingMode PaddingMode = PaddingMode.PKCS7)
        {
            var data = Convert.FromBase64String(Based64String);
            var key = Key.GetBytes();
            return Encoding.UTF8.GetString(AesDecrypt(data, key, CipherMode, PaddingMode));
        }

        public static string AesEncryptStringToHexString(string Data, byte[] Key, CipherMode CipherMode = CipherMode.ECB, PaddingMode PaddingMode = PaddingMode.PKCS7)
        {
            var data = Data.GetBytes();
            return AesEncrypt(data, Key, CipherMode, PaddingMode).ByteArrayToHexString();
        }
        public static string AesDecryptHexStringToString(string HexString, byte[] Key, CipherMode CipherMode = CipherMode.ECB, PaddingMode PaddingMode = PaddingMode.PKCS7)
        {
            var data = HexString.HexStringToByteArray();
            return AesDecrypt(data.Data, Key, CipherMode, PaddingMode).GetString();
        }



        public static byte[] AesEncryptV2(byte[] Data, string Key)
        {
            byte[] iv = new byte[16];
            byte[] keyBytes = Key.GetBytes();
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
            byte[] keyBytes = Key.GetBytes();

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
            byte[] cipherBytes = AesEncryptV2(PlainText.GetBytes(), Key);
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
                return Convert.ToBase64String(cipherBytes);
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
            if (plainBytes is null)
                return null;
            else
                return plainBytes.GetString(Encoding.UTF8);
        }



        public static string DesEncrypt(string data, string KEY_64, string IV_64)
        {
            if (data is null) return null;
            if (data == "") return "";

            byte[] odata = data.GetBytes();
            byte[] byKey = KEY_64.GetBytes();
            byte[] byIV = IV_64.GetBytes();

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
            byte[] byKey = KEY_64.GetBytes();
            byte[] byIV = IV_64.GetBytes();

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



        public static string RsaEncrypt(string str, string rsaKeyXmlFile = @"C:\rsa_private.xml")
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && rsaKeyXmlFile == @"C:\rsa_private.xml") rsaKeyXmlFile = "/etc/rsa_private.xml";

            if (File.Exists(rsaKeyXmlFile))
            {
                string rsaXml = File.ReadAllText(rsaKeyXmlFile);
                return RsaEncrypt(rsaXml, str, "UTF-8");
            }
            else
            {
                throw new FileNotFoundException("RSA密钥XML字符串文件没有找到。", rsaKeyXmlFile);
            }
        }
        public static string RsaDecrypt(string base64EncryptedString, string rsaKeyXmlFile = @"C:\rsa_private.xml")
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && rsaKeyXmlFile == @"C:\rsa_private.xml") rsaKeyXmlFile = "/etc/rsa_private.xml";

            if (File.Exists(rsaKeyXmlFile))
            {
                string rsaXml = File.ReadAllText(rsaKeyXmlFile);
                return RsaDecrypt(rsaXml, base64EncryptedString, "UTF-8");
            }
            else
            {
                throw new FileNotFoundException("RSA密钥XML字符串文件没有找到。", rsaKeyXmlFile);
            }
        }

        public static string RsaEncrypt(string xmlPublicKey, string str, string encoding = "UTF-8") => Convert.ToBase64String(RsaEncrypt(xmlPublicKey, str.GetBytes(Encoding.GetEncoding(encoding))));
        public static byte[] RsaEncrypt(string xmlPublicKey, byte[] data)
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.FromXmlString(xmlPublicKey);
                return rsa.Encrypt(data, RSAEncryptionPadding.OaepSHA1);
            }
        }
        public static string RsaDecrypt(string xmlPrivateKey, string base64EncryptedString, string encoding = "UTF-8") => RsaDecrypt(xmlPrivateKey, Convert.FromBase64String(base64EncryptedString)).GetString(Encoding.GetEncoding(encoding));
        public static byte[] RsaDecrypt(string xmlPrivateKey, byte[] data)
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.FromXmlString(xmlPrivateKey);
                return rsa.Decrypt(data, RSAEncryptionPadding.OaepSHA1);
            }
        }


        public static byte[] RsaSignData(byte[] data, string xmlPrivateKey, HashAlgorithmName hashAlgorithmName)
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.FromXmlString(xmlPrivateKey);
                return rsa.SignData(data, hashAlgorithmName, RSASignaturePadding.Pkcs1);
            }
        }
        public static bool RsaVerifySign(byte[] data, string xmlPublicKey, byte[] signature, HashAlgorithmName hashAlgorithmName)
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.FromXmlString(xmlPublicKey);
                return rsa.VerifyData(data, signature, hashAlgorithmName, RSASignaturePadding.Pkcs1);
            }
        }

        public static object AesEncryptStringToHexString(string s, object value) => throw new NotImplementedException();
    }
}
