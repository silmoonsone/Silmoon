using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Silmoon.Core.Authorization;
using Silmoon.Extension;
using Silmoon.Models;
using Silmoon.Secure;

namespace Silmoon.Core
{
    public static class CoreEncryptStringExtension
    {
        public static string DefaultKeyFilePath
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return "C:\\_smkey.raw";
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    return "/var/_smkey.raw";
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    return "/usr/local/_smkey.raw";
                }
                else
                {
                    throw new PlatformNotSupportedException("Unsupported operating system.");
                }
            }
        }
        public static string TryKeyFileDecrypt(this string encryptedBase64String, string keyPassword, string cipherPasswordEncrypted, string keyFilePath = null)
        {
            keyFilePath ??= DefaultKeyFilePath;

            if (File.Exists(keyFilePath))
            {
                var keyFileContent = File.ReadAllText(keyFilePath);
                var result = KeyManager.DecodeEncryptedKeyString(keyFileContent, keyPassword);
                if (result.State)
                {
                    using var rsa = RSA.Create();
                    rsa.ImportFromPem(result.Data.PrivateKey);
                    var password = rsa.Decrypt(Convert.FromBase64String(cipherPasswordEncrypted), RSAEncryptionPadding.OaepSHA256);
                    var clearData = EncryptHelper.AesDecryptV2(Convert.FromBase64String(encryptedBase64String), password.GetString());
                    return clearData.GetString(Encoding.UTF8);
                }
                else
                    throw new Exception(result.Message);
            }
            else
                throw new FileNotFoundException("Key file not found.", DefaultKeyFilePath);
        }
        public static string TryKeyFileDecryptSmkmUri(this string encryptedUriString, string keyFilePath = null)
        {
            keyFilePath ??= DefaultKeyFilePath;

            var uri = new Uri(encryptedUriString);
            if (uri.Scheme == "smkm" && uri.Host == "keyfile-rsa-password-encrypted.string.silmoon")
            {
                var keyPassword = HttpUtility.ParseQueryString(uri.Query)["KeyPassword"];
                var cipherPasswordEncrypted = HttpUtility.ParseQueryString(uri.Query)["CipherPasswordEncrypted"];
                var cipherData = HttpUtility.ParseQueryString(uri.Query)["CipherData"];
                return cipherData.TryKeyFileDecrypt(keyPassword, cipherPasswordEncrypted, keyFilePath);
            }
            else return encryptedUriString;
        }
        public static CipherPair KeyFileEncrypt(this string value, string keyPassword, string keyFilePath = null)
        {
            keyFilePath ??= DefaultKeyFilePath;

            if (File.Exists(keyFilePath))
            {
                var keyFileContent = File.ReadAllText(keyFilePath);
                var result = KeyManager.DecodeEncryptedKeyString(keyFileContent, keyPassword);
                if (result.State)
                {
                    using var rsa = RSA.Create();
                    rsa.ImportFromPem(result.Data.PrivateKey);
                    var password = HashHelper.RandomChars(32);
                    var cipherData = EncryptHelper.AesEncryptStringV2(value, password, false);
                    var cipherPassword = rsa.Encrypt(password.GetBytes(), RSAEncryptionPadding.OaepSHA256);
                    return CipherPair.Create(cipherData, cipherPassword.GetBase64String());
                }
                else
                    throw new Exception(result.Message);
            }
            else
                throw new FileNotFoundException("Key file not found.", DefaultKeyFilePath);
        }
        public static string KeyFileEncryptToSmkmUri(this string value, string keyPassword, string keyFilePath = null)
        {
            keyFilePath ??= DefaultKeyFilePath;

            var cipher = value.KeyFileEncrypt(keyPassword, keyFilePath);
            var uri = new Uri($"smkm://keyfile-rsa-password-encrypted.string.silmoon/?KeyPassword={HttpUtility.UrlEncode(keyPassword)}&CipherPasswordEncrypted={HttpUtility.UrlEncode(cipher.Password)}&CipherData=" + HttpUtility.UrlEncode(cipher.CipherData));
            return uri.ToString();
        }
    }
}
