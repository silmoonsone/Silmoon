using Newtonsoft.Json.Linq;
using Silmoon.Extension;
using Silmoon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silmoon.SdkApi.Warpper.Google
{
    public class reCaptcha
    {
        public static async Task<StateSet<bool, reCaptchaVerifyResponse>> reCaptchaVerify(string secret, string response)
        {
            var result = await JsonHelperV2.GetJsonAsync($"https://www.recaptcha.net/recaptcha/api/siteverify?secret={secret}&response={response}");
            if (result.State)
            {
                var Success = result.Data["success"].Value<string>().ToBool();
                if (Success)
                {
                    var data = new reCaptchaVerifyResponse() { Success = Success, Hostname = result.Data["hostname"].Value<string>(), Time = DateTime.Parse(result.Data["challenge_ts"].Value<string>()) };
                    return new StateSet<bool, reCaptchaVerifyResponse>(true, data);
                }
                else
                {
                    var data = new reCaptchaVerifyResponse() { Success = Success, ErrorCodes = result.Data["error-codes"].Value<JArray>().ToObjects<string>() };
                    return new StateSet<bool, reCaptchaVerifyResponse>(true, data);
                }
            }
            else
            {
                return new StateSet<bool, reCaptchaVerifyResponse>(false, null, result.Message);
            }
        }
    }

    public class reCaptchaVerifyResponse
    {
        public bool Success { get; set; }
        public DateTime Time { get; set; }
        public string Hostname { get; set; }
        public string[] ErrorCodes { get; set; }
    }
}
