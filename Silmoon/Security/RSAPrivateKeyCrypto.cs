using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using System.IO;

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
        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)        //expect integer
                return 0;
            bt = binr.ReadByte();


            if (bt == 0x81)
                count = binr.ReadByte();    // data size in next byte
            else
                if (bt == 0x82)
            {
                highbyte = binr.ReadByte();    // data size in next 2 bytes
                lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;        // we already have the data size
            }


            while (binr.ReadByte() == 0x00)
            {    //remove high order zeros in data
                count -= 1;
            }
            //binr.BaseStream.Seek(-1, SeekOrigin.Current);        //last ReadByte wasn't a removed zero, so back up a byte
            return count;
        }

        #region IDisposable 成员

        public void Dispose()
        {

        }

        #endregion
    }
}