using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

namespace Silmoon
{
    public class AppHelper
    {
        public static string[] CommandLineArgs { get; private set; }
        static AppHelper()
        {
            CommandLineArgs = Environment.GetCommandLineArgs();
        }
        public static string GetAppRunPath()
        {
            // �洢�����в����ĵ�һ��Ԫ��
            var commandLineArg = CommandLineArgs[0];

            // ��������в����Ƿ�����ȫ�޶�·��
            bool isFullPath = commandLineArg.EndsWith(@"\") || commandLineArg.EndsWith("/");

            // �������ȫ�޶�·�����ͷ���Ӧ�ó������ڵ�Ŀ¼
            if (isFullPath)
            {
                // ��ȡ���һ��Ŀ¼�ָ�����λ��
                int lastDirectorySeparatorPos = Math.Max(commandLineArg.LastIndexOf(@"\"), commandLineArg.LastIndexOf("/"));

                // ��·���ַ����Ŀ�ͷ��ȡ�����һ��Ŀ¼�ָ�����λ��
                return commandLineArg.Substring(0, lastDirectorySeparatorPos);
            }

            // ���������ȫ�޶�·�����ͷ��ص�ǰ�Ĺ���Ŀ¼
            return Environment.CurrentDirectory;
        }
    }
}
