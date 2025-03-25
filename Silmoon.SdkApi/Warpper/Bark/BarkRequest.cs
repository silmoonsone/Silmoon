using MongoDB.Driver.Core.Misc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Silmoon.Extension.Converters;
using Silmoon.SdkApi.Warpper.Bark.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.SdkApi.Warpper.Bark
{
    public class BarkRequest
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("body")]
        public string Body { get; set; }
        [JsonProperty("level")]
        [JsonConverter(typeof(StringEnumCamelCaseNamingConverter))]
        public BarkPushLevel? Level { get; set; }
        [JsonProperty("badge")]
        public int? Badge { get; set; }
        [JsonProperty("autoCopy")]
        public string AutoCopy { get; set; }
        [JsonProperty("copy")]
        public string Copy { get; set; }
        [JsonProperty("sound")]
        public string Sound { get; set; }
        [JsonProperty("icon")]
        public string Icon { get; set; }
        [JsonProperty("group")]
        public string Group { get; set; }
        [JsonProperty("isArchive")]
        public int? IsArchive { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        private BarkRequest(string title, string body = null, BarkPushLevel level = BarkPushLevel.Active, int? badge = null, string autoCopy = null, string copy = null, string sound = "default", string icon = null, string group = null, int? isArchive = null, string url = null)
        {
            Title = title;
            Body = body;
            Level = level;
            Badge = badge;
            AutoCopy = autoCopy;
            Copy = copy;
            Sound = sound;
            Icon = icon;
            Group = group;
            IsArchive = isArchive;
            Url = url;
        }
        public static BarkRequest Create(string body = null, string title = null, BarkPushLevel level = BarkPushLevel.Active, int? badge = null, string autoCopy = null, string copy = null, string sound = "default", string icon = null, string group = null, bool? isArchive = null, string url = null)
        {
            int? isArchive2 = null;
            if (isArchive.HasValue)
            {
                isArchive2 = isArchive.Value ? 1 : 0;
            }
            return new BarkRequest(title, body, level, badge, autoCopy, copy, sound, icon, group, isArchive2, url);
        }
    }
}
