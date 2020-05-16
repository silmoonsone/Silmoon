using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Silmoon.MySilmoon
{
    public interface ISilmoonProductGBCInternat : IRunningAble
    {
        event OutputTextMessageHandler OnOutput;
        event OutputTextMessageHandler OnInput;
        event ThreadExceptionEventHandler OnException;

        string ProductString
        {
            get;
            set;
        }
        int Revision
        {
            get;
            set;
        }
        string ReleaseVersion
        {
            get;
            set;
        }
        bool InitProductInfo(string productString, int revision, string releaseVersion = "0");
        void Output(string message);
        void Output(string message, int flag);
        void Input(string message);
        void Input(string message, int flag);
    }
}