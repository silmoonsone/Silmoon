using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

namespace Silmoon
{
    public class AppHelper
    {
        public static string GetAppRunPath()
        {
            // 存储命令行参数的第一个元素
            var commandLineArg = Environment.GetCommandLineArgs()[0];

            // 检查命令行参数是否是完全限定路径
            bool isFullPath = commandLineArg.EndsWith(@"\") || commandLineArg.EndsWith("/");

            // 如果是完全限定路径，就返回应用程序所在的目录
            if (isFullPath)
            {
                // 获取最后一个目录分隔符的位置
                int lastDirectorySeparatorPos = Math.Max(commandLineArg.LastIndexOf(@"\"), commandLineArg.LastIndexOf("/"));

                // 从路径字符串的开头截取到最后一个目录分隔符的位置
                return commandLineArg.Substring(0, lastDirectorySeparatorPos);
            }

            // 如果不是完全限定路径，就返回当前的工作目录
            return Environment.CurrentDirectory;
        }
    }
}
