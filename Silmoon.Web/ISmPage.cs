using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Web
{
    public interface ISmPage
    {
        void InitPage();
        void DrawPage();
        void BindData();
    }
}
