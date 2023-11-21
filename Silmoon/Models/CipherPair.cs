using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Models
{
    public class CipherPair
    {

        public string CipherData { get; set; }
        public string Password { get; set; }
        public CipherPair()
        {

        }
        public CipherPair(string CipherData, string Password)
        {
            this.CipherData = CipherData;
            this.Password = Password;
        }
        public static CipherPair Create(string CipherData, string Password)
        {
            return new CipherPair(CipherData, Password);
        }
    }
}
