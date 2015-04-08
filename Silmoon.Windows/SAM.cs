using System;
using System.Collections.Generic;
using System.Text;
using System.DirectoryServices;
using System.Collections;

namespace Silmoon.Windows
{
    /// <summary>
    /// 管理本地用户和组
    /// </summary>
    public class SAM
    {
        /// <summary>
        /// 实例化管理本地用户和组的一个实例
        /// </summary>
        public SAM()
        {

        }

        private DirectoryEntry getASMDirectoryEntryRoot()
        {
            return new DirectoryEntry("WinNT://" + Environment.MachineName + ",computer");
        }
        private DirectoryEntry getUserAndGroupDirectoryEntryRoot(string identity)
        {
            return getASMDirectoryEntryRoot().Children.Find(identity);
        }

        /// <summary>
        /// 创建一个用户
        /// </summary>
        /// <param name="info">用户账户信息结构</param>
        public void CreateUser(NTUserInfo info)
        {
            DirectoryEntry rootad = getASMDirectoryEntryRoot();
            DirectoryEntry NewUser = rootad.Children.Add(info.Username, "User");
            NewUser.Invoke("SetPassword", new object[] { info.Password });
            NewUser.Invoke("Put", "UserFlags", info.UserFlags);

            NewUser.Properties["Description"].Value = info.Description;
            NewUser.Properties["Fullname"].Value = info.Fullname;

            NewUser.CommitChanges();
            NewUser.Dispose();
            rootad.Dispose();
        }
        /// <summary>
        /// 创建一个用户组
        /// </summary>
        /// <param name="groupname">用户组名称</param>
        /// <param name="description">用户组描述</param>
        public void CreateGroup(string groupname, string description)
        {
            DirectoryEntry rootad = getASMDirectoryEntryRoot();
            DirectoryEntry NewUser = rootad.Children.Add(groupname, "Group");
            NewUser.Properties["Description"].Value = description;
            NewUser.CommitChanges();
            NewUser.Dispose();
            rootad.Dispose();
        }
        /// <summary>
        /// 删除一个用户或组
        /// </summary>
        /// <param name="identity">用户或组的名称</param>
        public void DeleteUserOrGroup(string identity)
        {
            DirectoryEntry rootad = getASMDirectoryEntryRoot();
            DirectoryEntry UAD = rootad.Children.Find(identity);
            if (UAD == null) throw new NullReferenceException("指定的用户不存在！");
            rootad.Children.Remove(UAD);
            rootad.Dispose();
        }

        /// <summary>
        /// 添加一个用户到组
        /// </summary>
        /// <param name="username">用户账户名</param>
        /// <param name="groupname">用户组名</param>
        public void AddUserToGroup(string username, string groupname)
        {
            if (GetIdentityType(username) == IdentityType.User)
            {
                DirectoryEntry identityRoot = getUserAndGroupDirectoryEntryRoot(groupname);
                identityRoot.Invoke("Add", getASMDirectoryEntryRoot().Children.Find(username).Path);
                identityRoot.Dispose();
            }
        }
        /// <summary>
        /// 从一个组中删除一个用户账户
        /// </summary>
        /// <param name="username">用户账户名</param>
        /// <param name="groupname">用户组名</param>
        public void RemoveUserFromGroup(string username, string groupname)
        {
            if (GetIdentityType(username) == IdentityType.User)
            {

                DirectoryEntry identityRoot = getUserAndGroupDirectoryEntryRoot(groupname);
                identityRoot.Invoke("Remove", getASMDirectoryEntryRoot().Children.Find(username).Path);
                identityRoot.Dispose();
            }
        }

        /// <summary>
        /// 检查一个用户或则组是否存在
        /// </summary>
        /// <param name="identity">用户帐号名或者组名称</param>
        /// <returns></returns>
        public bool ExistIdentity(string identity)
        {
            DirectoryEntry rootad = getASMDirectoryEntryRoot();
            foreach (DirectoryEntry ent in rootad.Children)
            {
                if (ent.SchemaClassName.ToLower() == "user" || ent.SchemaClassName.ToLower() == "group")
                {
                    if (ent.Name == identity)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取本地所有的用户账户
        /// </summary>
        public NTUserInfo[] GetUsernames
        {
            get
            {
                DirectoryEntry rootad = getASMDirectoryEntryRoot();
                ArrayList _arr = new ArrayList();
                foreach (DirectoryEntry ent in rootad.Children)
                {
                    if (ent.SchemaClassName.ToLower() == "user")
                    {
                        NTUserInfo info = new NTUserInfo();
                        info.Username = ent.Name;
                        info.Description = (string)ent.Properties["Description"].Value;
                        info.Fullname = (string)ent.Properties["Fullname"].Value;
                        _arr.Add(info);
                    }
                }
                rootad.Dispose();
                return (NTUserInfo[])_arr.ToArray(typeof(NTUserInfo));
            }
        }
        /// <summary>
        /// 获取本地所有组名称
        /// </summary>
        public string[] GetGroups
        {
            get
            {
                DirectoryEntry rootad = getASMDirectoryEntryRoot();
                ArrayList _arr = new ArrayList();
                foreach (DirectoryEntry ent in rootad.Children)
                {
                    if (ent.SchemaClassName.ToLower() == "group")
                    {
                        _arr.Add(ent.Name);
                    }
                }
                rootad.Dispose();
                return (string[])_arr.ToArray(typeof(string));
            }
        }

        /// <summary>
        /// 检查一个标识的类型
        /// </summary>
        /// <param name="identity">标识</param>
        /// <returns></returns>
        public IdentityType GetIdentityType(string identity)
        {
            DirectoryEntry ent = getASMDirectoryEntryRoot().Children.Find(identity);
            if (ent.SchemaClassName.ToLower() == "user")
                return IdentityType.User;
            else if (ent.SchemaClassName.ToLower() == "group")
                return IdentityType.Group;
            return IdentityType.Unknown;
        }
    }
    /// <summary>
    /// 系统账户信息结构
    /// </summary>
    public class NTUserInfo
    {
        public string Username;
        public string Password;
        public string Fullname = "";
        public string Description = "";
        public int UserFlags = 66049;
    }

    /// <summary>
    /// 标识类型
    /// </summary>
    public enum IdentityType
    {
        Unknown = 0,
        User = 1,
        Group = 2,
    }
}