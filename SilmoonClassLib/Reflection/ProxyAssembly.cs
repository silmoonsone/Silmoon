using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.IO;

namespace Silmoon.Reflection
{
    public class ProxyAssembly : MarshalByRefObject
    {
        Assembly _assembly;
        Type _Type;
        object _object;


        bool _fileLoaded = false;
        bool _createType = false;
        bool _newType = false;

        public object AssemblyObject
        {
            get { return _object; }
        }

        public void LoadAssembly(string assemblyPath)
        {
            _assembly = Assembly.LoadFile(assemblyPath);
            _fileLoaded = true;
        }
        public bool CreateInstance(string typeName)
        {
            if (_fileLoaded)
            {
                if (!_createType)
                {
                    _Type = _assembly.GetType(typeName);
                    _createType = true;
                }
                if (!_newType)
                {
                    _object = Activator.CreateInstance(_Type);
                    _newType = true;
                }
                return true;
            }
            else return false;
        }
        public object Invoke(string methodName, object[] parameters)
        {
            if (!_newType) return null;
            if (!_createType) return null;

            MethodInfo method = _Type.GetMethod(methodName);
            if (method == null) return null;

            return method.Invoke(_object, parameters);
        }
        public object InvokeStatic(string typeName, string methodName, object[] parameters)
        {
            MethodInfo method = _assembly.GetType(typeName).GetMethod(methodName);
            if (method == null) return null;

            return method.Invoke(null, parameters);
        }
    }
}
