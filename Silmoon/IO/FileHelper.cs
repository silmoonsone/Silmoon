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
        /// 创建指定路径及其所有父级目录。
        /// </summary>
        /// <param name="directoryPath">需要创建目录的完整路径。</param>
        public static void CreateDirectory(string directoryPath)
        {
            string[] directoryParts = directoryPath.Split(Path.DirectorySeparatorChar);
            string accumulatedPath = string.Empty;

            foreach (var part in directoryParts)
            {
                accumulatedPath += part + Path.DirectorySeparatorChar;
                if (!Directory.Exists(accumulatedPath))
                {
                    Directory.CreateDirectory(accumulatedPath);
                }
            }
        }

        /// <summary>
        /// 清理指定目录下的所有空目录，直到达到指定的保留目录。
        /// </summary>
        /// <param name="targetDirectoryPath">目标目录路径，该目录及其子目录将被检查并清理空目录。</param>
        /// <param name="reserveDirectoryPath">保留目录路径，该路径及其父路径不会被清理。</param>
        public static void ClearEmptyDirectory(string targetDirectoryPath, string reserveDirectoryPath)
        {
            reserveDirectoryPath += Path.DirectorySeparatorChar;
            reserveDirectoryPath = Path.GetDirectoryName(reserveDirectoryPath) + Path.DirectorySeparatorChar;

            string[] pathParts = targetDirectoryPath.Split(Path.DirectorySeparatorChar);
            string cumulativePath = string.Empty;
            List<string> directoriesToEvaluate = new List<string>();

            foreach (var part in pathParts)
            {
                cumulativePath += part + Path.DirectorySeparatorChar;
                directoriesToEvaluate.Add(cumulativePath);
            }

            directoriesToEvaluate.Reverse();

            foreach (var directory in directoriesToEvaluate)
            {
                if (directory == reserveDirectoryPath) break;
                if (Directory.Exists(directory))
                {
                    var files = Directory.GetFiles(directory);
                    var subdirectories = Directory.GetDirectories(directory);

                    if (files.Length == 0 && subdirectories.Length == 0)
                    {
                        Directory.Delete(directory);
                    }
                }
            }
        }
    }
}