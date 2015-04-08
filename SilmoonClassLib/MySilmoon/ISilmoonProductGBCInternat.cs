using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.MySilmoon
{
    public interface ISilmoonProductGBCInternat
    {
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
        void onOutputText(string message, int flag);

        void onInputText(string message);
        void onInputText(string message, int flag);
    }
}