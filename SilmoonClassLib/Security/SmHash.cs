using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Security
{
    public class SmHash
    {
        public SmHash()
        {

        }

        public static string Get16MD5(string strSource)
        {
            //new 
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();

            //获取密文字节数组 
            byte[] bytResult = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(strSource));

            //转换成字符串，并取9到25位 
            string strResult = BitConverter.ToString(bytResult, 4, 8);
            //转换成字符串，32位 
            //string strResult = BitConverter.ToString(bytResult); 

            //BitConverter转换出来的字符串会在每个字符中间产生一个分隔符，需要去除掉 
            strResult = strResult.Replace("-", "");
            return strResult;
        }
        /// <summary> 
        /// 进行MD5的32位加密
        /// </summary> 
        /// <param name="strSource">需要加密的明文</param> 
        /// <returns>返回32位加密结果</returns> 
        public static string Get32MD5(string strSource)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(strSource, "MD5");
        }

        public static string GenerateCheckCodeNum(int codeCount)
        {
            return GenerateCheckCodeNum(codeCount, 1);
        }
        /// <summary>
        /// 生成随机数字字符串
        /// </summary>
        /// <param name="codeCount">待生成的位数</param>
        /// <param name="seed">随机种子</param>
        /// <returns>生成的数字字符串</returns>
        public static string GenerateCheckCodeNum(int codeCount, int seed)
        {
            string str = string.Empty;
            long num2 = DateTime.Now.Ticks + seed;
            seed++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> seed)));
            for (int i = 0; i < codeCount; i++)
            {
                int num = random.Next();
                str = str + ((char)(0x30 + ((ushort)(num % 10)))).ToString();
            }
            return str;
        }
        public static string GenerateCheckCode(int codeCount)
        {
            return GenerateCheckCode(codeCount, 1);
        }
        /// <summary>
        /// 生成随机字母字符串(数字字母混和)
        /// </summary>
        /// <param name="codeCount">待生成的位数</param>
        /// <param name="seed">随机种子</param>
        /// <returns>生成的字母字符串</returns>
        public static string GenerateCheckCode(int codeCount, int seed)
        {
            string str = string.Empty;
            long num2 = DateTime.Now.Ticks + seed;
            seed++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> seed)));
            for (int i = 0; i < codeCount; i++)
            {
                char ch;
                int num = random.Next();
                if ((num % 2) == 0)
                {
                    ch = (char)(0x30 + ((ushort)(num % 10)));
                }
                else
                {
                    ch = (char)(0x41 + ((ushort)(num % 0x1a)));
                }
                str = str + ch.ToString();
            }
            return str;
        }
    }
}
