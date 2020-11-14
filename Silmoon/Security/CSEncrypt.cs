using System;
using System.IO;
using System.Security.Cryptography;

using System.Text;
using System.Collections.Generic;

namespace Silmoon.Security
{
    /// <summary>   
    /// 对称加密算法类   
    /// </summary>   
    public class CSEncrypt : IDisposable
    {
        private SymmetricAlgorithm mobjCryptoService;
        private string Key;
        /// <summary>   
        /// 对称加密类的构造函数   
        /// </summary>   
        public CSEncrypt()
        {
            mobjCryptoService = new RijndaelManaged();
            Key = "8UFCy76G7jH7yuBI0456lhj!y6&(*jkP8FtmaTuz(%&hj9H$ilJ$75&fvHx*h%(H";
        }
        public CSEncrypt(string hashKey)
        {
            mobjCryptoService = new RijndaelManaged();
            Key = hashKey;
        }
        /// <summary>   
        /// 获得密钥   
        /// </summary>   
        /// <returns>密钥</returns>   
        private byte[] GetLegalKey()
        {
            string sTemp = Key;
            mobjCryptoService.GenerateKey();
            byte[] bytTemp = mobjCryptoService.Key;
            int KeyLength = bytTemp.Length;
            if (sTemp.Length > KeyLength)
                sTemp = sTemp.Substring(0, KeyLength);
            else if (sTemp.Length < KeyLength)
                sTemp = sTemp.PadRight(KeyLength, ' ');
            return ASCIIEncoding.UTF8.GetBytes(sTemp);
        }
        /// <summary>   
        /// 获得初始向量IV   
        /// </summary>   
        /// <returns>初试向量IV</returns>   
        private byte[] GetLegalIV()
        {
            string sTemp = "6Gfghj*Ghg7!UNI57i%$hjkE4Wk7%g6HJ($jhHBh(ughUb#er&!hg4ufb&95GUY8";
            mobjCryptoService.GenerateIV();
            byte[] bytTemp = mobjCryptoService.IV;
            int IVLength = bytTemp.Length;
            if (sTemp.Length > IVLength)
                sTemp = sTemp.Substring(0, IVLength);
            else if (sTemp.Length < IVLength)
                sTemp = sTemp.PadRight(IVLength, ' ');
            return ASCIIEncoding.UTF8.GetBytes(sTemp);
        }
        /// <summary>   
        /// 加密方法   
        /// </summary>   
        /// <param name="Source">待加密的串</param>   
        /// <returns>经过加密的串</returns>   
        public string EncryptoByte(string Source)
        {
            if (Source == "") return "";
            byte[] bytIn = UTF8Encoding.UTF8.GetBytes(Source);
            MemoryStream ms = new MemoryStream();
            mobjCryptoService.Key = GetLegalKey();
            mobjCryptoService.IV = GetLegalIV();
            ICryptoTransform encrypto = mobjCryptoService.CreateEncryptor();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
            cs.Write(bytIn, 0, bytIn.Length);
            cs.FlushFinalBlock();
            ms.Close();
            byte[] bytOut = ms.ToArray();
            return EncryptString.EncryptSilmoonBinary(Convert.ToBase64String(bytOut));
        }
        /// <summary>   
        /// 解密方法   
        /// </summary>   
        /// <param name="Source">待解密的串</param>   
        /// <returns>经过解密的串</returns>   
        public string DecryptoByte(string Source)
        {
            try
            {
                if (EncryptString.DiscryptSilmoonBinary(Source) == "") return "";
                byte[] bytIn = Convert.FromBase64String(EncryptString.DiscryptSilmoonBinary(Source));
                MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
                mobjCryptoService.Key = GetLegalKey();
                mobjCryptoService.IV = GetLegalIV();
                ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cs);
                return sr.ReadToEnd();
            }
            catch { return ""; }
        }
        /// <summary>   
        /// 加密方法   
        /// </summary>   
        /// <param name="Source">待加密的串</param>   
        /// <returns>经过加密的串</returns>   
        public string Encrypt(string Source)
        {
            if (Source == "") return "";
            byte[] bytIn = UTF8Encoding.UTF8.GetBytes(Source);
            MemoryStream ms = new MemoryStream();
            mobjCryptoService.Key = GetLegalKey();
            mobjCryptoService.IV = GetLegalIV();
            ICryptoTransform encrypto = mobjCryptoService.CreateEncryptor();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
            cs.Write(bytIn, 0, bytIn.Length);
            cs.FlushFinalBlock();
            ms.Close();
            byte[] bytOut = ms.ToArray();
            return Convert.ToBase64String(bytOut);
        }
        /// <summary>   
        /// 解密方法   
        /// </summary>   
        /// <param name="Source">待解密的串</param>   
        /// <returns>经过解密的串</returns>   
        public string Decrypt(string Source)
        {
            try
            {
                if (Source == "") return "";
                byte[] bytIn = Convert.FromBase64String(Source);
                MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
                mobjCryptoService.Key = GetLegalKey();
                mobjCryptoService.IV = GetLegalIV();
                ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cs);
                return sr.ReadToEnd();
            }
            catch { return ""; }
        }

        /// <summary>   
        /// 加密方法   
        /// </summary>   
        /// <param name="Source">待加密的串</param>   
        /// <returns>经过加密的串</returns>   
        public byte[] Encrypt(byte[] Source)
        {
            string s = Convert.ToBase64String(Source);
            Source = Encoding.ASCII.GetBytes(s);
            byte[] bytIn = Source;
            MemoryStream ms = new MemoryStream();
            mobjCryptoService.Key = GetLegalKey();
            mobjCryptoService.IV = GetLegalIV();
            ICryptoTransform encrypto = mobjCryptoService.CreateEncryptor();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
            cs.Write(bytIn, 0, bytIn.Length);
            cs.FlushFinalBlock();
            ms.Close();
            byte[] bytOut = ms.ToArray();
            return bytOut;
        }
        /// <summary>   
        /// 解密方法   
        /// </summary>   
        /// <param name="Source">待解密的串</param>
        /// <returns>经过解密的串</returns>   
        public byte[] Decrypt(byte[] Source)
        {
            try
            {
                byte[] bytIn = Source;
                MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
                mobjCryptoService.Key = GetLegalKey();
                mobjCryptoService.IV = GetLegalIV();
                ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cs);
                List<byte> list = new List<byte>();
                while (!sr.EndOfStream)
                {
                    list.Add((byte)sr.Read());
                }
                byte[] bytOut = list.ToArray();

                string s = Encoding.ASCII.GetString(bytOut);
                bytOut = Convert.FromBase64String(s);

                return bytOut;
            }
            catch { return null; }
        }
        /// <summary>
        /// 重新设置一个哈希关键值
        /// </summary>
        /// <param name="key"></param>
        public void Setkey(string key)
        {
            this.Key = key;
        }

        #region IDisposable 成员

        public void Dispose()
        {
            mobjCryptoService.Clear();
        }

        #endregion
    }
}
