using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Silmoon.MySilmoon
{
    public interface IServerApplication : IRunningAble
    {
        event OutputTextMessageHandler OnOutputLine;
        event OutputTextMessageHandler OnInputLine;
        event ThreadExceptionEventHandler OnException;

        string ProductString { get; set; }
        int Revision { get; set; }
        string ReleaseVersion { get; set; }
        bool InitProductInfo(string productString, int revision, string releaseVersion = "0");
        void OutputLine();
        void OutputLine(object message, int flag);
        void OutputLine(string message, int flag);
        void InputLine(string message, int flag);
    }
}
