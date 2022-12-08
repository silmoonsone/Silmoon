using Microsoft.Extensions.Options;
using Silmoon.AspNetCore.Filters;
using Silmoon.AspNetCore.Services;
using Silmoon.AspNetCore.Services.Interfaces;
using Silmoon.Models;

namespace Silmoon.AspNetCore.Test.Services
{
    public class SilmoonDevAppServiceImpl : SilmoonDevAppService
    {
        public SilmoonDevAppServiceImpl(IOptions<SilmoonDevAppOptions> options) : base(options)
        {

        }

        public override async Task<StateSet<bool, (string SignatureKey, string EncryptKey)>> GetKey(string AppId)
        {
            return await Task.FromResult(StateSet<bool, (string SignatureKey, string EncryptKey)>.Create(true, ("123", ""), "ok"));
        }
    }
}
