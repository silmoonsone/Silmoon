using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Extensions.Hosting.Interfaces
{
    public interface ISilmoonConfigureFileReadService
    {
        string GetFileContent(string filePath);
        bool FileExists(string filePath);
    }
}
