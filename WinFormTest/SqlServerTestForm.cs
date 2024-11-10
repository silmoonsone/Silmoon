using Silmoon.Data.SqlServer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormTest
{
    public partial class SqlServerTestForm : Form
    {
        public SqlServerTestForm()
        {
            InitializeComponent();
        }

        private void ctlMainTestButton_Click(object sender, EventArgs e)
        {
            using SqlExecuter sqlExecuter = new SqlExecuter("Server=(local); Uid=LibraryTest; Pwd=LibraryTest; Database=LibraryTest; TrustServerCertificate=true");
            sqlExecuter.CreateTable<User>("users");
            var user = new User()
            {
                Username = "silmoon1"
            };
            using (var trans = sqlExecuter.BeginTransaction())
            {
                sqlExecuter.AddObject("users", user);
                sqlExecuter.CommitTransaction(trans);
            }
            sqlExecuter.AddObject("users", user);

        }
    }
}
