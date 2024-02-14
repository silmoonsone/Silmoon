using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Models
{
    public class Keypair
    {
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        public Keypair()
        {

        }
        public Keypair(string PublicKey, string PrivateKey)
        {
            this.PublicKey = PublicKey;
            this.PrivateKey = PrivateKey;
        }
        public static Keypair Create(string PublicKey, string PrivateKey)
        {
            return new Keypair(PublicKey, PrivateKey);
        }
    }
}
