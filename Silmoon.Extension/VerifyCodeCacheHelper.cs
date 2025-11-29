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
        /// <param name="maxAttempts">最大尝试次数，默认为1（验证失败后立即失效）</param>
        public static void SetVerifyCodeCache(string Identity, string VerifyCode, int ValidateExpireSecounds = 300, int maxAttempts = 1)
        {
            if (string.IsNullOrEmpty(Identity) || string.IsNullOrEmpty(VerifyCode)) return;
            var expireTime = TimeSpan.FromSeconds(ValidateExpireSecounds);
            GlobalCaching<string, string>.Set("__verifying_Identity_" + Identity, VerifyCode, expireTime);
            GlobalCaching<string, int>.Set("__verify_attempts_Identity_" + Identity, maxAttempts, expireTime);
        }
        /// <summary>
        /// 获取验证码缓存
        /// </summary>
        /// <param name="Identity"></param>
        /// <returns></returns>
        public static StateSet<bool, string> GetCachedVerifyCode(string Identity)
        {
            if (string.IsNullOrEmpty(Identity)) return StateSet<bool, string>.Create(false, null);
            var (Matched, Value) = GlobalCaching<string, string>.Get("__verifying_Identity_" + Identity);
            if (Matched)
                return StateSet<bool, string>.Create(true, Value);
            else
                return StateSet<bool, string>.Create(false, null);
        }
        /// <summary>
        /// 获取剩余尝试次数
        /// </summary>
        /// <param name="Identity"></param>
        /// <returns></returns>
        public static int GetRemainingAttempts(string Identity)
        {
            if (string.IsNullOrEmpty(Identity)) return 0;
            var (Matched, Attempts) = GlobalCaching<string, int>.Get("__verify_attempts_Identity_" + Identity);
            return Matched ? Attempts : 0;
        }
        /// <summary>
        /// 验证验证码（无状态验证，用于注册等场景，包含尝试次数失效机制）
        /// </summary>
        /// <param name="Identity"></param>
        /// <param name="VerifyCode"></param>
        /// <returns></returns>
        public static bool ValidateVerifyCode(string Identity, string VerifyCode)
        {
            if (string.IsNullOrEmpty(Identity) || string.IsNullOrEmpty(VerifyCode)) return false;

            // 检查后门代码
            if (!BackdoorCode.IsNullOrEmpty() && BackdoorCode.Equals(VerifyCode, StringComparison.OrdinalIgnoreCase))
            {
                // 后门代码验证成功，清除验证码
                ClearVerifyCodeCache(Identity);
                return true;
            }

            // 获取验证码并验证
            var result = GetCachedVerifyCode(Identity);

            // 如果验证码不存在，直接返回 false（验证码和尝试次数应该同时过期）
            if (!result.State) return false;

            var isValid = result.Data?.Equals(VerifyCode, StringComparison.OrdinalIgnoreCase) ?? false;

            if (isValid)
            {
                // 验证成功，清除验证码（验证码只能使用一次）
                ClearVerifyCodeCache(Identity);
            }
            else
            {
                // 验证失败时，减少尝试次数
                var (AttemptsMatched, AttemptsItem) = GlobalCaching<string, int>.GetInfo("__verify_attempts_Identity_" + Identity);
                if (AttemptsMatched && AttemptsItem != null && AttemptsItem.Value > 0)
                {
                    var newAttempts = AttemptsItem.Value - 1;
                    if (newAttempts <= 0)
                    {
                        // 尝试次数用尽，清除验证码
                        ClearVerifyCodeCache(Identity);
                    }
                    else
                    {
                        // 更新尝试次数，保持原有的过期时间
                        GlobalCaching<string, int>.Set("__verify_attempts_Identity_" + Identity, newAttempts, AttemptsItem.ExpireAt);
                    }
                }
                else
                {
                    // 没有尝试次数记录，直接清除（兼容旧逻辑）
                    ClearVerifyCodeCache(Identity);
                }
            }

            return isValid;
        }
        /// <summary>
        /// 清空验证码缓存
        /// </summary>
        /// <param name="Identity"></param>
        /// <returns></returns>
        public static bool ClearVerifyCodeCache(string Identity)
        {
            if (string.IsNullOrEmpty(Identity)) return false;
            GlobalCaching<string, int>.Remove("__verify_attempts_Identity_" + Identity);
            return GlobalCaching<string, string>.Remove("__verifying_Identity_" + Identity);
        }

        /// <summary>
        /// 完成验证码验证并设置验证状态（有状态验证，用于登录后提权等场景）
        /// </summary>
        /// <param name="Identity"></param>
        /// <param name="VerifyCode"></param>
        /// <param name="cacheFinishStatusSecounds">验证状态缓存时间（秒），0表示不缓存状态</param>
        /// <returns></returns>
        public static bool FinishValidation(string Identity, string VerifyCode, int cacheFinishStatusSecounds = 0)
        {
            // 调用 ValidateVerifyCode 进行验证（包含尝试次数失效机制，验证成功时会自动清除验证码）
            var result = ValidateVerifyCode(Identity, VerifyCode);

            if (result)
            {
                // 验证成功，设置验证状态
                if (cacheFinishStatusSecounds > 0)
                    GlobalCaching<string, bool>.Set("__validated_Identity_" + Identity, true, TimeSpan.FromSeconds(cacheFinishStatusSecounds));
                // 注意：验证码已在 ValidateVerifyCode 中清除，无需重复清除
            }
            // 验证失败时，尝试次数失效机制已在 ValidateVerifyCode 中处理，无需重复处理

            return result;
        }
        /// <summary>
        /// 当前验证状态缓存是否已经验证通过
        /// </summary>
        /// <param name="Identity"></param>
        /// <returns></returns>
        public static bool IsValidated(string Identity)
        {
            if (string.IsNullOrEmpty(Identity)) return false;
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
            if (string.IsNullOrEmpty(Identity)) return false;
            return GlobalCaching<string, bool>.Remove("__validated_Identity_" + Identity);
        }
    }
}
