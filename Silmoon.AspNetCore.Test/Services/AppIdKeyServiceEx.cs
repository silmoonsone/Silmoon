using Silmoon.AspNetCore.Filters;
using Silmoon.AspNetCore.Services;
using Silmoon.Models;

namespace Silmoon.AspNetCore.Test.Services
{
    public class AppIdKeyServiceEx : AppIdKeyService
    {
        public override StateSet<bool, (string SignatureKey, string EncryptKey)> GetKey(string AppId)
        {
            return StateSet<bool, (string SignatureKey, string EncryptKey)>.Create(true, ("123", ""), "ok");
        }
    }
}
