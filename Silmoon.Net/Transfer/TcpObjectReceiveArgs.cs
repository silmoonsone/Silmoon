using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Net.Transfer
{
    public class TcpObjectReceiveArgs<T> : TcpEventArgs
    {
        public T Object { get; set; }
        public TcpEventArgs TcpArgs { get; set; }
    }
}
