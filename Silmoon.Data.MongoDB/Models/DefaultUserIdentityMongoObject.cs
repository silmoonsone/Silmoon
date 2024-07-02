using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Silmoon.Extension.Models.Identities;
using Silmoon.Extension.Models.Identities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Data.MongoDB.Models
{
    /// <summary>
    /// 表示一个基本用户，为MongoDB单独设立一个类型，原因是有部分属性需要指定MongoDB属性标志。
    /// </summary>
    [Serializable]
    public class DefaultUserIdentityMongoObject : IdObject, IDefaultUserIdentity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Nickname { get; set; }
        public IdentityRole Role { get; set; }
        public IdentityStatus Status { get; set; }
    }
}
