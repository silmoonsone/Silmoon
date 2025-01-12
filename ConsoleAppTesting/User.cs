using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTesting
{
    public class User
    {
        public int _id { get; set; } = 0;
        public string Username { get; set; }
        public string Password { get; set; }
        public int Flag { get; set; } = 1;
        public DateTime create_at { get; set; } = DateTime.Now;
    }
}
