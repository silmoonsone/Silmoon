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
        public Keypair(string publicKey, string privateKey)
        {
            PublicKey = publicKey;
            PrivateKey = privateKey;
        }
        public static Keypair Create(string publicKey, string privateKey)
        {
            return new Keypair(publicKey, privateKey);
        }
    }
}
