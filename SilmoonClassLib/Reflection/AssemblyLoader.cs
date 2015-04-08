using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using System.Windows.Forms;

namespace Silmoon.Reflection
{
    /// <summary>
    /// 管理加载和卸载程序集
    /// </summary>
    public class AssemblyLoader
    {
        ProxyAssembly _object;
        AppDomain _domain;
        string _dllPath;
        bool _fileLoaded = false;
        bool _assemblyCreated = false;

        /// <summary>
        /// 获取程序集实例对象
        /// </summary>
        public object AssemblyObject
        {
            get { return _object.AssemblyObject; }
        }


        /// <summary>
        /// 获取文件是否已经被载入
        /// </summary>
        public bool FileLoaded
        {
            get { return _fileLoaded; }
            set { _fileLoaded = value; }
        }
        /// <summary>
        /// 获取程序集是否已经被实例化
        /// </summary>
        public bool AssemblyCreated
        {
            get { return _assemblyCreated; }
            set { _assemblyCreated = value; }
        }

        /// <summary>
        /// 实例化程序集管理
        /// </summary>
        /// <param name="assemblyPath">程序集DLL文件路径</param>
        /// <param name="newDomainName">新建立程序域名称</param>
        /// <param name="newDomain">新建立程序域设置</param>
        public AssemblyLoader(string assemblyPath, string newDomainName, AppDomainSetup newDomain)
        {
            _domain = AppDomain.CreateDomain(newDomainName, null, newDomain);
            _object = (ProxyAssembly)_domain.CreateInstanceFromAndUnwrap(Assembly.GetExecutingAssembly().CodeBase, "Silmoon.Reflection.ProxyAssembly");
            _dllPath = assemblyPath;
        }
        /// <summary>
        /// 装载程序集DLL文件
        /// </summary>
        /// <returns>文件是否加载成功，如果失败可能是已经加载！</returns>
        public bool Load()
        {
            if (_fileLoaded) return false;
            else
            {
                _object.LoadAssembly(_dllPath);
                _fileLoaded = true;
                return true;
            }
        }
        /// <summary>
        /// 装载程序集DLL文件并且实例化其类型
        /// </summary>
        /// <param name="typeName">类型名称</param>
        /// <returns>加载文件，如果成功加载实例化指定的类型，返回FALSE可能文件或者类型已经加载</returns>
        public bool Load(string typeName)
        {
            if (Load()) return CreateInstance(typeName);
            else return false;
        }
        /// <summary>
        /// 实例化目标类型
        /// </summary>
        /// <param name="typeName">类型名称</param>
        /// <returns>实例化类型，如果返回FALSE，可能是已经实例化或者没有加载文件！</returns>
        public bool CreateInstance(string typeName)
        {
            if (_assemblyCreated) return false;
            bool result = _object.CreateInstance(typeName);
            _assemblyCreated = true;
            return result;
        }
        /// <summary>
        /// 实例化类型，并且调用一个外部方法
        /// </summary>
        /// <param name="typeName">类型名称</param>
        /// <param name="methodName">方法名称</param>
        /// <param name="parameters">参数</param>
        /// <returns>返回调用所返回的对象</returns>
        public object CreateInstanceAndInvoke(string typeName, string methodName, params object[] parameters)
        {
            if (CreateInstance(typeName))
                return _object.Invoke(methodName, parameters);
            else return null;
        }
        /// <summary>
        /// 调用一个外部方法
        /// </summary>
        /// <param name="methodName">方法名称</param>
        /// <param name="parameters">参数</param>
        /// <returns>返回调用所返回的对象</returns>
        public object Invoke(string methodName, params object[] parameters)
        {
            return _object.Invoke(methodName, parameters);
        }
        /// <summary>
        /// 调用一个外部的静态方法
        /// </summary>
        /// <param name="typeName">类型名称</param>
        /// <param name="methodName">方法名称</param>
        /// <param name="parameters">参数</param>
        /// <returns>返回调用所返回的对象</returns>
        public object InvokeStatic(string typeName, string methodName, object[] parameters)
        {
            return _object.InvokeStatic(typeName, methodName, parameters);
        }
        /// <summary>
        /// 卸载当前的程序集和应用程序域
        /// </summary>
        public void Unload()
        {
            AppDomain.Unload(_domain);
        }
    }
}
