using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.MySilmoon
{
    public class ServerApplicationEventArgs
    {
        public int Flag { get; set; }
        public string Message { get; set; }
        ServerApplicationEventArgs(string message, int flag)
        {
            Flag = flag;
            Message = message;
        }
        public static ServerApplicationEventArgs Create(string Message, int Flag = 0)
        {
            return new ServerApplicationEventArgs(Message, Flag);
        }
    }
}
