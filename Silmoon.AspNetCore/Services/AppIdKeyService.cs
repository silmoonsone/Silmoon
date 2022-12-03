using Silmoon.Models;

namespace Silmoon.AspNetCore.Services
{
    /// <summary>
    /// 为应用程序提供AppId和Key
    /// </summary>
    public abstract class AppIdKeyService
    {
        public abstract StateSet<bool, (string SignatureKey, string EncryptKey)> GetKey(string AppId);
    }
}
