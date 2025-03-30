using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Enums
{
    public enum FileCategroyType
    {
        Unknown = 0,
        PlainText = 1,
        Image = 2,
        Audio = 4,
        Video = 5,
        Document = 16,
        Archive = 32,
        Executable = 64,
    }
}
