using Silmoon.Secure;
using System.Text;
using System.Text.RegularExpressions;
using Silmoon.Extension;
using Silmoon.Compress;

namespace WinFormTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string s = textBox1.Text.IsNullOrEmpty() ? "import tiktoken import tiktoken import tiktoken import tiktoken import tiktoken import tiktoken" : textBox1.Text;




            var result = EncryptHelper.AesEncryptStringV2(s, "0123456789012345", false);
            var str = EncryptHelper.AesDecryptStringV2(result, "0123456789012345", false);

            var result2 = EncryptHelper.AesEncryptStringToBase64String(s, "0123456789012345");
            var str2 = EncryptHelper.AesDecryptBase64StringToString(result2, "0123456789012345");

            var result3 = EncryptHelper.AesEncryptStringToHexString(s, "0123456789012345".GetBytes(Encoding.Default));
            var str3 = EncryptHelper.AesDecryptHexStringToString(result3, "0123456789012345".GetBytes(Encoding.Default));

            var sameResult = result == result2;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string s = "0xa5643bf20000000000000000000000000000000000000000000000000000000000000060000000000000000000000000000000000000000000000000000000000000000100000000000000000000000000000000000000000000000000000000000000a0000000000000000000000000000000000000000000000000000000000000000464617665000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000003000000000000000000000000000000000000000000000000000000000000000100000000000000000000000000000000000000000000000000000000000000020000000000000000000000000000000000000000000000000000000000000003";
            var s1 = CompressHelper.CompressStringToBase64String(s);
            var s2 = CompressHelper.DecompressBase64StringToString(s1);

            var s3 = CompressHelper.CompressStringToHexString(s);
            var s4 = CompressHelper.DecompressHexStringToString(s3);
        }
    }
}