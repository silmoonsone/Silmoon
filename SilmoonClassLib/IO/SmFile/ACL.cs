using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.AccessControl;

namespace Silmoon.IO.SmFile
{
    /// <summary>
    /// 控制访问控制表
    /// </summary>
    public sealed class ACL
    {
        /// <summary>
        /// 删除所有的系统访问权限
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public static void RemoveAllSystemAccessRule(string filePath)
        {
            if (File.Exists(filePath))
            {
                FileSecurity _fs = File.GetAccessControl(filePath);
                try
                {
                    _fs.RemoveAccessRuleAll(new FileSystemAccessRule("SYSTEM", FileSystemRights.FullControl, AccessControlType.Allow));
                    _fs.RemoveAccessRuleAll(new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, AccessControlType.Allow));
                    _fs.RemoveAccessRuleAll(new FileSystemAccessRule("Administrators", FileSystemRights.FullControl, AccessControlType.Allow));
                    _fs.RemoveAccessRuleAll(new FileSystemAccessRule("NETWORK", FileSystemRights.FullControl, AccessControlType.Allow));
                    _fs.RemoveAccessRuleAll(new FileSystemAccessRule("NETWORK SERVICE", FileSystemRights.FullControl, AccessControlType.Allow));
                    _fs.RemoveAccessRuleAll(new FileSystemAccessRule("LOCAL SERVICE", FileSystemRights.FullControl, AccessControlType.Allow));
                    _fs.RemoveAccessRuleAll(new FileSystemAccessRule("CREATOR OWNER", FileSystemRights.FullControl, AccessControlType.Allow));
                    _fs.RemoveAccessRuleAll(new FileSystemAccessRule("Users", FileSystemRights.FullControl, AccessControlType.Allow));
                }
                catch { }
                try { _fs.RemoveAccessRuleAll(new FileSystemAccessRule("Power Users", FileSystemRights.FullControl, AccessControlType.Allow)); }
                catch { }
                try { _fs.RemoveAccessRuleAll(new FileSystemAccessRule("IIS_WPG", FileSystemRights.FullControl, AccessControlType.Allow)); }
                catch { }
                try { _fs.RemoveAccessRuleAll(new FileSystemAccessRule("Guests", FileSystemRights.FullControl, AccessControlType.Allow)); }
                catch { }
                File.SetAccessControl(filePath, _fs);
            }
            else if (Directory.Exists(filePath))
            {
                DirectorySecurity _fs = Directory.GetAccessControl(filePath);
                try
                {
                    _fs.RemoveAccessRuleAll(new FileSystemAccessRule("SYSTEM", FileSystemRights.FullControl, AccessControlType.Allow));
                    _fs.RemoveAccessRuleAll(new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, AccessControlType.Allow));
                    _fs.RemoveAccessRuleAll(new FileSystemAccessRule("Administrators", FileSystemRights.FullControl, AccessControlType.Allow));
                    _fs.RemoveAccessRuleAll(new FileSystemAccessRule("NETWORK", FileSystemRights.FullControl, AccessControlType.Allow));
                    _fs.RemoveAccessRuleAll(new FileSystemAccessRule("NETWORK SERVICE", FileSystemRights.FullControl, AccessControlType.Allow));
                    _fs.RemoveAccessRuleAll(new FileSystemAccessRule("LOCAL SERVICE", FileSystemRights.FullControl, AccessControlType.Allow));
                    _fs.RemoveAccessRuleAll(new FileSystemAccessRule("CREATOR OWNER", FileSystemRights.FullControl, AccessControlType.Allow));
                    _fs.RemoveAccessRuleAll(new FileSystemAccessRule("Users", FileSystemRights.FullControl, AccessControlType.Allow));
                }
                catch { }
                try { _fs.RemoveAccessRuleAll(new FileSystemAccessRule("Power Users", FileSystemRights.FullControl, AccessControlType.Allow)); }
                catch { }
                try { _fs.RemoveAccessRuleAll(new FileSystemAccessRule("IIS_WPG", FileSystemRights.FullControl, AccessControlType.Allow)); }
                catch { }
                try { _fs.RemoveAccessRuleAll(new FileSystemAccessRule("Guests", FileSystemRights.FullControl, AccessControlType.Allow)); }
                catch { }
                Directory.SetAccessControl(filePath, _fs);
            }
            else
                throw new FileNotFoundException("要操作的文件没有找到", filePath);
        }
        public static FileSecurity RemoveAllSystemAccessRule(FileSecurity fs)
        {
            try
            {
                fs.RemoveAccessRuleAll(new FileSystemAccessRule("SYSTEM", FileSystemRights.FullControl, AccessControlType.Allow));
                fs.RemoveAccessRuleAll(new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, AccessControlType.Allow));
                fs.RemoveAccessRuleAll(new FileSystemAccessRule("Administrators", FileSystemRights.FullControl, AccessControlType.Allow));
                fs.RemoveAccessRuleAll(new FileSystemAccessRule("NETWORK", FileSystemRights.FullControl, AccessControlType.Allow));
                fs.RemoveAccessRuleAll(new FileSystemAccessRule("NETWORK SERVICE", FileSystemRights.FullControl, AccessControlType.Allow));
                fs.RemoveAccessRuleAll(new FileSystemAccessRule("LOCAL SERVICE", FileSystemRights.FullControl, AccessControlType.Allow));
                fs.RemoveAccessRuleAll(new FileSystemAccessRule("CREATOR OWNER", FileSystemRights.FullControl, AccessControlType.Allow));
                fs.RemoveAccessRuleAll(new FileSystemAccessRule("Users", FileSystemRights.FullControl, AccessControlType.Allow));
            }
            catch { }
            try { fs.RemoveAccessRuleAll(new FileSystemAccessRule("Power Users", FileSystemRights.FullControl, AccessControlType.Allow)); }
            catch { }
            try { fs.RemoveAccessRuleAll(new FileSystemAccessRule("IIS_WPG", FileSystemRights.FullControl, AccessControlType.Allow)); }
            catch { }
            try { fs.RemoveAccessRuleAll(new FileSystemAccessRule("Guests", FileSystemRights.FullControl, AccessControlType.Allow)); }
            catch { }
            return fs;
        }
        public static DirectorySecurity RemoveAllSystemAccessRule(DirectorySecurity ds)
        {
            try
            {
                ds.RemoveAccessRuleAll(new FileSystemAccessRule("SYSTEM", FileSystemRights.FullControl, AccessControlType.Allow));
                ds.RemoveAccessRuleAll(new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, AccessControlType.Allow));
                ds.RemoveAccessRuleAll(new FileSystemAccessRule("Administrators", FileSystemRights.FullControl, AccessControlType.Allow));
                ds.RemoveAccessRuleAll(new FileSystemAccessRule("Administrator", FileSystemRights.FullControl, AccessControlType.Allow));
                ds.RemoveAccessRuleAll(new FileSystemAccessRule("NETWORK", FileSystemRights.FullControl, AccessControlType.Allow));
                ds.RemoveAccessRuleAll(new FileSystemAccessRule("NETWORK SERVICE", FileSystemRights.FullControl, AccessControlType.Allow));
                ds.RemoveAccessRuleAll(new FileSystemAccessRule("LOCAL SERVICE", FileSystemRights.FullControl, AccessControlType.Allow));
                ds.RemoveAccessRuleAll(new FileSystemAccessRule("CREATOR OWNER", FileSystemRights.FullControl, AccessControlType.Allow));
                ds.RemoveAccessRuleAll(new FileSystemAccessRule("Users", FileSystemRights.FullControl, AccessControlType.Allow));
            }
            catch { }
            try { ds.RemoveAccessRuleAll(new FileSystemAccessRule("Power Users", FileSystemRights.FullControl, AccessControlType.Allow)); }
            catch { }
            try { ds.RemoveAccessRuleAll(new FileSystemAccessRule("IIS_WPG", FileSystemRights.FullControl, AccessControlType.Allow)); }
            catch { }
            try { ds.RemoveAccessRuleAll(new FileSystemAccessRule("Guests", FileSystemRights.FullControl, AccessControlType.Allow)); }
            catch { }
            return ds;
        }

        /// <summary>
        /// 设置文件继承权限保护
        /// </summary>
        /// <param name="filePath">目标文件或者目录</param>
        /// <param name="isProtected">是否受保护的</param>
        /// <param name="preserveInheritance">是否保留设置</param>
        public static void SetProtectionRule(string filePath,bool isProtected, bool preserveInheritance)
        {
            if (File.Exists(filePath))
            {
                FileSecurity fs = File.GetAccessControl(filePath);
                fs.SetAccessRuleProtection(isProtected, preserveInheritance);
                File.SetAccessControl(filePath, fs);
            }
            else if (Directory.Exists(filePath))
            {
                DirectorySecurity ds = Directory.GetAccessControl(filePath);
                ds.SetAccessRuleProtection(isProtected, preserveInheritance);
                Directory.SetAccessControl(filePath, ds);
            }
            else
                throw new FileNotFoundException("要操作的文件没有找到", filePath);
        }

        /// <summary>
        /// 添加标致到路径完全控制
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="identity">标致</param>
        public static void AddAccessRule(string filePath, string identity)
        {
            if (File.Exists(filePath))
            {
                FileSecurity _fs = File.GetAccessControl(filePath);
                _fs.AddAccessRule(new FileSystemAccessRule(identity, FileSystemRights.FullControl, AccessControlType.Allow));
                File.SetAccessControl(filePath, _fs);
            }
            else if (Directory.Exists(filePath))
            {
                DirectorySecurity _fs = Directory.GetAccessControl(filePath);
                _fs.AddAccessRule(new FileSystemAccessRule(identity, FileSystemRights.FullControl, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                _fs.AddAccessRule(new FileSystemAccessRule(identity, FileSystemRights.FullControl, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                Directory.SetAccessControl(filePath, _fs);
            }
            else throw new FileNotFoundException("要操作的文件没有找到", filePath);
        }
        /// <summary>
        /// 添加一个用户访问权限到目录
        /// </summary>
        /// <param name="ds">目录安全对象</param>
        /// <param name="identity">用户标识</param>
        /// <param name="rights">权限</param>
        /// <param name="Inhert">是否应用到子对象</param>
        /// <returns></returns>
        public static DirectorySecurity AddAccessRule(DirectorySecurity ds, string identity, FileSystemRights rights, bool Inhert)
        {
            ds.AddAccessRule(new FileSystemAccessRule(identity, rights, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
            if (Inhert) ds.AddAccessRule(new FileSystemAccessRule(identity, rights, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
            return ds;
        }

        /// <summary>
        /// 删除指定用户的ACL
        /// </summary>
        /// <param name="identity">Windows帐户</param>
        /// <param name="filePath">文件路径</param>
        public static void RemoveAccessRule(string filePath, string identity)
        {
            if (File.Exists(filePath))
            {
                FileSecurity _fs = File.GetAccessControl(filePath);
                _fs.RemoveAccessRuleAll(new FileSystemAccessRule(identity, FileSystemRights.FullControl, AccessControlType.Allow));
                File.SetAccessControl(filePath, _fs);
            }
            else if (Directory.Exists(filePath))
            {
                DirectorySecurity _fs = Directory.GetAccessControl(filePath);
                _fs.RemoveAccessRuleAll(new FileSystemAccessRule(identity, FileSystemRights.FullControl, AccessControlType.Allow));
                Directory.SetAccessControl(filePath, _fs);
            }
            else throw new FileNotFoundException("要操作的文件没有找到", filePath);
        }
        /// <summary>
        /// 删除指定标致的目录安全
        /// </summary>
        /// <param name="ds">目录安全实例</param>
        /// <param name="identity">标致</param>
        /// <returns></returns>
        public static DirectorySecurity RemoveAccessRule(DirectorySecurity ds, string identity)
        {
            ds.RemoveAccessRuleAll(new FileSystemAccessRule(identity, FileSystemRights.FullControl, AccessControlType.Allow));
            return ds;
        }
    }
}