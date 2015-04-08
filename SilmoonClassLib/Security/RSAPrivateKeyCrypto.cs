using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Collections;

namespace Silmoon.Security
{
    public class RSAPrivateKeyCrypto : IDisposable
    {
        RSAParameters paramsters;
        public RSAPrivateKeyCrypto(RSAParameters paramsters)
        {
            this.paramsters = paramsters;
        }
        public byte[] Encrypt(byte[] source)
        {
            BigInteger d = new BigInteger(paramsters.D);
            BigInteger n = new BigInteger(paramsters.Modulus);
            int sug = 127;
            int len = source.Length;
            int cycle = 0;
            if ((len % sug) == 0) cycle = len / sug; else cycle = len / sug + 1;

            ArrayList temp = new ArrayList();
            int blockLen = 0;
            for (int i = 0; i < cycle; i++)
            {
                if (len >= sug) blockLen = sug; else blockLen = len;

                byte[] context = new byte[blockLen];
                int po = i * sug;
                Array.Copy(source, po, context, 0, blockLen);

                BigInteger biText = new BigInteger(context);
                BigInteger biEnText = biText.modPow(d, n);

                byte[] b = biEnText.getBytes();
                temp.AddRange(b);
                len -= blockLen;
            }
            return (byte[])temp.ToArray(typeof(byte));
        }
        public byte[] Decrypt(byte[] source)
        {
            BigInteger e = new BigInteger(paramsters.Exponent);
            BigInteger n = new BigInteger(paramsters.Modulus);

            int bk = 128;
            int len = source.Length;
            int cycle = 0;
            if ((len % bk) == 0) cycle = len / bk; else cycle = len / bk + 1;

            ArrayList temp = new ArrayList();
            int blockLen = 0;
            for (int i = 0; i < cycle; i++)
            {
                if (len >= bk) blockLen = bk; else blockLen = len;

                byte[] context = new byte[blockLen];
                int po = i * bk;
                Array.Copy(source, po, context, 0, blockLen);

                BigInteger biText = new BigInteger(context);
                BigInteger biEnText = biText.modPow(e, n);

                byte[] b = biEnText.getBytes();
                temp.AddRange(b);
                len -= blockLen;
            }
            return (byte[])temp.ToArray(typeof(byte));
        }

        #region IDisposable 成员

        public void Dispose()
        {

        }

        #endregion
    }
}