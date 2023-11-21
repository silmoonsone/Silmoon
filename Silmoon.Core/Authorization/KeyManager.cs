using Silmoon.Extension;
using Silmoon.Core.Models;
using Silmoon.Models;
using Silmoon.Secure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Silmoon.Core.Authorization
{
    public class KeyManager
    {
        public static string GenerateEncryptedKeyString(string password)
        {
            RSA rsa = RSA.Create();

            var keyFile = new KeyFile()
            {
                HashId = HashHelper.RandomChars(64),
                PublicKey = rsa.ExportRSAPublicKeyPem(),
                PrivateKey = rsa.ExportRSAPrivateKeyPem(),
                Name = Environment.MachineName,
            };

            var fileContent = JsonSerializer.Serialize(keyFile, JsonContext.Default.KeyFile);
            fileContent = EncryptHelper.AesEncryptStringV2(fileContent, password.PadRight(32), false);
            return fileContent;
        }
        public static StateSet<bool, KeyFile> DecodeEncryptedKeyString(string encryptedKeyString, string password)
        {
            if (encryptedKeyString.IsNullOrEmpty()) return StateSet<bool, KeyFile>.Create(false, null, "Invalid key file.");

            encryptedKeyString = EncryptHelper.AesDecryptStringV2(encryptedKeyString, password.PadRight(32), false);
            if (encryptedKeyString.IsNullOrEmpty()) return StateSet<bool, KeyFile>.Create(false, null, "Password error or invalid key string.");

            var key = JsonSerializer.Deserialize<KeyFile>(encryptedKeyString, JsonContext.Default.KeyFile);
            return StateSet<bool, KeyFile>.Create(true, key, "Success");
        }
    }
}
