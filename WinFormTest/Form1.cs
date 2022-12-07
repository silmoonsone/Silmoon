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
            // 要验证的 URL
            string url = "http://www.example.com/path/to/resource";

            // 验证 URL 的正则表达式
            string pattern = @"^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})(:[0-9]{1,5})?([\/\w \.-]*)*\/?$";

            // 创建一个 Regex 对象
            Regex regex = new Regex(pattern);

            // 检查 URL 是否符合格式
            if (regex.IsMatch(url))
            {
                MessageBox.Show("URL 格式正确");
            }
            else
            {
                MessageBox.Show("URL 格式不正确");
            }
        }
    }
}