using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Silmoon.MySilmoon
{
    [Obsolete("Use General hosting service instead")]
    public interface IServerApplication : IRunable
    {
        event OutputTextMessageHandler OnOutputLine;
        event OutputTextMessageHandler OnInputLine;
        event ThreadExceptionEventHandler OnError;

        string ProductString { get; set; }
        int Revision { get; set; }
        string ReleaseVersion { get; set; }
        bool InitProductInfo(string productString, int revision, string releaseVersion = "0");
        void OutputLine();
        void OutputLine(string message, int flag);
        void InputLine(string message, int flag);
        void RunAndWaitConsoleLine();
    }
}
