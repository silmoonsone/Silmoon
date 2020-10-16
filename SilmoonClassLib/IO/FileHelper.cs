using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silmoon.IO
{
    public class FileHelper
    {
        /// <summary>
        /// 创建目录，可深度创建多级目录
        /// </summary>
        /// <param name="DirectoryPath"></param>
        public static void CreateDirectory(string DirectoryPath)
        {
            string[] dirs = DirectoryPath.Split(Path.DirectorySeparatorChar);
            string dir = default;

            foreach (var item in dirs)
            {
                dir += item + Path.DirectorySeparatorChar;
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                    Console.WriteLine(dir);
                }
            }
        }
        public static void ClearEmptyDirectory(string DirectoryPath)
        {
            string[] dirs = DirectoryPath.Split(Path.DirectorySeparatorChar);
            string dir = default;

            foreach (var item in dirs)
            {
                dir += item + Path.DirectorySeparatorChar;
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                    Console.WriteLine(dir);
                }
            }
        }
    }
}