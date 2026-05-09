using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Extensions.Hosting.Interfaces
{
    public interface ISilmoonPlatformDirectoryService
    {
        string AppConfigDirectory { get; }
        string AppDataDirectory { get; }
        string AppWorkingDirectory { get; }
    }
}
