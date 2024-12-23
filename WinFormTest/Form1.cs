using Silmoon.Secure;
using System.Text;
using System.Text.RegularExpressions;
using Silmoon.Extension;
using Silmoon.Compress;
using Silmoon.Runtime;
using Silmoon.Core;
using Silmoon.Data.Models;

namespace WinFormTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void ctlAesEncryptTestButton_Click(object sender, EventArgs e)
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

        private void ctlCompressTestButton_Click(object sender, EventArgs e)
        {
            string s = "0xa5643bf20000000000000000000000000000000000000000000000000000000000000060000000000000000000000000000000000000000000000000000000000000000100000000000000000000000000000000000000000000000000000000000000a0000000000000000000000000000000000000000000000000000000000000000464617665000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000003000000000000000000000000000000000000000000000000000000000000000100000000000000000000000000000000000000000000000000000000000000020000000000000000000000000000000000000000000000000000000000000003";
            int size = s.Length;
            string s1 = CompressHelper.CompressStringToBase64String(s);
            string s2 = CompressHelper.DecompressBase64StringToString(s1);

            string s3 = CompressHelper.CompressStringToHexString(s);
            string s4 = CompressHelper.DecompressHexStringToString(s3);

            byte[] s5 = CompressHelper.CompressStringToByteArray(s);
            string s6 = CompressHelper.DecompressByteArrayToString(s5);
        }

        private void ctlSubStringTestButton_Click(object sender, EventArgs e)
        {
            var result = textBox1.Text.SubstringSpecial(2, 4);
            MessageBox.Show(result);
        }

        private void ctlCopyTestButton_Click(object sender, EventArgs e)
        {
            List<User> users = new List<User>
            {
                new User() { Username = "a" },
                new User() { Username = "b" },
                new User() { Username = "c" }
            };

            var result = Copy.ArrayNew<User, UserEx>(users.ToArray());
            Console.WriteLine(result.ToFormattedJsonString());
        }

        private void ctlKeyFileEncryptoButton_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text.KeyFileEncryptToSmkmUri("diiFG0eCmgB523qiNRUOa3Lk4TYN93g6");
            textBox1.Text = textBox1.Text.TryKeyFileDecryptSmkmUri();
        }

        private async void ctlJsonHelperTestButton_Click(object sender, EventArgs e)
        {
            var result = await JsonHelperV2.GetJsonAsync("https://api.trongrid.io/v1/blocks/47355207/events?limit=200");
        }

        private async void ctlDownloadTestButton_Click(object sender, EventArgs e)
        {
            using HttpClientEx client = new HttpClientEx();
            client.DownloadStarted += (sender, e) => MessageBox.Show("Download started.");
            client.DownloadProgressChanged += (sender, e) => label1.Text = $"Download progress: {e.BytesReceived}/{e.TotalBytesToReceive}";
            client.DownloadCompleted += (sender, e) => MessageBox.Show("Download completed.");
            await client.DownloadFileAsync("https://huobao-bin.oss-cn-qingdao.aliyuncs.com/bin/mongodb/mongod.exe", "D:\\test\\bin.bin");
        }

        private void ctlSqlServerTestButton_Click(object sender, EventArgs e)
        {
            new SqlServerTestForm().Show();
        }
    }
    class User : SqlObject
    {
        public string Username { get; set; }
    }
    class UserEx : User
    {
        public string Password { get; set; }
    }
}