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
                }
            }
        }
        public static void ClearEmptyDirectory(string DirectoryPath, string KeepPath)
        {
            KeepPath += Path.DirectorySeparatorChar;
            KeepPath = Path.GetDirectoryName(KeepPath);
            KeepPath += Path.DirectorySeparatorChar;

            string[] dirs = DirectoryPath.Split(Path.DirectorySeparatorChar);
            string dir = default;
            List<string> dirs2 = new List<string>();

            foreach (var item in dirs)
            {
                dir += item + Path.DirectorySeparatorChar;
                dirs2.Add(dir);
            }

            dirs2.Reverse();

            foreach (var item in dirs2)
            {
                if (item == KeepPath) break;
                if (Directory.Exists(item))
                {
                    var files = Directory.GetFiles(item);
                    dirs = Directory.GetDirectories(item);

                    if (files.Length == 0 && dirs.Length == 0)
                    {
                        Directory.Delete(item);
                    }
                }
            }
        }
    }
}