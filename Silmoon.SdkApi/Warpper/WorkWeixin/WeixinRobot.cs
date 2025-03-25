using Newtonsoft.Json.Linq;
using Silmoon.Extension;
using Silmoon.Extension.Http;
using Silmoon.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Silmoon.SdkApi.Warpper.WorkWeixin
{
    public class WeixinRobot
    {
        public static string WebHookUrl { get; } = "https://qyapi.weixin.qq.com/cgi-bin/webhook/send?key=";
        public string WebHookKey { get; set; }

        public async Task<StateSet<bool>> SendText(string content)
        {
            if (string.IsNullOrEmpty(WebHookKey)) throw new ArgumentNullException("WebHookKey");
            var url = $"{WebHookUrl}{WebHookKey}";
            var data = new
            {
                msgtype = "text",
                text = new
                {
                    content = content
                }
            };
            var result = await JsonRequest.PostAsync<JObject, object>(url, data, null, new JsonRequestSetting());

            if (result.IsSuccess && result.IsSuccessStatusCode)
                return StateSet<bool>.Create(result.Result["errcode"].Value<int>() == 0, result.Result["errmsg"].Value<string>());
            else
                return StateSet<bool>.Create(false, result.Exception?.Message ?? result.Response);
        }
        public async Task<StateSet<bool>> SendMarkdown(string markdown)
        {
            if (string.IsNullOrEmpty(WebHookKey)) throw new ArgumentNullException("WebHookKey");
            var url = $"{WebHookUrl}{WebHookKey}";
            var data = new
            {
                msgtype = "markdown",
                markdown = new
                {
                    content = markdown
                }
            };
            var result = await JsonRequest.PostAsync<JObject, object>(url, data, null, new JsonRequestSetting());

            if (result.IsSuccess && result.IsSuccessStatusCode)
                return StateSet<bool>.Create(result.Result["errcode"].Value<int>() == 0, result.Result["errmsg"].Value<string>());
            else
                return StateSet<bool>.Create(false, result.Exception?.Message ?? result.Response);
        }

        public static async Task<StateSet<bool>> SendText(string webHookKey, string content)
        {
            if (string.IsNullOrEmpty(webHookKey)) throw new ArgumentNullException("WebHookKey");
            var url = $"{WebHookUrl}{webHookKey}";
            var data = new
            {
                msgtype = "text",
                text = new
                {
                    content = content
                }
            };
            var result = await JsonRequest.PostAsync<JObject, object>(url, data, null, new JsonRequestSetting());

            if (result.IsSuccess && result.IsSuccessStatusCode)
                return StateSet<bool>.Create(result.Result["errcode"].Value<int>() == 0, result.Result["errmsg"].Value<string>());
            else
                return StateSet<bool>.Create(false, result.Exception?.Message ?? result.Response);
        }
        public static async Task<StateSet<bool>> SendMarkdown(string webHookKey, string markdown)
        {
            if (string.IsNullOrEmpty(webHookKey)) throw new ArgumentNullException("WebHookKey");
            var url = $"{WebHookUrl}{webHookKey}";
            var data = new
            {
                msgtype = "markdown",
                markdown = new
                {
                    content = markdown
                }
            };
            var result = await JsonRequest.PostAsync<JObject, object>(url, data, null, new JsonRequestSetting());
            if (result.IsSuccess && result.IsSuccessStatusCode)
                return StateSet<bool>.Create(result.Result["errcode"].Value<int>() == 0, result.Result["errmsg"].Value<string>());
            else
                return StateSet<bool>.Create(false, result.Exception?.Message ?? result.Response);
        }
    }
}
