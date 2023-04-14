using Silmoon.Secure;
using System.Text;
using System.Text.RegularExpressions;

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
            string s = "import tiktoken";




            var result = EncryptHelper.AesEncryptStringV2(s, "01234567890123450123456789012345", false);
            var str = Convert.ToBase64String(Encoding.UTF8.GetBytes(result));
            //str = EncryptHelper.AesDecryptStringV2(Encoding.UTF8.GetString(Convert.FromBase64String(str)), "0123456789012345");

            var result2 = EncryptHelper.AesEncrypt(s, "01234567890123450123456789012345");
            var str2 = Convert.ToBase64String(Encoding.UTF8.GetBytes(result2));
            //str2 = EncryptHelper.AesDecrypt(Encoding.UTF8.GetString(Convert.FromBase64String(str)), "0123456789012345");
        }
    }
}