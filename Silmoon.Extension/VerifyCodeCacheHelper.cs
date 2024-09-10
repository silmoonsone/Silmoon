using Silmoon.Models;
using Silmoon.Runtime.Cache;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Extension
{
    public class VerifyCodeCacheHelper
    {
        public static string BackdoorCode { get; set; }
        /// <summary>
        /// 设置验证码缓存
        /// </summary>
        /// <param name="Identity"></param>
        /// <param name="VerifyCode"></param>
        /// <param name="ValidateExpireSecounds"></param>
        public static void SetVerifyCodeCache(string Identity, string VerifyCode, int ValidateExpireSecounds = 300)
        {
            GlobalCaching<string, string>.Set("__verifying_Identity_" + Identity, VerifyCode, TimeSpan.FromSeconds(ValidateExpireSecounds));
        }
        /// <summary>
        /// 获取验证码缓存
        /// </summary>
        /// <param name="Identity"></param>
        /// <returns></returns>
        public static StateSet<bool, string> GetCachedVerifyCode(string Identity)
        {
            var (Matched, Value) = GlobalCaching<string, string>.Get("__verifying_Identity_" + Identity);
            if (Matched)
                return StateSet<bool, string>.Create(true, Value);
            else
                return StateSet<bool, string>.Create(false, null);
        }
        /// <summary>
        /// 验证验证码
        /// </summary>
        /// <param name="Identity"></param>
        /// <param name="VerifyCode"></param>
        /// <returns></returns>
        public static bool ValidateVerifyCode(string Identity, string VerifyCode)
        {
            var result = GetCachedVerifyCode(Identity);
            if (!BackdoorCode.IsNullOrEmpty() && BackdoorCode.Equals(VerifyCode, StringComparison.OrdinalIgnoreCase)) return true;
            return result.State && result.Data.Equals(VerifyCode, StringComparison.OrdinalIgnoreCase);
        }
        /// <summary>
        /// 清空验证码缓存
        /// </summary>
        /// <param name="Identity"></param>
        /// <returns></returns>
        public static bool ClearVerifyCodeCache(string Identity)
        {
            return GlobalCaching<string, string>.Remove("__verifying_Identity_" + Identity);
        }

        /// <summary>
        /// 使用缓存机制进行验证码验证，验证后保留验证状态。
        /// </summary>
        /// <param name="Identity"></param>
        /// <param name="VerifyCode"></param>
        /// <param name="IfErrorClear"></param>
        /// <param name="cacheFinishStatusSecounds"></param>
        /// <returns></returns>
        public static bool FinishValidation(string Identity, string VerifyCode, bool IfErrorClear = true, int cacheFinishStatusSecounds = 0)
        {
            var result = ValidateVerifyCode(Identity, VerifyCode);
            if (result)
            {
                if (cacheFinishStatusSecounds > 0)
                    GlobalCaching<string, bool>.Set("__validated_Identity_" + Identity, true, TimeSpan.FromSeconds(cacheFinishStatusSecounds));
                ClearVerifyCodeCache(Identity);
            }
            else if (IfErrorClear)
                ClearVerifyCodeCache(Identity);
            return result;
        }
        /// <summary>
        /// 当前验证状态缓存是否已经验证通过
        /// </summary>
        /// <param name="Identity"></param>
        /// <returns></returns>
        public static bool IsValidated(string Identity)
        {
            var (Matched, Value) = GlobalCaching<string, bool>.Get("__validated_Identity_" + Identity);
            return Matched && Value;
        }
        /// <summary>
        /// 清空验证状态缓存
        /// </summary>
        /// <param name="Identity"></param>
        /// <returns></returns>
        public static bool ClearValidationStatus(string Identity)
        {
            return GlobalCaching<string, bool>.Remove("__validated_Identity_" + Identity);
        }
    }
}
