using Silmoon.Secure;
using System.Text;
using System.Text.RegularExpressions;
using Silmoon.Extension;

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
            string s = "import tiktoken import tiktoken import tiktoken import tiktoken import tiktoken import tiktoken ";




            var result = EncryptHelper.AesEncryptStringV2(s, "0123456789012345", false);
            var str = EncryptHelper.AesDecryptStringV2(result, "0123456789012345", false);

            var result2 = EncryptHelper.AesEncryptStringToBase64String(s, "0123456789012345");
            var str2 = EncryptHelper.AesDecryptBase64StringToString(result2, "0123456789012345");

            var result3 = EncryptHelper.AesEncryptStringToHexString(s, "0123456789012345".GetBytes(Encoding.Default));
            var str3 = EncryptHelper.AesDecryptHexStringToString(result3, "0123456789012345".GetBytes(Encoding.Default));

            var sameResult = result == result2;
        }
    }
}