using Silmoon.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Silmoon.Extensions
{
    public static class FileSystemExtension
    {
#if NET10_0_OR_GREATER

        extension(File)
        {

        }
        extension(Directory)
        {
            public static void CreateDirectory(string directoryPath) => FileHelper.CreateDirectory(directoryPath);
            public static void ClearEmptyDirectory(string targetDirectoryPath, string reserveDirectoryPath) => FileHelper.ClearEmptyDirectory(targetDirectoryPath, reserveDirectoryPath);
        }
#endif
    }
}
