using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Models.Interfaces
{
    public interface IStateFlag : IStateFlagBase
    {
        object Data
        {
            get;
            set;
        }
    }
}
