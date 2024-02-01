using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon
{
    public delegate void EventHandler<SenderT, EventArgsT>(SenderT sender, EventArgsT e);
}
